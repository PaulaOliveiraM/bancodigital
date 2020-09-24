FROM mcr.microsoft.com/dotnet/core/sdk:3.1  AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1  AS build
WORKDIR /src
COPY BancoDigital.sln ./
COPY BancoDigital.Models/*.csproj ./BancoDigital.Models/
COPY BancoDigital.Repository/*.csproj ./BancoDigital.Repository/
COPY BancoDigital.Service/*.csproj ./BancoDigital.Service/
COPY BancoDigital.Tests/*.csproj ./BancoDigital.Tests/
COPY BancoDigital.API/*.csproj ./BancoDigital.API/

RUN dotnet restore "BancoDigital.API/BancoDigital.API.csproj"
COPY . .

WORKDIR /src/BancoDigital.API
RUN dotnet build -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "BancoDigital.API.csproj" -c Release -o /app/publish


# Build da imagem
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish  .
ENTRYPOINT ["dotnet", "BancoDigital.API.dll"]