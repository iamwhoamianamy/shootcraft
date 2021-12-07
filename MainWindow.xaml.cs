using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using shootcraft.src;

using Keyboard = OpenTK.Input.Keyboard;
using Key = OpenTK.Input.Key;

namespace shootcraft
{
   /// <summary>
   /// Логика взаимодействия для MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private Timer drawingTimer;
      private Timer worldUpdatingTimer;
      private float t = 0.0f;
      private const int fps = 60;
      private const int ticksToUpdate = 5;
      private int currentTick = 0;
      private const float ellapsed = 1.0f / fps;
      private Logger logger;
      private int screenW = 800, screenH = 450;
      private Player player;
      private Vector2 screenCenterWindow;
      private Vector2 screenCenterGame;
      private Vector2 translation;
      private float scale;
      private int currentChunk = 0;
      private float globalScaling = 20.0f;

      public MainWindow()
      {
         InitializeComponent();
      }
      private void WindowsFormsHost_Initialized(object sender, EventArgs e)
      {
         glControl.MakeCurrent();
      }

      private void glControl_Load(object sender, EventArgs e)
      {
         //glControl.Cursor = System.Windows.Forms.Cursors.No;
         GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
         GL.Enable(EnableCap.DepthTest);

         ClassesHandler.Init();
         logger = Logger.Get();
         TexturesHandler.Init();

         screenCenterWindow.X = screenW / 2;
         screenCenterWindow.Y = screenH / 2;

         scale = 1.0f;

         if (true)
         {
            World.Init();
            player = new Player(new Vector2(0, 40.0f));
         }
         else
         {
            World.RestoreFromJson("world1");
            player = SavesHandler.RestorePlayerFromJson("world1");
         }

         worldUpdatingTimer = new Timer(1.0 / ticksToUpdate * 1000);
         worldUpdatingTimer.Elapsed += WorldUpdatingTimer_Elapsed;
         worldUpdatingTimer.AutoReset = false;
         worldUpdatingTimer.Start();

         drawingTimer = new Timer(1.0 / fps * 1000);
         drawingTimer.Elapsed += DrawingTimer_Elapsed;
         drawingTimer.AutoReset = false;
         drawingTimer.Start();

         player.ResolveCollisionPrediction(ellapsed);
      }

      private void DrawingTimer_Elapsed(object sender, ElapsedEventArgs e)
      {
         UpdatePhysics();
         UpdateWorld();

         glControl.Invalidate();
         drawingTimer.Start();
      }

      private void UpdateWorld()
      {
         if (currentTick == 0)
         {
            World.SetVisibleChunks(player.pos, 11);
            World.UpdateVisibleChunks();
         }

         currentTick++;

         if (currentTick == ticksToUpdate)
            currentTick = 0;
      }

      private void WorldUpdatingTimer_Elapsed(object sender, ElapsedEventArgs e)
      {

         worldUpdatingTimer.Start();
      }

      private void UpdatePhysics()
      {
         if (!player.IsStanding)
            player.ApplyForce(new Vector2(0.0f, -World.gForce));

         //player.UpdateLocation(ellapsed);
         //player.ResolveCollisionEfremov(chunkHandler, ellapsed);
         player.ResolveCollisionPrediction(ellapsed);
         player.CheckForStanding();
         player.CheckForWater();

         ControllPlayer();
      }

      private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
      {
         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

         // Draw objects here
         GL.ClearColor(World.SkyColor);

         translation = screenCenterGame - player.pos;

         GL.Translate(screenCenterGame.X, screenCenterGame.Y, 0);
         GL.Scale(scale, scale, 0);
         GL.Translate(-screenCenterGame.X, -screenCenterGame.Y, 0);

         GL.Translate(translation.X, translation.Y, 0);

         
         World.DrawVisibleChunks();

         player.Draw();

         //Chunk player_chunk = chunkHandler.GetChunk(player.pos);
         //player_chunk.DrawBorders();
         //Block player_block = player_chunk.GetBlock(player.pos);
         //player_block.DrawBorders();

         //Chunk cursor_chunk = World.GetChunk(player.cursor.pos);
         //currentChunk = cursor_chunk.Index;
         //cursor_chunk.DrawBorders();
         Block cursor_block = player.GetBlockUnderCursor();
         cursor_block?.DrawBorders();

         player.DrawCursor();

         GL.Translate(-translation.X, -translation.Y, 0);

         GL.Translate(screenCenterGame.X, screenCenterGame.Y, 0);
         GL.Scale(1 / scale, 1 / scale, 0);
         GL.Translate(-screenCenterGame.X, -screenCenterGame.Y, 0);

         glControl.SwapBuffers();

         Title = $"{player.cursor.pos} {currentChunk}";
      }

      private void glControl_Resize(object sender, EventArgs e)
      {
         screenW = glControl.Width;
         screenH = glControl.Height;

         screenCenterWindow.X = screenW / 2;
         screenCenterWindow.Y = screenH / 2;

         screenCenterGame = screenCenterWindow / globalScaling;

         GL.Disable(EnableCap.DepthTest);
         GL.Viewport(0, 0, screenW, screenH);
         GL.MatrixMode(MatrixMode.Projection);
         GL.LoadIdentity();
         GL.Ortho(0, screenW / globalScaling, 0, screenH / globalScaling, -1.0, 1.0);
         GL.MatrixMode(MatrixMode.Modelview);
         GL.LoadIdentity();
      }

      private void Window_Closed(object sender, EventArgs e)
      {
         Logger.Close();
         World.SaveToJson("world1");
         SavesHandler.SaveToJson(player, "world1");
      }
   }
}
