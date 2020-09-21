FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Users/API/API.csproj", "Users/API/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/Shared/"]
COPY ["Users/Domain/Domain.csproj", "Users/Domain/"]
COPY ["Users/Infrastructure/Infrastructure.csproj", "Users/Infrastructure/"]
RUN dotnet restore "Users/API/API.csproj"
COPY . .
WORKDIR "/src/Users/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]