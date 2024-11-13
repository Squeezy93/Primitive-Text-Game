FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PrimitiveTextGame.Telegram/PrimitiveTextGame.Telegram.csproj", "src/PrimitiveTextGame.Telegram/"]
RUN dotnet restore "src/PrimitiveTextGame.Telegram/PrimitiveTextGame.Telegram.csproj"
COPY . .
WORKDIR "/src/src/PrimitiveTextGame.Telegram"
RUN dotnet build "PrimitiveTextGame.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PrimitiveTextGame.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrimitiveTextGame.Telegram.dll"]
