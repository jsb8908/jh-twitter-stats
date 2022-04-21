// need these everywhere
global using jh_twitter_stats_api.Services;
global using jh_twitter_stats_api.Repositories;
global using jh_twitter_stats_api.Models;
global using jh_twitter_stats_api.DTOs;
global using System.Text.Json;

using jh_twitter_stats_api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.Configure<GeneralOptions>(builder.Configuration.GetSection(GeneralOptions.General));
builder.Services.Configure<TwitterAPIOptions>(builder.Configuration.GetSection(TwitterAPIOptions.TwitterAPI));

// 'background services'
builder.Services.AddHostedService<SampleStreamBackgroundService>();
builder.Services.AddHostedService<StatsCalculatorBackgroundService>();

// 'repository services'
builder.Services.AddSingleton<ITweetModelRepository, TweetModelInMemoryRepository>();
builder.Services.AddSingleton<IStatModelRepository, StatModelInMemoryRepository>();

// 'Business Logic services'
builder.Services.AddTransient<IStatsService, StatsService>();
builder.Services.AddTransient<IQueueService, QueueService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
