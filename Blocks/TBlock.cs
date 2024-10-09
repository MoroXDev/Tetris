using System.Numerics;
public class TBlock : Block
{
  public TBlock()
  {
    int PURPLE = (int)Colors.PURPLE;
    grid = new int[3, 3]
    {
      {0, PURPLE, 0},
      {PURPLE, PURPLE, 0},
      {0, PURPLE, 0}
    };
  }
}