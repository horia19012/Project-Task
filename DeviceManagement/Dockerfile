FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file first to maximize restore layer caching.
COPY DeviceManagement.csproj ./
RUN dotnet restore ./DeviceManagement.csproj

# Copy the remaining source and publish the app.
COPY . .
RUN dotnet publish ./DeviceManagement.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DeviceManagement.dll"]