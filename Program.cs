var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add<ErrorExtension>(); // Register globally
//    options.Filters.Add<CustomAuthenticationFilter>();

//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new { ErrorMessage = "An unexpected error occurred." };
        await context.Response.WriteAsJsonAsync(response);
    });
});

// Enable routing and controllers
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
//app.UseHttpsRedirection();
// Enable Swagger (if needed)
app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
