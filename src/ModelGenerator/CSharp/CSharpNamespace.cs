//  This file is part of gen-mdl - A Source code generator for model definitions.
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
  using System;
  using System.Linq;
  using System.Collections.Generic;
  using System.IO;

  public class CSharpNamespace : IGenerationRoot, IEquatable<CSharpNamespace>
  {
    public string Name { get; set; }

    public IList<string> Namespaces { get; set; }

    public IList<CSharpType> Types { get; set; }

    public void Generate(TextWriter output)
    {
      output.WriteLine($"namespace {Name}");
      output.WriteLine(@"{");

      if (Types != null && Types.Count > 0)
      {
        if (Namespaces != null && Namespaces.Count != 0)
        {
          for (int i = 0; i < Namespaces.Count; i++)
          {
            output.WriteLine($"  using {Namespaces[i]};");
          }

          output.WriteLine();
        }

        Types[0].Generate(output);
        for (var i = 1; i < Types.Count; i++)
        {
          output.WriteLine();
          Types[i].Generate(output);
        }
      }

      output.WriteLine(@"}");
    }

    public bool Equals(CSharpNamespace other) =>
      other != null &&
      Name == other.Name &&
      (Namespaces == other.Namespaces || (Namespaces?.SequenceEqual(other.Namespaces) ?? false)) &&
      (Types == other.Types || (Types?.SequenceEqual(other.Types) ?? false));

    public bool Equals(IGenerationRoot other) => Equals(other as CSharpNamespace);

    public override bool Equals(object obj) => Equals(obj as CSharpNamespace);

    public override int GetHashCode() => Name?.GetHashCode() ?? 0;
  }
}
