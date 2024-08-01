# Base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the source code and build the application
COPY . .
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release -o out --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Expose the port
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "WhojooSite.Bootstrap.dll"]