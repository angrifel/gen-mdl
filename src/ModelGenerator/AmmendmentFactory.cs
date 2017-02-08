namespace ModelGenerator
{
  using ModelGenerator.CSharp;
  using ModelGenerator.TypeScript;

  public class AmmendmentFactory : IAmmedmentFactory
  {
    public ITargetAmmedment CreateAmmendment(string target)
    {
      switch (target)
      {
        case Constants.CSharpTarget: return new CSharpAmmendment();
        case Constants.TypeScriptTarget: return new TypeScriptAmmendment();
        default: return null;
      }
    }
  }
}
