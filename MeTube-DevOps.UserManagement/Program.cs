using Swashbuckle.AspNetCore.SwaggerGen;


var builder = WebApplication.CreateBuilder(args);

// ✅ Lägg till stöd för Controllers
builder.Services.AddControllers();

// ✅ Lägg till OpenAPI/Swagger support (om det behövs)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Konfigurera HTTP-request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// ✅ Lägg till Controllers-mappning så att dina Controllers fungerar
app.MapControllers();

app.Run();


