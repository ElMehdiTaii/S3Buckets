
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bucket/Bucket.csproj", "Bucket/"]
RUN dotnet restore "Bucket/Bucket.csproj"
COPY . .
WORKDIR "/src/Bucket"
RUN dotnet build "Bucket.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bucket.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
ENTRYPOINT ["dotnet", "Bucket.dll"]

