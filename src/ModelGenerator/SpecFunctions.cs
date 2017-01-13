namespace ModelGenerator
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public static class SpecFunctions
  {
    public static string ToPascalCase(string specIdentifier)
    {
      var parts = specIdentifier.Split('_');
      var resultBuilder = new StringBuilder();
      for (int i = 0; i < parts.Length; i++)
      {
        resultBuilder.Append(char.ToUpperInvariant(parts[i][0]));
        resultBuilder.Append(parts[i].Substring(1).ToLowerInvariant());
      }

      return resultBuilder.ToString();
    }

    public static string ToCamelCase(string specIdentifier)
    {
      var parts = specIdentifier.Split('_');
      var resultBuilder = new StringBuilder();
      resultBuilder.Append(parts[0].ToLowerInvariant());
      for (int i = 1; i < parts.Length; i++)
      {
        resultBuilder.Append(char.ToUpperInvariant(parts[i][0]));
        resultBuilder.Append(parts[i].Substring(1).ToLowerInvariant());
      }

      return resultBuilder.ToString();
    }

    public static string ToHyphenatedCase(string identifier)
    {
      var parts = identifier.Split('_');
      var resultBuilder = new StringBuilder();
      resultBuilder.Append(parts[0].ToLowerInvariant());
      for (int i = 1; i < parts.Length; i++)
      {
        resultBuilder.Append("-");
        resultBuilder.Append(parts[i].ToLowerInvariant());
      }

      return resultBuilder.ToString();
    }
  }
}
