namespace ModelGenerator.Model
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class Alternative <T, U>
  {
    private readonly object _value;

    public Alternative(T first, U second)
    {
      _value = (object)first ?? second; 
    }

    public object Value { get { return _value; } }
  }
}
