using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization; // Thêm thư viện này
using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.Hubs;
using hoangngocthe_2123110488.Repository;
using hoangngocthe_2123110488.Service;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ── 1. CẤU HÌNH JSON (Fix lỗi Swagger 500 do vòng lặp dữ liệu) ─────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// ── 2. DATABASE ──────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── 3. JWT AUTH ──────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"] ?? "Key_Mac_Dinh_Sieu_Bao_Mat_123456";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ── 4. DEPENDENCY INJECTION (Gộp lại cho gọn) ─────────────────
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSignalR();

// Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStreamRepository, StreamRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IDonationRepository, DonationRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IBanRepository, BanRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStreamService, StreamService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IBanService, BanService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStreamCategoryService, StreamCategoryService>();

// ── 5. CORS (Gộp thành 1 chính sách duy nhất) ────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Cho phép tất cả để test deploy cho dễ
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ── 6. SWAGGER CONFIG ────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Streaming API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ── 7. DATABASE INITIALIZER ──────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // db.Database.Migrate(); // Mở dòng này nếu muốn tự động chạy Migration
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Lỗi DB: " + ex.Message);
    }
}

// ── 8. MIDDLEWARE PIPELINE ───────────────────────────────────

// Swagger luôn chạy để xem được trên Render
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Streaming API V1");
    c.RoutePrefix = string.Empty; // Swagger nằm ở trang chủ
});

app.UseCors("AllowAll");

// Tắt cái này khi chạy trên Render để tránh lỗi vòng lặp Redirect
// app.UseHttpsRedirection(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();