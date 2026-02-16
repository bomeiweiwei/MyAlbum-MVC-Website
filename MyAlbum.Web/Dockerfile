# 請參閱 https://aka.ms/customizecontainer 了解如何自訂您的偵錯容器，以及 Visual Studio 如何使用此 Dockerfile 來組建您的映像，以加快偵錯速度。

# 此階段用於以快速模式從 VS 執行時 (偵錯設定的預設值)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1) 先 copy solution 與 csproj（讓 restore 有 cache）
COPY ["MyAlbum.Web/MyAlbum.Web.csproj", "MyAlbum.Web/"]
COPY ["MyAlbum.Application/MyAlbum.Application.csproj", "MyAlbum.Application/"]
COPY ["MyAlbum.Domain/MyAlbum.Domain.csproj", "MyAlbum.Domain/"]
COPY ["MyAlbum.Infrastructure/MyAlbum.Infrastructure.csproj", "MyAlbum.Infrastructure/"]
COPY ["MyAlbum.IoC/MyAlbum.IoC.csproj", "MyAlbum.IoC/"]
COPY ["MyAlbum.Models/MyAlbum.Models.csproj", "MyAlbum.Models/"]
COPY ["MyAlbum.Repository/MyAlbum.Repository.csproj", "MyAlbum.Repository/"]
COPY ["MyAlbum.Shared/MyAlbum.Shared.csproj", "MyAlbum.Shared/"]

RUN dotnet restore "MyAlbum.Web/MyAlbum.Web.csproj"

# 2) 再 copy 全部程式碼
COPY . .
WORKDIR "/src/MyAlbum.Web"
RUN dotnet publish "MyAlbum.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyAlbum.Web.dll"]
