# Multi-stage build for .NET 9.0 application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["CompanyService.csproj", "./"]
RUN dotnet restore "CompanyService.csproj"

# Copy source code
COPY . .
RUN dotnet build "CompanyService.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CompanyService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "CompanyService.dll"] 