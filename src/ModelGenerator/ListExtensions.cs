using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGenerator
{
  public static class ListExtensions
  {
    public static void AddIfNotExists<T>(this IList<T> list, T item)
    {
      if (!list.Contains(item))
      {
        list.Add(item);
      }
    }
  }
}
