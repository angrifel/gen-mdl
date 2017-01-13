namespace ModelGenerator.CSharp
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using ModelGenerator.Model;

  public class CSharpAmmendment : ITargetAmmedment
  {
    public void AmmedSpecification(Spec spec)
    {
      if (!spec.Targets.ContainsKey(Constants.CSharpTarget)) return;
      var target = spec.Targets[Constants.CSharpTarget];
      var ta = target.TypeAliases;

      ta.AddIfNotExists("bool", "bool");
      ta.AddIfNotExists("byte", "byte");
      ta.AddIfNotExists("short", "short");
      ta.AddIfNotExists("int", "int");
      ta.AddIfNotExists("long", "long");
      ta.AddIfNotExists("float", "float");
      ta.AddIfNotExists("double", "double");
      ta.AddIfNotExists("decimal", "decimal");
      ta.AddIfNotExists("char", "char");
      ta.AddIfNotExists("string", "string");
      ta.AddIfNotExists("guid", "System.Guid");
      ta.AddIfNotExists("date", "System.DateTime");
      ta.AddIfNotExists("time", "System.TimeSpan");
      ta.AddIfNotExists("datetime", "System.DateTimeOffset");
      ta.AddIfNotExists("object", "object");

      target.NativeTypes = new HashSet<string>(new[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "string", "System.Guid", "System.DateTime", "System.TimeSpan", "System.DateTimeOffset", "object" });
    }
  }
}
