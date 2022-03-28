#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Suzan.API/Suzan.API.csproj", "Suzan.API/"]
COPY ["Suzan.Application/Suzan.Application.csproj", "Suzan.Application/"]
COPY ["Suzan.Domain/Suzan.Domain.csproj", "Suzan.Domain/"]
RUN dotnet restore "Suzan.API/Suzan.API.csproj"
COPY . .
WORKDIR "/src/Suzan.API"
RUN dotnet build "Suzan.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Suzan.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Suzan.API.dll"]