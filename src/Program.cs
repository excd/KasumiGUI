using KasumiGUI.Discord;

namespace KasumiGUI
{
    internal static class Program
    {
        public static DiscordClient? DiscordClient { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        public static Task Main(string[] args) => MainAsync();

        private static async Task MainAsync()
        {
            Window window = new();
            DiscordClient = new();
            await Task.Run(() =>
            {
                ApplicationConfiguration.Initialize();
                Application.Run(window);
            });
        }

        public static async Task TerminateApplication()
        {
            if (DiscordClient != null)
                await DiscordClient.Stop();
            await Task.Run(static () =>
            {
                Application.Exit();
                Environment.Exit(0);
            });
        }
    }
}