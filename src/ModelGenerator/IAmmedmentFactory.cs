namespace ModelGenerator
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public interface IAmmedmentFactory
  {
    ITargetAmmedment CreateAmmendment(string target);
  }
}
