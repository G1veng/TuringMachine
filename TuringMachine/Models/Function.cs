using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringMachine.Models
{
  public class Function
  {
    public string currentFunction { get; set; }
    public string nextFunction { get; set; }
    public string findThis { get; set; }
    public string replaceWith { get; set; }
    public string direction { get; set; }
  }
}
