# See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG YakovAkk
ARG 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

WORKDIR /src

# Copy nuget.config and set GitHub credentials
COPY nuget.config ./
RUN sed -i "s/%GITHUB_USERNAME%/$GITHUB_USERNAME/g" nuget.config
RUN sed -i "s/%GITHUB_TOKEN%/$GITHUB_TOKEN/g" nuget.config

# Copy project files
COPY ["./CulturalShare.Gateway/CulturalShare.Gateway.csproj", "CulturalShare.Gateway/"]
COPY ["./CulturalShare.Gateway.Common/CulturalShare.Gateway.Common.csproj", "CulturalShare.Gateway.Common/"]

# Restore dependencies
RUN dotnet restore "./CulturalShare.Gateway/CulturalShare.Gateway.csproj"

# Copy the rest of the project files
COPY . .

WORKDIR "/src/CulturalShare.Gateway"

# Build the project
RUN dotnet build "./CulturalShare.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Publish the project
RUN dotnet publish "./CulturalShare.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Copy the published output to the final image
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CulturalShare.Gateway.dll"]
