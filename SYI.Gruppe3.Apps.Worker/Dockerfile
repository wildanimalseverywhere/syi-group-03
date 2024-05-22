#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SYI.Gruppe3.Apps.Worker/SYI.Gruppe3.Apps.Worker.csproj", "SYI.Gruppe3.Apps.Worker/"]
RUN dotnet restore "SYI.Gruppe3.Apps.Worker/SYI.Gruppe3.Apps.Worker.csproj"
COPY . .
WORKDIR "/src/SYI.Gruppe3.Apps.Worker"
RUN dotnet build "SYI.Gruppe3.Apps.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SYI.Gruppe3.Apps.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SYI.Gruppe3.Apps.Worker.dll"]