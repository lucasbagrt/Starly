#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8086
EXPOSE 8087

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Review/Review.API/Review.API.csproj", "Review/Review.API/"]
COPY ["Common/Starly.CrossCutting/Starly.CrossCutting.csproj", "Common/Starly.CrossCutting/"]
COPY ["Review/Review.Infra.Data/Review.Infra.Data.csproj", "Review/Review.Infra.Data/"]
COPY ["Common/Starly.Infra.Data/Starly.Infra.Data.csproj", "Common/Starly.Infra.Data/"]
COPY ["Common/Starly.Domain/Starly.Domain.csproj", "Common/Starly.Domain/"]
COPY ["Common/Starly.Service/Starly.Service.csproj", "Common/Starly.Service/"]
COPY ["Review/Review.Domain/Review.Domain.csproj", "Review/Review.Domain/"]
COPY ["Review/Review.Service/Review.Service.csproj", "Review/Review.Service/"]
RUN dotnet restore "./Review/Review.API/Review.API.csproj"
COPY . .
WORKDIR "/src/Review/Review.API"
RUN dotnet build "./Review.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Review.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:86
ENTRYPOINT ["dotnet", "Review.API.dll"]