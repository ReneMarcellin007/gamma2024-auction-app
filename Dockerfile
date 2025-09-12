FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE $PORT
ENV ASPNETCORE_URLS=http://+:$PORT

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js for Vue.js build
RUN apt-get update && \
    apt-get install -y curl gnupg && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    node --version && \
    npm --version

# Copy solution and project files
COPY ["Gamma2024.Server/Gamma2024.Server.csproj", "Gamma2024.Server/"]
COPY ["gamma2024.client/gamma2024.client.esproj", "gamma2024.client/"]
COPY ["Gamma2024.sln", "./"]

# Restore dependencies
RUN dotnet restore "Gamma2024.Server/Gamma2024.Server.csproj"

# Copy source code
COPY . .

# Build Vue.js frontend manually
WORKDIR /src/gamma2024.client
RUN npm install
RUN npm run build:railway

# Backup Images folder, clear wwwroot completely, copy Vue.js build (exclude conflicting files), restore Images
RUN mkdir -p /tmp/backup && \
    cp -r /src/Gamma2024.Server/wwwroot/Images /tmp/backup/ 2>/dev/null || true && \
    rm -rf /src/Gamma2024.Server/wwwroot && \
    mkdir -p /src/Gamma2024.Server/wwwroot && \
    rm -f dist/favicon.ico && \
    rm -rf dist/icons && \
    rm -rf dist/images && \
    cp -r dist/* /src/Gamma2024.Server/wwwroot/ && \
    cp -r /tmp/backup/Images /src/Gamma2024.Server/wwwroot/ 2>/dev/null || true && \
    mv dist /tmp/vue-dist

WORKDIR /src

# Build the application
RUN dotnet build "Gamma2024.Server/Gamma2024.Server.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Gamma2024.Server/Gamma2024.Server.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user
RUN addgroup --system --gid 1001 dotnetgroup && \
    adduser --system --uid 1001 --ingroup dotnetgroup dotnetuser

# Set ownership and permissions
RUN chown -R dotnetuser:dotnetgroup /app
USER dotnetuser

ENTRYPOINT ["dotnet", "Gamma2024.Server.dll"]