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

namespace TestMatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TestMatch.ViewModels.ConsoleViewModel _viewModel;

        Practice session = new  Practice();

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new TestMatch.ViewModels.ConsoleViewModel();
            DataContext = _viewModel;

            Init();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                _viewModel.AddLogLine(textBox.Text);
                textBox.Clear();
            }
        }


            private void Init()
            {
                Practice session = new Practice();
                
                




            }

    }


}