using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestMatch.Models;

namespace TestMatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TestMatch.ViewModels.ConsoleViewModel _viewModel;

        Innings session = new Innings();

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new TestMatch.ViewModels.ConsoleViewModel();
            DataContext = _viewModel;

            // Set focus to the input text box on window load
            Loaded += (sender, e) => InputTextBox.Focus();

            Init();
        }

        private void LogScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0)
            {
                LogScrollViewer.ScrollToBottom();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Temporarily detach the event handler to avoid re-entrancy issues
                textBox.TextChanged -= TextBox_TextChanged;
                try
                {
                    string text = textBox.Text;
                    if (!string.IsNullOrEmpty(text))
                    {
                        // Check if the input is a single digit 0-9 (0 used for effort ball)
                        if (text.Length == 1 && text[0] >= '0' && text[0] <= '9')
                        {
                            if (text[0] == '0')
                            {
                                EffortToggleButton.IsChecked = EffortToggleButton.IsChecked != true;
                            }
                            else
                            {
                                DoSomething(int.Parse(text));
                            }
                        }



                        // Always ensure the text box is cleared after processing
                        textBox.Clear();
                    }
                }
                finally
                {
                    // Re-attach the event handler
                    textBox.TextChanged += TextBox_TextChanged;
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow characters '0'-'9' to be entered
            e.Handled = e.Text.Length != 1 || e.Text[0] < '0' || e.Text[0] > '9';
        }


        private void Init()
        {
            try
            {

                // Load Australia team sheet for testing
                string rosterPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "australia.csv");
                Team australia = Team.LoadFromCsv(rosterPath);
                
                _viewModel.AddLogLine($"Loaded team: {australia.Name}");
                foreach (var player in australia.Players)
                {
                    _viewModel.AddLogLine($" - {player.Name} (Pace: {player.Pace}, Accuracy: {player.Accuracy})");
                }
            }
            catch (Exception ex)
            {
                _viewModel.AddLogLine($"Error loading default team sheet: {ex.Message}");
            }
        }

        private void DoSomething(int target)
        {
            // Simulate a delivery using the EffortToggleButton checked state
            bool isEffortBall = EffortToggleButton.IsChecked == true;
            var delivery = session.Bowl(isEffortBall, target);
            _viewModel.AddLogLine($"[{session.OverProgress:0.0} overs] Bowler {session.bowler.Name} aimed at {target}. {delivery}");
        }

    }


}