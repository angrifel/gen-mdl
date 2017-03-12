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

namespace ModelGenerator.TypeScript
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;

  public class TypeScriptGenerator : IGenerator
  {
    private readonly SpecAnalyzer _specAnalyzer;
    
    public TypeScriptGenerator(SpecAnalyzer specAnalyzer)
    {
      if (specAnalyzer == null) throw new ArgumentNullException(nameof(specAnalyzer));
      _specAnalyzer = specAnalyzer;
    }

    public IEnumerable<GeneratorOutput> GenerateOutputs()
    {
      var targetInfo = _specAnalyzer.Spec.Targets[Constants.TypeScriptTarget];
      var barrelContents = new List<TypeScriptDeclarationOrStatement>();
      var result = new GeneratorOutput[_specAnalyzer.Spec.Enums.Count + _specAnalyzer.Spec.Entities.Count + 1];
      var index = 0;
      foreach (var @enum in _specAnalyzer.Spec.Enums)
      {
        var enumFileName = GetFileName(@enum.Key);
        result[index++] =
          new GeneratorOutput
          {
            Path = Path.Combine(targetInfo.Path, Path.ChangeExtension(enumFileName, Constants.TypeScriptExtension)),
            GenerationRoot = GenerateEnum(@enum.Key, @enum.Value)
          };

        barrelContents.Add(new TypeScriptReExportStatement { FileName = enumFileName });
      }

      foreach (var entity in _specAnalyzer.Spec.Entities)
      {
        var entityFileName = GetFileName(entity.Key);
        result[index++] =
          new GeneratorOutput
          {
            Path = Path.Combine(targetInfo.Path, Path.ChangeExtension(entityFileName, Constants.TypeScriptExtension)),
            GenerationRoot = GenerateEntity(entity.Key, entity.Value.Members)
          };

        barrelContents.Add(new TypeScriptReExportStatement { FileName = entityFileName });
      }

      result[index++] =
        new GeneratorOutput
        {
          Path = Path.Combine(targetInfo.Path, Path.ChangeExtension("index", Constants.TypeScriptExtension)),
          GenerationRoot = new TypeScriptFile { Contents = barrelContents }
        };

      return result;
    }

    private static TypeScriptFile GenerateEnum(string enumName, IList<EnumMember> enumMembers)
    {
      var normalizedEnumName = SpecFunctions.ToPascalCase(enumName);
      var members = new List<TypeScriptEnumMember>(enumMembers.Count);
      for (int i = 0; i < enumMembers.Count; i++)
      {
        members.Add(GenerateEnumMember(enumMembers[i]));
      }

      return new TypeScriptFile
      {
        Contents = new List<TypeScriptDeclarationOrStatement>
        {
          new TypeScriptEnum
          {
            Name = normalizedEnumName,
            Members = members
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
      var fileContents = new List<TypeScriptDeclarationOrStatement>();
      var enumDependencies = _specAnalyzer.GetDirectEnumDependencies(entityName);
      var entityDependencies = _specAnalyzer.GetDirectEntityDependencies(Constants.TypeScriptTarget, entityName);
      var members = new List<TypeScriptClassMember>(entityMembers.Count);
      foreach (var entityMember in entityMembers)
      {
        if (!entityMember.Value.Exclude.Contains(Constants.TypeScriptTarget))
        {
          members.Add(GenerateEntityMember(entityMember));
        }
      }

      foreach (var @enum in _specAnalyzer.GetDirectEnumDependencies(entityName))
      {
        fileContents.Add(new TypeScriptImportStatement { ObjectName = SpecFunctions.ToPascalCase(@enum), File = GetFileName(@enum) });
      }

      foreach (var entity in _specAnalyzer.GetDirectEntityDependencies(Constants.TypeScriptTarget, entityName))
      {
        fileContents.Add(new TypeScriptImportStatement { ObjectName = SpecFunctions.ToPascalCase(entity), File = GetFileName(entity) });
      }

      fileContents.Add(
        new TypeScriptExportStatement
        {
          IsDefault = true,
          TypeDeclaration =
              new TypeScriptClass
              {
                Name = normalizedEntityName,
                Members = members
              }
        });

      return 
        new TypeScriptFile
        {
          Contents = fileContents
        };
    }

    private TypeScriptClassMember GenerateEntityMember(KeyValuePair<string, IEntityMemberInfo> member)
    {
      var resolvedType = _specAnalyzer.GetResolvedType(Constants.TypeScriptTarget, member.Value.Type);
      var normalizedType = _specAnalyzer.IsNativeType(Constants.TypeScriptTarget, resolvedType) ? resolvedType : SpecFunctions.ToPascalCase(resolvedType);
      var normalizedMemberName = SpecFunctions.ToCamelCase(member.Key);
      if (member.Value.IsCollection)
      {
        normalizedType += "[]";
      }

      return new TypeScriptClassMember { Name = normalizedMemberName, Type = normalizedType };
    }

    private static string GetFileName(string type) => SpecFunctions.ToHyphenatedCase(type);
  }
}
