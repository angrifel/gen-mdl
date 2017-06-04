//  This file is part of gen-mdl - A Source code generator for model definitions.
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

namespace ModelGenerator.Tests.TypeScript.Mocks
{
  using ModelGenerator.TypeScript;
  using System;
  using System.IO;

  public class TypeScriptDeclarationOrStatementMock2 : TypeScriptDeclarationOrStatement, IEquatable<TypeScriptDeclarationOrStatementMock2>
  {
    private readonly string _desiredOutput;

    public TypeScriptDeclarationOrStatementMock2(string desiredOutput)
    {
      _desiredOutput = desiredOutput;
    }

    public override void Generate(TextWriter output)
    {
      output.WriteLine(_desiredOutput);
    }

    public bool Equals(TypeScriptDeclarationOrStatementMock2 other) =>
      other != null &&
      _desiredOutput == other._desiredOutput;

    public override bool Equals(TypeScriptDeclarationOrStatement other) => Equals(other as TypeScriptDeclarationOrStatementMock2);

    public override bool Equals(object obj) => Equals(obj as TypeScriptDeclarationOrStatementMock2);

    public override int GetHashCode() => _desiredOutput?.GetHashCode() ?? 0;
  }
}
