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
  using ModelGenerator.CSharp.Utilities;
  using ModelGenerator.Model;
  using ModelGenerator.TypeScript.Utilities;
  using System.Collections.Generic;
  using System.Linq;

  public static class SpecExtensions
  {
    private static readonly string[] _emptyArray = new string[0];

    public static IEnumerable<string> GetDirectEnumDependencies(this Spec spec, string target, string type) =>
      spec.IsEntity(type)
        ? spec.Entities[type].Members.Where(_ => !_.Value.Exclude.Contains(target) && spec.IsEnum(_.Value.Type)).Select(_ => _.Value.Type)
        : _emptyArray;

    public static IEnumerable<string> GetDirectEntityDependencies(this Spec spec, string target, string type) =>
      spec.IsEntity(type)
        ? spec.Entities[type].Members.Where(_ => !_.Value.Exclude.Contains(target) && spec.IsEntity(_.Value.Type)).Select(_ => _.Value.Type)
        : _emptyArray;

    public static bool IsTypeResolvable(this Spec spec, string target, string type) =>
      spec.ResolvedAliases[target].ContainsKey(type) ||
      spec.IsEnum(type) ||
      spec.IsEntity(type);

    public static string GetResolvedType(this Spec spec, string target, string type) =>
      spec.ResolvedAliases.ContainsKey(target)
        ? spec.ResolvedAliases[target].ContainsKey(type)
          ? spec.ResolvedAliases[target][type]
          : spec.IsEnum(type) || spec.IsEntity(type)
            ? type
            : null
        : null;

    public static bool IsEntity(this Spec spec, string type) => spec.Entities.ContainsKey(type);

    public static bool IsEnum(this Spec spec, string type) => spec.Enums.ContainsKey(type);

    public static bool IsProperType(this Spec spec, string target, string resolvedType) =>
      (
        (target == Constants.CSharpTarget && CSharpFacts.IsNativeType(resolvedType)) || 
        (target == Constants.TypeScriptTarget && TypeScriptFacts.IsNativeType(resolvedType))
      ) || 
      spec.IsEntity(resolvedType) || 
      spec.IsEnum(resolvedType);
  }
}
