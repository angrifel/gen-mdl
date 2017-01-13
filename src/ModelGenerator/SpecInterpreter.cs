namespace ModelGenerator
{
  using ModelGenerator.Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class SpecInterpreter
  {
    private static readonly string[] _emptyArray = new string[0];
    private readonly Spec _spec;
    private readonly Dictionary<string, Dictionary<string, string>> _resolvedAliases;

    public SpecInterpreter(Spec spec)
    {
      _spec = spec;
      _resolvedAliases =  new Dictionary<string, Dictionary<string, string>>();

      foreach (var target in _spec.Targets.Keys)
      {
        var targetInfo = _spec.Targets[target];
        var resolvedAliases = new Dictionary<string, string>();
        var visitedAliases = new LinkedList<string>();
        foreach (var alias in targetInfo.TypeAliases.Keys)
        {
          visitedAliases.AddLast(alias);
          var resolvedType = targetInfo.TypeAliases[alias];
          while (!IsProperType(target, resolvedType))
          {
            if (visitedAliases.Contains(resolvedType))
            {
              throw CreateCircularReferenceException(target, resolvedType, visitedAliases);
            }

            visitedAliases.AddLast(resolvedType);
            resolvedType = targetInfo.TypeAliases[resolvedType];
          }

          resolvedAliases.Add(alias, resolvedType);
          visitedAliases.Clear();
        }

        _resolvedAliases.Add(target, resolvedAliases);
      }
    }

    public Spec Spec { get { return _spec; } }

    public IEnumerable<string> GetDirectEnumDependencies(string type) =>
      IsEntity(type)
        ? _spec.Entities[type].Where(_ => _spec.Enums.ContainsKey(_.Value)).Select(_ => _.Value)
        : _emptyArray;

    public IEnumerable<string> GetDirectEntityDependencies(string type) =>
      IsEntity(type)
        ? _spec.Entities[type].Where(_ => IsEntity(_.Value)).Select(_ => _.Value)
        : _emptyArray;

    public bool IsTypeResolvable(string target, string type) =>
      _resolvedAliases[target].ContainsKey(type) ||
      IsEnum(type) ||
      IsEntity(type);

    public bool IsNativeType(string target, string type) => _spec.Targets[target].NativeTypes.Contains(type);

    public string GetResolvedType(string target, string type) =>
      _resolvedAliases.ContainsKey(target)
        ? _resolvedAliases[target].ContainsKey(type)
          ? _resolvedAliases[target][type]
          : IsEnum(type) || IsEntity(type)
            ? type
            : null
        : null;

    private bool IsEntity(string type) => _spec.Entities.ContainsKey(type);

    private bool IsEnum(string type) => _spec.Enums.ContainsKey(type);

    private bool IsProperType(string target, string resolvedType) =>
      IsNativeType(target, resolvedType) || IsEntity(resolvedType) || IsEnum(resolvedType);

    private static Exception CreateCircularReferenceException(string target, string resolvedType, IEnumerable<string> visitedAliases)
    {
      var makeMessage = false;
      var errorBuilder = new StringBuilder();
      errorBuilder.Append($"{target} verification failed: circular reference found. ");
      foreach (var va in visitedAliases)
      {
        if (!makeMessage && va == resolvedType)
        {
          makeMessage = true;
        }

        if (makeMessage)
        {
          errorBuilder.Append($"'{va}' -> ");
        }
      }

      errorBuilder.Append($"'{resolvedType}'.");

      return new Exception(errorBuilder.ToString());
    }
  }
}
