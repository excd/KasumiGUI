namespace KasumiGUI {
    public partial class Window : Form {
        public Window() {
            InitializeComponent();
        }

        /// <summary>
        ///     Outputs a message to the log text box.
        /// </summary>
        /// <param name="message"></param>
        public void Out(string message) {
            logTextBox.AppendText(message + "\r\n");
        }

        #region Event Handlers
        private void OpenWindow(object sender, EventArgs e) {
            ActiveForm.ActiveControl = null;
        }

        private void CloseWindow(object sender, FormClosedEventArgs? e) {
            Program.TerminateApplication();
        }

        private void ExitButton(object sender, EventArgs e) {
            Program.TerminateApplication();
        }

        private void StartButton(object sender, EventArgs e) {
            Program.DiscordClient?.Start();
        }

        private void StopButton(object sender, EventArgs e) {
            Program.DiscordClient?.Stop();
        }

        private void RestartButton(object sender, EventArgs e) {
            Program.DiscordClient?.Restart();
        }
        #endregion Event Handlers
    }
}