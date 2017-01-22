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

namespace ModelGenerator.TypeScript
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class TypeScriptGenerator : ITargetGenerator
  {
    public void Generate(string basePath, SpecInterpreter specInterpreter)
    {
      var targetInfo = specInterpreter.Spec.Targets[Constants.TypeScriptTarget];
      if (!PathFunctions.IsSupportedPath(targetInfo.Path)) throw new InvalidOperationException("Path not supported");
      var targetDir = PathFunctions.IsPathRelative(targetInfo.Path) ? Path.Combine(basePath, targetInfo.Path) : targetInfo.Path;
      Directory.CreateDirectory(targetDir);

      var barrelPath = Path.Combine(targetDir, Path.ChangeExtension("index", Constants.TypeScriptExtension));
      var barrelOutput = (Stream)null;
      var barrelWriter = (TextWriter)null;

      try
      {
        barrelOutput = new FileStream(barrelPath, FileMode.Create, FileAccess.Write, FileShare.None);
        barrelWriter = new StreamWriter(barrelOutput);

        foreach (var @enum in specInterpreter.Spec.Enums)
        {
          var enumFileName = GetFileName(@enum.Key);
          var path = Path.Combine(targetDir, Path.ChangeExtension(enumFileName, Constants.TypeScriptExtension));
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

        foreach (var entity in specInterpreter.Spec.Entities)
        {
          var entityFileName = GetFileName(entity.Key);
          var path = Path.Combine(targetDir, Path.ChangeExtension(entityFileName, Constants.TypeScriptExtension));
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

    private static void GenerateEnum(StreamWriter output, string enumName, IList<Alternative<string, QualifiedEnumMember>> enumMembers)
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

    private static void GenerateEnumMember(StreamWriter output, Alternative<string, QualifiedEnumMember> member, bool isLastOne)
    {
      var name = member.Value as string ?? ((QualifiedEnumMember)member.Value).Name;
      var nomalizedMemberName = SpecFunctions.ToPascalCase(name);
      var separator = isLastOne ? string.Empty : ",";
      var qem = member.Value as QualifiedEnumMember;
      if (qem == null)
      {
        output.WriteLine($"  {nomalizedMemberName}{separator}");
      }
      else
      {
        output.WriteLine($"  {nomalizedMemberName} = {qem.Value}{separator}");
      }
    }

    private static void GenerateEntity(StreamWriter output, SpecInterpreter specInterpreter, string entityName, IDictionary<string, Alternative<string, EntityMemberInfo>> entityMembers)
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

    private static void GenerateEntityMembers(StreamWriter output, SpecInterpreter specInterpreter, IDictionary<string, Alternative<string, EntityMemberInfo>> entityMembers)
    {
      var members = new KeyValuePair<string, Alternative<string, EntityMemberInfo>>[entityMembers.Count];
      entityMembers.CopyTo(members, 0);
      for (int i = 0; i < members.Length; i++)
      {
        GenerateEntityMember(output, specInterpreter, members[i]);
      }
    }

    private static void GenerateEntityMember(StreamWriter output, SpecInterpreter specInterpreter, KeyValuePair<string, Alternative<string, EntityMemberInfo>> member)
    {
      var specType = member.Value.GetMemberType();
      var resolvedType = specInterpreter.GetResolvedType(Constants.TypeScriptTarget, specType);
      var normalizedType = specInterpreter.IsNativeType(Constants.TypeScriptTarget, resolvedType) ? resolvedType : SpecFunctions.ToPascalCase(resolvedType);
      var normalizedMemberName = SpecFunctions.ToCamelCase(member.Key);
      output.WriteLine($"  {normalizedMemberName} : {normalizedType};");
    }

    private static string GetFileName(string type) => SpecFunctions.ToHyphenatedCase(type);
  }
}
