namespace ModelGenerator
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public static class DictionaryExtensions
  {
    public static void AddIfNotExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
      if (dictionary.ContainsKey(key)) return;
      dictionary.Add(key, value);
    }
  }
}
