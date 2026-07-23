FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sisjog.sln", "."]
COPY ["Sisjog.Api/Sisjog.Api.csproj", "Sisjog.Api/"]
COPY ["Sisjog.Application/Sisjog.Application.csproj", "Sisjog.Application/"]
COPY ["Sisjog.Domain/Sisjog.Domain.csproj", "Sisjog.Domain/"]
COPY ["Sisjog.Infrastructure/Sisjog.Infrastructure.csproj", "Sisjog.Infrastructure/"]
RUN dotnet restore "Sisjog.sln"
COPY . .
WORKDIR "/src/Sisjog.Api"
RUN dotnet publish "Sisjog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "Sisjog.Api.dll"]
