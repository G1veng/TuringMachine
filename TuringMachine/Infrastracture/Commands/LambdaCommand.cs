using TuringMachine.Infrastructure.Commands.Base;
using System;
using System.Threading.Tasks;

namespace TuringMachine.Infrastructure.Commands
{
  internal class LambdaCommand : Command
  {
    private readonly Action<object> _Execute;
    private readonly Func<object, bool> _CanExecute;
    public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
    {
      _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
      _CanExecute = CanExecute;
    }
    public virtual async void AsyncExecute(object parameter)
    {
      Task task = Task.Run(()=>_Execute(parameter)
      );
      await task;
    }
    public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
    public override void Execute(object parameter) => _Execute(parameter);
  }
}
