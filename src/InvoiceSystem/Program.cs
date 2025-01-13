using InvoiceSystem.Services;
using Microsoft.EntityFrameworkCore;
using InvoiceSystem.Services.Interfaces;
using InvoicesSystem;
using InvoiceSystem.Api.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseAllOfToExtendReferenceSchemas();
    c.ConfigureForDateOnly();
});


// Register the InvoiceService
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Register AutoMapper and mapping profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var connectionString = builder.Configuration.GetConnectionString("InvoiceConnectionString");
builder.Services.AddBusinessDbContext(connectionString);
builder.Services.RegisterRepositories();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
