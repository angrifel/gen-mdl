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

namespace ModelGenerator
{
  using Model;
  using System;
  using System.Collections.Generic;

  public class SpecTranslator
  {
    private static readonly string[] Targets = new[] { Constants.CSharpTarget, Constants.TypeScriptTarget };

    private readonly ISpecSource _specSource;

    private readonly IGeneratorFactory _generatorFactory;

    private readonly IAmmedmentFactory _ammendmentFactory;

    public SpecTranslator(ISpecSource specSource, IGeneratorFactory generatorFactory, IAmmedmentFactory ammendmentFactory)
    {
      _specSource = specSource;
      _generatorFactory = generatorFactory;
      _ammendmentFactory = ammendmentFactory;
    }

    public IEnumerable<GeneratorOutput> GetOutput()
    {
      var spec = _specSource.GetSpec();

      ValidateSpec(spec);
      AmmedSpecification(spec);
      VerifySpecification(spec);

      foreach (var target in Targets)
      {
        var generator = _generatorFactory.CreateGenerator(target);
        foreach (var output in generator.GenerateOutputs(spec))
        {
          yield return output;
        }
      }
    }

    private void AmmedSpecification(Spec spec)
    {
      foreach (var target in spec.Targets.Keys)
      {
        var ammendment = _ammendmentFactory.CreateAmmendment(target);
        ammendment.AmmedSpecification(spec);
      }
    }

    private void VerifySpecification(Spec spec)
    {
      foreach (var target in spec.Targets.Keys)
      {
        var targetInfo = spec.Targets[target];

        foreach (var entity in spec.Entities)
        {
          foreach (var member in (IDictionary<string, IEntityMemberInfo>)entity.Value.Members)
          {
            if (!spec.IsTypeResolvable(target, member.Value.Type))
            {
              throw new Exception($"{target} verification failed: Unrecognized type '{member.Value}' in '{entity.Key}.{member.Key}'.");
            }
          }
        }
      }
    }

    private static void ValidateSpec(Spec spec)
    {
      #region Local functions

      void validateIdentifier(string identifier, string errorLocationPrefix)
      {
        if (!SpecFunctions.IsSnakeCaseLowerCase(identifier))
        {
          throw new Exception(errorLocationPrefix + " must be composed of lowercase letters, digits and underscores only.");
        }

        if (!char.IsLetter(identifier[0]))
        {
          throw new Exception(errorLocationPrefix + " cannot start with a digit or underscore.");
        }
      }

      bool containsUnsupportedTarget(IList<string> targetList)
      {
        for (int i = 0; i < targetList.Count; i++)
        {
          if (!IsSupportedTarget(targetList[i]))
          {
            return true;
          }
        }

        return false;
      }

      #endregion

      foreach (var target in spec.Targets)
      {
        if (!IsSupportedTarget(target.Key))
        {
          throw new Exception($"Unsupported Target '{target.Key}'");
        }

        foreach (var typeAlias in target.Value.TypeAliases)
        {
          validateIdentifier(typeAlias.Key, $"'{target.Key}' type alias '{typeAlias.Key}'");
        }
      }

      foreach (var @enum in spec.Enums)
      {
        validateIdentifier(@enum.Key, $"Enum '{@enum.Key}'");

        foreach (var member in @enum.Value)
        {
          validateIdentifier(member.Name, $"Enum member '{@enum.Key}.{member.Name}'");
        }
      }

      foreach (var entity in spec.Entities)
      {
        validateIdentifier(entity.Key, $"Entity '{entity.Key}'");

        foreach (var member in (IDictionary<string, IEntityMemberInfo>)entity.Value.Members)
        {
          var prefix = $"Entity member '{entity.Key}.{member.Key}'";
          validateIdentifier(member.Key, $"Entity member '{entity.Key}.{member.Key}'");
          if (containsUnsupportedTarget(member.Value.Exclude))
          {
            throw new Exception(prefix + " excludes an unsupported target.");
          }
        }
      }
    }

    private static bool IsSupportedTarget(string target) => 
      target == Constants.CSharpTarget || target == Constants.TypeScriptTarget;
  }
}
