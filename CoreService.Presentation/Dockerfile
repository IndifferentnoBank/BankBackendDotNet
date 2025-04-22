FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoreService.Presentation/CoreService.Presentation.csproj", "CoreService.Presentation/"]
COPY ["CoreService.Persistence/CoreService.Persistence.csproj", "CoreService.Persistence/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["CoreService.Domain/CoreService.Domain.csproj", "CoreService.Domain/"]
COPY ["CoreService.Contracts/CoreService.Contracts.csproj", "CoreService.Contracts/"]
COPY ["CoreService.Infrastructure/CoreService.Infrastructure.csproj", "CoreService.Infrastructure/"]
COPY ["CoreService.Application/CoreService.Application.csproj", "CoreService.Application/"]
COPY ["CoreService.Kafka/CoreService.Kafka.csproj", "CoreService.Kafka/"]
RUN dotnet restore "CoreService.Presentation/CoreService.Presentation.csproj"
COPY . .
WORKDIR "/src/CoreService.Presentation"
RUN dotnet build "CoreService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoreService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreService.Presentation.dll"]
