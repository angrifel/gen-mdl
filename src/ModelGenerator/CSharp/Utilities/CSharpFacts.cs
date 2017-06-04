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

namespace ModelGenerator.CSharp.Utilities
{
  using System.Collections.Generic;

  public class CSharpFacts
  {
    private readonly static string[] _nativeTypes = new[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "string", "Guid", "DateTime", "TimeSpan", "DateTimeOffset", "object" };

    private readonly static string[] _structNativeTypes = new[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "Guid", "DateTime", "TimeSpan", "DateTimeOffset" };

    public static IEnumerable<string> GetNativeTypes() => _nativeTypes;

    public static bool IsNativeType(string type)
    {
      for (int i = 0; i < _nativeTypes.Length; i++)
      {
        if (type == _nativeTypes[i])
        {
          return true;
        }
      }

      return false;
    }

    public static bool IsStructNativeType(string type)
    {
      for (int i = 0; i < _structNativeTypes.Length; i++)
      {
        if (type == _structNativeTypes[i])
        {
          return true;
        }
      }

      return false;
    }
  }
}
