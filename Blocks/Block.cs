using System.Numerics;
using Raylib_cs;
using static Game;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Security.Cryptography;

public abstract class Block
{
  protected int[,] grid = new int[3, 3];
  Vector2 position = Vector2.Zero;

  public void Draw()
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        var rect = new Rectangle((position.X + x) * gridSize, (position.Y + y) * gridSize, gridSize, gridSize);
        DrawRectangleRounded(rect, 0.3f, 20, ToRayColor((Colors)grid[x, y]));
        if (grid[x, y] != (int)Colors.BLANK)
          DrawRectangleRoundedLines(rect, 0.3f, 20, 2, backgroundColor);
      }
    }
  }

  public void DrawAt(Vector2 offset)
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        var fullSize = GetLength() * gridSize;
        var pos = new Vector2((position.X + x + offset.X) * gridSize - fullSize.X / 2, (position.Y + y + offset.Y) * gridSize - fullSize.Y / 2);
        var rect = new Rectangle(pos, new Vector2(gridSize));
        DrawRectangleRounded(rect, 0.3f, 20, ToRayColor((Colors)grid[x, y]));
        if (grid[x, y] != (int)Colors.BLANK)
          DrawRectangleRoundedLines(rect, 0.3f, 20, 2, backgroundColor);
      }
    }
  }

  public Vector2 GetLength()
  {
    return new Vector2(grid.GetLength(0), grid.GetLength(1));
  }

  public void Move()
  {
    CheckForGameOver();
    TryAddBlockToGrid();
    position.Y++;
  }

  void CheckForGameOver()
  {
    if (CheckCollisionBlocks(position, grid))
      GameOver = true;
  }

  void TryAddBlockToGrid()
  {
    if (CheckCollisionDown() || CheckCollisionBlocks(position + new Vector2(0, 1), grid))
    {
      AddBlockToGrid();
      ChangeBlock();
    }
  }

  bool CheckCollisionBlocks(Vector2 position, int[,] grid)
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (grid[x, y] == (int)Colors.BLANK) continue;
        if (Game.grid[x + (int)position.X, y + (int)position.Y] != (int)Colors.BLANK)
          return true;
      }
    }
    return false;
  }

  bool CheckCollisionDown()
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (grid[x, y] == (int)Colors.BLANK)
          continue;
        if (position.Y + y + 1 > Game.grid.GetLength(1) - 1)
          return true;
      }
    }
    return false;
  }

  void AddBlockToGrid()
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (grid[x, y] == (int)Colors.BLANK)
          continue;
        Game.grid[(int)position.X + x, (int)position.Y + y] = grid[x, y];
      }
    }
  }

  public void UpdateInput()
  {
    if (IsKeyPressed(KeyboardKey.Z))
      RotateLeft();
    if (IsKeyPressed(KeyboardKey.C))
      RotateRight();

    if (IsKeyPressed(KeyboardKey.Right))
      TryMoveRight();
    if (IsKeyPressed(KeyboardKey.Left))
      TryMoveLeft();
  }

  void TryMoveRight()
  {
    if (!CheckCollisionRight(grid, 1) && !CheckCollisionBlocks(position + new Vector2(1, 0), grid))
      position.X++;
  }

  void TryMoveLeft()
  {
    if (!CheckCollisionLeft(grid, -1) && !CheckCollisionBlocks(position + new Vector2(-1, 0), grid))
      position.X--;
  }

  bool CheckCollisionLeft(int[,] grid, int offset)
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (grid[x, y] == (int)Colors.BLANK) continue;
        if (x + position.X + offset < 0) return true;
      }
    }
    return false;
  }

  bool CheckCollisionRight(int[,] grid, int offset)
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        if (grid[x, y] == (int)Colors.BLANK) continue;
        if (x + position.X + offset > Game.grid.GetLength(0) - 1) return true;
      }
    }
    return false;
  }


  void RotateLeft()
  {
    int[,] grid = new int[this.grid.GetLength(0), this.grid.GetLength(1)];
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        grid[y, grid.GetLength(0) - 1 - x] = this.grid[x, y];
      }
    }
    if (CheckCollisionRight(grid, 0))
      return;
    if (CheckCollisionLeft(grid, 0))
      return;
    if (CheckCollisionBlocks(position, grid))
      return;
    this.grid = grid;
  }

  void RotateRight()
  {
    int[,] grid = new int[this.grid.GetLength(0), this.grid.GetLength(1)];
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        grid[grid.GetLength(1) - 1 - y, x] = this.grid[x, y];
      }
    }
    if (CheckCollisionRight(grid, 0))
      return;
    if (CheckCollisionLeft(grid, 0))
      return;
    if (CheckCollisionBlocks(position, grid))
      return;
    this.grid = grid;
  }
}