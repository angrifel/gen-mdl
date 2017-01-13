namespace ModelGenerator.CSharp
{
  using Model;
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class CSharpGenerator : ITargetGenerator
  {
    public void Generate(SpecInterpreter specInterpreter)
    {
      var targetInfo = specInterpreter.Spec.Targets[Constants.CSharpTarget];

      Directory.CreateDirectory(targetInfo.Path);

      foreach (var @enum in (IDictionary<string, List<string>>)specInterpreter.Spec.Enums)
      {
        var enumFilename = GetFilename(@enum.Key);
        var path = Path.Combine(targetInfo.Path, Path.ChangeExtension(enumFilename, Constants.CSharpExtension));
        using (var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
          using (var writer = new StreamWriter(output))
          {
            GenerateEnum(writer, targetInfo.Namespace, @enum.Key, @enum.Value);
            writer.Flush();
          }
        }
      }

      foreach (var entity in (IDictionary<string, OrderedDictionary<string, string>>)specInterpreter.Spec.Entities)
      {
        var entityFilename = GetFilename(entity.Key);
        var path = Path.Combine(targetInfo.Path, Path.ChangeExtension(entityFilename, Constants.CSharpExtension));
        using (var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
          using (var writer = new StreamWriter(output))
          {
            GenerateEntity(writer, specInterpreter, entity.Key, entity.Value);
            writer.Flush();
          }
        }
      }
    }

    private static void GenerateEnum(StreamWriter output, string @namespace, string enumName, IList<string> enumMembers)
    {
      var normalizedEnumName = SpecFunctions.ToPascalCase(enumName);
      output.WriteLine($"namespace {@namespace}");
      output.WriteLine(@"{");
      output.WriteLine($"  public enum {normalizedEnumName}");
      output.WriteLine(@"  {");

      for (int i = 0; i < enumMembers.Count - 1; i++)
      {
        GenerateEnumMember(output, enumMembers[i], false);
      }

      GenerateEnumMember(output, enumMembers[enumMembers.Count - 1], true);

      output.WriteLine(@"  }");
      output.WriteLine(@"}");
    }

    private static void GenerateEnumMember(TextWriter output, string memberName, bool isLastOne)
    {
      var nomalizedMemberName = SpecFunctions.ToPascalCase(memberName);
      var separator = isLastOne ? string.Empty : ",";
      output.WriteLine($"    {nomalizedMemberName}{separator}");
    }

    private static void GenerateEntity(StreamWriter output, SpecInterpreter specInterpreter, string entityName, IDictionary<string, string> entityMembers)
    {
      var normalizedEntityName = SpecFunctions.ToPascalCase(entityName);
      output.WriteLine($"namespace {@specInterpreter.Spec.Targets[Constants.CSharpTarget].Namespace}");
      output.WriteLine(@"{");
      output.WriteLine($"  public class {normalizedEntityName}");
      output.WriteLine(@"  {");
      if (entityMembers.Count > 0)
      {
        GenerateEntityMembers(output, specInterpreter, entityMembers);
      }

      output.WriteLine(@"  }");
      output.WriteLine(@"}");
    }

    private static void GenerateEntityMembers(StreamWriter output, SpecInterpreter specInterpreter, IDictionary<string, string> entityMembers)
    {
      var members = new KeyValuePair<string, string>[entityMembers.Count];
      entityMembers.CopyTo(members, 0);
      for (int i = 0; i < members.Length - 1; i++)
      {
        GenerateEntityMember(output, specInterpreter, members[i].Key, members[i].Value, false);
      }

      GenerateEntityMember(output, specInterpreter, members[members.Length - 1].Key, members[members.Length - 1].Value, true);
    }

    private static void GenerateEntityMember(StreamWriter output, SpecInterpreter specInterpreter, string name, string type, bool isLastOne)
    {
      var resolvedType = specInterpreter.GetResolvedType(Constants.CSharpTarget, type);
      var normalizedType = specInterpreter.IsNativeType(Constants.CSharpTarget, resolvedType) ? type : SpecFunctions.ToPascalCase(resolvedType);
      var normalizedName = SpecFunctions.ToPascalCase(name);
      output.WriteLine($"    public {normalizedType} {normalizedName} {{ get; set; }}");
    }

    private static string GetFilename(string type) => SpecFunctions.ToPascalCase(type);
  }
}
