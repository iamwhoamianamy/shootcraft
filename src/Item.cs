using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace shootcraft.src
{
   public class Item
   {
      public IStorable Storage { get; private set; }

      public Item(IStorable storage)
      {
         Storage = storage;
      }

      public Item()
      {
         Storage = null;
      }

      public void Draw(Rectangle rect)
      {
         if (Storage is not null &&
             TexturesHandler.blockTextures.TryGetValue(Storage.GetType().Name, out int id))
         {
            GL.BindTexture(TextureTarget.Texture2D, id);
            rect.DrawTexture();
         }
      }
   }
}
