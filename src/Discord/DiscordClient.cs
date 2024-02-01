using Discord;
using Discord.Commands;
using Discord.WebSocket;
using KasumiGUI.Utility;
using System.Configuration;

namespace KasumiGUI.Discord {
    internal class DiscordClient {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly CommandHandler commandHandler;
        private readonly string? token;

        public DiscordClient() {
            this.client = new(new DiscordSocketConfig() {
                LogLevel = Logger.GetLogLevel(),
                MessageCacheSize = 1000,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages |
                GatewayIntents.DirectMessages | GatewayIntents.MessageContent
            });
            this.commands = new();
            this.commandHandler = new(ref client, ref commands);
            this.token = ConfigurationManager.AppSettings["Token"];
            Initialize();
        }

        private async void Initialize() {
            client.Log += Logger.LogAsync;
            client.Connected += Connected;
            client.Disconnected += Disconnected;
            commands.Log += Logger.LogAsync;
            await commandHandler.InitializeAsync();
        }

        private async Task Connected() =>
            await Task.Run(() => Window.ActiveWindow?.Invoke(new Action(() => Window.UpdateStatus("Connected"))));

        private async Task Disconnected(Exception? exception) =>
            await Task.Run(() => Window.ActiveWindow?.Invoke(new Action(() => Window.UpdateStatus("Disconnected"))));

        public async void Start() {
            if (client.ConnectionState == ConnectionState.Disconnected) {
                if (!string.IsNullOrEmpty(token)) {
                    await client.LoginAsync(TokenType.Bot, token);
                    await client.StartAsync();
                }
                else {
                    await Logger.LogAsync(new LogMessage(LogSeverity.Error, "Config", "Token is null! Check application config."));
                }
            }
        }

        public async void Stop() {
            if (client.ConnectionState == ConnectionState.Connected) {
                await client.LogoutAsync();
                await client.StopAsync();
            }
        }

        public async void Restart() {
            if (client.ConnectionState == ConnectionState.Connected) {
                Stop();
                await Task.Delay(1000);
                Start();
            }
        }
    }
}