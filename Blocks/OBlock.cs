using System.Numerics;

public class OBlock : Block
{
  public OBlock()
  {
    int YELLOW = (int)Colors.YELLOW;
    grid = new int[2, 2]
    {
      {YELLOW, YELLOW},
      {YELLOW, YELLOW}
    };
  }
}