using System.Numerics;
public class ZBlock : Block
{
  public ZBlock()
  {
    int GREEN = (int)Colors.GREEN;
    grid = new int[3, 3]
    {
      {GREEN, 0, 0},
      {GREEN, GREEN, 0},
      {0, GREEN, 0}
    };
  }
}