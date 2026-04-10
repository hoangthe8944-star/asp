# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy toàn bộ code vào container
COPY . .
# Restore các thư viện
RUN dotnet restore
# Build và Publish ra thư mục 'out'
RUN dotnet publish -c Release -o out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# Copy kết quả build từ stage 1 sang stage 2
COPY --from=build /app/out .

# Cấu hình Port cho Render (Render dùng port 8080 mặc định cho Docker)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Thay hoangngocthe_2123110488.dll bằng tên project của bạn
ENTRYPOINT ["dotnet", "hoangngocthe_2123110488.dll"]