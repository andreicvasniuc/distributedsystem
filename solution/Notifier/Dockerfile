# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BASE_IMAGE} AS base
WORKDIR /app

FROM ${BUILD_IMAGE} AS build
WORKDIR /src
COPY ["Notifier/Notifier.csproj", "Notifier/"]
COPY ["Messages/Messages.csproj", "Messages/"]
RUN dotnet restore "Notifier/Notifier.csproj"
COPY . .
WORKDIR "/src/Notifier"
RUN dotnet build "Notifier.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notifier.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifier.dll"]