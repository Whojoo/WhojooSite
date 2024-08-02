# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# restore
COPY ["backend/Directory.Build.props", "."]
COPY ["backend/src/WhojooSite.Common/WhojooSite.Common.csproj", "WhojooSite.Common/"]
COPY ["backend/src/WhojooSite.Recipes/WhojooSite.Recipes.Module/WhojooSite.Recipes.Module.csproj", "WhojooSite.Recipes/WhojooSite.Recipes.Module/"]
COPY ["backend/src/WhojooSite.Bootstrap/WhojooSite.Bootstrap.csproj", "WhojooSite.Bootstrap/"]
RUN dotnet restore 'WhojooSite.Bootstrap/WhojooSite.Bootstrap.csproj'

# build
COPY ["backend/src/WhojooSite.Common/", "WhojooSite.Common/"]
COPY ["backend/src/WhojooSite.Recipes/", "WhojooSite.Recipes/"]
COPY ["backend/src/WhojooSite.Bootstrap/", "WhojooSite.Bootstrap/"]
WORKDIR /src/WhojooSite.Bootstrap
RUN dotnet build 'WhojooSite.Bootstrap.csproj' -c Release -o /app/build

# Stage 2: Publish stage
FROM build AS publish
RUN dotnet publish 'WhojooSite.Bootstrap.csproj' -c Release -o /app/publish

# Stage 3: Run stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "WhojooSite.Bootstrap.dll" ]