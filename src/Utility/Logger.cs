using Discord;

namespace KasumiGUI.Utility {
    internal class Logger {
        private readonly Window window;

        public Logger(ref Window window) {
            this.window = window;
        }

        public Task LogAsync(LogMessage message) {
            if (window.IsHandleCreated)
                return Task.Run(() => window.Invoke(new Action(() => {
                    window.Out(string.Format("[{0}][{1,8}][{2,7}] {3}", DateTime.Now, message.Source, message.Severity, message.Message));

                    if (message.Exception != null)
                        window.Out(message.Exception.ToString());
                })));
            else
                return Task.CompletedTask;
        }
    }
}