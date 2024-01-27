using KasumiGUI.Discord;
using KasumiGUI.Utility;

namespace KasumiGUI {
    internal static class Program {
        public static Logger? Logger { get; private set; }
        public static DiscordClient? DiscordClient { get; private set; }

        public static Task Main(string[] args) => MainAsync();

        private static async Task MainAsync() {
            Window window = new();
            Logger = new(ref window);
            DiscordClient = new();

            await Task.Run(() => {
                ApplicationConfiguration.Initialize();
                Application.Run(window);
            });
        }

        public static async void TerminateApplication() {
            DiscordClient?.Stop();

            await Task.Run(() => {
                Application.Exit();
                Environment.Exit(0);
            });
        }
    }
}