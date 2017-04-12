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
  using ModelGenerator.Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class SpecAnalyzer
  {
    private static readonly string[] _emptyArray = new string[0];
    private readonly Spec _spec;
    private Dictionary<string, Dictionary<string, string>> _resolvedAliasesInternal;

    public SpecAnalyzer(Spec spec)
    {
      _spec = spec;
    }

    public Spec Spec { get { return _spec; } }

    public IEnumerable<string> GetDirectEnumDependencies(string target, string type) =>
      IsEntity(type)
        ? _spec.Entities[type].Members.Where(_ => !_.Value.Exclude.Contains(target) && IsEnum(_.Value.Type)).Select(_ => _.Value.Type)
        : _emptyArray;

    public IEnumerable<string> GetDirectEntityDependencies(string target, string type) =>
      IsEntity(type)
        ? _spec.Entities[type].Members.Where(_ => !_.Value.Exclude.Contains(target) && IsEntity(_.Value.Type)).Select(_ => _.Value.Type)
        : _emptyArray;

    public bool IsTypeResolvable(string target, string type) =>
      ResolvedAliases[target].ContainsKey(type) ||
      IsEnum(type) ||
      IsEntity(type);

    public bool IsNativeType(string target, string type) => _spec.Targets[target].NativeTypes.Contains(type);

    public string GetResolvedType(string target, string type) =>
      ResolvedAliases.ContainsKey(target)
        ? ResolvedAliases[target].ContainsKey(type)
          ? ResolvedAliases[target][type]
          : IsEnum(type) || IsEntity(type)
            ? type
            : null
        : null;

    private Dictionary<string, Dictionary<string, string>> ResolvedAliases
    {
      get
      {
        if (_resolvedAliasesInternal == null)
        {
          var ra = new Dictionary<string, Dictionary<string, string>>();

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

            ra.Add(target, resolvedAliases);
          }

          _resolvedAliasesInternal = ra;
        }

        return _resolvedAliasesInternal;
      }
    }

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
