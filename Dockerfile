FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ECommerce.API/ECommerce.API.csproj ECommerce.API/
RUN dotnet restore ECommerce.API/ECommerce.API.csproj

COPY ECommerce.API/ ECommerce.API/
WORKDIR /src/ECommerce.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ECommerce.API.dll"]
