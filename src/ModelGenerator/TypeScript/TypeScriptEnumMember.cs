﻿//  This file is part of gen-mdl - A Source code generator for model definitions.
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

  public class TypeScriptEnumMember : IEquatable<TypeScriptEnumMember>
  {
    public string Name { get; set; }

    public long? Value { get; set; }

    public void Generate(TextWriter output, bool isLastOne)
    {
      var separator = isLastOne ? string.Empty : ",";
      if (Value == null)
      {
        output.WriteLine($"  {Name}{separator}");
      }
      else
      {
        output.WriteLine($"  {Name} = {Value}{separator}");
      }
    }

    public bool Equals(TypeScriptEnumMember other) =>
      other != null &&
      Name == other.Name &&
      Value == other.Value;

    public override bool Equals(object obj) => Equals(obj as TypeScriptEnumMember);

    public override int GetHashCode() =>
      unchecked(
        (Name?.GetHashCode() ?? 0) +
        Value?.GetHashCode() ?? 0);
  }
}
