FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5224

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Lab_7/Lab_7.csproj", "Lab_7/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "Lab_7/Lab_7.csproj"
COPY . .

WORKDIR "/src/Lab_7"
RUN dotnet build "Lab_7.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Lab_7.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lab_7.dll"]
