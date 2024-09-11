FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MessageService.Api.csproj", "MessageService.Api/"]
RUN dotnet restore "./MessageService.Api/MessageService.Api.csproj"
WORKDIR "/src/MessageService.Api"
COPY . .
RUN dotnet build "./MessageService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./MessageService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/MessageService.Api/wwwroot /app/wwwroot
ENTRYPOINT ["dotnet", "MessageService.Api.dll"]
