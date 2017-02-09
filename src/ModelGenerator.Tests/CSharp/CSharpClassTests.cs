using ModelGenerator.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ModelGenerator.Tests.CSharp
{
  public class CSharpClassTests
  {
    [Fact]
    public void TestGenerateClassWithNoMembers()
    {
      // arrange
      var csharpClass = new CSharpClass { Name = "Blog" };
      var output = new StringWriter();
      var expectedOutput =
        "  public class Blog" + Environment.NewLine +
        "  {" + Environment.NewLine +
        "  }" + Environment.NewLine;

      // act
      csharpClass.Generate(output);

      // assert
      var generatedOutput = output.GetStringBuilder().ToString();
      Assert.Equal(expectedOutput, generatedOutput);
    }

    [Fact]
    public void TestGenerateClassWithMembers()
    {
      // arrange
      var idMember = new CSharpClassMember { Name = "Id", Type = "int" };
      var descriptionMembers = new CSharpClassMember { Name = "Description", Type = "string" };
      var csharpClass = new CSharpClass { Name = "Blog", Members = new List<CSharpClassMember> { idMember, descriptionMembers } };
      var output = new StringWriter();
      var expectedOutputWriter = new StringWriter();
      expectedOutputWriter.Write("  public class Blog" + Environment.NewLine);
      expectedOutputWriter.Write("  {" + Environment.NewLine);
      idMember.Generate(expectedOutputWriter);
      descriptionMembers.Generate(expectedOutputWriter);
      expectedOutputWriter.Write("  }" + Environment.NewLine);

      // act
      csharpClass.Generate(output);

      // assert
      var generatedOutput = output.GetStringBuilder().ToString();
      Assert.Equal(expectedOutputWriter.GetStringBuilder().ToString(), generatedOutput);
    }
  }
}
