# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và restore (thay đúng tên file .csproj của bạn)
COPY ["hoangngocthe_2123110488/hoangngocthe_2123110488.csproj", "hoangngocthe_2123110488/"]
RUN dotnet restore "hoangngocthe_2123110488/hoangngocthe_2123110488.csproj"

# Copy toàn bộ code và build
COPY . .
WORKDIR "/src/hoangngocthe_2123110488"
RUN dotnet build "hoangngocthe_2123110488.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "hoangngocthe_2123110488.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Cấu hình Port để Render có thể nhận diện
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "hoangngocthe_2123110488.dll"]