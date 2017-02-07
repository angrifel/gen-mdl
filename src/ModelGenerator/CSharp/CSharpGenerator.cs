//  This file is part of mdlgen - A Source code generator for model definitions.
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

namespace ModelGenerator.CSharp
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class CSharpGenerator : IGenerator
  {
    private static readonly string[] StructNativeTypes = new string[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "System.Guid", "System.DateTime", "System.TimeSpan", "System.DateTimeOffset" };

    private readonly SpecInterpreter _specInterpreter;

    public CSharpGenerator(SpecInterpreter specInterpreter)
    {
      if (specInterpreter == null) throw new ArgumentNullException(nameof(specInterpreter));
      _specInterpreter = specInterpreter;
    }

    public IEnumerable<GeneratorOutput> GenerateOutputs()
    {
      var targetInfo = _specInterpreter.Spec.Targets[Constants.CSharpTarget];
      foreach (var @enum in _specInterpreter.Spec.Enums)
      {
        yield return new GeneratorOutput
        {
          Path = Path.Combine(targetInfo.Path, Path.ChangeExtension(GetFilename(@enum.Key), Constants.CSharpExtension)),
          GenerationRoot = GenerateEnum(targetInfo.Namespace, @enum.Key, @enum.Value)
        };
      }

      foreach (var entity in _specInterpreter.Spec.Entities)
      {
        yield return new GeneratorOutput
        {
          Path = Path.Combine(targetInfo.Path, Path.ChangeExtension(GetFilename(entity.Key), Constants.CSharpExtension)),
          GenerationRoot = GenerateEntity(entity.Key, entity.Value)
        };
      }
    }

    private static IGenerationRoot GenerateEnum(string @namespace, string enumName, IList<EnumMember> enumMembers)
    {
      return new CSharpNamespace
      {
        Name = @namespace,
        Types = new List<CSharpType>
        {
          new CSharpEnum
          {
            Name = SpecFunctions.ToPascalCase(enumName),
            Members = enumMembers.Select(GenerateEnumMember).ToList()
          }
        }
      };
    }

    private static CSharpEnumMember GenerateEnumMember(EnumMember member)
    {
      return new CSharpEnumMember
      {
        Name = SpecFunctions.ToPascalCase(member.Name),
        Value = member.Value
      };
    }

    private IGenerationRoot GenerateEntity(string entityName, IDictionary<string, Alternative<string, EntityMemberInfo>> entityMembers)
    {
      return new CSharpNamespace
      {
        Name = _specInterpreter.Spec.Targets[Constants.CSharpTarget].Namespace,
        Types = new List<CSharpType>
        {
          new CSharpClass
          {
            Name = SpecFunctions.ToPascalCase(entityName),
            Members = entityMembers.Select(_ => GenerateEntityMember(_.Key, _.Value)).ToList()
          }
        }
      };
    }

    private CSharpClassMember GenerateEntityMember(string name, Alternative<string, EntityMemberInfo> valueOrEntityMemberInfoAlternative)
    {
      var specType = valueOrEntityMemberInfoAlternative.GetMemberType();
      var isNullable = valueOrEntityMemberInfoAlternative.GetIsNullable();
      var resolvedType = _specInterpreter.GetResolvedType(Constants.CSharpTarget, specType);
      var normalizedType = _specInterpreter.IsNativeType(Constants.CSharpTarget, specType) ? specType : SpecFunctions.ToPascalCase(resolvedType);
      var isStruct = IsStruct(normalizedType);
      var memberType = normalizedType + (isStruct && isNullable ? "?" : string.Empty);
      var memberName = SpecFunctions.ToPascalCase(name);
      return new CSharpClassMember { Type = memberType, Name = memberName };
    }
    
    private static string GetFilename(string type) => SpecFunctions.ToPascalCase(type);

    private static bool IsStruct(string type) => StructNativeTypes.Contains(type);
  }
}
