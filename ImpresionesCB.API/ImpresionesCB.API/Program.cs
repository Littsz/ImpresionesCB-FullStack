using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ImpresionesCB.API.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.SetIsOriginAllowed(_ => true) 
              .AllowAnyHeader()             
              .AllowAnyMethod();            
    });
});

var app = builder.Build();




app.UseHttpsRedirection();
app.UseCors("PermitirTodo");



app.UseAuthorization();
app.MapControllers();
app.Run();
