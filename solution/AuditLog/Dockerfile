# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BASE_IMAGE} AS base
WORKDIR /app

FROM ${BUILD_IMAGE} AS build
WORKDIR /src
COPY ["AuditLog/AuditLog.csproj", "AuditLog/"]
RUN dotnet restore "AuditLog/AuditLog.csproj"
COPY . .
WORKDIR "/src/AuditLog"
RUN dotnet build "AuditLog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuditLog.csproj" -c Release -o /app/publish

FROM base AS final
# WORKDIR /app/wwwroot
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuditLog.dll"]