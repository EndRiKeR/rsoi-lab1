# syntax=docker/dockerfile:1

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

COPY . /source
WORKDIR /source

ARG TARGETARCH

# Упрощенная версия без cache mount (совместимая с Railway)
RUN dotnet restore && \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Копируем собранное приложение
COPY --from=build /app .

# Создаем non-root пользователя (лучшая практика для Railway)
RUN adduser -D -s /bin/sh -u 1001 appuser && \
    chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Test.dll"]