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
  public class CSharpClassMemberTests
  {
    [Fact]
    public void TestGenerate()
    {
      // arrange
      var classMember = new CSharpClassMember { Name = "Id", Type = "int" };
      var output = new StringWriter();
      var expectedOutput = "    public int Id { get; set; }" + Environment.NewLine;

      // act
      classMember.Generate(output);

      // assert
      Assert.Equal(expectedOutput, output.GetStringBuilder().ToString());
    }
  }
}
