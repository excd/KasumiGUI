using Discord;

namespace KasumiGUI.utility {
    internal class Logger {
        private readonly Window window;

        public Logger(ref Window window) {
            this.window = window;
        }

        public Task Log(LogMessage message) {
            if (window.IsHandleCreated)
                return Task.Run(() => window.Invoke(new Action(() => window.Out(message.ToString()))));
            else
                return Task.CompletedTask;
        }
    }
}