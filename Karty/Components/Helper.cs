using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Cards;

namespace UNO.Components
{
    /// <summary>
    /// Helper class. It contains what did not fit elsewhere.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Compares two cards. Returns true if they are playable on top of each other.
        /// </summary>
        public static bool CompareCards(Card c1, Card c2)
        {
            // Color comparison
            if (c1.Color == c2.Color)
            {
                return true;
            }

            // Comparing numbers
            if (c1.Number == c2.Number)
            {
                return true;
            }

            if(c1.Color == Color.Black)
            {
                return true;
            }

            // Cards have nothing to do with it, it returns false
            return false;
        }
    }
}
