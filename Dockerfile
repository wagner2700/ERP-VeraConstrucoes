FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY VeraConstrucoes/VeraConstrucoes.sln VeraConstrucoes/
COPY VeraConstrucoes/VeraConstrucoes.API/VeraConstrucoes.API.csproj VeraConstrucoes/VeraConstrucoes.API/
COPY VeraConstrucoes/VeraConstrucoes.Application/VeraConstrucoes.Application/VeraConstrucoes.Application.csproj VeraConstrucoes/VeraConstrucoes.Application/VeraConstrucoes.Application/
COPY VeraConstrucoes/VeraConstrucoes.Communication/VeraConstrucoes.Communication/VeraConstrucoes.Communication.csproj VeraConstrucoes/VeraConstrucoes.Communication/VeraConstrucoes.Communication/
COPY VeraConstrucoes/VeraConstrucoes.Domain/VeraConstrucoes.Domain/VeraConstrucoes.Domain.csproj VeraConstrucoes/VeraConstrucoes.Domain/VeraConstrucoes.Domain/
COPY VeraConstrucoes/VeraConstrucoes.Exception/VeraConstrucoes.Exception/VeraConstrucoes.Exception.csproj VeraConstrucoes/VeraConstrucoes.Exception/VeraConstrucoes.Exception/
COPY VeraConstrucoes/VeraConstrucoes.Infrastructure/VeraConstrucoes.Infrastructure/VeraConstrucoes.Infrastructure.csproj VeraConstrucoes/VeraConstrucoes.Infrastructure/VeraConstrucoes.Infrastructure/

RUN dotnet restore VeraConstrucoes/VeraConstrucoes.API/VeraConstrucoes.API.csproj

COPY VeraConstrucoes/ VeraConstrucoes/

WORKDIR /src/VeraConstrucoes/VeraConstrucoes.API
RUN dotnet publish VeraConstrucoes.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

ENV ENABLE_ELECTRON=false
ENV NFC_XML_DIR=/data/XMLs_Console

RUN mkdir -p /data/XMLs_Console

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080} dotnet VeraConstrucoes.API.dll"]
