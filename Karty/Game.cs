using UNO.Cards;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UNO;
using System.Collections.Generic;
using Karty.Components;

namespace UNO
{
    /// <summary>
    /// Translation - Dirty program classs
    /// </summary>
    public partial class Game : Form
    {
        /// <summary>
        /// Game container (form)
        /// </summary>
        public static Game container = null;
        public static bool gameOver = false;   


        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {
            // Initialize form's controls
            InitializeComponent();

            // Assign game container
            container = this;

            // Turn on DoubleBuffer to prevent flickering
            DoubleBuffered = true;

            // Start in full screen
            WindowState = FormWindowState.Maximized;

            // Initialize timer
            mainTimer.Start();

            // Create new deck
            Deck.Create();

            // Align deck
            Deck.Align(this);

            // Creates new pile
            Pile.Create();

            // Align pile
            Pile.Align(this);

            // Align enemy hand
            Enemy.AlignHand(this);

            // Initializes enemy
            // Enemy.Init();

            // Setup game
            // Draw 7 cards for player
            Player.Draw(7);

            // Draw 7 cards for enemy
            Enemy.Draw(7);

            // Předá kolo hráči
            Player.Play();

            container.KeyPress += Window_KeyPress;
        }

        /// <summary>
        /// Update method / timer tick
        /// </summary>
        private void Update(object sender, EventArgs e)
        {
            checkWinner();
           
            // Updates player
            Player.Update();

            // Update enemy
            Enemy.Update();

            // Updates deck
            Deck.Update();

            // Updates pile
            Pile.Update();

            WildSelector.DrawSelector(CreateGraphics());

            // Redraw whole scene
            Invalidate();
        }

        private void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("HELO MOTHER FUCKER");
            switch (e.KeyChar)
            {
                case (char)Keys.D1:
                    WildSelector.PickColor(1);
                    break;
                case (char)Keys.D2:
                    WildSelector.PickColor(2);
                    break;
                case (char)Keys.D3:
                    WildSelector.PickColor(3);
                    break;
                case (char)Keys.D4:
                    WildSelector.PickColor(4);
                    break;
            }
        }

        /// <summary>
        /// Draw method. Paints whole game scene
        /// </summary>
        private void Draw(object sender, PaintEventArgs e)
        {
            // Draw player's stuff
            Player.Draw(this, e.Graphics);

            // Draw enemy
            Enemy.Draw(this, e.Graphics);

            // Draw deck
            Deck.Draw(e.Graphics);

            // Draw pile
            Pile.Draw(e.Graphics);
        }

        /// <summary>
        /// Form's click event
        /// </summary>
        private void OnClick(object sender, EventArgs e)
        {
            // Handles players click actions
            Player.Click(this);

            // Handles deck click actions
            Deck.Click(this);

            Debug();
        }

        /// <summary>
        /// Form's resize event
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            // Align player's hand
            Player.AlignHand(this);

            // Align deck
            Deck.Align(this);

            // Align pile
            Pile.Align(this);

            // Align enemy hand
            Enemy.AlignHand(this);
        }

        /// <summary>
        /// Calls form's resize event
        /// </summary>
        public void RefreshLayout()
        {
           OnResize(EventArgs.Empty);
        }

        //checking if either players cards are empmty if so, makes game over
        public void checkWinner()
        {
            if (gameOver)
            {
                Player.canPlay = false;
                return;
            }
            if(Player.hand.Count == 0)
            {
                gameOver = true;
                Player.canPlay = false;
                MessageBox.Show("Game Over. You Win!");
            }
            else if(Enemy.hand.Count == 0)
            {
                gameOver = true;
                Player.canPlay=false;
                MessageBox.Show("Game Over. Emeny Wins!");

            }
            else
            {
                return;
            }
        }

        private void Debug()
        {
            // Count of cards in Draw, Discard, Player, and Enemy
            Console.WriteLine("Deck:" + Deck.cards.Count() + "\tPile:" + Pile.cards.Count() + "\tPlayer:" + Player.hand.Count() + "\tEnemy:" + Enemy.hand.Count());

            // Gather all cards made, Count of card colors
            List<Card> debug_cards = new List<Card>();
            debug_cards.AddRange(Deck.cards);
            debug_cards.AddRange(Pile.cards);
            debug_cards.AddRange(Player.hand);
            debug_cards.AddRange(Enemy.hand);
            Console.WriteLine("Red:" + debug_cards.Where(c => c.Color == Color.Red).Count()
                + "\tBlue:" + debug_cards.Where(c => c.Color == Color.Blue).Count()
                + "\tGreen:" + debug_cards.Where(c => c.Color == Color.Green).Count()
                + "\tYellow:" + debug_cards.Where(c => c.Color == Color.Yellow).Count());
        }
    }
}
