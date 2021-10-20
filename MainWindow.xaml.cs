﻿using System;
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
      private Timer timer;
      private float t = 0.0f;
      private const int fps = 60;
      private const float ellapsed = 1.0f / fps;
      private Logger logger;
      private int screenW = 800, screenH = 450;
      private Player player;
      private Vector2 screenCenter;
      private float GForce = 3500.0f;

      private ChunkHandler chunkHandler;

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

         logger = Logger.Get();

         screenCenter.X = screenW / 2;
         screenCenter.Y = screenH / 2;

         chunkHandler = new ChunkHandler();
         player = new Player(screenCenter);

         timer = new Timer(1.0 / fps * 1000);
         timer.Elapsed += Timer_Elapsed;
         timer.AutoReset = false;
         timer.Start();

         player.ResolveCollisionPrediction(chunkHandler, ellapsed);
         //player.ResolveCollisionDiagonal(chunkHandler, 1.0f / fps);
      }

      private void Timer_Elapsed(object sender, ElapsedEventArgs e)
      {
         UpdatePhysics();
         glControl.Invalidate();

         timer.Start();
      }

      private void UpdatePhysics()
      {
         if (!player.IsStanding)
            player.ApplyForce(new Vector2(0.0f, -GForce));

         //player.UpdateLocation(ellapsed);
         //player.ResolveCollisionEfremov(chunkHandler, ellapsed);
         player.ResolveCollisionPrediction(chunkHandler, ellapsed);
         player.CheckForStanding(chunkHandler);

         ControllPlayer();
      }

      private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
      {
         GL.Viewport(0, 0, glControl.Width, glControl.Height);
         GL.Clear(ClearBufferMask.ColorBufferBit);

         // Draw objects here
         GL.ClearColor(1.0f, 1.0f, 1.0f, 1f);

         Vector2 drawing_center = player.pos;

         chunkHandler.DrawVisibleChunks(drawing_center, 2);
         player.Draw();

         //Chunk player_chunk = chunkHandler.GetChunk(player.pos);
         //player_chunk.DrawBorders();
         //Block player_block = player_chunk.GetBlock(player.pos);
         //player_block.DrawBorders();

         Chunk cursor_chunk = chunkHandler.GetChunk(player.cursor.pos);
         //cursor_chunk.DrawBorders();
         Block cursor_block = cursor_chunk.GetBlock(player.cursor.pos);
         cursor_block.DrawBorders();

         player.cursor.Draw();

         glControl.SwapBuffers();

         Title = $"{player.cursor.pos}";
      }

      private void glControl_Resize(object sender, EventArgs e)
      {
         screenW = glControl.Width;
         screenH = glControl.Height;

         screenCenter.X = screenW / 2;
         screenCenter.Y = screenH / 2;

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
      }
   }
}
