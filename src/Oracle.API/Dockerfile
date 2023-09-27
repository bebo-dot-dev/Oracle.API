FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy csproj files and restore
COPY src/Oracle.API/*.csproj src/Oracle.API/
COPY src/Oracle.Core/*.csproj src/Oracle.Core/
COPY src/Oracle.Database/*.csproj src/Oracle.Database/
RUN dotnet restore "src/Oracle.API/Oracle.API.csproj"

# copy everything else and build app
COPY src/. src/
WORKDIR /src/src/Oracle.API
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM base AS final
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "Oracle.API.dll"]