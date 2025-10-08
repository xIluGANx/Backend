using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS policies
builder.Services.AddCors(options =>
{
    // Policy 1: Allow everything (for development)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    // Policy 2: Restricted policy with specific origins
    options.AddPolicy("Restricted", policy =>
    {
        policy.WithOrigins("https://localhost:3000", "https://example.com")
              .WithMethods("GET", "POST")
              .WithHeaders("Content-Type", "Authorization")
              .AllowCredentials();
    });

    // Policy 3: Named policy for specific API
    options.AddPolicy("ApiPolicy", policy =>
    {
        policy.WithOrigins("https://api.example.com")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .WithHeaders("Content-Type", "Authorization", "X-API-Key");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Use AllowAll policy in development
    app.UseCors("AllowAll");
}
else
{
    // Use more restrictive policy in production
    app.UseCors("Restricted");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();