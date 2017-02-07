namespace ModelGenerator
{
  using ModelGenerator.Model;
  using System;
  using System.IO;
  using YamlDotNet.Serialization;
  using YamlDotNet.Serialization.NamingConventions;
  using YamlDotNetExtensions;

  public class FileSpecSource : ISpecSource
  {
    private readonly string _specFile;

    public FileSpecSource(string specFile)
    {
      if (_specFile == null) throw new ArgumentNullException(nameof(specFile));
      _specFile = specFile;
    }

    public Spec GetSpec()
    {
      Stream specStream = null;
      TextReader specReader = null;
      try
      {
        specStream = new FileStream(_specFile, FileMode.Open);
        specReader = new StreamReader(specStream);

        var deserializer = (Deserializer)null;
        deserializer = new DeserializerBuilder()
          .WithTypeConverter(new IEntityMemberInfoConverter(() => deserializer))
          .WithTypeConverter(new EnumMemberConverter())
          .WithNamingConvention(new UnderscoredNamingConvention()).Build();
        return deserializer.Deserialize<Spec>(specReader);
      }
      finally
      {
        specReader?.Dispose();
        specStream?.Dispose();
      }
    }
  }
}
