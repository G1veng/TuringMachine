using TuringMachine.Views;
using TuringMachine.ViewModels;
using System.Windows;

namespace TuringMachine
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
    protected override void OnStartup(StartupEventArgs e)
    {
      var window = new Window();
      var view = new MainWindow();
      var viewModel = new MainWindowViewModel
      {
        TextBoxes = new System.Collections.ObjectModel.ObservableCollection<TextBoxViewModel> ()
      };

      view.DataContext = viewModel;
      window.Content = view;
      window.DataContext = viewModel;
      window.Show();

      displayRootRegistry.RegisterWindowType<GreetingsWindowViewModel, GreetingsWindow>();
    }
  }
}
