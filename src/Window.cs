namespace KasumiGUI {
    public partial class Window : Form {
        public static Window? ActiveWindow { get; set; }

        public Window() {
            ActiveWindow = this;
            InitializeComponent();
        }

        /// <summary>
        ///     Outputs a message to the log text box.
        /// </summary>
        /// <param name="message"></param>
        public static void Out(string message) => ActiveWindow?.logTextBox.AppendText(message + "\r\n");

        /// <summary>
        ///     Updates the status label.
        /// </summary>
        /// <param name="status"></param>
        public static void UpdateStatus(string status) {
            if (ActiveWindow != null)
                ActiveWindow.statusLabel.Text = status;
        }

        #region Event Handlers
        private void OpenWindow(object sender, EventArgs e) => ActiveForm.ActiveControl = null;

        private void CloseWindow(object sender, FormClosedEventArgs? e) => Program.TerminateApplication();

        private void ExitButton(object sender, EventArgs e) => Program.TerminateApplication();

        private void StartButton(object sender, EventArgs e) => Program.DiscordClient?.Start();

        private void StopButton(object sender, EventArgs e) => Program.DiscordClient?.Stop();

        private void RestartButton(object sender, EventArgs e) => Program.DiscordClient?.Restart();

        private void ClearButton(object sender, EventArgs e) => Invoke(new Action(() => logTextBox.Clear()));
        #endregion Event Handlers
    }
}