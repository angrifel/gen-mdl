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
  using System.Linq;

  public class SpecTranslator
  {
    private static readonly string[] Targets = new[] { Constants.CSharpTarget, Constants.TypeScriptTarget };

    private readonly ISpecSource _specSource;

    private readonly IGeneratorFactory _generatorFactory;

    private readonly IAmmedmentFactory _ammendmentFactory;

    private SpecAnalyzer _specAnalyzer;

    public SpecTranslator(ISpecSource specSource, IGeneratorFactory generatorFactory, IAmmedmentFactory ammendmentFactory)
    {
      _specSource = specSource;
      _generatorFactory = generatorFactory;
      _ammendmentFactory = ammendmentFactory;
    }

    public IEnumerable<GeneratorOutput> GetOutput()
    {
      var spec = _specSource.GetSpec();
      _specAnalyzer = new SpecAnalyzer(spec);

      foreach (var target in spec.Targets.Keys)
      {
        if (!Targets.Contains(target))
        {
          throw new Exception($"Unsupported Target '{target}'");
        }
      }

      AmmedSpecification(spec);
      VerifySpecification(spec);

      foreach (var target in Targets)
      {
        var generator = _generatorFactory.CreateGenerator(target, _specAnalyzer);
        foreach (var output in generator.GenerateOutputs())
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
            if (!_specAnalyzer.IsTypeResolvable(target, member.Value.Type))
            {
              throw new Exception($"{target} verification failed: Unrecognized type '{member.Value}' in '{entity.Key}.{member.Key}'.");
            }
          }
        }
      }
    }
  }
}
