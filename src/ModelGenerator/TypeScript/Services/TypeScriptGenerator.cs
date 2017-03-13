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
  using Model;
  using ModelGenerator.TypeScript.Utilities;
  using System;
  using System.Collections.Generic;
  using System.IO;

  public class TypeScriptGenerator : IGenerator
  {
    private readonly SpecAnalyzer _specAnalyzer;

    private readonly ITypeScriptEntityGeneratorFactory _typescriptEntityGeneratorFactory;

    public TypeScriptGenerator(SpecAnalyzer specAnalyzer, ITypeScriptEntityGeneratorFactory typescriptEntityGeneratorFactory)
    {
      _specAnalyzer = specAnalyzer ?? throw new ArgumentNullException(nameof(specAnalyzer));
      _typescriptEntityGeneratorFactory = typescriptEntityGeneratorFactory ?? throw new ArgumentNullException(nameof(typescriptEntityGeneratorFactory));
    }

    public IEnumerable<GeneratorOutput> GenerateOutputs()
    {
      var targetInfo = _specAnalyzer.Spec.Targets[Constants.TypeScriptTarget];
      var barrelContents = new List<TypeScriptDeclarationOrStatement>();
      var result = new GeneratorOutput[_specAnalyzer.Spec.Enums.Count + _specAnalyzer.Spec.Entities.Count + 1];
      var index = 0;
      foreach (var @enum in _specAnalyzer.Spec.Enums)
      {
        var enumFileName = TypeScriptFileUtilities.GetFileName(@enum.Key);
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
        var entityFileName = TypeScriptFileUtilities.GetFileName(entity.Key);
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
      return _typescriptEntityGeneratorFactory.CreateEntityGenerator(_specAnalyzer).GenerateEntity(entityName, entityMembers);
    }
  }
}
