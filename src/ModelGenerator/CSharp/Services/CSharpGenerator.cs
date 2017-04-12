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
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;

  public class CSharpGenerator : IGenerator
  {
    private readonly SpecAnalyzer _specAnalizer;

    public CSharpGenerator(SpecAnalyzer specAnalizer)
    {
      _specAnalizer = specAnalizer ?? throw new ArgumentNullException(nameof(specAnalizer));
    }

    public IEnumerable<GeneratorOutput> GenerateOutputs()
    {
      var targetInfo = _specAnalizer.Spec.Targets[Constants.CSharpTarget];
      var result = new GeneratorOutput[_specAnalizer.Spec.Enums.Count + _specAnalizer.Spec.Entities.Count];
      var index = 0;
      foreach (var @enum in _specAnalizer.Spec.Enums)
      {
        result[index++] =
          new GeneratorOutput
          {
            Path = Path.Combine(targetInfo.Path, GetFilename(@enum.Key, targetInfo.AppendGeneratedExtension) + "." + Constants.CSharpExtension),
            GenerationRoot = GenerateEnum(targetInfo.Namespace, @enum.Key, @enum.Value)
          };
      }

      foreach (var entity in _specAnalizer.Spec.Entities)
      {
        result[index++] =
          new GeneratorOutput
          {
            Path = Path.Combine(targetInfo.Path, GetFilename(entity.Key, targetInfo.AppendGeneratedExtension) + "." + Constants.CSharpExtension),
            GenerationRoot = GenerateEntity(entity.Key)
          };
      }

      return result;
    }

    private static IGenerationRoot GenerateEnum(string @namespace, string enumName, IList<EnumMember> enumMembers)
    {
      var members = new List<CSharpEnumMember>(enumMembers.Count);
      foreach (var member in enumMembers)
      {
        members.Add(GenerateEnumMember(member));
      }

      return new CSharpNamespace
      {
        Name = @namespace,
        Types = new List<CSharpType>
        {
          new CSharpEnum
          {
            Name = SpecFunctions.ToPascalCase(enumName),
            Members = members
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

    private IGenerationRoot GenerateEntity(string entityName)
    {
      var namespaces = new List<string>();
      var csharpClass = CSharpEntityGenerator.GenerateEntity(_specAnalizer.Spec, entityName);
      csharpClass.PopulateNamespaces(namespaces);
      namespaces.Sort();

      return new CSharpNamespace
      {
        Name = _specAnalizer.Spec.Targets[Constants.CSharpTarget].Namespace,
        Namespaces = namespaces,
        Types = new List<CSharpType> { csharpClass }
      };

    }

    private static string GetFilename(string type, bool appendGeneratedExtension) =>
      SpecFunctions.ToPascalCase(type) + (appendGeneratedExtension ? ".generated" : string.Empty);
  }
}
