using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading.Tasks;

namespace UNO.Cards
{
    /// <summary>
    /// Card status.
    /// Normal - the card is classically visible
    /// Back - the card is turned face down(only the back is visible)
    /// </summary>


    public enum State
    {
        Normal,
        Back
    }

    /// <summary>
    /// Possible card types
    /// Classic - a classic card with color and number
    /// </summary>
    public enum Type
    {
        Classic
    }

    /// <summary>
    /// Card class
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card status
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Card type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Card color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Card Number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Actual card position
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Z index of the card - the larger, the closer along the Z axis to the user
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// Card target position
        /// </summary>
        public Point TargetPosition { get; set; }

        /// <summary>
        /// Card dimensions
        /// </summary>
        public Size Dimensions { get; set; }

        /// <summary>
        /// Card rotation
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Can we play the card? - Gameplay framework
        /// </summary>
        public bool Playable { get; set; }

        /// <summary>
        /// True if the card is face down
        /// </summary>
        public bool Flipped { get; set; }

        /// <summary>
        /// True, if the card should be turned face down - it controls the animation
        /// </summary>
        private bool flipDown = false;

        /// <summary>
        /// True, if the card should turn face up - it controls the animation
        /// </summary>
        private bool flipUp = false;

        /// <summary>
        /// Scale cards along the X-axis
        /// </summary>
        private float flipX = 1;

        /// <summary>
        /// True if the card should reset its spin
        /// </summary>
        private bool settleRotation = false;

        /// <summary>
        /// Animation speed
        /// </summary>
        private int speed = 25;

        /// <summary>
        /// True if the number zoom animation should run
        /// </summary>
        private bool zoom = false;

        /// <summary>
        /// Maximum magnification
        /// </summary>
        private int zoomMaximum = 30;

        /// <summary>
        /// Font size increment after scaling
        /// </summary>
        private int zoomScale = -20;

        /// <summary>
        /// Target angle
        /// </summary>
        private int targetAngle = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public Card(Color color, int number)
        {
            // Initializes the initial internal state of the object 
            Color = color;
            Number = number;

            // Defines standard card dimensions
            Dimensions = new Size(200, 250);

            // Default status
            State = State.Normal;

            // Default type
            Type = Type.Classic;
        }

        /// <summary>
        /// Update card method
        /// </summary>
        public void Update()
        {
            // Should the card be face down?
            if (flipDown)
            {
                // Still not fully turned?
                if (flipX > -1)
                {
                    // Let's turn it a little more
                    flipX -= 0.1f;

                    // Has the arrow crossed the blind spot? = is it completely perpendicular to us?
                    if (flipX < 0)
                    {
                        // He turns the card face down
                        State = State.Back;
                    }
                }
                else
                {
                    // Rotation completed
                    flipDown = false;

                    // Updates the rotation state
                    Flipped = true;
                }
            }

            // Should the card be turned face up?
            if (flipUp)
            {
                // Still not fully turned?
                if (flipX < 1)
                {
                    // Let's turn it a little more
                    flipX += 0.1f;

                    // Has the arrow crossed the blind spot? = is it completely perpendicular to us?
                    if (flipX > 0)
                    {
                        // He turns the card face up
                        State = State.Normal;
                    }
                }
                else
                {
                    // Rotation completed
                    flipUp = false;

                    // Updates the rotation state
                    Flipped = false;
                }
            }

            // Updates the card position. If the actual position is not equal to the target position, the actual position is shifted by a bit
            // closer to the target - first by the animation speed and then, when the card is already very close, only by a pixel to
            // to the perfect match of both positions. It is repeated for all 4 possible directions of movement.
            if (Position.X > TargetPosition.X)
                Position = new Point(Position.X - ((Position.X - TargetPosition.X > speed) ? speed : 1), Position.Y);

            if (Position.X < TargetPosition.X)
                Position = new Point(Position.X + ((TargetPosition.X - Position.X > speed) ? speed : 1), Position.Y);

            if (Position.Y > TargetPosition.Y)
                Position = new Point(Position.X, Position.Y - ((Position.Y - TargetPosition.Y > speed) ? speed : 1));

            if (Position.Y < TargetPosition.Y)
                Position = new Point(Position.X, Position.Y + ((TargetPosition.Y - Position.Y > speed) ? speed : 1));

            // It solves rotation
            // Should the card settle?
            if (settleRotation)
            {
                // Is the rotation greater than the target angle?
                if (Rotation > targetAngle)
                {
                    Rotation -= (Math.Abs(Rotation - targetAngle) > 15) ? 15 : 1;
                }
                // Is the rotation less than the target angle
                else if (Rotation < targetAngle)
                {
                    Rotation += (Math.Abs(Rotation - targetAngle) > 15) ? 15 : 1;
                }
                // The target and actual rotation match, the process is over
                else
                {
                    settleRotation = false;
                }
            }

            // Fixes the zoom animation
            if (zoom)
            {
                // Is the zoom still smaller than the zoom maximum?
                if (zoomScale < zoomMaximum)
                {
                    // Add scale
                    zoomScale += 2;
                }
                else
                {
                    // Maximum reached
                    // Exits zoom
                    zoom = false;

                    // Reverses the zoom scale
                    zoomScale = -zoomMaximum;
                }
            }
        }

