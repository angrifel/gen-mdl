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
          foreach (var member in (IDictionary<string, IEntityMemberInfo>)entity.Value)
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
