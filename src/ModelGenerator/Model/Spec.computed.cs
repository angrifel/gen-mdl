//  This file is part of gen-mdl - A Source code generator for model definitions.
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

namespace ModelGenerator.Model
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using YamlDotNet.Serialization;

  public partial class Spec
  {
    private Dictionary<string, Dictionary<string, string>> _resolvedAliasesInternal;

    [YamlIgnore]
    internal Dictionary<string, Dictionary<string, string>> ResolvedAliases
    {
      get
      {
        #region CreateCircularReferenceException
        Exception CreateCircularReferenceException(string target, string resolvedType, IEnumerable<string> visitedAliases)
        {
          var makeMessage = false;
          var errorBuilder = new StringBuilder()
              .Append(target)
              .Append(" verification failed: circular reference found. ");
          foreach (var va in visitedAliases)
          {
            if (!makeMessage && va == resolvedType)
            {
              makeMessage = true;
            }

            if (makeMessage)
            {
              errorBuilder.Append("'").Append(va).Append("' -> ");
            }
          }

          errorBuilder.Append("'").Append(resolvedType).Append("'.");

          return new Exception(errorBuilder.ToString());
        }
        #endregion

        if (_resolvedAliasesInternal == null)
        {
          var ra = new Dictionary<string, Dictionary<string, string>>();

          foreach (var target in Targets.Keys)
          {
            var targetInfo = Targets[target];
            var resolvedAliases = new Dictionary<string, string>();
            var visitedAliases = new LinkedList<string>();
            foreach (var alias in targetInfo.TypeAliases.Keys)
            {
              visitedAliases.AddLast(alias);
              var resolvedType = targetInfo.TypeAliases[alias];
              while (!this.IsProperType(target, resolvedType))
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
  }
}
