#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Presentation/WebApi/WebApi.csproj", "Presentation/WebApi/"]
COPY ["Infrastructure/Utilities/Utilities.csproj", "Infrastructure/Utilities/"]
COPY ["Core/Application/Application.csproj", "Core/Application/"]
COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Core/Helpers/Helpers.csproj", "Core/Helpers/"]
COPY ["Infrastructure/Persistence/Persistence.csproj", "Infrastructure/Persistence/"]
RUN dotnet restore "Presentation/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/Presentation/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install -y libgdiplus wget libfontenc1 x11-common xfonts-encodings fontconfig xfonts-75dpi xfonts-base xfonts-utils lsb-base 

WORKDIR /app
RUN wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox_0.12.6-1.buster_amd64.deb && dpkg -i wkhtmltox_0.12.6-1.buster_amd64.deb

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]