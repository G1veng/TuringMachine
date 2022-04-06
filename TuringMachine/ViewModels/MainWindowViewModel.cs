using System.Collections.Generic;
using TuringMachine.Infrastructure.Commands;
using TuringMachine.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using TuringMachine.Services;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace TuringMachine.ViewModels
{
  public class MainWindowViewModel : ViewModel
  {
    #region Properties
    private string _currentFunction = "0";
    private DisplayRootRegistry displayRootRegistry;
    private GreetingsWindowViewModel greetingsWindowViewModel;
    private ProcessCodeService processCodeService;

    private string _initialData = string.Empty;
    public string InitialData { get => _initialData; set => Set(ref _initialData, value); }
    private ObservableCollection<TextBoxViewModel> _textBoxes;
    public ObservableCollection<TextBoxViewModel> TextBoxes { get => _textBoxes; set => Set(ref _textBoxes, value); }

    private int _indexSelected = -1;
    public int IndexSelected { get => _indexSelected; set => Set(ref _indexSelected, value); }

    private string _textOfCommands = string.Empty;
    public string TextOfCommands { get => _textOfCommands; set => Set(ref _textOfCommands, value);}
    #endregion

    private MessageBoxResult Alarm(string message, string caption, MessageBoxButton button, MessageBoxImage icon) =>
      System.Windows.MessageBox.Show(message, caption, button, icon);

    #region Commands
    #region CreateBlockCommand
    public ICommand CreateBlockCommand { get; }
    private bool CanCreateBlockCommandExecute(object p) => InitialData.Length > 0;
    private void OnCreateBlockCommandExecuted(object p)
    {
      TextBoxes.Clear();
      InitialData = InitialData.Replace(" ", "");
      for (int i = 0; i < InitialData.Length; i++)
      {
        if (i == 0)
        {
          TextBoxes.Add(new TextBoxViewModel { Text = InitialData[i].ToString(), Colour = new SolidColorBrush(Colors.Green) });
        }
        else
          TextBoxes.Add(new TextBoxViewModel { Text = InitialData[i].ToString(), Colour = new SolidColorBrush() });
      }
      if(processCodeService == null)
        processCodeService = new ProcessCodeService(TextBoxes);
      processCodeService.Reset(TextBoxes);
    }
    #endregion

    #region ProcessProgrammCommand
    public ICommand ProcessProgrammCommand { get; }
    private bool CanProcessProgrammCommandExecute(object p) => TextOfCommands.Length > 1 && TextBoxes.Count != 0;
    private async void OnProcessProgrammCommandExecuted(object p)
    {
      while (processCodeService._currentFunction.ToLower() != "s")
      {
        if (!processCodeService._isBroken)
        {
          if (!processCodeService._isRunning)
          {
            await processCodeService.AsyncFunc(TextOfCommands);
          }
        }
        else
          return;
      }
      if(processCodeService._currentFunction.ToLower() == "s")
      {
        Alarm("Programm ended succesfully", "Word Processor", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }
    #endregion

    #region StepByStepExecutionCommand
    public ICommand StepByStepExecutionCommand { get; }
    private bool CanStepByStepExecutionCommandExecute(object p) => TextOfCommands.Length > 1 && TextBoxes.Count != 0;
    private void OnStepByStepExecutionCommandExecuted(object p)
    {
      if (_currentFunction.ToLower() == "s") return;
      processCodeService.Func(TextOfCommands);
    }
    #endregion

    #region SetColourCommand
    public ICommand SetColourCommand { get; }
    private bool CanSetColourCommandExecute(object p) => IndexSelected >= 0;
    private void OnSetColourCommandExecuted(object p)
    {
      int SaveIndex = IndexSelected;
      List<TextBoxViewModel> list = new List<TextBoxViewModel>();
      foreach(var item in TextBoxes)
        list.Add(item.Clone());
      TextBoxes.Clear();
      for (int i = 0; i < list.Count; i++)
      {
        if (i == SaveIndex)
        {
          TextBoxes.Add(list[i].Clone());
          TextBoxes[SaveIndex].Colour = new SolidColorBrush(Colors.Green);
        }
        else
          TextBoxes.Add(list[i].Clone());
      }
    }
    #endregion

    #region OpenINformationWindowCommand
    public ICommand OpenINformationWindowCommand { get; }
    private bool CanOpenINformationWindowCommandExecute(object p) => !displayRootRegistry.CheckForExistingWindows(greetingsWindowViewModel);
    private void OnOpenINformationWindowCommandExecuted(object p)
    {
      if (greetingsWindowViewModel == null)
        greetingsWindowViewModel = new GreetingsWindowViewModel();
      greetingsWindowViewModel.DisplayRootRegistry = displayRootRegistry;
      displayRootRegistry.ShowPresentation(greetingsWindowViewModel);
    }
    #endregion

    #region CloseApplicationCommand
    public ICommand CloseApplicationCommand { get; }
    private void OnCloseApplicationCommandExecuted(object p)
    {
      System.Windows.Application.Current.Shutdown();
    }
    private bool CanCloseApplicationCommandExecute(object p) => 
      !displayRootRegistry.CheckForExistingWindows(greetingsWindowViewModel);
    #endregion

    #endregion
    public MainWindowViewModel() 
    {
      displayRootRegistry = (System.Windows.Application.Current as App).displayRootRegistry;
      #region Commands
      OpenINformationWindowCommand = new LambdaCommand(OnOpenINformationWindowCommandExecuted, CanOpenINformationWindowCommandExecute);
      CreateBlockCommand = new LambdaCommand(OnCreateBlockCommandExecuted, CanCreateBlockCommandExecute);
      SetColourCommand = new LambdaCommand(OnSetColourCommandExecuted, CanSetColourCommandExecute);
      ProcessProgrammCommand = new LambdaCommand(OnProcessProgrammCommandExecuted, CanProcessProgrammCommandExecute);
      StepByStepExecutionCommand = new LambdaCommand(OnStepByStepExecutionCommandExecuted, CanStepByStepExecutionCommandExecute);
      CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
      #endregion
    }
  }
}
