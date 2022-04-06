using TuringMachine.ViewModels.Base;
using TuringMachine.Infrastructure.Commands;
using System.Windows.Media;

namespace TuringMachine.ViewModels
{
  public class TextBoxViewModel : ViewModel
  {
    #region Properties
    private string _text;
    public string Text { get => _text; set => Set(ref _text, value); }
    private System.Windows.Media.Brush _colour;
    public System.Windows.Media.Brush Colour { get => _colour; set { Set(ref _colour, value); } }
   // public SolidColorBrush color;
    public TextBoxViewModel Clone() => new TextBoxViewModel() { Text = this.Text, Colour = this.Colour };

    /*private void ChangeColor()
    {
      color = new SolidColorBrush(Colors.Green);
    }*/
    public TextBoxViewModel(string Text, System.Windows.Media.Brush Colour)
    {
      this.Text = Text;
      this.Colour = Colour;
    }
    public TextBoxViewModel()
    {

    }
    #endregion
  }
}
