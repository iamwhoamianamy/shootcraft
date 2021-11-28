using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shootcraft.src
{
   static class TexturesHandler
   {
      public static Dictionary<Type, int> blockTextures;
      private static string blocksPath = "../../resources/textures/";

      static TexturesHandler()
      {
         blockTextures = new Dictionary<Type, int>();

         foreach (var block in ClassesHandler.Blocks)
         {
            string texureName = blocksPath + block.Name + ".png";
            Bitmap texture;

            try
            {
               texture = new Bitmap(texureName);
            }
            catch
            {
               texture = new Bitmap(blocksPath + "Default.png");
            }

            GL.GenTextures(1, out int textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            blockTextures[block] = textureID;

            BitmapData data = texture.LockBits(new System.Drawing.Rectangle(0, 0, texture.Width, texture.Height),
            ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            texture.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
         }
      }

      public static void Init() { }
   }
}
