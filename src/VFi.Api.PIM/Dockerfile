#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM harbor.vfico.com/base/dotnet:6.0 AS base
RUN apt-get update && \
	apt-get install --no-install-recommends -y chromium && \
    apt-get autoremove && \
    apt-get clean
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM harbor.vfico.com/base/dotnet-sdk:6.0 AS build
WORKDIR /src
COPY ["src/VFi.Api.PIM/VFi.Api.PIM.csproj", "src/VFi.Api.PIM/"]
COPY ["src/VFi.Application.PIM/VFi.Application.PIM.csproj", "src/VFi.Application.PIM/"]
COPY ["src/VFi.Domain.PIM/VFi.Domain.PIM.csproj", "src/VFi.Domain.PIM/"]
COPY ["src/VFi.NetDevPack/VFi.NetDevPack.csproj", "src/VFi.NetDevPack/"]
COPY ["src/VFi.Infra.PIM/VFi.Infra.PIM.csproj", "src/VFi.Infra.PIM/"]
RUN dotnet restore "src/VFi.Api.PIM/VFi.Api.PIM.csproj"
COPY . .
WORKDIR "/src/src/VFi.Api.PIM"
RUN dotnet build "VFi.Api.PIM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VFi.Api.PIM.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY /src/VFi.Api.PIM/entrypoint.sh .
RUN chmod +x ./entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]

CMD ["dotnet", "VFi.Api.PIM.dll"]