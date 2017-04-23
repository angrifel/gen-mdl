namespace ModelGenerator.CSharp.Utilities
{
  using System.Collections.Generic;

  public class CSharpFacts
  {
    private readonly static string[] _nativeTypes = new[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "string", "Guid", "DateTime", "TimeSpan", "DateTimeOffset", "object" };

    private readonly static string[] _structNativeTypes = new[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "Guid", "DateTime", "TimeSpan", "DateTimeOffset" };

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

    public static bool IsStructNativeType(string type)
    {
      for (int i = 0; i < _structNativeTypes.Length; i++)
      {
        if (type == _structNativeTypes[i])
        {
          return true;
        }
      }

      return false;
    }
  }
}
