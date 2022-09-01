using UNO.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace UNO
{
    /// <summary>
    /// T - The pile of last played cards.
    /// </summary>
    public class Pile
    {
        /// <summary>
        /// T - Collection of last played cards.
        /// </summary>
        public static List<Card> cards = new List<Card>();

        /// <summary>
        /// T - Creates a new stack.
        /// </summary>
        public static void Create()
        {
        }

        /// <summary>
        /// T- Aligns the stack of cards to its correct position.
        /// </summary>
        public static void Align(Form f)
        {
            // T -An instance of the Random class
            Random r = new Random();

            //T - It goes through all the cards in the pile
            foreach (Card card in cards)
            {
                // T - It will set their position to the correct place just off the center
                card.TargetPosition = new Point(f.ClientRectangle.Width / 2 - card.Dimensions.Width / 2 + 170, f.ClientRectangle.Height / 2 - card.Dimensions.Height / 2);
            }
        }

        /// <summary>
        /// T - Update stacks of last played cards.
        /// </summary>
        public static void Update()
        {
            // T- It goes through all the cards
            foreach (Card card in cards)
            {
                // T - Updates the currently browsed card
                card.Update();
            }
        }

        /// <summary>
        /// T - Draws a stack
        /// </summary>
        public static void Draw(Graphics g)
        {
            // T - For performance reasons, it will only render 4 or fewer cards.
            for (int i = cards.Count - ((cards.Count - 4 > 4) ? 4 : cards.Count); i < cards.Count; i++)
            {
                // T - Draws the currently browsed card - calls its draw mwtod.
                cards[i].Draw(g);
            }
        }
    }
}
