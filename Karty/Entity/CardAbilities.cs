using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO;
using UNO.Cards;

namespace Karty.Components
{
    public class CardAbilities
    {
        public void SkipAbility()
        {
            if (Player.canPlay)
            {
                Player.Play();
            }
            if (!Player.canPlay)
            {
                Enemy.Play();
            }
        }
        public void ReverseAbility()
        {

        }

        public void DrawOneAbility()
        {
            if (Player.canPlay)
            {
                Enemy.Draw(1);
                return;
            }
            if (!Player.canPlay)
            {
                Player.Draw(1);
            }
        }

        public void DrawFiveAbility()
        {
            if (Player.canPlay)
            {
                Enemy.Draw(5);
                return;
            }
            if (!Player.canPlay)
            {
                Player.Draw(5);
            }
        }

        public void FlipAbility()
        {

        }

        public void WildAbility(Color color)
        {
            Pile.cards[Pile.cards.Count - 1].Color = color;
        }
        //Not quite sure if overrideing the Piles top cards color will break anything
        public void WildDrawTwoAbility(Color color)
        {
            if (Player.canPlay)
            {
                Enemy.Draw(2);
                Pile.cards[Pile.cards.Count - 1].Color = color;
            }
            if (!Player.canPlay)
            {
                Player.Draw(2);
                Pile.cards[Pile.cards.Count - 1].Color = color;
            }
        }
        /// <summary>
        /// Who ever plays this card makes the other player draw until they reach specified color
        /// </summary>

        public void DrawUntilColorWild(Color color)
        {
            if (Player.canPlay)
            {
                do
                {
                    Enemy.Draw(1);
                } while (Enemy.drawnCardColor != color);
            }
            if (!Player.canPlay)
            {
                do
                {
                    Player.Draw(1);
                } while (Player.drawnCardColor != color);
            }
        }
    }
}
