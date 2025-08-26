using CoreBot.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
builder.Services.AddSingleton<IMovieHallRepository, InMemoryMovieHallRepository>();
builder.Services.AddSingleton<IScheduleRepository, InMemoryScheduleRepository>();
builder.Services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();

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
