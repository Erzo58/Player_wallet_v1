FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080 

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Player_wallet_v1.csproj", "./"]
RUN dotnet restore "./Player_wallet_v1.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet build "Player_wallet_v1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Player_wallet_v1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Player_wallet_v1.dll"]