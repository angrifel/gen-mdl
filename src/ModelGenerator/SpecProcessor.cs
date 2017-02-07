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

namespace ModelGenerator
{
  using CSharp;
  using Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using TypeScript;

  public class SpecProcessor
  {
    private readonly Spec _spec;
    
    private static readonly Dictionary<string, ITargetAmmedment> _targetAmmendments = new Dictionary<string, ITargetAmmedment> {
      { Constants.TypeScriptTarget, new TypeScriptAmmendment() },
      { Constants.CSharpTarget, new CSharpAmmendment() },
    };

    private static readonly string[] Targets = new[] { Constants.CSharpTarget, Constants.TypeScriptTarget };

    private SpecInterpreter _specInterpreter;

    private IGeneratorFactory _generatorFactory;

    public SpecProcessor(Spec spec, IGeneratorFactory generatorFactory)
    {
      foreach (var target in spec.Targets.Keys)
      {
        if (!_targetAmmendments.Keys.Contains(target))
        {
          throw new Exception($"Unsupported Target '{target}'");
        }
      }

      _spec = spec;
      _generatorFactory = generatorFactory;
    }

    public void AmmedSpecification()
    {
      foreach (var target in _spec.Targets.Keys)
      {
        _targetAmmendments[target].AmmedSpecification(_spec);
      }
    }

    public void VerifySpecification()
    {
      _specInterpreter = new SpecInterpreter(_spec);

      foreach (var target in _spec.Targets.Keys)
      {
        var targetInfo = _spec.Targets[target];

        foreach (var entity in _spec.Entities)
        {
          foreach (var member in (IDictionary<string, Alternative<string, EntityMemberInfo>>)entity.Value)
          {
            if (!_specInterpreter.IsTypeResolvable(target, member.Value.GetMemberType()))
            {
              throw new Exception($"{target} verification failed: Unrecognized type '{member.Value}' in '{entity.Key}.{member.Key}'.");
            }
          }
        }
      }
    }

    public IEnumerable<GeneratorOutput> GenerateOutput()
    {
      foreach (var target in Targets)
      {
        var generator = _generatorFactory.CreateGenerator(target, _specInterpreter);
        foreach (var output in generator.GenerateOutputs())
        {
          yield return output;
        }
      }
    }
  }
}
