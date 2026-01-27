using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfRate.Data;
using ProfRate.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===== إضافة الـ Services =====

// 1. إضافة الـ Controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 2. إضافة الـ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. إضافة الـ Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. إضافة الـ Services (Dependency Injection)
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<LecturerService>();
builder.Services.AddScoped<EvaluationService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<StudentSubjectService>();
builder.Services.AddScoped<LecturerSubjectService>();

// 5. إضافة الـ JWT Authentication
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 6. إضافة الـ CORS
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "*" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
             policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else 
        {
             policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        }
    });
});

// 7. إضافة Memory Cache (للأداء)
builder.Services.AddMemoryCache();

// 8. إضافة Rate Limiting (للحماية من الهجمات)
builder.Services.AddRateLimiter(options =>
{
    // General Policy
    options.AddFixedWindowLimiter("general", opt =>
    {
        opt.PermitLimit = 30;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // Strict Login Policy
    options.AddFixedWindowLimiter("login", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(5);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});

var app = builder.Build();

// ===== Database Seeding - إنشاء Admin افتراضي =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // إنشاء أو تحديث Admin افتراضي
    var existingAdmin = context.Admins.FirstOrDefault(a => a.Username == "admin");
    if (existingAdmin == null)
    {
        context.Admins.Add(new ProfRate.Entities.Admin
        {
            Username = "admin",
            Password = "admin123",
            FirstName = "System",
            LastName = "Admin"
        });
        context.SaveChanges();
        Console.WriteLine("✅ تم إنشاء Admin افتراضي: admin / admin123");
    }
    else
    {
        // تحديث البيانات لو ناقصة
        bool updated = false;
        if (existingAdmin.Password != "admin123")
        {
            existingAdmin.Password = "admin123";
            updated = true;
        }
        if (string.IsNullOrEmpty(existingAdmin.FirstName))
        {
            existingAdmin.FirstName = "System";
            updated = true;
        }
        if (string.IsNullOrEmpty(existingAdmin.LastName))
        {
            existingAdmin.LastName = "Admin";
            updated = true;
        }
        if (updated)
        {
            context.SaveChanges();
            Console.WriteLine("✅ تم تحديث Admin: admin / admin123");
        }
    }
}

// ===== Configure the HTTP request pipeline =====
app.UseMiddleware<ProfRate.Middleware.ExceptionMiddleware>();

// Swagger (للتطوير فقط)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Static Files - لتقديم الـ Frontend
app.UseDefaultFiles();
app.UseStaticFiles();

// HTTPS Redirection
 app.UseHttpsRedirection();

// CORS
app.UseCors("AllowSpecificOrigins");

// Rate Limiter
app.UseRateLimiter();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Redirect root to frontend
app.MapGet("/", () => Results.Redirect("/frontend/login.html"));
app.MapGet("/login.html", () => Results.Redirect("/frontend/login.html"));

// تشغيل التطبيق
app.Run();
