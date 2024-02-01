using Discord;
using Discord.Commands;

namespace KasumiGUI.Discord.Commands {
    public class CommandModule : ModuleBase<SocketCommandContext> {
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task EchoAsync([Remainder][Summary("The text to echo.")] string message) =>
            await CommandHandler.ReplyAsync(Context, message);

        [Command("ping")]
        [Summary("Pings the bot.")]
        public async Task PingAsync() => await CommandHandler.ReplyAsync(Context, "Pong!");

        [Command("coin")]
        [Summary("Flips a coin.")]
        public async Task CoinAsync() => await CommandHandler.ReplyAsync(Context, new Random().Next(2) == 0 ? "Heads" : "Tails");

        [Command("roll")]
        [Summary("Rolls a dice.")]
        public async Task RollAsync([Summary("The number of sides on the dice.")] int sides = 6) =>
            await CommandHandler.ReplyAsync(Context, new Random().Next(1, sides + 1).ToString());

        [Command("choose")]
        [Summary("Chooses between a comma separated list of options.")]
        public async Task ChooseAsync([Remainder][Summary("Comma separated list of options.")] string message) {
            string[] options = message.Split(',');
            await CommandHandler.ReplyAsync(Context, options[new Random().Next(options.Length)]);
        }

        [Command("rate")]
        [Summary("Rates something out of 10.")]
        public async Task RateAsync([Remainder][Summary("The thing to rate.")] string message) =>
            await CommandHandler.ReplyAsync(Context, $"{message} is a {new Random().Next(11)}/10.");

        [Command("8ball")]
        [Summary("Ask the magic 8-ball a question.")]
        public async Task EightBallAsync([Remainder][Summary("Question to ask.")] string message) {
            string[] responses = {
                "It is certain.",
                "It is decidedly so.",
                "Without a doubt.",
                "Yes - definitely.",
                "You may rely on it.",
                "As I see it, yes.",
                "Most likely.",
                "Outlook good.",
                "Signs point to yes.",
                "Yes.",
                "Reply hazy, try again.",
                "Ask again later.",
                "Better not tell you now...",
                "Cannot predict now.",
                "Concentrate and ask again.",
                "Don't count on it.",
                "My reply is no.",
                "My sources say no...",
                "Outlook not so good...",
                "Very doubtful."
            };
            await CommandHandler.ReplyAsync(Context, responses[new Random().Next(responses.Length)]);
        }

        [Command("reverse")]
        [Summary("Reverses the text.")]
        public async Task ReverseAsync([Remainder][Summary("The text to reverse.")] string text) =>
            await CommandHandler.ReplyAsync(Context, new string(text.Reverse().ToArray()));

        [Command("mock")]
        [Summary("Mocks the text.")]
        public async Task MockAsync([Remainder][Summary("The text to mock.")] string text) {
            string mockedText = "";
            for (int i = 0; i < text.Length; i++)
                mockedText += i % 2 == 0 ? char.ToUpper(text[i]) : char.ToLower(text[i]);
            await CommandHandler.ReplyAsync(Context, mockedText);
        }

        [Command("owo")]
        [Summary("OwOifies the text.")]
        public async Task OwoAsync([Remainder][Summary("The text to OwOify.")] string text) =>
            await CommandHandler.ReplyAsync(Context, text.Replace("r", "w").Replace("l", "w").Replace("R", "W").Replace("L", "W"));

        [Command("bully")]
        [Summary("Bullies a user.")]
        public async Task BullyAsync([Summary("The user to bully.")] IUser user) {
            string[] insults = {
                $"{user.Mention}, you're ugly.",
                $"{user.Mention}, you're stupid.",
                $"{user.Mention}, no one likes you, go away.",
                $"{user.Mention}, you're a loser.",
                $"Hey {user.Mention}, fuck you!"
            };
            await CommandHandler.ReplyAsync(Context, insults[new Random().Next(insults.Length)]);
        }

        [Command("whois")]
        [Summary("Displays info about the current user, or the user parameter, if one passed.")]
        public async Task WhoIsAsync([Summary("The (optional) user to get info from.")] IUser? user = null) {
            IUser targetUser = user ?? Context.User;
            string userInfo = "User Information:```" +
                $"\n\tName: {targetUser.Username}#{targetUser.Discriminator}" +
                $"\n\tID: {targetUser.Id}" +
                $"\n\tAvatar ID: {targetUser.AvatarId}" +
                $"\n\tCreated At: {targetUser.CreatedAt}```";
            await CommandHandler.ReplyAsync(Context, userInfo);
        }
    }
}