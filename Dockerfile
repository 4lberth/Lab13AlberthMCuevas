# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Lab13AlberthMCuevas/Lab13AlberthMCuevas.csproj", "Lab13AlberthMCuevas/"]
COPY ["Lab13AlberthMCuevas.Application/Lab13AlberthMCuevas.Application.csproj", "Lab13AlberthMCuevas.Application/"]
COPY ["Lab13AlberthMCuevas.Domain/Lab13AlberthMCuevas.Domain.csproj", "Lab13AlberthMCuevas.Domain/"]
COPY ["Lab13AlberthMCuevas.Infrastructure/Lab13AlberthMCuevas.Infrastructure.csproj", "Lab13AlberthMCuevas.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Lab13AlberthMCuevas/Lab13AlberthMCuevas.csproj"

# Copy all source code
COPY . .

# Build the application
RUN dotnet build "Lab13AlberthMCuevas/Lab13AlberthMCuevas.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Lab13AlberthMCuevas/Lab13AlberthMCuevas.csproj" -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 3000
ENV ASPNETCORE_URLS=http://+:3000

ENTRYPOINT ["dotnet", "Lab13AlberthMCuevas.dll"]