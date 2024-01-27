﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Configuration;

namespace KasumiGUI.Discord {
    internal class DiscordClient {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly CommandHandler commandHandler;
        private readonly string? token;

        private readonly DiscordSocketConfig config = new() {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 1000,
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages |
                GatewayIntents.DirectMessages | GatewayIntents.MessageContent
        };

        public DiscordClient() {
            this.client = new(config);
            this.commands = new();
            this.commandHandler = new(ref client, ref commands);
            this.token = ConfigurationManager.AppSettings["Token"];
            Initialize();
        }

        private async void Initialize() {
            if (Program.Logger != null) {
                client.Log += Program.Logger.LogAsync;
                commands.Log += Program.Logger.LogAsync;
            }
            await commandHandler.InitializeAsync();
        }

        public async void Start() {
            if (client.ConnectionState == ConnectionState.Disconnected) {
                if (!string.IsNullOrEmpty(token)) {
                    await client.LoginAsync(TokenType.Bot, token);
                    await client.StartAsync();
                }
                else {
                    if (Program.Logger != null)
                        await Program.Logger.LogAsync(new LogMessage(LogSeverity.Error, "Config",
                            "Token is null! Check application config."));
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