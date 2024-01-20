using Discord;
using Discord.Commands;
using Discord.WebSocket;
using KasumiGUI.utility;
using System.Configuration;

namespace KasumiGUI.discord {
    internal class DiscordClient {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly Logger logger;
        private readonly string? token;

        public DiscordClient(ref Window window) {
            this.client = new();
            this.commands = new();
            this.logger = new(ref window);
            this.token = ConfigurationManager.AppSettings["token"];
            Initialize();
        }

        private void Initialize() {
            client.Log += logger.Log;
            commands.Log += logger.Log;
        }

        public async Task Start() {
            if (!string.IsNullOrEmpty(this.token)) {
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
            }
            else {
                await logger.Log(new LogMessage(LogSeverity.Error, "Config", "Token is null! Check application config."));
            }
        }

        public async Task Stop() {
            await client.LogoutAsync();
            await client.StopAsync();
        }
    }
}