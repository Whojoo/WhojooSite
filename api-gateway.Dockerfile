# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# restore
COPY ["backend/Directory.Build.props", "."]
COPY ["backend/Directory.Packages.props", "."]
COPY ["backend/src/WhojooSite.Yarp/WhojooSite.Yarp.csproj", "WhojooSite.Yarp/"]
RUN dotnet restore 'WhojooSite.Yarp/WhojooSite.Yarp.csproj'

# build
COPY ["backend/src/WhojooSite.Yarp/", "WhojooSite.Yarp/"]
WORKDIR /src/WhojooSite.Yarp
RUN dotnet build 'WhojooSite.Yarp.csproj' -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish stage
FROM build AS publish
RUN dotnet publish 'WhojooSite.Yarp.csproj' -c $BUILD_CONFIGURATION -o /app/publish

# Stage 3: Run stage
FROM base AS runtime
ENV ASPNETCORE_HTTP_PORTS=81
ENV ASPNETCORE_HTTPS_PORTS=80
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "WhojooSite.Yarp.dll" ]