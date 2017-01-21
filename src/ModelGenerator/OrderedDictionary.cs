using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelGenerator
{
  public class OrderedDictionary<TKey, TValue> : OrderedDictionary, IDictionary<TKey, TValue>
  {
    public TValue this[TKey key]
    {
      get { return (TValue)base[key]; }
      set { base[key] = value; }
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
    {
      get
      {
        var result = new List<TKey>(base.Keys.Count);
        foreach (var key in base.Keys)
        {
          result.Add((TKey)key);
        }

        return result;
      }
    }

    ICollection<TValue> IDictionary<TKey, TValue>.Values
    {
      get
      {
        var result = new List<TValue>(base.Values.Count);
        foreach (var value in base.Values)
        {
          result.Add((TValue)value);
        }

        return result;
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item) => base.Add(item.Key, item.Value);

    public void Add(TKey key, TValue value) => base.Add(key, value);

    public bool Contains(KeyValuePair<TKey, TValue> item) => base.Contains(item.Key) && base.Values.Cast<TValue>().Contains(item.Value);

    public bool ContainsKey(TKey key) => base.Contains(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      // NOTE: do not use base.CopyTo(Array array, int index). Items are not copied in order.

      var i = arrayIndex;
      foreach (var item in (IEnumerable<KeyValuePair<TKey, TValue>>)this)
      {
        if (i >= array.Length + arrayIndex) break;
        array[i++] = item;
      }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      if (Contains(item))
      {
        base.Remove(item.Key);
        return true;
      }

      return false;
    }

    public bool Remove(TKey key)
    {
      if (base.Contains(key))
      {
        base.Remove(key);
        return true;
      }

      return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      if (base.Contains(key))
      {
        value = (TValue)base[key];
        return true;
      }

      value = default(TValue);
      return false;
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      foreach (var key in base.Keys)
      {
        yield return new KeyValuePair<TKey, TValue>((TKey)key, (TValue)base[key]);
      }
    }
  }
}