        /// <summary>
        /// Rendering method. It will take care of rendering the card based on its values.
        /// </summary>
        public void Draw(Graphics g)
        {
            // Card face down
            if (State == State.Back)
            {
                // White border offset
                int borderPadding = 15;

                // Moves the initial rendering position to the center of the card
                g.TranslateTransform(Position.X + (Dimensions.Width / 2), Position.Y + (Dimensions.Height / 2));

                // Rotates the canvas
                g.RotateTransform(Rotation);

                // Expands the drawing canvas
                g.ScaleTransform(flipX, 1);

                // Turns on anti-aliasing
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Playable border size - the width of the card's effect when it can be played
                int pbs = 10;

                // Can we play the card?
                if (Playable)
                {
                    // Draws an effect around the card
                    using (LinearGradientBrush brush = new LinearGradientBrush(new Point(-(Dimensions.Width / 2) - pbs, -(Dimensions.Height / 2) - pbs), new Point(Dimensions.Width + (pbs * 2), Dimensions.Height + (pbs * 2)), Color.FromArgb(255, 217, 0), Color.FromArgb(231, 66, 0)))
                        g.FillPath(brush, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2) - pbs, -(Dimensions.Height / 2) - pbs, Dimensions.Width + (pbs * 2), Dimensions.Height + (pbs * 2)), 10));
                }


                // Draws the outline / shadow of the card
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, Color.Black)))
                    g.FillPath(brush, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2) - 1, -(Dimensions.Height / 2) - 1, Dimensions.Width + 2, Dimensions.Height + 2), 10));

                // There was a border around the card
                g.FillPath(Brushes.White, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2), -(Dimensions.Height / 2), Dimensions.Width, Dimensions.Height), 10));

                // Color fill
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(45, 45, 45)))
                    g.FillPath(brush, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2) + borderPadding, -(Dimensions.Height / 2) + borderPadding, Dimensions.Width - (borderPadding * 2), Dimensions.Height - (borderPadding * 2)), 10));

                // Rotation
                g.RotateTransform(-45f);

                // Inner white ellipse
                g.FillPath(Brushes.White, Paint.Ellipse(new Point(0, 0), 75, 105));

                // Sets back the scale for the correct rendering of the inscription "UNO"
                g.ScaleTransform(flipX, 1);

                // Creates a string format to center
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;

                // Turns on text antialiasing
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                // Renders UNO text
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(45, 45, 45)))
                    g.DrawString("UNO", new Font("Segoe UI", 36, FontStyle.Bold), brush, new Point(), format);

                // Turns off anti-aliasing
                g.SmoothingMode = SmoothingMode.Default;

                //  Resets the transformation
                g.ResetTransform();

                // Rendering is done - we'll be back
                return;
            }

            // Classic card
            if (Type == Type.Classic)
            {
                // Former border around card
                int borderPadding = 15;

                // Moves the initial rendering position to the center of the card
                g.TranslateTransform(Position.X + (Dimensions.Width / 2), Position.Y + (Dimensions.Height / 2));

                // Rotates the cards
                g.RotateTransform(Rotation);

                // He pulls out a card
                g.ScaleTransform(flipX, 1);

                // Turns off anti-aliasing
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Playable border size - the width of the card's effect when it can be played
                int pbs = 10;

                // Can we play the card?
                if (Playable)
                {
                    // Draws an effect around the card
                    using (LinearGradientBrush brush = new LinearGradientBrush(new Point(-(Dimensions.Width / 2) - pbs, -(Dimensions.Height / 2) - pbs), new Point(Dimensions.Width + (pbs * 2), Dimensions.Height + (pbs * 2)), Color.FromArgb(255, 217, 0), Color.FromArgb(231, 66, 0)))
                        g.FillPath(brush, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2) - pbs, -(Dimensions.Height / 2) - pbs, Dimensions.Width + (pbs * 2), Dimensions.Height + (pbs * 2)), 10));
                }

                // Draws a shadow / border around the card
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, Color.Black)))
                    g.FillPath(brush, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2) - 1, -(Dimensions.Height / 2) - 1, Dimensions.Width + 2, Dimensions.Height + 2), 10));

                // Draws a white border
                g.FillPath(Brushes.White, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2), -(Dimensions.Height / 2), Dimensions.Width, Dimensions.Height), 10));

                // Color fill
                using (SolidBrush brush = new SolidBrush(Color))
                    g.FillPath(brush, Paint.RoundedRectangle(new Rectangle(-(Dimensions.Width / 2) + borderPadding, -(Dimensions.Height / 2) + borderPadding, Dimensions.Width - (borderPadding * 2), Dimensions.Height - (borderPadding * 2)), 10));

                // Color fill
                g.RotateTransform(45f);

                // Draws an ellipse
                g.FillPath(Brushes.White, Paint.Ellipse(new Point(0, 0), 75, 105));

                // Creates a string format to center
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;

                // Turns on text anti-aliasing
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                // Resets rotation
                g.RotateTransform(-45f);

                // Draws a shadow under the card number
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(45, 45, 45)))
                    g.DrawString(Number.ToString(), new Font("Segoe UI", 76 - Math.Abs(zoomScale) + zoomMaximum, FontStyle.Bold | FontStyle.Italic), brush, new Point(-3, -3), format);

                // Renders the card number
                using (SolidBrush brush = new SolidBrush(Color))
                    g.DrawString(Number.ToString(), new Font("Segoe UI", 76 - Math.Abs(zoomScale) + zoomMaximum, FontStyle.Bold | FontStyle.Italic), brush, new Point(), format);

                // Draws the upper left digit
                if (Number == 6)
                    g.DrawString(Number.ToString(), new Font("Segoe UI", 18, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline), Brushes.White, new Point(-76, -105));
                else
                    g.DrawString(Number.ToString(), new Font("Segoe UI", 18, FontStyle.Bold | FontStyle.Italic), Brushes.White, new Point(-76, -105));

                // Spin upside down
                g.RotateTransform(-180f);

                // Draws the lower right number
                if (Number == 6)
                    g.DrawString(Number.ToString(), new Font("Segoe UI", 18, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline), Brushes.White, new Point(-76, -105));
                else
                    g.DrawString(Number.ToString(), new Font("Segoe UI", 18, FontStyle.Bold | FontStyle.Italic), Brushes.White, new Point(-76, -105));

                // Turns off anti-aliasing
                g.SmoothingMode = SmoothingMode.Default;

                // Resets the transformation
                g.ResetTransform();
            }
        }

        /// <summary>
        /// Rotation animation
        /// </summary>
        public void Flip(bool skipAnimation = false)
        {
            // Is the card face down?
            if (Flipped)
            {
                // Starts the model's tummy turning animation
                flipUp = true;

                // Skips the animation if the method parameter is true
                if (skipAnimation)
                {
                    // Stops the progress of the animation
                    flipUp = false;

                    // Updates the card status to normal
                    State = State.Normal;

                    // Sets the horizontal scale of the cards to 1
                    flipX = 1;

                    // The card is no longer face down
                    Flipped = false;
                }
            }
            else
            {
                // The card is not face down
                // Starts the belly-down animation
                flipDown = true;

                // Skips the animation if the method parameter is true
                if (skipAnimation)
                {
                    // Stops the progress of the animation
                    flipDown = false;

                    // Updates the card state to face down
                    State = State.Back;

                    // Sets the horizontal scale of cards to -1
                    flipX = -1;

                    // The card is no longer face down
                    Flipped = true;
                }
            }
        }

        /// <summary>
        /// Sets the angle of the animation card
        /// </summary>
        public void Settle(int angle)
        {
            // Updates the target angle to the passed angle
            targetAngle = angle;

            // Starts the rotation animation
            settleRotation = true;
        }

        /// <summary>
        /// Zoom animation. Triggered when a new card is drawn
        /// </summary>
        public async void Zoom()
        {
            // Waits a while - delay added to start the animation the moment the card
            // arrives from the deck to the hand
            await Task.Delay(400);

            // Starts the zoom animation
            zoom = true;
        }

        /// <summary>
        /// Click the event card
        /// </summary>
        public void MouseClick()
        {
            // Can the player play? If not, there's no point in addressing his cliques
            if (!Player.canPlay)
                return;

            // Is the card in the player's hand?
            if (Player.hand.Contains(this))
            {
                // Is the card playable?
                if (Playable)
                {
                    // The card can be played
                    // We remove the card from the hand
                    Player.hand.Remove(this);

                    // We add it to the pile of played cards
                    Pile.cards.Add(this);

                    // We will set random rotation - just for effect
                    Settle(new Random().Next(-180, 180));

                    // I will refresh the layout so that everything moves to its new place
                    Game.container.RefreshLayout();
                }
                else
                {
                    // Card is not playable - we go back and wait until the player tries to click on a playable card
                    return;
                }
            }

            // The card is not in the player's hand.
            // Is the card in the deck?
            if (Deck.cards.Contains(this))
            {
                // Yes it is - we draw one card to the player
                Player.Draw(1);
            }

            // Turns off the gameplay frame - so that it is no longer visible on the stack of played cards
            Playable = false;

            // Updates the card
            Update();

            // End player's turn
            Player.End();

            // Pass turn to enemy
            Enemy.Play();
        }
    }
}
