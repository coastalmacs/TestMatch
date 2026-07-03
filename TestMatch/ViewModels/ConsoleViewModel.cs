using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TestMatch.ViewModels
{

    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel; // Added for INotifyPropertyChanged
    using System.Runtime.CompilerServices; // Added for CallerMemberName

    public class ConsoleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> LogOutput { get; set; } = new ObservableCollection<string>();

        public void AddLogLine(string message)
        {
            // Ensure UI updates from background threads safely
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LogOutput.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            });
        }

        // --- Fixes the compiler error ---
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
