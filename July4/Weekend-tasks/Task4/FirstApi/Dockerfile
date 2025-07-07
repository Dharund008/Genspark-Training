FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./FirstApi.csproj"
RUN dotnet publish "./FirstApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# Environment variable for Development to enable Swagger
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5000
ENTRYPOINT ["dotnet", "FirstApi.dll"]