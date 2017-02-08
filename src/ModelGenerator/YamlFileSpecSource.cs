namespace ModelGenerator
{
  using Model;
  using System;
  using System.IO;

  public class YamlFileSpecSource : ISpecSource
  {
    private readonly string _specFile;

    public YamlFileSpecSource(string specFile)
    {
      if (specFile == null) throw new ArgumentNullException(nameof(specFile));
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

        var yamlReaderSpecSource = new YamlReaderSpecSource(specReader);
        return yamlReaderSpecSource.GetSpec();
      }
      finally
      {
        specReader?.Dispose();
        specStream?.Dispose();
      }
    }
  }
}
