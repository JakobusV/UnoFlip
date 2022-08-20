using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNO.Cards;
using UNO.Components;

namespace UNO
{
    /// <summary>
    /// Třída nepřítel. Řeší "umělou inteligenci" - AI nepřítele. Definuje pár základních
    /// metod pro umožnění jeho interakce s hráčem a tak dále.
    /// </summary>
    class Enemy
    {
        /// <summary>
        /// Translation - A collection of cards that the enemy holds in his hand.
        /// </summary>
        public static List<Card> hand = new List<Card>();

        /// <summary>
        /// Translation - Update the enemy.
        /// </summary>
        public static void Update()
        {
            // Translation - Passes all the cards in his hand
            foreach (Card card in hand)
            {
                // Následně zavolá jejich update metody
                card.Update();
            }
        }

        /// <summary>
        /// Translation - He draws the opponent the cards he has on his hand. The method accepts rendering context and form,
        /// - so that the cards have somewhere to draw and correctly zarovanalis the height and width of the submitted form.
        /// </summary>
        public static void Draw(Form f, Graphics g)
        {
            // Translation - Passes all the cards in his hand
            foreach (Card card in hand)
            {
                // Translation - Renders the tab that you are currently browsing
                card.Draw(g);
            }
        }

        /// <summary>
        /// Translation - Solves the positioning of each card in the hand of the enemy. The method is usually called at the start of the game or when the window is resized.
        /// </summary>
        public static void AlignHand(Form f)
        {
            // Translation - Total width of all cards in the hand
            int handWidth = 0;

            // Translation - Offset of individual cards. In short, by how large a piece the individual cards will overlap.
            // Translation - The offset increases with the increasing number of cards in the hand to fit as many as possible on the screen (the number is limited by default to 20)
            int xOffset = -(hand.Count * 7);

            // Translation - Calculates the total width of the cards in the hand, including the offset
            // Translation - Passes all the cards in your hand
            foreach (Card card in hand)
            {
                // Translation - Increments the total width of the hand by the width of the card just passed.
                handWidth += card.Dimensions.Width + xOffset;
            }

            // Translation - Starting position of the first card in hand
            // translation - The fact is that the cards will always be nicely on the center
            int start = (f.ClientRectangle.Width / 2) - (handWidth / 2) + (xOffset / 2);

            // Translation - Draws the hand of the enemy
            for (int i = 0; i < hand.Count; i++)
            {
                // Transtation - Sets the target position of the card to the newly updated position based on previous calculations
                // Translation - The target position differs from the real position in such a way that the actual position changes until it is equal to the target position.
                // translation - In short, the target position is where the card should be and the real one where the card is physically located.
                // Translation - Thanks to this system, card movement animations are achieved.
                hand[i].TargetPosition = new Point(start + ((hand[i].Dimensions.Width + xOffset) * i), 20);
            }
        }

        /// <summary>
        /// translation - Enemy licking method. This is not a rendering method, but only a name collision between the English words lick
        ///and render. The method accepts as a parameter an int number that represents the number of cards the enemy should draw.
        /// </summary>
        public static async void Draw(int count)
        {
            // Translation - Repeats X-times where X is the number of cards drawn (count)
            foreach (var i in Enumerable.Range(0, count))
            {
                // Translation - We check if there are any cards left in the deck
                if (Deck.cards.Count == 0)
                    Deck.Create();

                // translation - We check if the enemy still has less than the maximum 20 cards in his hand
                if (hand.Count == 20)
                    return;

                // Translation - The top card from the playing deck
                Card top = Deck.cards[Deck.cards.Count - 1];

                // Translation - Removes the top card from the deck (because the enemy drew it)
                Deck.cards.Remove(top);

                // Translation - Removes card rotation
                top.Settle(0);

                // Translation - Adds a card to the enemy's hand.
                hand.Add(top);

                // Translation - Refresh layout causes that after removing a card from the collection of the playing deck
                // and then adding it to the enemy's card collection will animate the transfer from the deck to the enemy's hand.
                Game.container.RefreshLayout();

                // Translation - Adds a delay so that the cards don't line up but nicely spaced out
                await Task.Delay(350);
            }
        }

