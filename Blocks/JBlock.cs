using System.Numerics;

public class JBlock : Block
{
  public JBlock()
  {
    int PINK = (int)Colors.PINK;
    grid = new int[3, 3]
    {
      {0, 0, 0},
      {0, 0, PINK},
      {PINK, PINK, PINK},
    };
  }
}