namespace KasumiGUI {
    public partial class Window : Form {
        private static Window? ThisWindow { get; set; }

        public Window() {
            ThisWindow = this;
            InitializeComponent();
        }

        /// <summary>
        ///     Outputs a message to the log text box.
        /// </summary>
        /// <param name="message"></param>
        public static void Out(string message) {
            if (ThisWindow != null)
                ThisWindow.logTextBox.AppendText(message + "\r\n");
        }

        /// <summary>
        ///     Updates the status label.
        /// </summary>
        /// <param name="status"></param>
        public static void UpdateStatus(string status) {
            if (ThisWindow != null)
                ThisWindow.statusLabel.Text = status;
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