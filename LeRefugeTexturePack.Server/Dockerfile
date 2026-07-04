# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

# Cet index est utilisé lors de l’exécution à partir de VS en mode rapide (par défaut pour la configuration de débogage)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update
RUN apt-get install -y ffmpeg
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Cette phase est utilisée pour générer le projet de service
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS with-node
RUN apt-get update
RUN apt-get -y install curl
RUN curl -sL https://deb.nodesource.com/setup_24.x | bash
RUN apt-get -y install nodejs


FROM with-node AS build
RUN --mount=type=secret,id=SIXLABORS_LICENSE_KEY \
    export SIXLABORS_LICENSE_KEY=$(cat /run/secrets/SIXLABORS_LICENSE_KEY)
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LeRefugeTexturePack.Server/LeRefugeTexturePack.Server.csproj", "LeRefugeTexturePack.Server/"]
COPY ["lerefugetexturepack.client/lerefugetexturepack.client.esproj", "lerefugetexturepack.client/"]
RUN rm -rf LeRefugeTexturePack.Server/TexturePack
RUN dotnet restore "./LeRefugeTexturePack.Server/LeRefugeTexturePack.Server.csproj"
COPY . .
WORKDIR "/src/LeRefugeTexturePack.Server"
RUN --mount=type=secret,id=SIXLABORS_LICENSE_KEY \
    dotnet build "./LeRefugeTexturePack.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build -p:SixLaborsLicenseKey="$(cat /run/secrets/SIXLABORS_LICENSE_KEY)"

# Cette étape permet de publier le projet de service à copier dans la phase finale
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=secret,id=SIXLABORS_LICENSE_KEY \
    dotnet publish "./LeRefugeTexturePack.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish -p:SixLaborsLicenseKey="$(cat /run/secrets/SIXLABORS_LICENSE_KEY)" /p:UseAppHost=false

# Cette phase est utilisée en production ou lors de l’exécution à partir de VS en mode normal (par défaut quand la configuration de débogage n’est pas utilisée)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeRefugeTexturePack.Server.dll"]
