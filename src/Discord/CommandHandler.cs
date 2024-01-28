using Discord;
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

        public static async Task ReplyAsync(SocketCommandContext context, string message) {
            if (Program.Logger != null) {
                await LogCommandAsync(context);
                await Program.Logger.LogAsync(new LogMessage(LogSeverity.Info, "Response",
                    $"Bot replied: {message}"));
            }
            await context.Channel.SendMessageAsync(message);
        }

        private static async Task LogCommandAsync(SocketCommandContext context) {
            if (Program.Logger != null)
                await Program.Logger.LogAsync(new LogMessage(LogSeverity.Info, "Command",
                    $"User {context.User.Username}#{context.User.Discriminator} executed: {context.Message}"));
        }
    }
}