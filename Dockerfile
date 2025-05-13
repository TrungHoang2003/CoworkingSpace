FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebAPi.CoworkingSpace/WebAPi.CoworkingSpace.csproj", "WebAPi.CoworkingSpace/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Application/Application.csproj", "Application/"]
RUN dotnet restore "WebAPi.CoworkingSpace/WebAPi.CoworkingSpace.csproj"
COPY . .
WORKDIR "/src/WebAPi.CoworkingSpace"
RUN dotnet build "WebAPi.CoworkingSpace.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef

RUN dotnet publish "WebAPi.CoworkingSpace.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Chạy migration trước khi khởi động ứng dụng
RUN until dotnet ef database update --project ../Infrastructure --startup-project . --no-build --no-restore; do echo "Migration failed, retrying in 2 seconds..."; sleep 2; done || true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPi.CoworkingSpace.dll"]