using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNO;

namespace Karty.Components
{
    public static class WildSelector
    {
        public static int ColorId = 0;
        public static Color Color = Color.Red;

        public static Color PickColor(int number)
        {
            if (number > 4 || number < 1)
            {
                return Color;
            }

            number = number - 1;
            ColorId = number;

            if (Deck.isFlipped)
            {
                Color = Deck.FlipColors[number];
            } else
            {
                Color = Deck.LightColors[number];
            }

            return Color;
        }

        public static void DrawCircle(this Graphics g, Pen pen,
                                  float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        public static void FillCircle(this Graphics g, Brush brush,
                                      float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        public static void DrawSelector(Graphics g)
        {
            FillCircle(g, new SolidBrush(Color), 50, 50, 15);
        }
    }
}
