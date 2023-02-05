FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY . /src
WORKDIR /src
RUN ls
RUN dotnet build "someapi2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "someapi2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "someapi2.dll"]
