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

            // Set focus to the window on load so key inputs are captured immediately
            Loaded += (sender, e) => this.Focus();
            PreviewKeyDown += MainWindow_KeyDown;

            Init();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D1 && e.Key <= Key.D9)
            {
                int target = e.Key - Key.D1 + 1;
                DoSomething(target);
            }
            else if (e.Key >= Key.NumPad1 && e.Key <= Key.NumPad9)
            {
                int target = e.Key - Key.NumPad1 + 1;
                DoSomething(target);
            }
            else if (e.Key == Key.D0 || e.Key == Key.NumPad0)
            {
                EffortToggleButton.IsChecked = EffortToggleButton.IsChecked != true;
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                VariationToggleButton.IsChecked = VariationToggleButton.IsChecked != true;
            }
            else if (e.Key == Key.Up)
            {
                if (BowlersListBox.SelectedIndex > 0)
                {
                    BowlersListBox.SelectedIndex--;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (BowlersListBox.SelectedIndex < BowlersListBox.Items.Count - 1)
                {
                    BowlersListBox.SelectedIndex++;
                }
                e.Handled = true;
            }
        }

        private void LogScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0)
            {
                LogScrollViewer.ScrollToBottom();
            }
        }


        private void Init()
        {
            try
            {
                // Load Australia team sheet for testing
                string rosterPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "australia.csv");
                Team australia = Team.LoadFromCsv(rosterPath);
                
                // Sort players by batting order ascending
                var sortedPlayers = System.Linq.Enumerable.ToList(System.Linq.Enumerable.OrderBy(australia.Players, p => p.BattingOrder));

                BowlersListBox.ItemsSource = sortedPlayers;
                
                // Automatically select player with BattingOrder == 11 (tailender bowler) as the default first bowler
                var defaultBowler = System.Linq.Enumerable.FirstOrDefault(sortedPlayers, p => p.BattingOrder == 11);
                if (defaultBowler != null)
                {
                    BowlersListBox.SelectedItem = defaultBowler;
                    session.bowler = defaultBowler;
                }
                else if (sortedPlayers.Count > 0)
                {
                    BowlersListBox.SelectedIndex = 0;
                    session.bowler = sortedPlayers[0];
                }
                _viewModel.StatusMessage = $"Loaded team: {australia.Name}.";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"Error: {ex.Message}";
            }
        }

        private void BowlersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BowlersListBox.SelectedItem is Player selectedPlayer)
            {
                session.bowler = selectedPlayer;
                _viewModel.StatusMessage = $"Active bowler: {selectedPlayer.Name} (Pace: {selectedPlayer.Pace}, Accuracy: {selectedPlayer.Accuracy}, Health: {selectedPlayer.Health})";
                BowlersListBox.ScrollIntoView(selectedPlayer);
            }
        }

        private void DoSomething(int target)
        {
            if (!session.CanBowl(session.bowler, out string errorMessage))
            {
                _viewModel.StatusMessage = $"Error: {errorMessage}";
                return;
            }

            // Simulate a delivery using toggle button states
            bool isEffortBall = EffortToggleButton.IsChecked == true;
            bool isVariationBall = VariationToggleButton.IsChecked == true;
            var delivery = session.Bowl(isEffortBall, isVariationBall, target);
            if (delivery == null)
            {
                return;
            }
            
            string legalityStr = delivery.Legality switch
            {
                Legality.Legal => "Legal",
                Legality.NoBall => "No-Ball",
                Legality.Wide => "Wide",
                _ => "Unknown"
            };

            _viewModel.AddLogLine($"[{session.OverProgress:0.0} overs] Bowler {session.bowler.Name} aimed at {target}. Speed: {delivery.Speed}, Line: {delivery.Line}, Length: {delivery.Length} ({legalityStr})");
            _viewModel.StatusMessage = $"Ready. Last delivery by {session.bowler.Name} (Target: {target}, Speed: {delivery.Speed}, {legalityStr}).";

            // Update health of all players and refresh the ListBox
            if (BowlersListBox.ItemsSource is System.Collections.Generic.IEnumerable<Player> players)
            {
                foreach (var p in players)
                {
                    p.Health = session.GetBowlerHealth(p);
                }
                BowlersListBox.Items.Refresh();

                // Check if the current over just completed
                var lastCompletedOver = session.Overs.Count > 0 ? session.Overs[session.Overs.Count - 1] : null;
                if (lastCompletedOver != null && lastCompletedOver.IsComplete)
                {
                    Player? nextDefaultBowler = null;
                    if (session.Overs.Count == 1)
                    {
                        // Over 1 just completed. Default next bowler (for Over 2) is BattingOrder 10.
                        nextDefaultBowler = System.Linq.Enumerable.FirstOrDefault(players, p => p.BattingOrder == 10);
                    }
                    else if (session.Overs.Count >= 2)
                    {
                        // Over N just completed. Default next bowler is the one who bowled Over N-1.
                        var secondToLastOver = session.Overs[session.Overs.Count - 2];
                        var bowlerFromSecondToLast = secondToLastOver.Bowler;
                        nextDefaultBowler = System.Linq.Enumerable.FirstOrDefault(players, p => p.Name == bowlerFromSecondToLast.Name);
                    }

                    if (nextDefaultBowler != null)
                    {
                        BowlersListBox.SelectedItem = nextDefaultBowler;
                        session.bowler = nextDefaultBowler;
                    }
                }
            }
        }

    }


}