//  This file is part of genmdl - A Source code generator for model definitions.
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

namespace ModelGenerator.Tests.TypeScript
{
  using ModelGenerator.Model;
  using ModelGenerator.TypeScript;
  using ModelGenerator.TypeScript.Builders;
  using ModelGenerator.TypeScript.Services;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Xunit;

  public class TypeScriptGeneratorTests
  {
    [Fact]
    public void TestGenerateOutputOnEmptySpec()
    {
      // arrange
      var spec =
        new Spec
        {
          Targets = new Dictionary<string, TargetInfo>
          {
            { Constants.TypeScriptTarget, new TargetInfo { Path = "some_path" } }
          }
        };
      var generator = new TypeScriptGenerator();

      // act
      var outputs = generator.GenerateOutputs(spec);

      // assert
      Assert.Equal(0, outputs.Count());
    }


    [Fact]
    public void TestGenerateOutputWithEnum()
    {
      // arrange
      var spec =
        new Spec
        {
          Enums = new Dictionary<string, List<EnumMember>>
          {
            {
              "blog_entry_status",
              new List<EnumMember> { new EnumMember { Name = "draft" }, new EnumMember { Name = "final" } }
            }
          },
          Targets = new Dictionary<string, TargetInfo>
          {
            { Constants.TypeScriptTarget, new TargetInfo { Path = "some_path"} }
          }
        };

      var generator = new TypeScriptGenerator();

      // act
      var outputs = generator.GenerateOutputs(spec);

      // assert
      var outputList = outputs?.ToList();
      var indexOutput = outputList.FirstOrDefault(_ => _.Path == Path.Combine("some_path", "index.ts"));
      var blogEntryStatusOutput = outputList.FirstOrDefault(_ => _.Path == Path.Combine("some_path", "blog-entry-status.ts"));
      var indexFile = (TypeScriptFile)indexOutput?.GenerationRoot;
      var blogEntryStatusOutputFile = (TypeScriptFile)blogEntryStatusOutput?.GenerationRoot;

      Assert.Equal(2, outputList?.Count);
      Assert.NotNull(indexOutput);
      Assert.NotNull(blogEntryStatusOutput);

      Assert.True(
        indexFile
          .Contents
          .Cast<TypeScriptReExportStatement>()
          .HasContents(_ => 
            _.Object == null && 
            _.FileName == "blog-entry-status"));
      Assert.True(
        blogEntryStatusOutputFile
          .Contents.OfType<TypeScriptEnum>()
          .HasContents(
            _ =>
              _.Name == "BlogEntryStatus" &&
              _.Members.HasContents(
                __ => __.Name == "Draft" && __.Value == null,
                __ => __.Name == "Final" && __.Value == null)));

      Assert.True(
        blogEntryStatusOutputFile
          .Contents.OfType<TypeScriptExportStatement>()
          .HasContents(
            _ => _.IsDefault && _.Object == "BlogEntryStatus"));
    }

    [Fact]
    public void TestGenerateOutputWithClass()
    {
      // arrange
      var spec =
        new Spec
        {
          Entities = new Dictionary<string, EntityInfo>
          {
            {
              "blog_entry",
              new EntityInfo
              {
                Members =
                new OrderedDictionary<string, IEntityMemberInfo>
                {
                  { "id", new EntityMemberInfo { Type = "int" } },
                  { "title", new EntityMemberInfo { Type = "string"} }
                }
              }
            }
          },
          Targets = new Dictionary<string, TargetInfo>
          {
            { Constants.TypeScriptTarget, new TargetInfo { Path = "some_path" } }
          }
        };

      var typeScriptFiles =
        new GeneratorOutput[]
        {
          new GeneratorOutput
          {
            Path = Path.Combine("some_path", "blog-entry.ts"),
            GenerationRoot = 
              TypeScriptFileBuilder.Start()
                .Content(
                  TypeScriptExportStatementBuilder.Start()
                    .DefaultExport(
                      TypeScriptClassBuilder.Start()
                        .Name("BlogEntry")
                        .Member("id", "number")
                        .Member("title", "string").Build()
                      )
                    .Build())
                .Build()
          },
          new GeneratorOutput
          {
            Path = Path.Combine("some_path", "index.ts"),
            GenerationRoot =
              TypeScriptFileBuilder.Start()
                .Content(new TypeScriptReExportStatement { FileName = "blog-entry" })
                .Build()
          }
        };
      var ammendment = new AmmendmentFactory().CreateAmmendment(Constants.TypeScriptTarget);
      ammendment.AmmedSpecification(spec);
      var generator = new TypeScriptGenerator();

      // act
      var outputs = generator.GenerateOutputs(spec);

      // assert
      Assert.NotNull(outputs);
      Assert.True(outputs.SequenceEqual(typeScriptFiles, EqualityComparer<GeneratorOutput>.Default));
    }
  }
}
