namespace ModelGenerator.TypeScript
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class TypeScriptGenerator : ITargetGenerator
  {
    public void Generate(SpecInterpreter specInterpreter)
    {
      var targetInfo = specInterpreter.Spec.Targets[Constants.TypeScriptTarget];

      Directory.CreateDirectory(targetInfo.Path);

      var barrelPath = Path.Combine(targetInfo.Path, Path.ChangeExtension("index", Constants.TypeScriptExtension));
      var barrelOutput = (Stream)null;
      var barrelWriter = (TextWriter)null;

      try
      {
        barrelOutput = new FileStream(barrelPath, FileMode.Create, FileAccess.Write, FileShare.None);
        barrelWriter = new StreamWriter(barrelOutput);

        foreach (var @enum in (IDictionary<string, List<string>>)specInterpreter.Spec.Enums)
        {
          var enumFileName = GetFileName(@enum.Key);
          var path = Path.Combine(targetInfo.Path, Path.ChangeExtension(enumFileName, Constants.TypeScriptExtension));
          using (var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
          {
            using (var writer = new StreamWriter(output))
            {
              GenerateEnum(writer, @enum.Key, @enum.Value);
              writer.Flush();
            }
          }

          barrelWriter.WriteLine($"export * from './{enumFileName}'");
        }

        foreach (var entity in (IDictionary<string, OrderedDictionary<string, string>>)specInterpreter.Spec.Entities)
        {
          var entityFileName = GetFileName(entity.Key);
          var path = Path.Combine(targetInfo.Path, Path.ChangeExtension(entityFileName, Constants.TypeScriptExtension));
          using (var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
          {
            using (var writer = new StreamWriter(output))
            {
              GenerateEntity(writer, specInterpreter, entity.Key, entity.Value);
              writer.Flush();
            }
          }

          barrelWriter.WriteLine($"export * from './{entityFileName}'");
        }
      }
      finally
      {
        barrelWriter?.Flush();
        barrelWriter?.Dispose();
        barrelOutput?.Dispose();
      }
    }

    private static void GenerateEnum(StreamWriter output, string enumName, IList<string> enumMembers)
    {
      var normalizedEnumName = SpecFunctions.ToPascalCase(enumName);
      output.WriteLine($"enum {normalizedEnumName} {{");
      for (int i = 0; i < enumMembers.Count - 1; i++)
      {
        GenerateEnumMember(output, enumMembers[i], false);
      }

      GenerateEnumMember(output, enumMembers[enumMembers.Count - 1], true);

      output.WriteLine("}");
      output.WriteLine();
      output.WriteLine($"export default {normalizedEnumName}");
    }

    private static void GenerateEnumMember(StreamWriter output, string memberName, bool last)
    {
      var separator = last ? string.Empty : ",";
      var normalizedMemberName = SpecFunctions.ToPascalCase(memberName);
      output.WriteLine($"  {normalizedMemberName}{separator}");
    }

    private static void GenerateEntity(StreamWriter output, SpecInterpreter specInterpreter, string entityName, IDictionary<string, string> entityMembers)
    {
      var enumDependencies = specInterpreter.GetDirectEnumDependencies(entityName);
      var entityDependencies = specInterpreter.GetDirectEntityDependencies(entityName);
      var directDependencies = enumDependencies.Concat(entityDependencies).ToList();
      var normalizedEntityName = SpecFunctions.ToPascalCase(entityName);

      if (directDependencies.Count > 0)
      {
        foreach (var type in directDependencies)
        {
          var normalizedTypeDependency = SpecFunctions.ToPascalCase(type);
          var directDependencyFileName = GetFileName(type);
          output.WriteLine($"import {normalizedTypeDependency} from './{directDependencyFileName}';");
        }

        output.WriteLine();
      }

      output.WriteLine($"export default class {normalizedEntityName} {{");

      if (entityMembers.Count > 0)
      {
        GenerateEntityMembers(output, specInterpreter, entityMembers);
      }

      output.WriteLine("}");
    }

    private static void GenerateEntityMembers(StreamWriter output, SpecInterpreter specInterpreter, IDictionary<string, string> entityMembers)
    {
      var members = new KeyValuePair<string, string>[entityMembers.Count];
      entityMembers.CopyTo(members, 0);
      for (int i = 0; i < members.Length; i++)
      {
        GenerateEntityMember(output, specInterpreter, members[i]);
      }
    }

    private static void GenerateEntityMember(StreamWriter output, SpecInterpreter specInterpreter, KeyValuePair<string, string> member)
    {
      var resolvedType = specInterpreter.GetResolvedType(Constants.TypeScriptTarget, member.Value);
      var normalizedType = specInterpreter.IsNativeType(Constants.TypeScriptTarget, resolvedType) ? resolvedType : SpecFunctions.ToPascalCase(resolvedType);
      var normalizedMemberName = SpecFunctions.ToCamelCase(member.Key);
      output.WriteLine($"  {normalizedMemberName} : {normalizedType};");
    }

    private static string GetFileName(string type) => SpecFunctions.ToHyphenatedCase(type);
  }
}
