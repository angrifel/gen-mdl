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

namespace ModelGenerator.TypeScript
{
  using ModelGenerator.Model;
  using System.Collections.Generic;

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
