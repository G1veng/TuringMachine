using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TuringMachine.ViewModels;

namespace TuringMachine.Services
{
  internal class ProcessCodeService
  {
    #region Fields
    public bool _isBroken = false, _isRunning = false;
    private int _iterator = 0;
    private int _prev = 0;
    public string _currentFunction = "0";
    private ObservableCollection<TextBoxViewModel> TextBoxes;
    private int _speed = 100;

    #endregion

    public ProcessCodeService(ObservableCollection<TextBoxViewModel> TextBoxes)
    {
      this.TextBoxes = TextBoxes;
    }
    private MessageBoxResult Alarm(string message, string caption, MessageBoxButton button, MessageBoxImage icon) =>
      System.Windows.MessageBox.Show(message, caption, button, icon);

    public void Reset(ObservableCollection<TextBoxViewModel> TextBoxes)
    {
      _isRunning = false;
      _isBroken = false;
      _prev = 0;
      _currentFunction = "0";
      _iterator = 0;
      this.TextBoxes = TextBoxes;
    }

    #region Func
    public void Func(string TextOfCommands)
    {
      TuringService turingService = new TuringService(TextOfCommands);
      if (turingService.IsErrorInText())
      {
        Alarm("Error at line " + turingService.GetLineWithError().ToString(), "Word Processor", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }
      var code = turingService.GetFunctions();
      foreach (var command in code)
        if (command.currentFunction == _currentFunction)
        {
          if (command.findThis == TextBoxes[_iterator].Text)
          {
            TextBoxes[_iterator].Text = command.replaceWith;
            _currentFunction = command.nextFunction;
            if (_currentFunction.ToLower() == "s")
              Alarm("Programm ended succesfully", "Word Processor", MessageBoxButton.OK, MessageBoxImage.Information);
            TextBoxes[_iterator].Colour = new SolidColorBrush();
            _prev = _iterator;
            if (command.direction.ToLower() == "l")
              _iterator--;
            if (command.direction.ToLower() == "r")
              _iterator++;
            if (command.direction.ToLower() != "l" && command.direction.ToLower() != "r" && command.direction.ToLower() != "h")
            {
              Alarm($"Unknown direcion of movement at {_currentFunction} function", "Word Processor", MessageBoxButton.OK, MessageBoxImage.Warning);
              return;
            }
            if (_iterator == TextBoxes.Count)
            {
              TextBoxes.Add(new TextBoxViewModel()
              {
                Text = "_",
                Colour = new SolidColorBrush(),
              });
            }
            if (_iterator < 0)
            {
              TextBoxes.Add(new TextBoxViewModel()
              {
                Text = "_",
                Colour = new SolidColorBrush(),
              });
              for (int i = TextBoxes.Count - 1; i > 0; i--)
              {
                TextBoxes[i] = TextBoxes[i - 1].Clone();
              }
              TextBoxes[0].Text = "_";
              TextBoxes[0].Colour = new SolidColorBrush();
              _iterator = 0;
              _prev = 1;
            }
            TextBoxes[_iterator].Colour = new SolidColorBrush(Colors.Green);
            return;
          }
        }
      Alarm($"There is no command for \"{_currentFunction}\" function", "Word Processor", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    #endregion

    #region AsyncFunc
    public async Task AsyncFunc(string TextOfCommands)
    {
      Task task = Task.Run(() =>
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          _isRunning = true;
        });
        TuringService turingService = new TuringService(TextOfCommands);
        Thread.Sleep(_speed);
        if (turingService.IsErrorInText())
        {
          Alarm("Error at line " + turingService.GetLineWithError().ToString(), "Word Processor", MessageBoxButton.OK, MessageBoxImage.Warning);
          _isBroken = true;
          App.Current.Dispatcher.Invoke((Action)delegate
          {
            _isRunning = false;
          });
          return;
        }
        var code = turingService.GetFunctions();
        foreach (var command in code)
          if (command.currentFunction == _currentFunction)
          {
            if (command.findThis == TextBoxes[_iterator].Text)
            {
              App.Current.Dispatcher.Invoke((Action)delegate
              {
                TextBoxes[_iterator].Text = command.replaceWith;
              });
              _currentFunction = command.nextFunction;
              if (_currentFunction.ToLower() == "s")
              {
                _isBroken = true;
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                  _isRunning = false;
                });
              }
              App.Current.Dispatcher.Invoke((Action)delegate
              {
                TextBoxes[_iterator].Colour = new SolidColorBrush();
              });
              _prev = _iterator;
              if (command.direction.ToLower() == "l")
                _iterator--;
              if (command.direction.ToLower() == "r")
                _iterator++;
              if (command.direction.ToLower() != "l" && command.direction.ToLower() != "r" && command.direction.ToLower() != "h")
              {
                Alarm($"Unknown direcion of movement at {_iterator} index", "Word Processor", MessageBoxButton.OK, MessageBoxImage.Warning);
                _isBroken = true;
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                  _isRunning = false;
                });
                return;
              }
              if (_iterator == TextBoxes.Count)
              {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                  TextBoxes.Add(new TextBoxViewModel()
                  {
                    Text = "_",
                    Colour = new SolidColorBrush(),
                  });
                });
              }
              if (_iterator < 0)
              {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                  TextBoxes.Add(new TextBoxViewModel()
                  {
                    Text = "_",
                    Colour = new SolidColorBrush(),
                  });
                  for (int i = TextBoxes.Count - 1; i > 0; i--)
                  {
                    TextBoxes[i] = TextBoxes[i - 1].Clone();
                  }
                  TextBoxes[0].Text = "_";
                  TextBoxes[0].Colour = new SolidColorBrush();
                  _iterator = 0;
                  _prev = 1;
                });
              }
              App.Current.Dispatcher.Invoke((Action)delegate
              {
                TextBoxes[_iterator].Colour = new SolidColorBrush(Colors.Green);
              });
              App.Current.Dispatcher.Invoke((Action)delegate
              {
                _isRunning = false;
              });
              return;
            }
          }
        Alarm($"There is no command for \"{_currentFunction}\" function", "Word Processor", MessageBoxButton.OK, MessageBoxImage.Warning);
        _isBroken = true;
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          _isRunning = false;
        });
      });
      await task;
    }
    #endregion
  }
}
