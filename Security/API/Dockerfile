FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Generate a private RSA key for the tokens.
RUN mkdir keys
ARG rsa_file_path=keys/rsa-private.pem
RUN openssl genrsa -out $rsa_file_path 2048
ENV RSA_KEY_FILE=$rsa_file_path

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Security/API/API.csproj", "Security/API/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/Shared/"]
COPY ["Security/Infrastructure/Infrastructure.csproj", "Security/Infrastructure/"]
RUN dotnet restore "Security/API/API.csproj"
COPY . .
WORKDIR "/src/Security/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=base $rsa_file_path $rsa_file_path
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]