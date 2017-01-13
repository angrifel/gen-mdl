namespace ModelGenerator.Model
{
  using System.Collections.Generic;

  public class Entity
  {
    public Entity()
    {
      this.Members = new OrderedDictionary<string, string>();
    }

    public string Name { get; set; }

    public OrderedDictionary<string, string> Members { get; set; }
  }
}
