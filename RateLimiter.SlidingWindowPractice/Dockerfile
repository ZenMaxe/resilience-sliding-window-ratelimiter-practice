FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RateLimiter.SlidingWindowPractice/RateLimiter.SlidingWindowPractice.csproj", "RateLimiter.SlidingWindowPractice/"]
RUN dotnet restore "RateLimiter.SlidingWindowPractice/RateLimiter.SlidingWindowPractice.csproj"
COPY . .
WORKDIR "/src/RateLimiter.SlidingWindowPractice"
RUN dotnet build "RateLimiter.SlidingWindowPractice.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RateLimiter.SlidingWindowPractice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RateLimiter.SlidingWindowPractice.dll"]