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

  public class CSharpClassMember : IEquatable<CSharpClassMember>
  {
    public string Name { get; set; }

    public IList<string> Namespaces { get; set; }

    public string Type { get; set; }

    public CSharpRequiredAttributeBehavior RequiredAttributeBehavior { get; set; }

    public void PopulateNamespaces(IList<string> namespaces)
    {
      if (Namespaces != null)
      {
        for (int i = 0; i < Namespaces.Count; i++)
        {
          namespaces.AddIfNotExists(Namespaces[i]);
        }
      }

      if (
        RequiredAttributeBehavior == CSharpRequiredAttributeBehavior.IssueRequired || 
        RequiredAttributeBehavior == CSharpRequiredAttributeBehavior.IssueRequiredAllowEmptyStrings)
      {
        namespaces.AddIfNotExists("System.ComponentModel.DataAnnotations");
      }
    }

    public void Generate(TextWriter output)
    {
      switch (RequiredAttributeBehavior)
      {
        case CSharpRequiredAttributeBehavior.IssueRequiredAllowEmptyStrings:
          output.WriteLine($"    [Required(AllowEmptyStrings = true)]");
          break;
        case CSharpRequiredAttributeBehavior.IssueRequired:
          output.WriteLine($"    [Required]");
          break;
      }

      output.WriteLine($"    public {Type} {Name} {{ get; set; }}");
    }

    public bool Equals(CSharpClassMember other) =>
      other != null &&
      Name == other.Name &&
      Type == other.Type &&
      RequiredAttributeBehavior == other.RequiredAttributeBehavior &&
      (Namespaces == other.Namespaces || (Namespaces?.SequenceEqual(other.Namespaces, StringComparer.Ordinal)  ?? false));

    public override bool Equals(object obj) => Equals(obj as CSharpClassMember);

    public override int GetHashCode() => unchecked((Type?.GetHashCode() ?? 0) + Name?.GetHashCode() ?? 0);
  }
}
