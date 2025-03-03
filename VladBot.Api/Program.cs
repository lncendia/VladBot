using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using VladBot.BLL.Services;
using VladBot.Core.Configuration;
using VladBot.Core.Repositories;
using VladBot.Core.Services;
using VladBot.DAL.Data;
using VladBot.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 27)),
        optionsBuilder => optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});
builder.Services.AddScoped<IUpdateHandler<Update>, UpdateHandler>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChannelService, ChannelService>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddOptions<Configuration>().Bind(builder.Configuration.GetSection("Links"))
    .ValidateDataAnnotations();
builder.Services.AddScoped(sp => sp.GetService<IOptions<Configuration>>()!.Value);

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient
        => new TelegramBotClient(builder.Configuration["BotConfiguration:Token"], httpClient));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("tgwebhook",
        builder.Configuration["BotConfiguration:Token"],
        new {controller = "Bot", action = "Post"});
    endpoints.MapControllers();
});

app.Run();