# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BASE_IMAGE} AS base
WORKDIR /app

FROM ${BUILD_IMAGE} AS build
WORKDIR /src
COPY ["IconClient/IconClient.csproj", "IconClient/"]
COPY ["Messages/Messages.csproj", "Messages/"]
RUN dotnet restore "IconClient/IconClient.csproj"
COPY . .
WORKDIR "/src/IconClient"
RUN dotnet build "IconClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IconClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IconClient.dll"]