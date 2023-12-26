using LearnHowToMakeCustomControl.ViewModels;
using System.Windows;

namespace LearnHowToMakeCustomControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow window = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            window.Show();
            base.OnStartup(e);
        }
    }
}
