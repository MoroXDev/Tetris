using System.Numerics;
public class SBlock : Block
{
  public SBlock()
  {
    int RED = (int)Colors.RED;
    grid = new int[3, 3]
    {
      {0, RED, 0},
      {RED, RED, 0},
      {RED, 0, 0}
    };
  }
}