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
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class TypeScriptGenerator : IGenerator
  {

    public IEnumerable<GeneratorOutput> GenerateOutputs(Spec spec)
    {
      var targetInfo = spec.Targets[Constants.TypeScriptTarget];
      var barrelContents = new List<TypeScriptDeclarationOrStatement>();
      var typeCount = spec.Enums.Count + spec.Entities.Count;
      if (typeCount == 0) return Enumerable.Empty<GeneratorOutput>();

      var result = new GeneratorOutput[typeCount + 1];
      var index = 0;
      foreach (var @enum in spec.Enums)
      {
        var enumOutput = GenerateEnum(spec, @enum.Key);
        result[index++] = enumOutput;
        barrelContents.Add(new TypeScriptReExportStatement { FileName = Path.GetFileNameWithoutExtension(enumOutput.Path) });
      }

      foreach (var entity in spec.Entities)
      {
        var entityFileName = TypeScriptFileUtilities.GetFileName(entity.Key, targetInfo.AppendGeneratedExtension);
        result[index++] =
          new GeneratorOutput
          {
            Path = Path.Combine(targetInfo.Path, entityFileName + "." + Constants.TypeScriptExtension),
            GenerationRoot = GenerateEntity(spec, entity.Key)
          };

        barrelContents.Add(new TypeScriptReExportStatement { FileName = entityFileName });
      }

      result[index++] =
        new GeneratorOutput
        {
          // index.ts must always have '.ts' extension. '.generated.ts' is not applicable here
          // because it won't be recognized by module definition
          Path = Path.Combine(targetInfo.Path, Path.ChangeExtension("index", Constants.TypeScriptExtension)),
          GenerationRoot = new TypeScriptFile { Contents = barrelContents }
        };

      return result;

    }

    private static GeneratorOutput GenerateEnum(Spec spec, string @enum)
    {
      var targetInfo = spec.Targets[Constants.TypeScriptTarget];
      var enumFileName = TypeScriptFileUtilities.GetFileName(@enum, targetInfo.AppendGeneratedExtension);
      var enumMembers = spec.Enums[@enum];
      var normalizedEnumName = SpecFunctions.ToPascalCase(@enum);
      var members = new List<TypeScriptEnumMember>(enumMembers.Count);
      for (int i = 0; i < enumMembers.Count; i++)
      {
        members.Add(GenerateEnumMember(enumMembers[i]));
      }

      return new GeneratorOutput
      {
        Path = Path.Combine(targetInfo.Path, enumFileName + "." + Constants.TypeScriptExtension),
        GenerationRoot = new TypeScriptFile
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

    private TypeScriptFile GenerateEntity(Spec spec, string entityName)
    {
      var fileContents = new List<TypeScriptDeclarationOrStatement>();

      fileContents.AddRange(TypeScriptEntityGenerator.GetImportStatementsForEntity(spec, entityName));
      fileContents.Add(
        new TypeScriptExportStatement
        {
          IsDefault = true,
          TypeDeclaration =TypeScriptEntityGenerator.GenerateEntity(spec, entityName)
        });

      return new TypeScriptFile { Contents = fileContents };
    }
  }
}
