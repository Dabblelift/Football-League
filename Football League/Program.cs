using System.Reflection;
using System.Text.Json.Serialization;
using Football_League.Data.Contexts;
using Football_League.Middlewares;
using Football_League.Repositories;
using Football_League.Repositories.Interfaces;
using Football_League.Services.MatchServices;
using Football_League.Services.MatchServices.Interfaces;
using Football_League.Services.ResultProcessingServices.Interfaces;
using Football_League.Services.ResultProcessingServices.Processors;
using Football_League.Services.ResultProcessingServices.ScoringStrategies;
using Football_League.Services.TeamServices;
using Football_League.Services.TeamServices.Interfaces;
using Football_League.Shared.APIResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FootballLeagueDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IScoringStrategy, StandardScoringStrategy>();
builder.Services.AddScoped<IResultProcessor, ResultProcessor>();

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Message = e.Value?.Errors.First().ErrorMessage
            });

        var response = string.Join(" /// ", errors.Select(e => $"{e.Field}: {e.Message}"));

        return new BadRequestObjectResult(new BaseAPIResponse(false, response));
    };
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
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
