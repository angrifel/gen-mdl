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

namespace ModelGenerator.TypeScript.Services
{
  using ModelGenerator.Model;
  using ModelGenerator.TypeScript.Utilities;
  using System.Collections.Generic;

  public class TypeScriptEntityMemberGenerator
  {
    public static TypeScriptClassMember GenerateEntityMember(Spec spec, KeyValuePair<string, IEntityMemberInfo> member)
    {
      var resolvedType = spec.GetResolvedType(Constants.TypeScriptTarget, member.Value.Type);
      var normalizedType = TypeScriptFacts.IsNativeType(resolvedType) ? resolvedType : SpecFunctions.ToPascalCase(resolvedType);
      var normalizedMemberName = SpecFunctions.ToCamelCase(member.Key);
      if (member.Value.IsCollection)
      {
        normalizedType += "[]";
      }

      return new TypeScriptClassMember { Name = normalizedMemberName, Type = normalizedType };
    }
  }
}
