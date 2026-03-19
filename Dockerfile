# Stage 1: Build Vue SPA
FROM node:20-alpine AS vuebuild
WORKDIR /app

COPY frontend/package*.json ./
RUN npm ci

COPY frontend/ .
ARG VITE_MAPBOX_TOKEN
ARG VITE_LANDING_VIDEO_URL
ENV VITE_MAPBOX_TOKEN=${VITE_MAPBOX_TOKEN}
ENV VITE_LANDING_VIDEO_URL=${VITE_LANDING_VIDEO_URL}
RUN npm run build

# Stage 2: Build .NET API
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY EcoMapGbg/EcoMapGbg.csproj ./EcoMapGbg/
RUN dotnet restore

COPY EcoMapGbg/ ./EcoMapGbg/
RUN dotnet publish EcoMapGbg/EcoMapGbg.csproj -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS production
WORKDIR /app

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .
COPY --from=vuebuild /app/dist/. ./wwwroot/

ENTRYPOINT ["dotnet", "EcoMapGbg.dll"]
