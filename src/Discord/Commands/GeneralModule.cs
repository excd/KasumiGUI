using Discord.Commands;

namespace KasumiGUI.Discord.Commands
{
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("blender")]
        [Summary("Como needs to turn off the blender.")]
        public async Task BlenderAsync() => await CommandHandler.ReplyAsync(Context, "ヽヽ༼༼ຈຈل͜ل͜ຈຈ༽༽ﾉﾉ COMONAD TURN OFF THE BLENDER ヽヽ༼༼ຈຈل͜ل͜ຈຈ༽༽ﾉﾉ");

        [Command("homework")]
        [Summary("Dechi needs to do his homework.")]
        public async Task HomeworkAsync() => await CommandHandler.ReplyAsync(Context, "Dechi go do your homework or ill shit in your mouth you useless waste of life\nalso ur gey");

        [Command("gacha")]
        [Summary("Have Kasumi roll a server member with a random rarity in a gacha.")]
        public async Task GachaAsync()
        {
            string[] rarities = ["Common", "Uncommon", "Rare", "Epic", "Legendary"];
            string[] users = [.. Context.Guild.Users.Select(user => user.Username)];
            await CommandHandler.ReplyAsync(Context, $"{users[new Random().Next(users.Length)]} - {rarities[new Random().Next(rarities.Length)]}");
        }

        [Command("pole")]
        [Summary("Duki, no.")]
        public async Task PoleAsync()
        {
            if (Context.User.Id == 118378275046162433)
                await CommandHandler.ReplyAsync(Context, "Duki, no.");
        }
    }
}