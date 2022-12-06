FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestAppK8S/TestAppK8S.csproj", "TestAppK8S/"]
RUN dotnet restore "TestAppK8S/TestAppK8S.csproj"
COPY . .
WORKDIR "/src/TestAppK8S"
RUN dotnet build "TestAppK8S.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAppK8S.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestAppK8S.dll"]