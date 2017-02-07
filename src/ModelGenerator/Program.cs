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

namespace ModelGenerator
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using YamlDotNet.Serialization;
  using YamlDotNet.Serialization.NamingConventions;
  using YamlDotNetExtensions;

  class Program
  {
    static int Main(string[] args)
    {
      if (args.Length == 0) { ShowHelp(); return 0; }
      if (args.Length > 1) { ShowHelp(); return 0; }
      var modelFilePath = args[0];
      if (!File.Exists(modelFilePath)) ShowFileDoesNotExist();
      try
      {
        var spec = GetSpecFromFile(modelFilePath);
        var specProcessor = new SpecProcessor(spec, new GeneratorFactory());
        specProcessor.AmmedSpecification();
        specProcessor.VerifySpecification();
        var outputs = specProcessor.GenerateOutput();
        WriteOutputs(Path.GetDirectoryName(modelFilePath), outputs);
        return 0;
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(ex.GetBaseException().Message);
        return 1;
      }
    }

    private static void ShowHelp()
    {
      Console.Write("usage: mdlgen <model-file-path>");
    }

    private static void ShowFileDoesNotExist()
    {
      Console.Error.WriteLine("model file does not exist.");
    }

    private static Spec GetSpecFromFile(string modelFilePath)
    {
      Stream modelFile = null;
      TextReader reader = null;
      try
      {
        modelFile = new FileStream(modelFilePath, FileMode.Open);
        reader = new StreamReader(modelFile);

        var deserializer = (Deserializer)null;
        deserializer = new DeserializerBuilder()
          .WithTypeConverter(new IEntityMemberInfoConverter(() => deserializer))
          .WithTypeConverter(new EnumMemberConverter())
          .WithNamingConvention(new UnderscoredNamingConvention()).Build();
        return deserializer.Deserialize<Spec>(reader);
      }
      finally
      {
        reader?.Dispose();
        modelFile?.Dispose();
      }
    }

    private static void WriteOutputs(string basePath, IEnumerable<GeneratorOutput> outputs)
    {
      foreach (var output in outputs)
      {
        var path = PathFunctions.IsPathRelative(output.Path) ? Path.Combine(basePath, output.Path) : output.Path;
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using (var outputStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
          using (var outputWriter = new StreamWriter(outputStream))
          {
            output.GenerationRoot.Generate(outputWriter);
            outputWriter.Flush();
          }
        }
      }
    }
  }
}
