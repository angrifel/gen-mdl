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

namespace ModelGenerator.TypeScript
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class TypeScriptGenerator : IGenerator
  {
    private readonly SpecAnalyzer _specInterpreter;
    
    public TypeScriptGenerator(SpecAnalyzer specInterpreter)
    {
      if (specInterpreter == null) throw new ArgumentNullException(nameof(specInterpreter));
      _specInterpreter = specInterpreter;
    }

    public IEnumerable<GeneratorOutput> GenerateOutputs()
    {
      var targetInfo = _specInterpreter.Spec.Targets[Constants.TypeScriptTarget];
      var barrelContents = new List<TypeScriptDeclarationOrStatement>();

      foreach (var @enum in _specInterpreter.Spec.Enums)
      {
        var enumFileName = GetFileName(@enum.Key);
        yield return 
          new GeneratorOutput
          {
            Path = Path.Combine(targetInfo.Path, Path.ChangeExtension(enumFileName, Constants.TypeScriptExtension)),
            GenerationRoot = GenerateEnum(@enum.Key, @enum.Value)
          };

        barrelContents.Add(new TypeScriptReExportStatement { FileName = enumFileName });
      }

      foreach (var entity in _specInterpreter.Spec.Entities)
      {
        var entityFileName = GetFileName(entity.Key);
        yield return
          new GeneratorOutput
          {
            Path = Path.Combine(targetInfo.Path, Path.ChangeExtension(entityFileName, Constants.TypeScriptExtension)),
            GenerationRoot = GenerateEntity(entity.Key, entity.Value)
          };

        barrelContents.Add(new TypeScriptReExportStatement { FileName = entityFileName });
      }

      yield return new GeneratorOutput
      {
        Path = Path.Combine(targetInfo.Path, Path.ChangeExtension("index", Constants.TypeScriptExtension)),
        GenerationRoot = new TypeScriptFile { Contents = barrelContents }
      };
    }

    private static TypeScriptFile GenerateEnum(string enumName, IList<EnumMember> enumMembers)
    {
      var normalizedEnumName = SpecFunctions.ToPascalCase(enumName);
      return new TypeScriptFile
      {
        Contents = new List<TypeScriptDeclarationOrStatement>
        {
          new TypeScriptEnum
          {
            Name = normalizedEnumName,
            Members = enumMembers.Select(GenerateEnumMember).ToList()
          },
          new TypeScriptExportStatement { IsDefault = true, Object = normalizedEnumName }
        }
      };
    }

    private static TypeScriptEnumMember GenerateEnumMember(EnumMember member)
    {
      return new TypeScriptEnumMember
      {
        Name = SpecFunctions.ToPascalCase(member.Name),
        Value = member.Value
      };
    }


    private TypeScriptFile GenerateEntity(string entityName, IDictionary<string, IEntityMemberInfo> entityMembers)
    {
      var normalizedEntityName = SpecFunctions.ToPascalCase(entityName);
      var enumDependencies = _specInterpreter.GetDirectEnumDependencies(entityName);
      var entityDependencies = _specInterpreter.GetDirectEntityDependencies(entityName);
      var directDependencies = enumDependencies.Concat(entityDependencies).ToList();
      var result = 
        new TypeScriptFile
        {
          Contents = directDependencies.Select(_ => (TypeScriptDeclarationOrStatement)(new TypeScriptImportStatement { ObjectName = SpecFunctions.ToPascalCase(_), File = GetFileName(_) })).ToList()
        };
      result.Contents.Add(
        new TypeScriptExportStatement
        {
          IsDefault = true,
          TypeDeclaration = 
            new TypeScriptClass
            {
              Name = normalizedEntityName,
              Members = entityMembers.Select(GenerateEntityMember).ToList()
            }
        });
      return result;
    }

    private TypeScriptClassMember GenerateEntityMember(KeyValuePair<string, IEntityMemberInfo> member)
    {
      var resolvedType = _specInterpreter.GetResolvedType(Constants.TypeScriptTarget, member.Value.Type);
      var normalizedType = _specInterpreter.IsNativeType(Constants.TypeScriptTarget, resolvedType) ? resolvedType : SpecFunctions.ToPascalCase(resolvedType);
      var normalizedMemberName = SpecFunctions.ToCamelCase(member.Key);
      return new TypeScriptClassMember { Name = normalizedMemberName, Type = normalizedType };
    }

    private static string GetFileName(string type) => SpecFunctions.ToHyphenatedCase(type);
  }
}
