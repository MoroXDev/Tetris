using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using System.Numerics;
using System.Diagnostics;
using System.Runtime.CompilerServices;
public class Game
{
  public static int[,] grid = new int[10, 20];
  public static int gridSize = 30;
  public static Color backgroundColor = new Color(0, 0, 33, 255);
  Stopwatch timer = new Stopwatch();
  static Block block = new IBlock();
  int restartTimeMs = 700;
  public static Block nextBlock = new TBlock();
  public static bool GameOver = false;
  Dictionary<Type, Texture2D> blockTextures = new Dictionary<Type, Texture2D>();
  int score = 0;
  Sound breakLinesSound;

  public Game()
  {
    timer.Start();
    DrawNextBlock();
    blockTextures.Add(typeof(OBlock), LoadTexture(@"Textures/OBlock.png"));
    blockTextures.Add(typeof(LBlock), LoadTexture(@"Textures/LBlock.png"));
    blockTextures.Add(typeof(JBlock), LoadTexture(@"Textures/JBlock.png"));
    blockTextures.Add(typeof(SBlock), LoadTexture(@"Textures/SBlock.png"));
    blockTextures.Add(typeof(ZBlock), LoadTexture(@"Textures/ZBlock.png"));
    blockTextures.Add(typeof(TBlock), LoadTexture(@"Textures/TBlock.png"));
    blockTextures.Add(typeof(IBlock), LoadTexture(@"Textures/IBlock.png"));
    breakLinesSound = LoadSound(@"Sounds\BreakSound.wav");
  }

  public void Update()
  {
    if (timer.Elapsed.TotalMilliseconds >= restartTimeMs)
    {
      block.Move();
      timer.Restart();
    }

    block.UpdateInput();
    UpdateSpeed();

    BreakBlockLines();
  }

  void BreakBlockLines()
  {
    int linesBrokenNum = 0;
    for (int y = 0; y < grid.GetLength(1); y++)
    {
      int blocksNum = 0;
      for (int x = 0; x < grid.GetLength(0); x++)
      {
        if (grid[x, y] != (int)Colors.BLANK) blocksNum++;
      }
      if (blocksNum == grid.GetLength(0))
      {
        MoveBlockLinesDown(y);
        linesBrokenNum++;
      }
    }
    AddToScore(linesBrokenNum);
    PlayBreakSound(linesBrokenNum);
  }

  void PlayBreakSound(int linesBrokenNum)
  {
    if (linesBrokenNum > 0) PlaySound(breakLinesSound);
  }

  void AddToScore(int linesBrokenNum)
  {
    switch (linesBrokenNum)
    {
      case 0: break;
      case 1: score += 100; break;
      case 2: score += 300; break;
      case 3: score += 500; break;
      default: score += 500; break;
    }
  }

  void MoveBlockLinesDown(int lineY)
  {
    int[,] gridCopy = CopyArray(grid);
    for (int y = 0; y <= lineY; y++)
    {
      for (int x = 0; x < grid.GetLength(0); x++)
      {
        if (y - 1 < 0) continue;
        grid[x, y] = gridCopy[x, y - 1];
      }
    }
  }

  int[,] CopyArray(int[,] array)
  {
    int[,] arrayCopy = new int[array.GetLength(0), array.GetLength(1)];
    for (int i = 0; i < array.GetLength(0); i++)
    {
      for (int j = 0; j < array.GetLength(1); j++)
      {
        arrayCopy[i, j] = array[i, j];
      }
    }
    return arrayCopy;
  }



  void UpdateSpeed()
  {
    if (IsKeyDown(KeyboardKey.X))
      restartTimeMs = 50;
    else
      restartTimeMs = 700;
  }

  public void Draw()
  {
    BeginDrawing();
    ClearBackground(backgroundColor);
    DrawGrid();
    block.Draw();
    DrawNextBlockUI(blockTextures[nextBlock.GetType()]);
    DrawScoreUI();
    EndDrawing();
  }

  void DrawNextBlockUI(Texture2D nextBlockTexture)
  {
    var nextBlockPos = new Vector2(13, 3) * gridSize;
    var textureSize = new Vector2(nextBlockTexture.Width, nextBlockTexture.Height);
    var squareSize = new Vector2(4) * gridSize;
    int fontSize = 25;
    DrawTextPro(GetFontDefault(), "Next", nextBlockPos - new Vector2(0, squareSize.Y / 2), new Vector2(MeasureText("Next", fontSize) / 2, fontSize), 0, fontSize, 3, White);
    DrawTextureV(nextBlockTexture, nextBlockPos - textureSize / 2, White);
    DrawRectangleLinesEx(new Rectangle(nextBlockPos - squareSize / 2, squareSize), 2, White);
  }

  void DrawScoreUI()
  {
    var scorePos = new Vector2(13, 7) * gridSize;
    var fontSize = 25;
    DrawTextPro(GetFontDefault(), "Score", scorePos, new Vector2(MeasureText("Score", fontSize) / 2, fontSize / 2), 0, fontSize, 3, White);
    DrawTextPro(GetFontDefault(), score.ToString(), scorePos + new Vector2(0, gridSize), new Vector2(MeasureText(score.ToString(), fontSize) / 2, fontSize / 2), 0, fontSize, 3, White);
  }

  public void Unload()
  {
    for (int i = 0; i < blockTextures.Count(); i++)
    {
      UnloadTexture(blockTextures.ElementAt(i).Value);
    }
    UnloadSound(breakLinesSound);
  }

  void DrawGrid()
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        var rect = new Rectangle(x * gridSize, y * gridSize, gridSize, gridSize);
        if (grid[x, y] == (int)Colors.BLANK)
          DrawRectangleRounded(rect, 0.3f, 20, White);
        else
          DrawRectangleRounded(rect, 0.3f, 20, ToRayColor((Colors)grid[x, y]));
        DrawRectangleRoundedLines(rect, 0.3f, 20, 2, backgroundColor);
      }
    }
  }

  public static Color ToRayColor(Colors color)
  {
    switch (color)
    {
      case Colors.RED: return Red;
      case Colors.GREEN: return Green;
      case Colors.BLUE: return Blue;
      case Colors.ORANGE: return Orange;
      case Colors.PINK: return Pink;
      case Colors.PURPLE: return Purple;
      case Colors.YELLOW: return Yellow;
      default: return new Color(0, 0, 0, 0);
    }
  }

  public static void DrawNextBlock()
  {
    Block[] blocks = new Block[]{
      new IBlock(),
      new JBlock(),
      new LBlock(),
      new OBlock(),
      new SBlock(),
      new TBlock(),
      new ZBlock()
    };

    Random rnd = new Random();
    nextBlock = blocks[rnd.Next(0, 7)];
  }

  public static void ChangeBlock()
  {
    block = nextBlock;
    DrawNextBlock();
  }
}