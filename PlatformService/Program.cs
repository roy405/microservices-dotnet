using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
var devenv = builder.Environment.IsProduction();


//Add services to the container.
if (devenv)
{
    
    Console.WriteLine("--> In Production Environment: Using SQLServer DataBase");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else
{
    Console.WriteLine("--> In Development Enviroment: Using In-Memory Database");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}

//This is the added service for DbContext to connect to the database
//This is to supply the correct repository, in the case of enquiry for the PlatformRepo interface,
//the actual platform repo implementation class is supplied
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
//Adding the Http Client Factory that will be used by the Http Command Client
builder.Services.AddHttpClient<ICommandClient, HttpCommandClient>();
builder.Services.AddControllers();
//Adding the automapper to the applicationdomain in order to map DTOs to Models and Repositories by creating profiles.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

PrepDb.PrepPopulation(app, devenv);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


