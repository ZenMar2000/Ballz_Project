using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballz
{
    enum DrawLayer { Background, Middleground, Playground, Foreground, GUI, Last }

    static class DrawMngr
    {
        private static List<I_Drawable>[] items;

        static DrawMngr()
        {
            items = new List<I_Drawable>[(int)DrawLayer.Last];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new List<I_Drawable>();
            }
        }

        public static void AddItem(I_Drawable item)
        {
            items[(int)item.Layer].Add(item);
        }

        public static void RemoveItem(I_Drawable item)
        {
            items[(int)item.Layer].Remove(item);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Clear();
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < items.Length; i++)
            {//array iteration
                for (int j = 0; j < items[i].Count; j++)
                {//list iteration
                    items[i][j].Draw();
                } 
            }
        }
    }
}
