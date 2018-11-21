#-------------------------------------------------------
# Base
#--------------------------------------------------------
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Local \
    ASPNETCORE_URLS=http://+:80

EXPOSE 80

#-------------------------------------------------------
# Dependences
#--------------------------------------------------------
FROM microsoft/dotnet:2.1-sdk AS dependences

WORKDIR /source

COPY Delivery.Authentication.sln .
COPY src/01-Presentation/Delivery.Authentication.Api/Delivery.Authentication.Api.csproj src/01-Presentation/Delivery.Authentication.Api/
COPY src/04-Domain/Delivery.Authentication.Domain.Model/Delivery.Authentication.Domain.Model.csproj src/04-Domain/Delivery.Authentication.Domain.Model/
COPY src/03-Infrastructure/Delivery.Authentication.Infrastructure.Data/Delivery.Authentication.Infrastructure.Data.csproj src/03-Infrastructure/Delivery.Authentication.Infrastructure.Data/
COPY src/02-Application/Delivery.Authentication.Application.Query/Delivery.Authentication.Application.Query.csproj src/02-Application/Delivery.Authentication.Application.Query/
COPY src/02-Application/Delivery.Authentication.Application.Command/Delivery.Authentication.Application.Command.csproj src/02-Application/Delivery.Authentication.Application.Command/
COPY src/03-Infrastructure/Delivery.Authentication.Infrastructure.Cassandra/Delivery.Authentication.Infrastructure.Cassandra.csproj src/03-Infrastructure/Delivery.Authentication.Infrastructure.Cassandra/
COPY src/99-Crosscutting/Delivery.Authentication.Crosscutting/Delivery.Authentication.Crosscutting.csproj src/99-Crosscutting/Delivery.Authentication.Crosscutting/

#Test's project
COPY test/Delivery.Authentication.Application.Command.Tests/Delivery.Authentication.Application.Command.Tests.csproj test/Delivery.Authentication.Application.Command.Tests/
COPY test/Delivery.Authentication.Application.Query.Tests/Delivery.Authentication.Application.Query.Tests.csproj test/Delivery.Authentication.Application.Query.Tests/
COPY test/Delivery.Authentication.Performance.Tests/Delivery.Authentication.Performance.Tests.csproj test/Delivery.Authentication.Performance.Tests/

RUN dotnet restore

#-------------------------------------------------------
# Build
#--------------------------------------------------------
FROM dependences AS build

WORKDIR /source

COPY . .

RUN dotnet build -c Release -o /app

#-------------------------------------------------------
# Test
#--------------------------------------------------------
FROM build AS test

WORKDIR /source/test

RUN dotnet test --no-restore

#-------------------------------------------------------
# Test Performance
#--------------------------------------------------------
FROM build AS test-performance

WORKDIR /source/test/Delivery.Authentication.Performance.Tests

RUN dotnet run -c Release --no-restore -- path=/performance

#-------------------------------------------------------
# Publish
#--------------------------------------------------------
FROM build AS publish

WORKDIR /source

RUN dotnet publish src/01-Presentation/Delivery.Authentication.Api --no-restore -c Release -o /app 
#/p:Version=$VERSION

#-------------------------------------------------------
# Main
#--------------------------------------------------------
FROM base AS main

WORKDIR /app

COPY --from=publish /app .

ENTRYPOINT ["dotnet", "Delivery.Authentication.Api.dll"]
