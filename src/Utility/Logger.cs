using Discord;
using System.Reflection;

namespace KasumiGUI.Utility {
    internal class Logger {
        private readonly Window window;

        public Logger(ref Window window) {
            this.window = window;
        }

        public async Task LogAsync(LogMessage message) {
            await LogToFileAsync(message);
            await LogToWindowAsync(message);
        }

        private async Task LogToWindowAsync(LogMessage message) {
            await Task.Run(() => window.Invoke(new Action(() => {
                if (window.IsHandleCreated) {
                    window.Out(FormatMessage(message));

                    if (message.Exception != null)
                        window.Out(message.Exception.ToString());
                }
            })));
        }

        private static async Task LogToFileAsync(LogMessage message) {
            await Task.Run(() => {
                using (StreamWriter writer = new($"{Assembly.GetEntryAssembly()?.GetName().Name}-log.txt", true)) {
                    writer.WriteLine(FormatMessage(message));

                    if (message.Exception != null)
                        writer.WriteLine(message.Exception.ToString());
                };
            });
        }

        private static string FormatMessage(LogMessage message) {
            return string.Format("[{0}][{1,8}][{2,7}] {3}", DateTime.Now, message.Source, message.Severity, message.Message);
        }
    }
}