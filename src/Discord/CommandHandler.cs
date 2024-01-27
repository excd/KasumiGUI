using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace KasumiGUI.Discord {
    internal class CommandHandler {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;

        public CommandHandler(ref DiscordSocketClient client, ref CommandService commands) {
            this.client = client;
            this.commands = commands;
        }

        public async Task InitializeAsync() {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam) {
            if (messageParam is not SocketUserMessage message || message.Author.IsBot)
                return;

            int argPos = 0;
            if (!message.HasCharPrefix('!', ref argPos))
                return;

            SocketCommandContext context = new(client, message);
            await commands.ExecuteAsync(context: context, argPos: argPos, services: null);
        }
    }
}