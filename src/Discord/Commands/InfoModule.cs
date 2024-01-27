using Discord;
using Discord.Commands;

namespace KasumiGUI.Discord.Commands {
    public class InfoModule : ModuleBase<SocketCommandContext> {
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task EchoAsync([Remainder][Summary("The text to echo.")] string message) {
            await LogCommandAsync(Context.Message.ToString());
            await ReplyAsync(message);
        }

        private async Task LogCommandAsync(string command) {
            if (Program.Logger != null)
                await Program.Logger.LogAsync(new LogMessage(LogSeverity.Info, "Command",
                    $"User {Context.User.Username}#{Context.User.Discriminator} executed: {command}"));
        }

        private async Task ReplyAsync(string message) {
            if (Program.Logger != null)
                await Program.Logger.LogAsync(new LogMessage(LogSeverity.Info, "Response",
                    $"Bot replied: {message}"));

            await base.ReplyAsync(message);
        }
    }
}