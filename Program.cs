using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

public class Program
{
  public static void Main()
  {
    InitWindow(Game.gridSize * 16, Game.gridSize * 20, "Tetris");
    InitAudioDevice();

    Game game = new Game();
    while (!WindowShouldClose() && !Game.GameOver)
    {
      game.Update();
      game.Draw();
    }
    CloseAudioDevice();
    CloseWindow();
    game.Unload();
  }

}