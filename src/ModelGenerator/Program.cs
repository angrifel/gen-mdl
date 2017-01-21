namespace ModelGenerator
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using YamlDotNet.Serialization;
  using YamlDotNet.Serialization.NamingConventions;
  using YamlDotNetExtensions;

  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length == 0) { ShowHelp(); return; }
      if (args.Length > 1) { ShowHelp(); return; }
      var modelFilePath = args[0];
      if (!File.Exists(modelFilePath)) ShowFileDoesNotExist();
      try
      {
        var spec = GetSpecFromFile(modelFilePath);
        var specProcessor = new SpecProcessor(spec);
        specProcessor.AmmedSpecification();
        specProcessor.VerifySpecification();
        specProcessor.ProcessSpecification(Path.GetDirectoryName(modelFilePath));
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(ex.GetBaseException().Message);
      }
    }

    private static void ShowHelp()
    {
      Console.Write("usage: mdl-gen <model-file-path>");
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
          .WithTypeConverter(new ValueOrEntityMemberInfoAlternativeConverter(() => deserializer))
          .WithTypeConverter(new ValueOrQualifiedEnumMemberAlternativeConverter())
          .WithNamingConvention(new UnderscoredNamingConvention()).Build();
        return deserializer.Deserialize<Spec>(reader);
      }
      finally
      {
        reader?.Dispose();
        modelFile?.Dispose();
      }
    }
  }
}
