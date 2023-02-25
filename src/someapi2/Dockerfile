FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY . /src
WORKDIR /src
RUN dotnet build "someapi2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "someapi2.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
# RUN addgroup -S -g 1001 someapi2 && \
#     adduser -S -u 1001 -g someapi2 someapi2
#USER 1001:1001
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "someapi2.dll"]
