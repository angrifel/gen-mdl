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
  using System;
  using System.Linq;
  using System.Collections.Generic;
  using System.IO;

  public class TypeScriptFile : IGenerationRoot, IEquatable<TypeScriptFile>
  {
    public IList<TypeScriptDeclarationOrStatement> Contents { get; set; }

    public void Generate(TextWriter output)
    {
      if (Contents == null) return;

      var previousItemType = (Type)null;
      foreach (var item in Contents)
      {
        if (previousItemType != null && previousItemType != item.GetType())
        {
          output.WriteLine();
        }

        item.Generate(output);
        previousItemType = item.GetType();
      }
    }

    public bool Equals(TypeScriptFile other) =>
      other != null &&
      (Contents == other.Contents || Contents.SequenceEqual(other.Contents));

    public override bool Equals(object obj) => Equals(obj as TypeScriptFile);

    public override int GetHashCode() => base.GetHashCode();
  }
}
