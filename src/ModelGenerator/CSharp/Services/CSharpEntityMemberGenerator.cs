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

namespace ModelGenerator.CSharp.Services
{
  using ModelGenerator.CSharp.Utilities;
  using ModelGenerator.Model;
  using System.Collections.Generic;

  public class CSharpEntityMemberGenerator
  {
    public static CSharpClassMember GenerateEntityMember(Spec spec, string entity, string member)
    {
      var memberInfo = spec.Entities[entity].Members[member];
      var resolvedType = spec.GetResolvedType(Constants.CSharpTarget, memberInfo.Type);
      var implementationType = CSharpFacts.IsNativeType(resolvedType) ? resolvedType : SpecFunctions.ToPascalCase(resolvedType);
      var isValueType = CSharpFacts.IsStructNativeType(implementationType) || spec.Enums.ContainsKey(resolvedType);
      var memberType = memberInfo.IsCollection
        ? "IList<" + implementationType + ">"
        : implementationType + (memberInfo.IsNullable && isValueType ? "?" : string.Empty);

      var memberName = SpecFunctions.ToPascalCase(member);
      var isString = resolvedType == "string";
      var implementationNamespace = GetImplementationTypeNamespace(implementationType);
      var namespaces = (IList<string>)null;
      if (implementationNamespace != null)
      {
        namespaces = new List<string> { implementationNamespace };
        if (memberInfo.IsCollection)
        {
          namespaces.AddIfNotExists("System.Collections.Generic");
        }
      }
      else if (memberInfo.IsCollection)
      {
        namespaces = new List<string> { "System.Collections.Generic" };
      }

      return new CSharpClassMember
      {
        Namespaces = namespaces,
        Type = memberType,
        Name = memberName,
        RequiredAttributeBehavior =
          !memberInfo.IsNullable && !isValueType
            ? isString
              ? CSharpRequiredAttributeBehavior.IssueRequiredAllowEmptyStrings
              : CSharpRequiredAttributeBehavior.IssueRequired
            : CSharpRequiredAttributeBehavior.NoRequiredAttribute
      };
    }

    private static string GetImplementationTypeNamespace(string type)
    {
      switch (type)
      {
        case "Guid":
        case "DateTime":
        case "TimeSpan":
        case "DateTimeOffset":
          return "System";
        default: return null;
      }
    }
  }
}
