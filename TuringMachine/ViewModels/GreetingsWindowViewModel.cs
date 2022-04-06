using System.Windows.Input;
using TuringMachine.ViewModels.Base;
using TuringMachine.Infrastructure.Commands;
using System.Collections.ObjectModel;
using System.IO;
using System;

namespace TuringMachine.ViewModels
{
  public class GreetingsWindowViewModel : ViewModel
  {
    #region Properties
    private  bool _isChecked;
    public bool IsChecked { get => _isChecked; set => Set(ref _isChecked, value); }
    private DisplayRootRegistry _displayRootRegistry;
    public DisplayRootRegistry DisplayRootRegistry
    {
      set { _displayRootRegistry = value; }
      get { return _displayRootRegistry; }
    }
    #endregion

    #region CloseApplicationCommand
    public ICommand CloseApplicationCommand { get; }
    private void OnCloseApplicationCommandExecuted(object p)
    {
      StreamWriter file = new StreamWriter("Agreement.txt");
      file.WriteLine((IsChecked.ToString()));
      file.Close();
      DisplayRootRegistry.HidePresentation(this);
    }
    private bool CanCloseApplicationCommandExecute(object p) => true;
    #endregion

    public GreetingsWindowViewModel()
    {

      #region Commands
      CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
      #endregion

    }
  }
}
