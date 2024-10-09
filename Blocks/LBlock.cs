using System.Numerics;

public class LBlock : Block
{
  public LBlock()
  {
    int ORANGE = (int)Colors.ORANGE;
    grid = new int[3, 3]
    {
      {ORANGE, ORANGE, ORANGE},
      {0, 0, ORANGE},
      {0, 0, 0},
    };
  }
}