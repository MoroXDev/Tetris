using System.Numerics;

public class IBlock : Block
{
  public IBlock()
  {
    int BLUE = (int)Colors.BLUE;
    grid = new int[4, 4]
    {
      {0, BLUE, 0, 0},
      {0, BLUE, 0, 0},
      {0, BLUE, 0, 0},
      {0, BLUE, 0, 0}
    };
  }
}