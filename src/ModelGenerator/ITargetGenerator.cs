using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelGenerator
{
  public interface ITargetGenerator
  {
    void Generate(string basePath, SpecInterpreter specInterpreter);
  }
}
