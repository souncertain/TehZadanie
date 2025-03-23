using BL.Mapping;
using Infrastructure;
using Infrastructure.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(CandleProfile));
builder.Services.AddAutoMapper(typeof(TradeProfile));
builder.Services.AddAutoMapper(typeof(TickerProfile));
builder.Services.AddTransient<BitRestClient>();

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

app.Run();
