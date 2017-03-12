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

namespace ModelGenerator
{
  using System.Text;

  public static class SpecFunctions
  {
    public static string ToPascalCase(string specIdentifier)
    {
      var parts = specIdentifier.Split('_');
      var resultBuilder = new StringBuilder();
      for (int i = 0; i < parts.Length; i++)
      {
        resultBuilder.Append(char.ToUpperInvariant(parts[i][0]));
        resultBuilder.Append(parts[i].Substring(1).ToLowerInvariant());
      }

      return resultBuilder.ToString();
    }

    public static string ToCamelCase(string specIdentifier)
    {
      var parts = specIdentifier.Split('_');
      var resultBuilder = new StringBuilder();
      resultBuilder.Append(parts[0].ToLowerInvariant());
      for (int i = 1; i < parts.Length; i++)
      {
        resultBuilder.Append(char.ToUpperInvariant(parts[i][0]));
        resultBuilder.Append(parts[i].Substring(1).ToLowerInvariant());
      }

      return resultBuilder.ToString();
    }

    public static bool IsLowerCaseWithUnderscore(string identifier)
    {
      for (var i = 0; i < identifier.Length; i++)
      {
        var c = identifier[i];
        if (!(char.IsLower(c) || char.IsDigit(c)) && c != '_')
        {
          return false;
        }
      }

      return true;
    }

    public static string ToHyphenatedCase(string identifier)
    {
      var parts = identifier.Split('_');
      var resultBuilder = new StringBuilder();
      resultBuilder.Append(parts[0].ToLowerInvariant());
      for (int i = 1; i < parts.Length; i++)
      {
        resultBuilder.Append("-");
        resultBuilder.Append(parts[i].ToLowerInvariant());
      }

      return resultBuilder.ToString();
    }
  }
}
