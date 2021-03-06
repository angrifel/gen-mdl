﻿//  This file is part of gen-mdl - A Source code generator for model definitions.
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

namespace ModelGenerator.Tests.CSharp
{
  using ModelGenerator.CSharp;
  using System;
  using System.IO;
  using Xunit;

  public class CSharpClassMemberTests
  {
    [Fact]
    public void TestGenerateDoNotIssueRequired()
    {
      // arrange
      var classMember = new CSharpClassMember { Name = "Id", Type = "int", RequiredAttributeBehavior = CSharpRequiredAttributeBehavior.NoRequiredAttribute };
      var output = new StringWriter();
      var expectedOutput = "    public int Id { get; set; }" + Environment.NewLine;

      // act
      classMember.Generate(output);

      // assert
      Assert.Equal(expectedOutput, output.GetStringBuilder().ToString());
    }

    [Fact]
    public void TestGenerateIssueRequiredForNullValuesOnly()
    {
      // arrange
      var classMember = new CSharpClassMember { Name = "Id", Type = "string", RequiredAttributeBehavior = CSharpRequiredAttributeBehavior.IssueRequiredAllowEmptyStrings };
      var output = new StringWriter();

      var expectedOutput =
        "    [Required(AllowEmptyStrings = true)]" + Environment.NewLine + 
        "    public string Id { get; set; }" + Environment.NewLine;

      // act
      classMember.Generate(output);

      // assert
      Assert.Equal(expectedOutput, output.GetStringBuilder().ToString());
    }

    [Fact]
    public void TestGenerateIssueRequiredForNullAndEmptyValues()
    {
      // arrange
      var classMember = new CSharpClassMember { Name = "Id", Type = "string", RequiredAttributeBehavior = CSharpRequiredAttributeBehavior.IssueRequired };
      var output = new StringWriter();
      var expectedOutput =
        "    [Required]" + Environment.NewLine +
        "    public string Id { get; set; }" + Environment.NewLine;

      // act
      classMember.Generate(output);

      // assert
      Assert.Equal(expectedOutput, output.GetStringBuilder().ToString());
    }

  }
}
