using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelGenerator.Tests
{
  public static class EnumerableExtensions
  {
    public static bool HasContents<TSource>(this IEnumerable<TSource> source, params Func<TSource, bool>[] predicates)
    {
      var used = new bool[predicates.Length];
      var usedCount = predicates.Length;
      foreach (var item in source)
      {
        if (usedCount == 0) return false;
        for (int i = 0; i < predicates.Length; i++)
        {
          if (!used[i] && predicates[i](item))
          {
            used[i] = true;
            usedCount--;
            break;
          }
        }
      }

      return usedCount == 0;
    }
  }
}
