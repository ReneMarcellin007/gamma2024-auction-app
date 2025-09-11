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