//  This file is part of genmdl - A Source code generator for model definitions.
//  Copyright (c) angrifel

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

namespace ModelGenerator.CSharp
{
  using System.Collections.Generic;
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
