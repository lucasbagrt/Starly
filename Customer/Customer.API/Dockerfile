#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Customer/Customer.API/Customer.API.csproj", "Customer/Customer.API/"]
COPY ["Customer/Customer.Domain/Customer.Domain.csproj", "Customer/Customer.Domain/"]
COPY ["Common/Starly.Domain/Starly.Domain.csproj", "Common/Starly.Domain/"]
COPY ["Customer/Customer.Infra.Data/Customer.Infra.Data.csproj", "Customer/Customer.Infra.Data/"]
COPY ["Common/Starly.Infra.Data/Starly.Infra.Data.csproj", "Common/Starly.Infra.Data/"]
COPY ["Common/Starly.Service/Starly.Service.csproj", "Common/Starly.Service/"]
COPY ["Customer/Customer.Service/Customer.Service.csproj", "Customer/Customer.Service/"]
COPY ["Common/Starly.CrossCutting/Starly.CrossCutting.csproj", "Common/Starly.CrossCutting/"]
RUN dotnet restore "./Customer/Customer.API/Customer.API.csproj"
COPY . .
WORKDIR "/src/Customer/Customer.API"
RUN dotnet build "./Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Customer.API.dll"]