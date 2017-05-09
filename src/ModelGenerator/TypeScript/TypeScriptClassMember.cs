﻿//  This file is part of genmdl - A Source code generator for model definitions.
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

  public class TypeScriptClassMember : IEquatable<TypeScriptClassMember>
  {
    public string Name { get; set; }

    public string Type { get; set; }

    public void Generate(TextWriter output)
    {
      output.WriteLine($"  {Name} : {Type};");
    }

    public bool Equals(TypeScriptClassMember other) =>
      other != null &&
      this.Name == other.Name && 
      this.Type == other.Type;

    public override bool Equals(object obj) => Equals(obj as TypeScriptClassMember);

    public override int GetHashCode() => unchecked((Name?.GetHashCode() ?? 0) + Type?.GetHashCode() ?? 0);
  }
}
