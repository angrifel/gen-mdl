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

namespace ModelGenerator.TypeScript
{
  using System;
  using System.IO;

  public class TypeScriptReExportStatement : TypeScriptDeclarationOrStatement, IEquatable<TypeScriptReExportStatement>
  {
    public string Object { get; set; }

    public string Alias { get; set; }

    public string FileName { get; set; }

    public override void Generate(TextWriter output)
    {
      if (Object == null)
      {
        output.WriteLine($"export * from './{FileName}';");
      }
      else
      {
        output.Write($"export {{ {Object}");
        if (Alias != null)
        {
          output.Write($" as {Alias}");
        }

        output.WriteLine($" }} from './{FileName}';");
      }
    }

    public bool Equals(TypeScriptReExportStatement other) =>
      other != null &&
      Object == other.Object &&
      Alias == other.Alias &&
      FileName == other.FileName;

    public override bool Equals(TypeScriptDeclarationOrStatement other) => Equals(other as TypeScriptReExportStatement);

    public override bool Equals(object obj) => Equals(obj as TypeScriptReExportStatement);

    public override int GetHashCode() => 
      unchecked(
        (Object?.GetHashCode() ?? 0) + 
        (Alias?.GetHashCode() ?? 0) +
        (FileName?.GetHashCode() ?? 0));
  }
}
