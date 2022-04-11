using ChessGameApi.Entities;
using ChessGameApi.Repositories;
using ChessGameApi.Settings;
using ChessGameSketch;
using ChessGameSketch.Validator;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IFigureRepository, MongoDBFiguresRepository>();
builder.Services.AddSingleton<IEnPassantRepository, MongoDBEnPassantRepository>();
builder.Services.AddSingleton<IChessValidator, ChessValidator>();
builder.Services.AddDbContext<FigureContext>(opt => opt.UseInMemoryDatabase("Figures"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