        /// <summary>
        /// Translation - Enemy play method. Its "AI". Here it is decided which card the enemy will play.
        /// </summary>
        public async static void Play()
        {
            // Translation - Creates a random delay between 0.75 and 1.5 seconds to make it look like the enemy is "thinking" xD
            await Task.Delay(new Random().Next(750, 1500));

            //Game over checkk
            if (Game.gameOver)
            {
                Player.Play();
                return;
            }
            // Translation - Has a card been played? - has the game just started?
            if (Pile.cards.Count == 0)
            {
                // Translation - He plays the most expendable card in the suit
                PlayCard(ChooseBestCard(hand));
            }
            else
            {
                // Translation - A card has already been loaded
                // Translation - He gets the last card played
                Card lastPlayed = Pile.cards.Last();

                // Translation - A list of all the cards the enemy could play - so far only his establishment
                List<Card> playable = new List<Card>();

                // Translation - Goes through all the cards in the enemy's hand
                foreach (Card card in hand)
                {
                    // Translation - It will compare if the currently browsed card and the last played card are compatible with each other.
                    // Transation - That is, if they can be played on top of each other.
                    if (Helper.CompareCards(card, lastPlayed))
                    {
                        // Translation - The cards can be played on each other, we will add it to the collection of playable cards
                        playable.Add(card);
                    }
                }

                // Translation - Does the enemy have something to play? Do we have any cards in the playable card collection?
                if (playable.Count == 0)
                {
                    // Translation - The enemy has nothing to play, so he licks
                    Draw(1);
                }
                else
                {
                    // Translation - The enemy can play something.
                    //Translation - Does he have any choices? Can he play more than one card?
                    if (playable.Count == 1)
                    {
                        // Tranlsation - He has no choice, so he plays the only card he can
                        PlayCard(playable.First());
                    }
                    else
                    {
                        // Translation - The enemy has a choice (can play 2 or more cards)
                        // Translation - We pass a deck of playable cards to a method that decides which one is best to play.
                        PlayCard(ChooseBestCard(playable));
                    }
                }

                // Translation - Clears the collection of playable cards for further use
                playable.Clear();
            }

            // Translation - Gives the bike to the user
            Player.Play();
        }

        /// <summary>
        /// Translation - The method accepts a sheet of cards, from which it selects the one that will be most advantageous for playing
        /// </summary>
        private static Card ChooseBestCard(IEnumerable<Card> cards)
        {
            // Tranlsation - Creates a new dictionary that will contain the number of individual cards from each suit
            Dictionary<Color, int> balance = new Dictionary<Color, int>();

            foreach (Color color in Deck.LightColors)
            {
                balance.Add(color, hand.Where(c => c.Color == color).Count());
            }

            // Translation - Sorts the dictionary by the number of individual colors in descending order. This means that there will be such a color at the top,
            // from which we have the most cards.
            var ordered = balance.OrderBy(item => item.Value).Reverse();

            // Translation - Let's go through the dictionary
            foreach (KeyValuePair<Color, int> entry in ordered)
            {
                // Translation - Goes through all playable cards
                foreach (Card card in cards)
                {
                    // Translation - Selects the first card from the playable cards from which we have the most suits
                    if (card.Color == entry.Key)
                    {
                        // Translation - He returns our card
                        return card;
                    }
                }
            }

            // Translaton - The AI ​​didn't come up with the best possible play, so it plays the first possible card
            return cards.First();
        }

        /// <summary>
        /// Translation - The method takes a tab as a parameter. The enemy then plays this card.
        /// </summary>
        public static void PlayCard(Card card)
        {
            // Translation - Removes a card from the enemy's hand.
            hand.Remove(card);

            // Translation - Adds a card to the played card pile
            Pile.cards.Add(card);

            // Translation - Turns the card (with animation) face up so that it is visible
            card.Flip();

            // Translation - Sets random card rotation (for effect only)
            card.Settle(new Random().Next(-180, 180));

            // Tranlsation - Aligns the entire playing field = moves the cards to where they belong based on their collections where they are placed.
            Game.container.RefreshLayout();
        }
    }
}
