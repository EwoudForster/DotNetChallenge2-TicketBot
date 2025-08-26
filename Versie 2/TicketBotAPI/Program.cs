using TicketBotApi.Data;
using CoreBot.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<BotDbContext>(options =>
	//options.UseSqlite("Data Source=bot.db"));

builder.Services.AddDbContext<BotDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Replace in-memory repositories with database repositories
builder.Services.AddScoped<IMovieRepository, DatabaseMovieRepository>();
builder.Services.AddScoped<IMovieHallRepository, DatabaseMovieHallRepository>();
builder.Services.AddScoped<IScheduleRepository, DatabaseScheduleRepository>();
builder.Services.AddScoped<ITicketRepository, DatabaseTicketRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Urls.Add("https://localhost:5001");
app.Urls.Add("http://localhost:5000");

app.Run();
