FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Gamma2024.Server.csproj", "."]
RUN dotnet restore "./Gamma2024.Server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Gamma2024.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gamma2024.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gamma2024.Server.dll"]