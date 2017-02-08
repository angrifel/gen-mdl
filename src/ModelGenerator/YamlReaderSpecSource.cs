namespace ModelGenerator
{
  using System;
  using ModelGenerator.Model;
  using System.IO;
  using YamlDotNet.Serialization;
  using YamlDotNetExtensions;
  using YamlDotNet.Serialization.NamingConventions;

  public class YamlReaderSpecSource : ISpecSource
  {
    private readonly TextReader _reader;

    public YamlReaderSpecSource(TextReader reader)
    {
      if (reader == null) throw new ArgumentNullException(nameof(reader));
      _reader = reader;
    }

    public Spec GetSpec()
    {
      var deserializer = (Deserializer)null;
      deserializer = new DeserializerBuilder()
        .WithTypeConverter(new IEntityMemberInfoConverter(() => deserializer))
        .WithTypeConverter(new EnumMemberConverter())
        .WithNamingConvention(new UnderscoredNamingConvention()).Build();
      return deserializer.Deserialize<Spec>(_reader);
    }
  }
}
