using Discord;
using Discord.Commands;
using Discord.WebSocket;
using KasumiGUI.Utility;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Reflection;

namespace KasumiGUI.Discord
{
    public class CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
    {
        private const string LoggerSrc = "Commands";

        private readonly string? prefixChar = ConfigurationManager.AppSettings["PrefixChar"];

        public async Task InitializeAsync()
        {
            _ = await commands.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(), services: services);
            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            int argPos = 0;
            if (messageParam is not SocketUserMessage message || message.Author.IsBot || (!string.IsNullOrEmpty(prefixChar) && !message.HasCharPrefix(prefixChar[0], ref argPos)))
                return;
            using IServiceScope scope = services.CreateScope();
            IResult result = await commands.ExecuteAsync(context: new SocketCommandContext(client, message), argPos: argPos, services: scope.ServiceProvider);
            if (!result.IsSuccess)
                await Logger.LogAsync(new LogMessage(LogSeverity.Warning, LoggerSrc, $"Command failed: {result.ErrorReason}"));
        }

        public static async Task ReplyAsync(SocketCommandContext context, string message)
        {
            await LogCommandAsync(context);
            await Logger.LogAsync(new LogMessage(LogSeverity.Info, LoggerSrc, $"Reply: {message}"));
            _ = await context.Channel.SendMessageAsync(message);
        }

        private static async Task LogCommandAsync(SocketCommandContext context) => await Logger.LogAsync(new LogMessage(LogSeverity.Info, "User", $"{context.User.Username}#{context.User.Discriminator}({context.User.Id}): {context.Message}"));
    }
}