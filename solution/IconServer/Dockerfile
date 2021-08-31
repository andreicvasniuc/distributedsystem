# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BASE_IMAGE} AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM ${BUILD_IMAGE} AS build
WORKDIR /src
COPY ["IconServer/IconServer.csproj", "IconServer/"]
RUN dotnet restore "IconServer/IconServer.csproj"
COPY . .
WORKDIR "/src/IconServer"
RUN dotnet build "IconServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IconServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IconServer.dll"]