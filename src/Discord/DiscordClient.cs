using Discord;
using Discord.Commands;
using Discord.WebSocket;
using KasumiGUI.Data.Models;
using KasumiGUI.Data.Services;
using KasumiGUI.Utility;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace KasumiGUI.Discord
{
    internal class DiscordClient
    {
        private const string LoggerSrc = "Client";

        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly IServiceProvider services;
        private readonly string? token;

        public DiscordClient()
        {
            services = new ServiceCollection()
                .AddSingleton(new DiscordSocketConfig()
                {
                    LogLevel = Logger.GetLogLevel(),
                    MessageCacheSize = 1000,
                    AlwaysDownloadUsers = true,
                    GatewayIntents = GatewayIntents.Guilds |
                        GatewayIntents.GuildMembers |
                        GatewayIntents.GuildMessages |
                        GatewayIntents.DirectMessages |
                        GatewayIntents.MessageContent
                })
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddDbContext<DBContext>()
                .AddScoped<DBInitializer>()
                .AddScoped<DataProvider>()
                .BuildServiceProvider();
            client = services.GetRequiredService<DiscordSocketClient>();
            commands = services.GetRequiredService<CommandService>();
            token = ConfigurationManager.AppSettings["Token"];
            Initialize();
        }

        private void Initialize()
        {
            client.Log += Logger.LogAsync;
            client.Connected += Connected;
            client.Disconnected += Disconnected;
            commands.Log += Logger.LogAsync;
        }

        private async Task Connected() =>
            await Task.Run(() => Window.ActiveWindow?.Invoke(new Action(() => Window.UpdateStatus("Connected"))));

        private async Task Disconnected(Exception? exception) =>
            await Task.Run(() => Window.ActiveWindow?.Invoke(new Action(() => Window.UpdateStatus("Disconnected"))));

        public async Task Start()
        {
            if (client.ConnectionState == ConnectionState.Disconnected)
            {
                using (IServiceScope scope = services.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<DBInitializer>().InitializeAsync();
                    await scope.ServiceProvider.GetRequiredService<CommandHandler>().InitializeAsync();
                }
                if (!string.IsNullOrEmpty(token))
                {
                    await client.LoginAsync(TokenType.Bot, token);
                    await client.StartAsync();
                }
                else
                {
                    await Logger.LogAsync(new LogMessage(LogSeverity.Error, LoggerSrc, "Token is null! Check application config."));
                }
            }
        }

        public async Task Stop()
        {
            if (client.ConnectionState == ConnectionState.Connected)
            {
                await client.LogoutAsync();
                await client.StopAsync();
            }
        }

        public async Task Restart()
        {
            if (client.ConnectionState == ConnectionState.Connected)
            {
                await Stop();
                await Task.Delay(1000);
                await Start();
            }
        }
    }
}