FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

EXPOSE 5124 7050

ENV ASPNETCORE_URLS="http://+:5124;https://+:7050"

CMD ["dotnet", "User-WBLK.dll"]

# Cach chay:
# docker build -t user-wblk .
# docker run --name user-wblk -dp 5124:5124 -dp 7050:7050 user-wblk