# Build EvoSC#
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /source
COPY . .

RUN dotnet publish "src/EvoSC/EvoSC.csproj" -r linux-musl-x64 --self-contained true -c Release -o /publish

# Set user & permissions
FROM alpine:latest as run-chown

WORKDIR /app
COPY --from=build /publish .
RUN true \
    && chown 9999:9999 -R /app \
    && true

# Create the image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine3.20 as create-image

ARG VERSION \
    BUILD_DATE \
    REVISION

# Disable invariant mode which is enabled on alpine to make localization work
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

LABEL org.opencontainers.image.title="EvoSC#" \
      org.opencontainers.image.description="Next-generation server controller for Trackmania." \
      org.opencontainers.image.authors="Evo" \
      org.opencontainers.image.vendor="Evo eSports e.V." \
      org.opencontainers.image.licenses="GPL-3.0 License " \
      org.opencontainers.image.version=${VERSION} \
      org.opencontainers.image.created=${BUILD_DATE} \
      org.opencontainers.image.revision=${REVISION}

WORKDIR /app
COPY --from=run-chown /app .

RUN true \
    && apk add --no-cache icu-libs \
    && adduser --disabled-password --home /app -u 9999 evosc \
    && true \

USER evosc
ENTRYPOINT ["./EvoSC", "run"]
