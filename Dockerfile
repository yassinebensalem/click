#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Staging
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Src/MipLivrelStore/MipLivrelStore.csproj", "Src/MipLivrelStore/"]
COPY ["Src/DDD.Application/DDD.Application.csproj", "Src/DDD.Application/"]
COPY ["Src/DDD.Domain/DDD.Domain.csproj", "Src/DDD.Domain/"]
COPY ["Src/DDD.Domain.Core/DDD.Domain.Core.csproj", "Src/DDD.Domain.Core/"]
COPY ["Src/DDD.Infra.CrossCutting.Bus/DDD.Infra.CrossCutting.Bus.csproj", "Src/DDD.Infra.CrossCutting.Bus/"]
COPY ["Src/DDD.Infra.Data/DDD.Infra.Data.csproj", "Src/DDD.Infra.Data/"]
COPY ["Src/DDD.Infra.CrossCutting.Identity/DDD.Infra.CrossCutting.Identity.csproj", "Src/DDD.Infra.CrossCutting.Identity/"]
COPY ["Src/DDD.Infra.CrossCutting.IoC/DDD.Infra.CrossCutting.IoC.csproj", "Src/DDD.Infra.CrossCutting.IoC/"]

RUN dotnet restore "Src/MipLivrelStore/MipLivrelStore.csproj"
COPY . .
WORKDIR "/src/Src/MipLivrelStore"

RUN dotnet build "MipLivrelStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MipLivrelStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MipLivrelStore.dll"]
