using KasumiGUI.discord;

namespace KasumiGUI {
    internal class Program {
        public static DiscordClient? DiscordClient { get; private set; }

        public static Task Main(string[] args) => MainAsync();

        private static async Task MainAsync() {
            Window window = new();
            DiscordClient = new(ref window);

            await Task.Run(() => {
                ApplicationConfiguration.Initialize();
                Application.Run(window);
            });
        }
    }
}