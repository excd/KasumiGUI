namespace KasumiGUI
{
    internal partial class Window : Form
    {
        public static Window? ActiveWindow { get; set; }

        public Window()
        {
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
        public static void UpdateStatus(string status)
        {
            if (ActiveWindow != null)
                ActiveWindow.statusLabel.Text = status;
        }

        #region Event Handlers
        private void OpenWindow(object sender, EventArgs e)
        {
            if (ActiveForm != null)
                ActiveForm.ActiveControl = null;
        }

        private async void CloseWindow(object sender, FormClosedEventArgs? e) => await Program.TerminateApplication();

        private async void ExitButton(object sender, EventArgs e) => await Program.TerminateApplication();

        private async void StartButton(object sender, EventArgs e)
        {
            if (Program.DiscordClient != null)
                await Program.DiscordClient.Start();
        }

        private async void StopButton(object sender, EventArgs e)
        {
            if (Program.DiscordClient != null)
                await Program.DiscordClient.Stop();
        }

        private async void RestartButton(object sender, EventArgs e)
        {
            if (Program.DiscordClient != null)
                await Program.DiscordClient.Restart();
        }

        private void ClearButton(object sender, EventArgs e) => Invoke(new Action(() => logTextBox.Clear()));
        #endregion Event Handlers
    }
}