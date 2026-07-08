using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TestMatch.ViewModels
{

    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel; 
    using System.Runtime.CompilerServices; 

    public class ConsoleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> LogOutput { get; set; } = new ObservableCollection<string>();

        public void AddLogLine(string message)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LogOutput.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            });
        }

        private string _statusMessage = "Ready";
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
