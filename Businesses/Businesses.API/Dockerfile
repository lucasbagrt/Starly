#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Businesses/Businesses.API/Businesses.API.csproj", "Businesses/Businesses.API/"]
COPY ["Common/Starly.CrossCutting/Starly.CrossCutting.csproj", "Common/Starly.CrossCutting/"]
COPY ["Businesses/Businesses.Domain/Businesses.Domain.csproj", "Businesses/Businesses.Domain/"]
COPY ["Common/Starly.Domain/Starly.Domain.csproj", "Common/Starly.Domain/"]
COPY ["Businesses/Businesses.Infra.Data/Businesses.Infra.Data.csproj", "Businesses/Businesses.Infra.Data/"]
COPY ["Common/Starly.Infra.Data/Starly.Infra.Data.csproj", "Common/Starly.Infra.Data/"]
COPY ["Common/Starly.Service/Starly.Service.csproj", "Common/Starly.Service/"]
COPY ["Businesses/Businesses.Service/Businesses.Service.csproj", "Businesses/Businesses.Service/"]
RUN dotnet restore "./Businesses/Businesses.API/Businesses.API.csproj"
COPY . .
WORKDIR "/src/Businesses/Businesses.API"
RUN dotnet build "./Businesses.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Businesses.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:82
ENTRYPOINT ["dotnet", "Businesses.API.dll"]