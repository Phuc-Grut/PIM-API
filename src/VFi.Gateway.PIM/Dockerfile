#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/VFi.Gateway.PIM/VFi.Gateway.PIM.csproj", "src/VFi.Gateway.PIM/"]
COPY ["src/VFi.NetDevPack/VFi.NetDevPack.csproj", "src/VFi.NetDevPack/"]
RUN dotnet restore "src/VFi.Gateway.PIM/VFi.Gateway.PIM.csproj"
COPY . .
WORKDIR "/src/src/VFi.Gateway.PIM"
RUN dotnet build "VFi.Gateway.PIM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VFi.Gateway.PIM.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VFi.Gateway.PIM.dll"]