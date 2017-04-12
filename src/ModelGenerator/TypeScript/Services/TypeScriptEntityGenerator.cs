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
  using System;
  using System.Collections.Generic;

  public class TypeScriptEntityGenerator : ITypeScriptEntityGenerator
  {
    private readonly SpecAnalyzer _specAnalyzer;

    private readonly ITypeScriptEntityMemberGeneratorFactory _typeScriptEntityMemberGeneratorFactory;

    public TypeScriptEntityGenerator(SpecAnalyzer specAnalyzer, ITypeScriptEntityMemberGeneratorFactory typeScriptEntityMemberGeneratorFactory)
    {
      _specAnalyzer = specAnalyzer ?? throw new ArgumentNullException(nameof(specAnalyzer));
      _typeScriptEntityMemberGeneratorFactory = typeScriptEntityMemberGeneratorFactory ?? throw new ArgumentNullException(nameof(typeScriptEntityMemberGeneratorFactory));
    }

    public IEnumerable<TypeScriptDeclarationOrStatement> GetImportStatementsForEntity(string entityName, IDictionary<string, IEntityMemberInfo> entityMembers)
    {
      var targetInfo = _specAnalyzer.Spec.Targets[Constants.TypeScriptTarget];
      var enumDependencies = _specAnalyzer.GetDirectEnumDependencies(Constants.TypeScriptTarget, entityName);
      var entityDependencies = _specAnalyzer.GetDirectEntityDependencies(Constants.TypeScriptTarget, entityName);

      foreach (var @enum in enumDependencies)
      {
        yield return new TypeScriptImportStatement { ObjectName = SpecFunctions.ToPascalCase(@enum), File = TypeScriptFileUtilities.GetFileName(@enum, targetInfo.AppendGeneratedExtension) };
      }

      foreach (var entity in entityDependencies)
      {
        yield return new TypeScriptImportStatement { ObjectName = SpecFunctions.ToPascalCase(entity), File = TypeScriptFileUtilities.GetFileName(entity, targetInfo.AppendGeneratedExtension) };
      }
    }

    public TypeScriptClass GenerateEntity(string entityName, IDictionary<string, IEntityMemberInfo> entityMembers)
    {
      var members = new List<TypeScriptClassMember>(entityMembers.Count);
      var typeScriptEntityMemberGenerator = _typeScriptEntityMemberGeneratorFactory.CreateTypeScriptEntityGenerator(_specAnalyzer);
      foreach (var entityMember in entityMembers)
      {
        if (!entityMember.Value.Exclude.Contains(Constants.TypeScriptTarget))
        {
          members.Add(typeScriptEntityMemberGenerator.GenerateEntityMember(entityMember));
        }
      }

      return new TypeScriptClass
      {
        Name = SpecFunctions.ToPascalCase(entityName),
        Members = members
      };
    }
  }
}
