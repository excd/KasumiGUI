using Discord;
using Discord.Commands;
using KasumiGUI.Data.Models;
using KasumiGUI.Data.Services;

namespace KasumiGUI.Discord.Commands
{
    public class DataModule(DataProvider dataProvider) : ModuleBase<SocketCommandContext>
    {
        private readonly DataProvider DataProvider = dataProvider;

        [Command("poke")]
        [Summary("Poke Kasumi and get a random response.")]
        public async Task PokeAsync()
        {
            List<PokeResponse> responses = await DataProvider.GetBotResponsesAsync(DataProvider.DB.Poke);
            await CommandHandler.ReplyAsync(Context, responses.Count > 0
                ? responses[new Random().Next(responses.Count)].Message ?? "Response had no message, fix your shit idiot programmer."
                : "No responses found, fix it stupid programmer.");
        }

        [Command("choose")]
        [Summary("Have Kasumi choose between a comma separated list of options.")]
        public async Task ChooseAsync([Remainder][Summary("Comma separated list of options.")] string? message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await CommandHandler.ReplyAsync(Context, "You need to give me things to choose from you dumb fuck.");
                return;
            }
            string[] options = message.Split(',');
            if (options.Length < 2)
            {
                await CommandHandler.ReplyAsync(Context, "You need at least two options separated by commas dumbass.");
                return;
            }
            string choice = options[new Random().Next(options.Length)].Trim();
            List<ChoiceResponse> responses = await DataProvider.GetBotResponsesAsync(DataProvider.DB.Choice);
            await CommandHandler.ReplyAsync(Context, responses.Count > 0
                ? responses[new Random().Next(responses.Count)].Message?.Replace("{{CHOICE}}", choice).Replace("\\n", "\n") ?? "Response had no message, fix your shit idiot programmer."
                : "No responses found, fix it stupid programmer.");
        }

        [Command("8ball")]
        [Summary("Ask Kasumi a question.")]
        public async Task EightBallAsync([Remainder][Summary("Question to ask.")] string? message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await CommandHandler.ReplyAsync(Context, "You need to actually ask something you dumb fuck.");
                return;
            }
            List<EightBallResponse> responses = await DataProvider.GetBotResponsesAsync(DataProvider.DB.EightBall);
            await CommandHandler.ReplyAsync(Context, responses.Count > 0
                ? responses[new Random().Next(responses.Count)].Message ?? "Response had no message, fix your shit idiot programmer."
                : "No responses found, fix it stupid programmer.");
        }

        [Command("dechi")]
        [Summary("Get a random quote from Dechi or other idiots from Discord.")]
        public async Task DechiAsync()
        {
            List<DechiResponse> responses = await DataProvider.GetBotResponsesAsync(DataProvider.DB.Dechi);
            await CommandHandler.ReplyAsync(Context, responses.Count > 0
                ? responses[new Random().Next(responses.Count)].Message ?? "Response had no message, fix your shit idiot programmer."
                : "No responses found, fix it stupid programmer.");
        }

        [Command("bully")]
        [Summary("Have Kasumi bully you or someone else.")]
        public async Task BullyAsync([Summary("The user to bully.")] IUser? user = null)
        {
            List<BullyResponse> responses = await DataProvider.GetBotResponsesAsync(DataProvider.DB.Bully);
            await CommandHandler.ReplyAsync(Context, responses.Count > 0
                ? responses[new Random().Next(responses.Count)].Message?.Replace("{{USER}}", (user ?? Context.User).Mention) ?? "Response had no message, fix your shit idiot programmer."
                : "No responses found, fix it stupid programmer.");
        }
    }
}