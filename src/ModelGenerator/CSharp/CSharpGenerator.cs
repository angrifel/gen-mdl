//  This file is part of mdlgen - A Source code generator for model definitions.
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

namespace ModelGenerator.CSharp
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class CSharpGenerator : ITargetGenerator
  {
    private static readonly string[] StructNativeTypes = new string[] { "bool", "byte", "short", "int", "long", "float", "double", "decimal", "char", "System.Guid", "System.DateTime", "System.TimeSpan", "System.DateTimeOffset" };

    public void Generate(string basePath, SpecInterpreter specInterpreter)
    {
      var targetInfo = specInterpreter.Spec.Targets[Constants.CSharpTarget];
      if (!PathFunctions.IsSupportedPath(targetInfo.Path)) throw new InvalidOperationException("Path not supported");
      var targetDir = PathFunctions.IsPathRelative(targetInfo.Path) ? Path.Combine(basePath, targetInfo.Path) : targetInfo.Path;

      Directory.CreateDirectory(targetDir);

      foreach (var @enum in specInterpreter.Spec.Enums)
      {
        var enumFilename = GetFilename(@enum.Key);
        var path = Path.Combine(targetDir, Path.ChangeExtension(enumFilename, Constants.CSharpExtension));
        using (var output = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
          using (var writer = new StreamWriter(output))
          {
            GenerateEnum(writer, targetInfo.Namespace, @enum.Key, @enum.Value);
            writer.Flush();
          }
        }
      }

      foreach (var entity in specInterpreter.Spec.Entities)
      {
        var entityFilename = GetFilename(entity.Key);
        var path = Path.Combine(targetDir, Path.ChangeExtension(entityFilename, Constants.CSharpExtension));
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

    private static void GenerateEnum(StreamWriter output, string @namespace, string enumName, IList<Alternative<string, QualifiedEnumMember>> enumMembers)
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

    private static void GenerateEnumMember(TextWriter output, Alternative<string, QualifiedEnumMember> member, bool isLastOne)
    {
      var name = member.Value as string ?? ((QualifiedEnumMember)member.Value).Name;
      var nomalizedMemberName = SpecFunctions.ToPascalCase(name);
      var separator = isLastOne ? string.Empty : ",";
      var qem = member.Value as QualifiedEnumMember;
      if (qem == null)
      {
        output.WriteLine($"    {nomalizedMemberName}{separator}");
      }
      else
      {
        output.WriteLine($"    {nomalizedMemberName} = {qem.Value}{separator}");
      }
    }

    private static void GenerateEntity(StreamWriter output, SpecInterpreter specInterpreter, string entityName, IDictionary<string, Alternative<string, EntityMemberInfo>> entityMembers)
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

    private static void GenerateEntityMembers(StreamWriter output, SpecInterpreter specInterpreter, IDictionary<string, Alternative<string, EntityMemberInfo>> entityMembers)
    {
      var members = new KeyValuePair<string, Alternative<string, EntityMemberInfo>>[entityMembers.Count];
      entityMembers.CopyTo(members, 0);
      for (int i = 0; i < members.Length - 1; i++)
      {
        GenerateEntityMember(output, specInterpreter, members[i].Key, members[i].Value, false);
      }

      GenerateEntityMember(output, specInterpreter, members[members.Length - 1].Key, members[members.Length - 1].Value, true);
    }

    private static void GenerateEntityMember(StreamWriter output, SpecInterpreter specInterpreter, string name, Alternative<string, EntityMemberInfo> valueOrEntityMemberInfoAlternative, bool isLastOne)
    {
      var specType = valueOrEntityMemberInfoAlternative.GetMemberType();
      var isNullable = valueOrEntityMemberInfoAlternative.GetIsNullable();
      var resolvedType = specInterpreter.GetResolvedType(Constants.CSharpTarget, specType);
      var normalizedType = specInterpreter.IsNativeType(Constants.CSharpTarget, specType) ? specType : SpecFunctions.ToPascalCase(resolvedType);
      var isStruct = IsStruct(normalizedType);
      var memberType = normalizedType + (isStruct && isNullable ? "?" : string.Empty);
      var memberName = SpecFunctions.ToPascalCase(name);
      output.WriteLine($"    public {memberType} {memberName} {{ get; set; }}");
    }

    private static string GetFilename(string type) => SpecFunctions.ToPascalCase(type);

    private static bool IsStruct(string type) => StructNativeTypes.Contains(type);
  }
}
