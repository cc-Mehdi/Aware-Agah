FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Agah/Agah.csproj", "Agah/Agah.csproj"]
COPY ["Datalayer/Datalayer.csproj", "Datalayer/Datalayer.csproj"]
COPY ["TestLayer/TestLayer.csproj", "TestLayer/TestLayer.csproj"]
RUN dotnet restore "Agah/Agah.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Agah/Agah.csproj" -c $BUILD_CONFIGURATION -o "/app/build"

FROM build as publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Agah/Agah.csproj" -c $BUILD_CONFIGURATION -o "/app/publish"

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Agah.dll" ]