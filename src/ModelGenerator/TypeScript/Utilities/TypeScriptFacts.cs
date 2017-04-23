namespace ModelGenerator.TypeScript.Utilities
{
  using System.Collections.Generic;

  public static class TypeScriptFacts
  {
    private static readonly string[] _nativeTypes = new string[] { "boolean", "number", "string", "Date", "any" };

    public static IEnumerable<string> GetNativeTypes() => _nativeTypes;

    public static bool IsNativeType(string type)
    {
      for (int i = 0; i < _nativeTypes.Length; i++)
      {
        if (type == _nativeTypes[i])
        {
          return true;
        }
      }

      return false;
    }
  }
}
