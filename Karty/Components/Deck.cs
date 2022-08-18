using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UNO.Cards;

namespace UNO
{
    /// <summary>
    /// Translation - Game pack. The package methods are static because I don't expect there to be multiple instances
    /// of the game package in the game = there is always only one game package.
    /// </summary>
    class Deck
    {
        // Translation - Creates a new instance of the Random class.
        static Random r = new Random();

        /// <summary>
        /// All light colors available in deck
        /// </summary>
        public static List<Color> LightColors = new List<Color>() 
        {
            Color.Red, 
            Color.Blue, 
            Color.Green, 
            Color.Yellow
        };

        /// <summary>
        /// Translation - A collection of all the cards in the deck.
        /// </summary>
        public static List<Card> cards = new List<Card>();

        /// <summary>
        /// Translation - Shuffles the deck - randomizes the order of the cards in the cards collection
        /// </summary>
        public static void Shuffle()
        {
            // Makes a duplicate of cards to keep track of unshuffled cards
            List<Card> unshuffled = new List<Card>();
            unshuffled.AddRange(cards);

            // Card being transfered to new pile
            Card focus_card = null;

            // Index used for setting card z index, also can be used for current card count.
            int current_index = 0;

            // Reset the card collection
            cards.Clear();

            while(unshuffled.Count > 0)
            {
                // Get card to transfer
                focus_card = unshuffled[r.Next(unshuffled.Count)];

                // Set card to new Z index
                focus_card.ZIndex = current_index;

                // Add card to Deck
                cards.Add(focus_card);

                // Remove from unshuffled list
                unshuffled.Remove(focus_card);
                
                // Increment for next index
                current_index++;
            }
        }

        /// <summary>
        /// Translation - Creates a new game deck - fills the cards collection 
        /// with cards (Card objects) according to the rules of the UNO card game.
        /// </summary>
        public static void Create()
        {
            // Translation - Resets the card collection if it was previously filled
            cards.Clear();

            // List of all possible colors
            List<Color> colors = LightColors;

            // Index used for setting card z index, also can be used for current card count.
            int current_index = 0;

            // Cycle through all possible colors
            foreach (Color color in colors)
            {
                // Cycle through 1 - 9
                for (int i = 1; i < 10; i++)
                {
                    // Create two of each numbered card
                    CreateCard(color, i, ref current_index);
                    CreateCard(color, i, ref current_index);
                }
            }

            // Shuffle new collection of cards
            Shuffle();
        }

        private static void CreateCard(Color color, int number, ref int index)
        {
            // Translation - Creates a Card instance and chooses a random color and number from 0 to 7.
            Card card = new Card(color, number);

            // Translation - Assigns a Z-Index to the card. This index tells us how close along the
            // imaginary Z axis the card is towards the observer (player / user).
            // The larger the Z-Index, the closer the card is to the observer.
            // The Z-Index is important to know which card the user clicked on if there are multiple
            // individual cards on top of each other - like in our game deck.
            card.ZIndex = index;

            // Translation - Turns all cards in the deck face down.
            // The boolean parameter of the method ensures that the unwanted animation
            // that would be seen at the start of the game is not performed.
            card.Flip(true);

            // Translation - Sets the rotation of the card from -180 to 180. Only a visual effect
            // and a detail that does not affect the functionality of the game.
            card.Rotation = r.Next(-180, 180);

            // Translation - Finally, we add our card to the collection of all cards in the pack.
            cards.Add(card);
        }

        /// <summary>
        /// Translation - Places the deck in the correct position in the game window. 
        /// The method is called whenever there is a change in the size of the window and thus the need
        /// to move the package so that it is always in the correct position a little from the center.
        /// </summary>
        public static void Align(Form f)
        {
            // Translation - Go through all the cards in our deck.
            // We want to update the position of each card in the deck.
            foreach (Card card in cards)
            {
                // Translation - Updates the position of the card so that it is always close to the center.
                card.Position = card.TargetPosition = new Point(f.ClientRectangle.Width / 2 - card.Dimensions.Width / 2 - 170, f.ClientRectangle.Height / 2 - card.Dimensions.Height / 2);
            }
        }

        /// <summary>
        /// Translation - Update method of the game package.
        /// Ensures that the Update method of each card in the deck is called.
        /// </summary>
        public static void Update()
        {
            // Translation - It goes through all the cards in our game deck.
            foreach (Card card in cards)
            {
                // Translation - Calls the update method of the currently browsed card.
                card.Update();
            }
        }

        /// <summary>
        /// Translation - Package rendering method. Ensures that your deck is rendered on the game screen.
        /// </summary>
        public static void Draw(Graphics g)
        {
            // Translation - It always passes the last 5 or less cards in the deck.
            // We only render 5 or fewer cards here due to performance limitations of Windows Forms.
            // As you know, they are not graphically accelerated and therefore do not use a graphics card for rendering, but only a processor.
            for (int i = cards.Count - ((cards.Count - 5 > 5) ? 5 : cards.Count); i < cards.Count; i++)
            {
                // Translation - It calls the Draw method of the currently browsed card = renders the card.
                cards[i].Draw(g);
            }
        }

        /// <summary>
        /// Translation - Click package method. Handles what happens when the user clicks on the package.
        /// </summary>
        public static void Click(Form f)
        {
            // Translation - A collection of cards that the user has clicked on.
            // We remember that the user can click on multiple tabs at the same time due to their overlap.
            List<Card> clickedCards = new List<Card>();

            // Translation - We iterate through the entire collection backwards for the reason that
            // we can modify the elements of the collection we are traversing on the fly.
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                // Translation - Gets the instance of the card we are currently browsing - a helper variable.
                Card card = cards[i];

                // Translation - Gets the mouse position.
                Point p = f.PointToClient(Cursor.Position);

                // Translation - Card area (rectangle) - again auxiliary variable
                Rectangle bounds = new Rectangle(card.Position.X, card.Position.Y, card.Dimensions.Width, card.Dimensions.Height);

                // Translation - Checks if the tab area contains a mouse.
                // Since we are in the Click event, this would mean that the user clicked on the tab.
                if (bounds.Contains(p))
                {
                    // Translation - The user actually clicked on the currently browsed tab.
                    // We will therefore add it to the collection of clicked cards.
                    clickedCards.Add(card);
                }
            }

            // Translation - It detects if the user has clicked on any tab at all.
            // If the collection is empty, none has been clicked and we can jump out of the click event.
            if (clickedCards.Count == 0)
                return;

            // Translation - The collection is not empty, so there is at least one or more cards that have been clicked.
            // Sorts the collection of clicked cards in descending order and calls the Click method on the
            // first card from the sorted collection. We do this to activate the click only for the card
            // that was on top of all the others (that is, closest to the user)
            clickedCards.OrderByDescending(item => item.ZIndex).First().MouseClick();

            // Translation - Finally, we clean up the collection of clicked cards so that it is ready for the next click.
            clickedCards.Clear();
        }
    }
}
