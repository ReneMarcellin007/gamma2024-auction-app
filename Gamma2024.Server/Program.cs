using Gamma2024.Server.Data;
using Gamma2024.Server.Hub;
using Gamma2024.Server.Interface;
using Gamma2024.Server.Models;
using Gamma2024.Server.Services;
using Gamma2024.Server.Services.Email;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Charger les fichiers de configuration selon l'environnement
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseSqlite("Data Source=gamma2024.db");
    }
    else
    {
        // Debug: afficher les variables d'environnement disponibles
        Console.WriteLine("=== DEBUG Railway Environment Variables ===");
        Console.WriteLine($"DATABASE_URL: {Environment.GetEnvironmentVariable("DATABASE_URL")}");
        Console.WriteLine($"PGDATABASE: {Environment.GetEnvironmentVariable("PGDATABASE")}");
        Console.WriteLine($"PGHOST: {Environment.GetEnvironmentVariable("PGHOST")}");
        Console.WriteLine($"PGPORT: {Environment.GetEnvironmentVariable("PGPORT")}");
        Console.WriteLine($"PGUSER: {Environment.GetEnvironmentVariable("PGUSER")}");
        Console.WriteLine($"PGPASSWORD: {Environment.GetEnvironmentVariable("PGPASSWORD")}");
        
        // Essayer de construire une connection string à partir des variables
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        var connectionString = "";
        
        if (!string.IsNullOrEmpty(databaseUrl))
        {
            try
            {
                // Railway fournit DATABASE_URL au format: postgres://user:pass@host:port/database
                var uri = new Uri(databaseUrl.Replace("postgres://", "postgresql://"));
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo[0];
                var password = userInfo.Length > 1 ? userInfo[1] : "";
                var host = uri.Host;
                var port = uri.Port > 0 ? uri.Port : 5432;
                var database = uri.AbsolutePath.TrimStart('/');
                
                connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SslMode=Require;Trust Server Certificate=true";
                Console.WriteLine($"Parsed DATABASE_URL successfully");
                Console.WriteLine($"Host: {host}, Port: {port}, Database: {database}, Username: {username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse DATABASE_URL: {ex.Message}");
                connectionString = "";
            }
        }
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Fallback: construire à partir des variables individuelles
            var host = Environment.GetEnvironmentVariable("PGHOST");
            var port = Environment.GetEnvironmentVariable("PGPORT") ?? "5432";
            var database = Environment.GetEnvironmentVariable("PGDATABASE");
            var username = Environment.GetEnvironmentVariable("PGUSER");
            var password = Environment.GetEnvironmentVariable("PGPASSWORD");
            
            if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(database) && !string.IsNullOrEmpty(username))
            {
                connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SslMode=Require;Trust Server Certificate=true";
                Console.WriteLine($"Built connection string from individual vars");
                Console.WriteLine($"Host: {host}, Port: {port}, Database: {database}, Username: {username}");
            }
        }
        
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("No database connection string found.");
        }
            
        Console.WriteLine($"Final connection string length: {connectionString.Length} characters");
        options.UseNpgsql(connectionString);
    }
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(builder => builder.EnableDetailedErrors = true);
builder.Services.AddSingleton<NotificationHub>();
builder.Services.AddScoped<ClientInscriptionService>();
builder.Services.AddScoped<ClientModificationService>();
builder.Services.AddScoped<VendeurService>();
builder.Services.AddScoped<AdministrateurService>();
builder.Services.AddScoped<EncanService>();
builder.Services.AddScoped<LotService>();
builder.Services.AddScoped<FactureService>();
builder.Services.AddScoped<FactureLivraisonService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHostedService<VerificationLotsBackgroundService>();

builder.Services.Configure<EmailConfiguration>(
    builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddTransient<IEmailSender, EmailService>();
builder.Services.Configure<InvoiceSettings>(builder.Configuration.GetSection("InvoiceSettings"));

// Important : Ajouter ceci avant AddHttpClient
builder.Services.AddHttpContextAccessor();

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

builder.Services.AddHttpClient("ApiClient", (serviceProvider, client) =>
{
    // URL de production hardcodée
    var apiUrl = "https://sqlinfocg.cegepgranby.qc.ca/2162067/";
    client.BaseAddress = new Uri(apiUrl);
    logger.LogInformation($"API Client configuré avec l'URL hardcodée: {apiUrl}");
});



// Configuration CORS pour différents environnements
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("Development", builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .WithOrigins("https://localhost:5173", "http://localhost:5173", "https://localhost:5174", "http://localhost:5174", "https://localhost:5175", "http://localhost:5175")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    }
    else
    {
        options.AddPolicy("Production", builder =>
        {
            builder
                .WithOrigins("https://sqlinfocg.cegepgranby.qc.ca/2162067")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    }
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/api/hub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    })
    .AddCookie(options =>
    {
        //options.Events.OnRedirectToAccessDenied =
        options.Events.OnRedirectToAccessDenied = c =>
        {
            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.FromResult<object>(null);
        };
    });

var multiSchemePolicy = new AuthorizationPolicyBuilder(
    CookieAuthenticationDefaults.AuthenticationScheme,
    JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser()
  .Build();

builder.Services.AddAuthorization(o => o.DefaultPolicy = multiSchemePolicy);


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


var app = builder.Build();

// Auto-migration et seeding en production
if (!app.Environment.IsDevelopment())
{
    Console.WriteLine("=== Running database migration and seeding for production ===");
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        try
        {
            // Appliquer les migrations
            Console.WriteLine("Applying database migrations...");
            context.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
            
            // Créer les rôles de base
            string[] roleNames = { "Admin", "Client", "Vendeur" };
            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                    Console.WriteLine($"Role '{roleName}' created.");
                }
            }
            
            // Créer un utilisateur admin par défaut si aucun n'existe
            if (!context.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };
                
                var result = userManager.CreateAsync(admin, "Admin123!").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
                    Console.WriteLine("Admin user created successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during database initialization: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    //app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseHttpsRedirection();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");

}
else
{
    app.UseCors("Production");

}
app.UseAuthentication();
app.UseAuthorization();

// Configurer le port pour Railway
if (!app.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.MapHub<LotMiseHub>("/api/hub/lotMiseHub"); // Permet de mapper les requêtes vers SignalR
app.MapHub<NotificationHub>("/api/hub/NotificationHub");
app.MapHub<EncanHub>("/api/hub/EncanHub");

app.MapControllers();

app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
//    RequestPath = ""
//});

app.MapFallbackToFile("index.html");





app.Run();
