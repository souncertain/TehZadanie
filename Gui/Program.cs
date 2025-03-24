using BL;
using BL.Mapping;
using Core.Interfaces;
using Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(CandleProfile));
builder.Services.AddAutoMapper(typeof(TradeProfile));
builder.Services.AddAutoMapper(typeof(TickerProfile));
builder.Services.AddTransient<BitRestClient>();

builder.Services.AddScoped<IBitRestClient, BitRestClient>();
builder.Services.AddScoped<BagCalc>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
