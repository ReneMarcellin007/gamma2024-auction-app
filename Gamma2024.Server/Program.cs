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
        
        // Railway fournit DATABASE_PUBLIC_URL ou DATABASE_URL
        var connectionString = "";
        
        // Essayer DATABASE_PUBLIC_URL en premier (Railway l'utilise)
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL") 
                       ?? Environment.GetEnvironmentVariable("DATABASE_URL");
        
        Console.WriteLine("=== Railway Database Configuration ===");
        Console.WriteLine($"DATABASE_PUBLIC_URL: {(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL")) ? "Set" : "Not set")}");
        Console.WriteLine($"DATABASE_URL: {(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL")) ? "Set" : "Not set")}");
        
        if (!string.IsNullOrEmpty(databaseUrl))
        {
            try
            {
                Console.WriteLine($"Parsing database URL...");
                
                // Convertir postgres:// en postgresql://
                var urlToParse = databaseUrl;
                if (databaseUrl.StartsWith("postgres://"))
                {
                    urlToParse = databaseUrl.Replace("postgres://", "postgresql://");
                }
                
                var uri = new Uri(urlToParse);
                
                // Extraire les composants
                var userInfo = uri.UserInfo.Split(':');
                var username = Uri.UnescapeDataString(userInfo[0]);
                var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
                var host = uri.Host;
                var port = uri.Port > 0 ? uri.Port : 5432;
                var database = uri.AbsolutePath.TrimStart('/');
                
                connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SslMode=Require;Trust Server Certificate=true";
                Console.WriteLine($"Database connection configured:");
                Console.WriteLine($"  Host: {host}");
                Console.WriteLine($"  Port: {port}");
                Console.WriteLine($"  Database: {database}");
                Console.WriteLine($"  Username: {username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse database URL: {ex.Message}");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
            }
        }
        else
        {
            // Fallback: essayer les variables PG individuelles
            var pgHost = Environment.GetEnvironmentVariable("PGHOST");
            var pgPort = Environment.GetEnvironmentVariable("PGPORT") ?? "5432";
            var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
            var pgUser = Environment.GetEnvironmentVariable("PGUSER");
            var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");
            
            Console.WriteLine("Checking individual PG variables:");
            Console.WriteLine($"  PGHOST: {(!string.IsNullOrEmpty(pgHost) ? "Set" : "Not set")}");
            Console.WriteLine($"  PGDATABASE: {(!string.IsNullOrEmpty(pgDatabase) ? "Set" : "Not set")}");
            Console.WriteLine($"  PGUSER: {(!string.IsNullOrEmpty(pgUser) ? "Set" : "Not set")}");
            
            if (!string.IsNullOrEmpty(pgHost) && !string.IsNullOrEmpty(pgDatabase) && 
                !string.IsNullOrEmpty(pgUser) && !string.IsNullOrEmpty(pgPassword))
            {
                connectionString = $"Host={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword};SslMode=Require;Trust Server Certificate=true";
                Console.WriteLine("Connection string built from PG environment variables.");
            }
        }
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Dernier recours: utiliser la configuration locale
            connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Using connection string from appsettings.");
            }
            else
            {
                throw new InvalidOperationException("No valid database connection string found!");
            }
        }
            
        Console.WriteLine($"Final connection string length: {connectionString.Length} characters");
        
        try
        {
            options.UseNpgsql(connectionString);
            Console.WriteLine("Successfully configured PostgreSQL connection");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR configuring PostgreSQL: {ex.Message}");
            throw;
        }
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

// Auto-migration et seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    Console.WriteLine("=== Database initialization ===");
    Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
    
    try
    {
        // Tester la connexion
        if (await context.Database.CanConnectAsync())
        {
            Console.WriteLine("Database connection successful.");
            
            // FORCER LA RECRÉATION COMPLÈTE EN PRODUCTION
            Console.WriteLine("FORCING COMPLETE DATABASE RECREATION!");
            try 
            {
                Console.WriteLine("Deleting existing database...");
                await context.Database.EnsureDeletedAsync();
                Console.WriteLine("Database deleted.");
            }
            catch (Exception delEx)
            {
                Console.WriteLine($"Could not delete database: {delEx.Message}");
            }
            
            Console.WriteLine("Creating fresh database with schema...");
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("Database created with all tables!");
            
            // Créer les rôles de base s'ils n'existent pas
            Console.WriteLine("Creating default roles...");
            string[] roles = { "Admin", "Client", "Vendeur" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"Role '{role}' created.");
                }
            }
            
            // Créer un admin par défaut s'il n'existe pas
            var adminEmail = "admin@encans.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Admin user created.");
                }
            }
            
            Console.WriteLine("Database seeding completed.");
        }
        else
        {
            Console.WriteLine("ERROR: Cannot connect to database!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"CRITICAL ERROR during database initialization: {ex.Message}");
        Console.WriteLine($"Exception type: {ex.GetType().Name}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            Console.WriteLine($"Inner exception type: {ex.InnerException.GetType().Name}");
        }
        
        // Don't throw - let the app start even if DB init fails
        // This allows us to see health endpoint errors
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

// Health check endpoint pour tester la connexion DB
app.MapGet("/api/health", async (ApplicationDbContext context) =>
{
    try
    {
        // Tester la connexion à la base de données
        var canConnect = await context.Database.CanConnectAsync();
        if (canConnect)
        {
            var userCount = await context.Users.CountAsync();
            var encanCount = await context.Encans.CountAsync();
            return Results.Ok(new 
            { 
                status = "healthy", 
                database = "connected",
                users = userCount,
                encans = encanCount,
                timestamp = DateTime.UtcNow 
            });
        }
        return Results.Json(new { status = "unhealthy", database = "cannot connect" }, statusCode: 503);
    }
    catch (Exception ex)
    {
        return Results.Json(new 
        { 
            status = "unhealthy", 
            database = "error",
            error = ex.Message,
            type = ex.GetType().Name
        }, statusCode: 503);
    }
});

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
