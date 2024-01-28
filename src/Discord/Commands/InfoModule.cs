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

    }
}