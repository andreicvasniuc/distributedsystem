# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE
ARG NODE_IMAGE
# ARG BUILD_CONFIGURATION

FROM ${BASE_IMAGE} AS base
WORKDIR /app
EXPOSE 5050

# ENV BUILD_CONFIGURATION=${BUILD_CONFIGURATION}

FROM ${BUILD_IMAGE} AS build
WORKDIR /src
COPY ["Website/Website.csproj", "Website/"]
RUN dotnet restore "Website/Website.csproj"
COPY . .
WORKDIR "/src/Website"
RUN dotnet build "Website.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Website.csproj" -c Release -o /app/publish

FROM ${NODE_IMAGE} AS node-builder
WORKDIR /node
WORKDIR /
COPY ["Website/ClientApp", "node/"]
WORKDIR /node
RUN npm install --silent
RUN npm run build

FROM base AS final
WORKDIR /app/wwwroot
WORKDIR /app
# RUN mkdir -p /app/wwwroot
COPY --from=publish /app/publish .
COPY --from=node-builder /node/build ./wwwroot
ENTRYPOINT ["dotnet", "Website.dll"]