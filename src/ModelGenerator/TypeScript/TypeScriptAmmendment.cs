namespace ModelGenerator.TypeScript
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using ModelGenerator.Model;

  public class TypeScriptAmmendment : ITargetAmmedment
  {
    public void AmmedSpecification(Spec spec)
    {
      if (!spec.Targets.ContainsKey("typescript")) return;
      var target = spec.Targets["typescript"];
      var ta = target.TypeAliases;
      ta.AddIfNotExists("bool", "boolean");
      ta.AddIfNotExists("byte", "number");
      ta.AddIfNotExists("short", "number");
      ta.AddIfNotExists("int", "number");
      ta.AddIfNotExists("long", "string");
      ta.AddIfNotExists("float", "number");
      ta.AddIfNotExists("double", "number");
      ta.AddIfNotExists("decimal", "string");
      ta.AddIfNotExists("char", "string");
      ta.AddIfNotExists("string", "string");
      ta.AddIfNotExists("guid", "string");
      ta.AddIfNotExists("date", "Date");
      ta.AddIfNotExists("time", "Date");
      ta.AddIfNotExists("datetime", "Date");
      ta.AddIfNotExists("object", "any");
      target.NativeTypes = new HashSet<string>(new[] { "boolean", "number", "string", "Date", "any" });
    }
  }
}
