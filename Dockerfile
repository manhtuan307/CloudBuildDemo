FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY CloudBuildDemo/*.csproj ./CloudBuildDemo/
RUN dotnet restore

# Copy everything else and build website
COPY CloudBuildDemo/. ./CloudBuildDemo/
WORKDIR /app/CloudBuildDemo
RUN dotnet publish -c Release -o /Output

# Final stage / image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /Output
COPY --from=build /Output ./
ENTRYPOINT ["dotnet", "CloudBuildDemo.dll"]