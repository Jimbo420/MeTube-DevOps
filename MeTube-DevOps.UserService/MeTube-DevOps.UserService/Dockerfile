FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5218

ENV ASPNETCORE_URLS=http://+:5218

FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["MeTube-DevOps.UserService/MeTube-DevOps.UserService/MeTube-DevOps.UserService.csproj", "MeTube-DevOps.UserService/MeTube-DevOps.UserService/"]
RUN dotnet restore "MeTube-DevOps.UserService\MeTube-DevOps.UserService\MeTube-DevOps.UserService.csproj"
COPY . .
WORKDIR "/src/MeTube-DevOps.UserService/MeTube-DevOps.UserService"
RUN dotnet build "MeTube-DevOps.UserService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "MeTube-DevOps.UserService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeTube-DevOps.UserService.dll"]
