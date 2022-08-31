using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UNO.Cards;
using UNO.Components;

namespace UNO
{
    /// <summary>
    /// Translation - Player class. It procures his cards in hand, handles his events like mouse clicks and so on
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Translation - A collection of cards in a player's hand
        /// </summary>
        public static List<Card> hand = new List<Card>();
        public static Color drawnCardColor = Color.White;

        /// <summary>
        /// Translation - True if it is the user's turn and can play
        /// </summary>
        public static bool canPlay = true;

        /// <summary>
        /// Translation - Update player method
        /// </summary>
        public static void Update()
        {
            // Translation - He goes through all the cards in his hand
            foreach (Card card in hand)
            {
                // Translation - Can the user play?
                if (canPlay)
                {
                    // Translation - Is there a played card on the pile?
                    if (Pile.cards.Count > 0)
                    {
                        // Translation - The top card of the pile
                        Card pileTop = Pile.cards.Last();

                        // Translation - Can the card be played?
                        if (Helper.CompareCards(card, pileTop))
                        {
                            // Translation - The card is playable
                            card.Playable = true;
                        }
                        else
                        {
                            // Translation - The card cannot be played now
                            card.Playable = false;
                        }
                    }
                    else
                    {
                        // Translation - There is no played card on the pile. The game has probably just started, so we can play any card.
                        card.Playable = true;
                    }
                }
                else
                {
                    // Translation - User can't play - turns off the gameplay frame
                    card.Playable = false;
                }

                // Translation - Finally, we call our own update of the currently browsed card
                card.Update();
            }

            // Translation - Number of cards we can play
            int playable = 0;

            // Translation - He goes through all the cards in his hand
            foreach (Card card in hand)
            {
                // Translation - Can we play the card?
                if (card.Playable)
                    // Translation - Yes - we will increase the total number of playable cards
                    playable++;
            }

            // Translation - Is it the user's turn?
            if (canPlay)
            {
                // Translation - Auxiliary variable - the top card of the playing deck
                Card top = Deck.cards.Last();

                // Translation - If we cannot play any card
                if (playable == 0)
                {
                    // Translation - We have nothing to play
                    // We will highlight the top card of the playing deck
                    top.Playable = true;
                }
                else
                {
                    // Translation - We have something to play
                    // There is no reason to emphasize the top card of the playing deck
                    top.Playable = false;
                }

                // Translation - We are updating the top tab
                top.Update();
            }
        }

        /// <summary>
        /// Translation - Renders the player's hand
        /// </summary>
        public static void Draw(Form f, Graphics g)
        {
            // Translation - He goes through all the cards in his hand
            for (int i = 0; i < hand.Count; i++)
            {
                // Translation - Gets the currently browsed tab - helper variable
                Card card = hand[i];

                // Translation - Sets the Z-Index
                card.ZIndex = i;

                // Translation - Finally, we draw the card - we call its draw method
                card.Draw(g);
            }
        }

        /// <summary>
        /// Translation - Aligns the player's hand
        /// </summary>
        public static void AlignHand(Form f)
        {
            // Translation - Total width of all cards in hand
            int handWidth = 0;

            // Translation - Offset of individual cards. In short, by how big a piece will the individual cards overlap.
            // The offset increases with the increasing number of cards in the hand to fit as many as
            // possible on the screen (the number is limited to 20 by default)
            int xOffset = - (hand.Count * 7);

            // Translation - Calculates the total width of the cards in the hand including offsets
            // Go through all the cards in the hand
            foreach (Card card in hand)
            {
                // Translation - Increments the total width of the hand by the width of the currently scrolled card.
                handWidth += card.Dimensions.Width + xOffset;
            }

            // Translation - The starting position of the first card in the hand
            // This ensures that the cards are always nicely centered
            int start = (f.ClientRectangle.Width / 2) - (handWidth / 2) + (xOffset / 2);

            // Translaiton - Renders the enemy's hand
            for (int i = 0; i < hand.Count; i++)
            {
                // Translation - Sets the target position of the card to a newly updated position based on previous calculations
                // The target position differs from the actual position by changing the actual position until it equals the target position.
                // In short, the target position is where the card should be and the actual position is where the card is physically located.
                // Thanks to this system, card movement animations are achieved.
                hand[i].TargetPosition = new Point(start + ((hand[i].Dimensions.Width + xOffset) * i), f.ClientRectangle.Height - hand[i].Dimensions.Height - 20);
            }
        }

        /// <summary>
        /// Translation - The player's lick method.
        /// This is not a rendering method, but merely a name collision between the English words lick and render. 
        /// The method accepts as a parameter an int number that represents the number of cards to be drawn by the player.
        /// </summary>
        public static async void Draw(int count)
        {
            // Translation - Repeats X times where X is the number of cards licked (count)
            foreach (var i in Enumerable.Range(0, count))
            {
                // Translation - We check if there are any cards left in the deck
                if (Deck.cards.Count == 0)
                    Deck.Create();

                // Translation - We check if the player still has less than the maximum 20 cards in his hand
                if (hand.Count == 20)
                    return;

                // Translation - The top card from the playing deck
                Card top = Deck.cards[Deck.cards.Count - 1];

                // Translation - Removes the top card from the deck (because the player drew it)
                Deck.cards.Remove(top);

                // Translation - Removes card rotation
                top.Settle(0);

                // Translation - He turns the card face up for us to see
                top.Flip();

                // Translation - Starts the card number animation
                top.Zoom();

                // Translation - Adds a card to our hand
                hand.Add(top);
                drawnCardColor = top.Color;

                // Translation - Updates the layout and sends all the cards to where they belong
                Game.container.RefreshLayout();

                // Translation - Adds a small delay so we don't find all the cards at once
                await Task.Delay(350);
            }
        }

        /// <summary>
        /// Translation - Handles user (player) clique actions (events)
        /// </summary>
        public static void Click(Form f)
        {
            // The same procedure as the click event for the package class (Deck.cs) - described in more detail there
            // A collection of cards that have been clicked on - just setting it up so far
            List<Card> clickedCards = new List<Card>();

            // Translation - He goes through all the cards in his hand
            foreach (Card card in hand)
            {
                // Translation - Gets the mouse position
                Point p = f.PointToClient(Cursor.Position);

                // Cards bounds
                Rectangle bounds = new Rectangle(card.Position.X, card.Position.Y, card.Dimensions.Width, card.Dimensions.Height);

                // Translation - Did the user click on the currently browsed tab?
                if (bounds.Contains(p))
                {
                    // Translation - Adds a card to the collection
                    clickedCards.Add(card);
                }
            }

            // Translation - Did we click on any tab?
            if (clickedCards.Count == 0)
                // Translation - No - we can go back in no time
                return;

            // Translation - Triggers the click method of the card closest to the user
            clickedCards.OrderByDescending(item => item.ZIndex).First().MouseClick();

            // Translation - Resets the collection of clicked cards for further use at the next click event
            clickedCards.Clear();
        }

        // Translation - The player's playing method.
        public static void Play()
        {
            // Translation - It will allow players to play
            canPlay = true;
        }

        /// <summary>
        /// Translation - Ends the player's turn
        /// </summary>
        public static void End()
        {
            // Translation - He ends his turn
            canPlay = false;
        }
    }
}
