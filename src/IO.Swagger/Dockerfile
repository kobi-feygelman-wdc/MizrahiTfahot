FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

RUN CD
# copy csproj and restore as distinct layers
COPY /*.csproj ./
RUN dotnet restore

# copy everything else and build
#COPY ./src/IO.Swagger ./
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "IO.Swagger.dll"]
