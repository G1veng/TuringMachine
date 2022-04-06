using System;
using System.Collections.Generic;
using TuringMachine.Models;

namespace TuringMachine.Services
{
  public class TuringService
  {
    private List<Function> _functions;
    private bool _isNotEnoughArguments = false;
    private int _lineWithError;
    public TuringService(string CommandText)
    {
      int counter = 0;
      List<string>  linesOfCommands = new List<string>();
      string[] tempCommands = CommandText.Split('\r');
      foreach (var str in tempCommands)
        linesOfCommands.Add(str.Trim('\n'));
      _functions = new List<Function>();
      foreach (var str in linesOfCommands)
      {
        counter++;
        if (str == string.Empty) continue;
        string[] temp = str.Split(' ');
        List<string> commandInList = new List<string>();
        foreach (var partOfFunction in temp)
        {
          if (partOfFunction == string.Empty) continue;
          commandInList.Add(partOfFunction);
        }
        if (commandInList.Count != 5)
        {
          _lineWithError = counter;
          _isNotEnoughArguments = true;
          return;
        }
        _functions.Add(new Function()
        {
          currentFunction = commandInList[0],
          findThis = commandInList[1],
          replaceWith = commandInList[2],
          direction = commandInList[3],
          nextFunction = commandInList[4]
        });
      }
    }
    public bool IsErrorInText()
    {
      if (_isNotEnoughArguments) return true;
      return false;
    }
    public int GetLineWithError() => _lineWithError;
    public List<Function> GetFunctions() => _functions;
  }
}
