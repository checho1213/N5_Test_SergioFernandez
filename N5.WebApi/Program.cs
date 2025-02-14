using Microsoft.EntityFrameworkCore;
using N5.Infraestructure.context;
using N5.Infraestructure.Interfaces;
using N5.Infraestructure.Repositories;
using MediatR;
using System.Reflection;
using N5.Infraestructure.Settings;
using N5.Domain.Interfaces;
using N5.Domain.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<InfraestructureSettings>(builder.Configuration);
builder.Services.AddDbContext<N5Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("n5DataBase"));
});
builder.Services.AddTransient<IUnitofWork, UnitOfWork>();
builder.Services.AddTransient<IKafkaRepository,KafkaRepository>();
builder.Services.AddTransient<IPermissionDomainServices,PermissionDomainServices>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
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
