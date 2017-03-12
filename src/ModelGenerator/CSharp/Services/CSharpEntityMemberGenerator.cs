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

namespace ModelGenerator.CSharp.Services
{
  using ModelGenerator.Model;
  using System;
  using System.Collections.Generic;

  public class CSharpEntityMemberGenerator : ICSharpEntityMemberGenerator
  {
    private static readonly string[] StructNativeTypes = new string[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "System.Guid", "System.DateTime", "System.TimeSpan", "System.DateTimeOffset" };

    private readonly SpecAnalyzer _specAnalizer;

    public CSharpEntityMemberGenerator(SpecAnalyzer specAnalizer)
    {
      if (specAnalizer == null) throw new ArgumentNullException(nameof(specAnalizer));
      _specAnalizer = specAnalizer;
    }

    public CSharpClassMember GenerateEntityMember(KeyValuePair<string, IEntityMemberInfo> member)
    {
      var resolvedType = _specAnalizer.GetResolvedType(Constants.CSharpTarget, member.Value.Type);
      var implementationType = _specAnalizer.IsNativeType(Constants.CSharpTarget, resolvedType) ? resolvedType : SpecFunctions.ToPascalCase(resolvedType);
      var isValueType = IsValueType(implementationType) || _specAnalizer.Spec.Enums.ContainsKey(resolvedType);
      var memberType = member.Value.IsCollection
        ? "System.Collections.Generic.IList<" + implementationType + ">"
        : implementationType + (member.Value.IsNullable && isValueType ? "?" : string.Empty);

      var memberName = SpecFunctions.ToPascalCase(member.Key);
      var isString = resolvedType == "string";
      return new CSharpClassMember
      {
        Type = memberType,
        Name = memberName,
        RequiredAttributeBehavior =
          !member.Value.IsNullable && !isValueType
            ? isString
              ? CSharpRequiredAttributeBehavior.IssueRequiredAllowEmptyStrings
              : CSharpRequiredAttributeBehavior.IssueRequired
            : CSharpRequiredAttributeBehavior.NoRequiredAttribute
      };
    }

    private static bool IsValueType(string type)
    {
      for (int i = 0; i < StructNativeTypes.Length; i++)
      {
        if (StructNativeTypes[i] == type)
        {
          return true;
        }
      }

      return false;
    }
  }
}
