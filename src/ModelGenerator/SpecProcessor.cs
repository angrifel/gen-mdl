namespace ModelGenerator
{
  using CSharp;
  using Model;
  using System;
  using System.Collections;
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

    private static readonly Dictionary<string, ITargetGenerator> _targetGenerator = new Dictionary<string, ITargetGenerator> {
      { Constants.TypeScriptTarget, new TypeScriptGenerator() },
      { Constants.CSharpTarget, new CSharpGenerator() }
    };

    private SpecInterpreter _specInterpreter;

    public SpecProcessor(Spec spec)
    {
      foreach (var target in spec.Targets.Keys)
      {
        if (!_targetAmmendments.Keys.Contains(target))
        {
          throw new Exception($"Unsupported Target '{target}'");
        }
      }

      _spec = spec;
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

    public void ProcessSpecification()
    {
      foreach (var targetGenerator in _targetGenerator)
      {
        targetGenerator.Value.Generate(_specInterpreter);
      }
    }
  }
}
