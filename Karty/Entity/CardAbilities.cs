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
        /// <summary>
        /// Skips the next players turn (case 10 in Card.cs)
        /// </summary>
        public static void SkipAbility()
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

        /// <summary>
        /// Skips all players turn, the player that played the card gets to play again (case 11 in Card.cs)
        /// </summary>
        public void SkipAllAbility()
        {

        }

        /// <summary>
        /// Reverses play order (case 12 in Card.cs)
        /// </summary>
        public static void ReverseAbility()
        {

        }

        /// <summary>
        /// Next player draws 1 card (case 13 in Card.cs)
        /// </summary>
        public static void DrawOneAbility()
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

        /// <summary>
        /// Next player draws 5 cards (case 14 in Card.cs)
        /// </summary>
        public static void DrawFiveAbility()
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

        /// <summary>
        /// All card in both hands and deck become (case 15 in Card.cs)
        /// </summary>
        public static void FlipAbility()
        {
            List<Card> allCards = new List<Card>();

            allCards.AddRange(Deck.cards);
            allCards.AddRange(Pile.cards);
            allCards.AddRange(Player.hand);
            allCards.AddRange(Enemy.hand);

            foreach (Card card in allCards)
            {
                card.UnoFlip();
            }

            Deck.isFlipped = !Deck.isFlipped;
        }

        /// <summary>
        /// Lets player choose what color is the playable color (case 16 in Card.cs)
        /// </summary>
        /// <param name="color"></param>
        public static void WildAbility(Color color)
        {
            Pile.cards[Pile.cards.Count - 1].Color = color;
        }
        /// <summary>
        /// Lets player choose what color is the playable color & next player draws 2 cards (case 17 in Card.cs)
        /// NOTE: Not quite sure if overrideing the Piles top cards color will break anything
        /// </summary>
        /// <param name="color"></param>
        public static void WildDrawTwoAbility(Color color)
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
        /// Who ever plays this card makes the other player draw until they reach specified color (case 18 in Card.cs)
        /// </summary>
        public static void DrawUntilColorWild(Color color)
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
