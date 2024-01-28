using Discord;
using System.Reflection;

namespace KasumiGUI.Utility {
    public static class Logger {
        public static async Task LogAsync(LogMessage message) {
            await LogToFileAsync(message);
            await LogToWindowAsync(message);
        }

        private static async Task LogToWindowAsync(LogMessage message) {
            await Task.Run(() => Window.ActiveForm.Invoke(new Action(() => {
                if (Window.ActiveForm.IsHandleCreated) {
                    Window.Out(FormatMessage(message));

                    if (message.Exception != null)
                        Window.Out(message.Exception.ToString());
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