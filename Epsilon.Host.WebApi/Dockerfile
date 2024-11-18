FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Epsilon.Host.WebApi/Epsilon.Host.WebApi.csproj", "Epsilon.Host.WebApi/"]
RUN dotnet restore "Epsilon.Host.WebApi/Epsilon.Host.WebApi.csproj"
COPY . .
WORKDIR "/src/Epsilon.Host.WebApi"

RUN dotnet build "Epsilon.Host.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Epsilon.Host.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Epsilon.Host.WebApi.dll"]
