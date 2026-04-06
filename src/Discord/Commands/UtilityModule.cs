using Discord;
using Discord.Commands;
using System.Reflection;
using System.Text;

namespace KasumiGUI.Discord.Commands
{
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("coin")]
        [Summary("Have Kasumi flip a coin.")]
        public async Task CoinAsync() => await CommandHandler.ReplyAsync(Context, $"It's **{(new Random().Next(2) == 0 ? "heads" : "tails")}**.");

        [Command("roll")]
        [Summary("Have Kasumi roll a dice.")]
        public async Task RollAsync([Summary("The number of sides on the dice.")] int sides = 6) => await CommandHandler.ReplyAsync(Context, $"Rolling a {sides}-sided dice...\nYou rolled **{new Random().Next(1, sides + 1)}**.");

        [Command("rate")]
        [Summary("Have Kasumi rate something out of 10.")]
        public async Task RateAsync([Remainder][Summary("The thing to rate.")] string? message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await CommandHandler.ReplyAsync(Context, "You need to give me something to rate you dumb fuck.");
                return;
            }
            await CommandHandler.ReplyAsync(Context, $"I give {message} a {new Random().Next(11)}/10.");
        }

        [Command("reverse")]
        [Summary("Have Kasumi reverse the text.")]
        public async Task ReverseAsync([Remainder][Summary("The text to reverse.")] string text) => await CommandHandler.ReplyAsync(Context, new string([.. text.Reverse()]));

        [Command("owo")]
        [Summary("Have Kasumi OwOify the text.")]
        public async Task OwoAsync([Remainder][Summary("The text to OwOify.")] string text) => await CommandHandler.ReplyAsync(Context, text.Replace("r", "w").Replace("l", "w").Replace("R", "W").Replace("L", "W").Replace("th", "f").Replace("Th", "F").Replace("TH", "F"));

        [Command("uwu")]
        [Summary("Have Kasumi UwUify the text.")]
        public async Task UwuAsync([Remainder][Summary("The text to UwUify.")] string text) => await OwoAsync(text);

        [Command("avatar")]
        [Summary("Have Kasumi display your avatar or a specified user's avatar.")]
        public async Task AvatarAsync([Summary("The (optional) user to get the avatar of.")] IUser? user = null)
        {
            IUser targetUser = user ?? Context.User;
            await CommandHandler.ReplyAsync(Context, $"{targetUser.Username}'s shitty avatar.\n{targetUser.GetAvatarUrl() ?? targetUser.GetDefaultAvatarUrl()}");
        }

        [Command("whois")]
        [Summary("Have Kasumi display info about you or a specified user.")]
        public async Task WhoIsAsync([Summary("The (optional) user to get info from.")] IUser? user = null)
        {
            IUser targetUser = user ?? Context.User;
            await CommandHandler.ReplyAsync(Context, "User Information:```" +
                $"\nName: {targetUser.Username}#{targetUser.Discriminator}" +
                $"\nID: {targetUser.Id}" +
                $"\nAvatar ID: {targetUser.AvatarId}" +
                $"\nAccount Created: {targetUser.CreatedAt}" +
                $"\nJoined Server: {(targetUser as IGuildUser)?.JoinedAt}```" +
                targetUser.GetAvatarUrl()
            );
        }

        [Command("help")]
        [Summary("Have Kasumi display a list of commands.")]
        public async Task HelpAsync()
        {
            StringBuilder response = new("Available commands:\n");
            IEnumerable<(string Name, string Summary)> commands = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ModuleBase<SocketCommandContext>)))
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                .Select(m => (
                    Name: m.GetCustomAttribute<CommandAttribute>()!.Text,
                    Summary: m.GetCustomAttribute<SummaryAttribute>()?.Text ?? "No description."
                ));
            foreach ((string Name, string Summary) in commands)
                _ = response.AppendLine($"**{Name}**: {Summary}");
            await CommandHandler.ReplyAsync(Context, response.ToString());
        }
    }
}