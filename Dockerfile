# Etapa base de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de compilación y restauración
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos de proyecto para optimizar cache de Docker
COPY ["src/Sensus.Domain/Sensus.Domain.csproj", "src/Sensus.Domain/"]
COPY ["src/Sensus.Application/Sensus.Application.csproj", "src/Sensus.Application/"]
COPY ["src/Sensus.Infrastructure/Sensus.Infrastructure.csproj", "src/Sensus.Infrastructure/"]
COPY ["src/Sensus.Api/Sensus.Api.csproj", "src/Sensus.Api/"]

# Restaurar dependencias
RUN dotnet restore "src/Sensus.Api/Sensus.Api.csproj"

# Copiar el código fuente completo
COPY . .

# Compilar proyecto API
WORKDIR "/src/src/Sensus.Api"
RUN dotnet build "Sensus.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Sensus.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final de producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sensus.Api.dll"]
