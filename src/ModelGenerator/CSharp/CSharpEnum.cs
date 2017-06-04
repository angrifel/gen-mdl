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

  public class CSharpEnum : CSharpType, IEquatable<CSharpEnum>
  {
    public string Name { get; set; }

    public IList<CSharpEnumMember> Members { get; set; }

    public override void Generate(TextWriter output)
    {
      output.WriteLine($"  public enum {Name}");
      output.WriteLine(@"  {");

      for (int i = 0; i < Members.Count - 1; i++)
      {
        Members[i].Generate(output, false);
      }

      Members[Members.Count - 1].Generate(output, true);
      output.WriteLine(@"  }");
    }

    public override void PopulateNamespaces(IList<string> namespaces)
    {
      // always empty
    }

    public bool Equals(CSharpEnum other) =>
      other != null &&
      Name == other.Name &&
      (Members == other.Members || (Members?.SequenceEqual(other.Members) ?? false));

    public override bool Equals(CSharpType other) => Equals(other as CSharpEnum);

    public override bool Equals(object obj) => Equals(obj as CSharpEnum);

    public override int GetHashCode() => this.Name?.GetHashCode() ?? 0;
  }
}
