namespace KasumiGUI {
    public partial class Window : Form {
        public Window() {
            InitializeComponent();
        }

        #region Window Methods
        /// <summary>
        ///    Outputs a message to the log text box.
        /// </summary>
        /// <param name="message"></param>
        public void Out(string message) {
            logTextBox.AppendText(message + "\r\n");
        }

        private void TerminateApplication() {
            Program.DiscordClient?.Stop();
            Application.Exit();
            Environment.Exit(0);
        }
        #endregion Window Methods

        #region Event Handlers
        private void CloseWindow(object sender, FormClosedEventArgs? e) {
            TerminateApplication();
        }

        private void ExitMenuButton(object sender, EventArgs e) {
            TerminateApplication();
        }

        private void StartButton(object sender, EventArgs e) {
            Program.DiscordClient?.Start();
        }

        private void StopButton(object sender, EventArgs e) {
            Program.DiscordClient?.Stop();
        }
        #endregion Event Handlers
    }
}