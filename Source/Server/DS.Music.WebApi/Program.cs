using System.Reflection;
using AutoMapper;
using DS.Application.CQRS.Mapping;
using DS.DataAccess;
using DS.DataAccess.ContentStorages;
using DS.DataAccess.Context;
using DS.Music.WebApi.Middlewares;
using MediatR;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
// Not sure if it is needed, just in case, as we inject service into mapper we might should have this
builder.Services.AddSingleton<IContentStorage, FileSystemStorage>();
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new DomainToResponse(provider.GetService<IContentStorage>()));
}).CreateMapper());

builder.Services.AddDbContext<MusicDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteTest"));
});

builder.Services.AddScoped<IMusicContext, MusicDbContext>();

var storage =
    new FileSystemStorage(builder.Configuration
        .GetSection("StorageDirectories")
        .GetValue<string>("RelativeTestDirectory"));

builder.Services.AddScoped<IContentStorage>(_ => storage);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();