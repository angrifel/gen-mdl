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

  public class TypeScriptAmmendment : ITargetAmmedment
  {
    public void AmmedSpecification(Spec spec)
    {
      if (!spec.Targets.ContainsKey(Constants.TypeScriptTarget)) return;
      var target = spec.Targets[Constants.TypeScriptTarget];
      var ta = target.TypeAliases;
      string getExceptionMessage(string builtInType) => "builtin type '" + builtInType + "' cannot be overriden.";

      ta.Add("bool", "boolean", getExceptionMessage);
      ta.Add("byte", "number", getExceptionMessage);
      ta.Add("short", "number", getExceptionMessage);
      ta.Add("int", "number", getExceptionMessage);
      ta.Add("long", "string", getExceptionMessage);
      ta.Add("float", "number", getExceptionMessage);
      ta.Add("double", "number", getExceptionMessage);
      ta.Add("decimal", "string", getExceptionMessage);
      ta.Add("char", "string", getExceptionMessage);
      ta.Add("string", "string", getExceptionMessage);
      ta.Add("guid", "string", getExceptionMessage);
      ta.Add("date", "Date", getExceptionMessage);
      ta.Add("time", "Date", getExceptionMessage);
      ta.Add("datetime", "Date", getExceptionMessage);
      ta.Add("object", "any", getExceptionMessage);
    }
  }
}
