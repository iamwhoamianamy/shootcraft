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
using shootcraft.src.blocks;

using Keyboard = OpenTK.Input.Keyboard;
using Key = OpenTK.Input.Key;
using System.IO;

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
      private float globalScaling = 20.0f;

      public MainWindow()
      {
         InitializeComponent();
      }
      private void WindowsFormsHost_Initialized(object sender, EventArgs e)
      {
         glControl.MakeCurrent();
      }

      public Player GetPlayer()
      {
         return player;
      }

      public void SetPlayer(Player player)
      {
         this.player = player;
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

         GUI.SetDimensions(screenW, screenH);

         scale = 1.0f;

         //if (File.Exists(SavesHandler.path + SavesHandler.worldName + ".zip"))
         if(false)
         {
            SavesHandler.DecompressJsons(SavesHandler.worldName);
            World.RestoreFromJson(SavesHandler.worldName);
            player = SavesHandler.RestorePlayerFromJson(SavesHandler.worldName);
            SavesHandler.CompressJsons(SavesHandler.worldName);
         }
         else
         {
            World.Init();
            player = new Player(new Vector2(0, 40.0f));
         }

         World.SetVisibleChunks(player.pos, player.fow);
         World.UpdateLighting();

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
            World.SetVisibleChunks(player.pos, player.fow);
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

         GL.ClearColor(World.SkyColor);

         translation = screenCenterGame - player.pos;

         GL.Scale(globalScaling, globalScaling, 0);
         GL.Translate(screenCenterGame.X, screenCenterGame.Y, 0);
         GL.Scale(scale, scale, 0);
         GL.Translate(-screenCenterGame.X, -screenCenterGame.Y, 0);
         GL.Translate(translation.X, translation.Y, 0);

         DrawInWorldCoordinates();

         GL.Translate(-translation.X, -translation.Y, 0);
         GL.Translate(screenCenterGame.X, screenCenterGame.Y, 0);
         GL.Scale(1 / scale, 1 / scale, 0);
         GL.Translate(-screenCenterGame.X, -screenCenterGame.Y, 0);
         GL.Scale(1 / globalScaling, 1 / globalScaling, 0);

         if (player.IsEnventoryOpened)
            player.MyInventory.Draw();

         //Title = $"{player.cursor.pos} {World.PosToChunkId(player.cursor.pos)}";
         Title = $"{player.MyInventory.ActiveCell}";

         glControl.SwapBuffers();
      }

      private void DrawInWorldCoordinates()
      {
         World.DrawVisibleChunks();

         player.Draw();

         Block cursor_block = player.GetBlockUnderCursor();

         GL.Color4(Color4.Black);
         cursor_block?.DrawBorders();

         player.DrawCursor();
      }

      private void DrawCircle()
      {
         Vector2 start = player.pos;
         List<Vector2> circle = RasterMaster.GetShiftedCircle(start);

         GL.Color4(Color4.Green);

         foreach (var vec in circle)
         {
            var ray = RasterMaster.RasterLine(start, vec);

            foreach (var (block, dist) in ray)
            {
               float saturation = dist / RasterMaster.circleRadius;
               block.GetRectangle().DrawColor(new Color4(saturation, saturation, saturation, 1.0f));
            }

            //World.TryGetBlock(vec)?.DrawBorders();
         }
      }

      private void glControl_Resize(object sender, EventArgs e)
      {
         screenW = glControl.Width;
         screenH = glControl.Height;

         screenCenterWindow.X = screenW / 2;
         screenCenterWindow.Y = screenH / 2;

         screenCenterGame = screenCenterWindow / globalScaling;
         GUI.SetDimensions(screenW, screenH);

         GL.Disable(EnableCap.DepthTest);
         GL.Viewport(0, 0, screenW, screenH);
         GL.MatrixMode(MatrixMode.Projection);
         GL.LoadIdentity();
         GL.Ortho(0, screenW, 0, screenH, -1.0, 1.0);
         GL.MatrixMode(MatrixMode.Modelview);
         GL.LoadIdentity();
      }

      private void Window_Closed(object sender, EventArgs e)
      {
         Logger.Close();
         SavesHandler.DecompressJsons(SavesHandler.worldName);
         SavesHandler.SaveToJson(player, SavesHandler.worldName);
         World.SaveToJson(SavesHandler.worldName);
         SavesHandler.CompressJsons(SavesHandler.worldName);
         SavesHandler.AddLastPlayedInfo();
      }
   }
}
