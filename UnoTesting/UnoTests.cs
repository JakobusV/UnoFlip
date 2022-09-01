using Microsoft.VisualStudio.TestTools.UnitTesting;
using UNO;
using System;
using Karty.Components;
using System.Drawing;
using UNO.Cards;
using System.Collections.Generic;

namespace UnoTesting
{
    [TestClass]
    public class UnoTests
    {
        //shuffle, create, mouseClick, checkWin, card abiities
        [TestMethod]
        public void GenerateDeckTest()
        {
            Deck.Create();
            Card topCard = Deck.cards[0];
            Deck.Shuffle();
            Assert.AreNotEqual(topCard, Deck.cards[0]);
        }

        [TestMethod]
        public void IsFlippedTest()
        {
            Deck.isFlipped = true;
            if (Deck.isFlipped)
            {
                
            }
            else
            {

            }
        }

        [TestMethod]
        public void ColorPickerTest1()
        {
            var color = WildSelector.PickColor(69);
            if (color.IsKnownColor)
            {
                Assert.IsTrue(color.IsKnownColor);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ColorPickerTest2()
        {
            var color = WildSelector.PickColor(1);
            if (color != Color.Red)
            {
                Assert.Fail();
            }
            else {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void ColorPickerTest3()
        {
            var color = WildSelector.PickColor(2);
            if (color != Color.Blue)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void ColorPickerTest4()
        {
            var color = WildSelector.PickColor(3);
            if (color != Color.Green)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void ColorPickerTest5()
        {
            var color = WildSelector.PickColor(4);
            if (color != Color.Yellow)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void ColorPickerTest6()
        {
            Deck.isFlipped = true;
            var color = WildSelector.PickColor(1);
            if (color != Color.Orange)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void ColorPickerTest7()
        {
            Deck.isFlipped = true;
            var color = WildSelector.PickColor(2);
            if (color != Color.Purple)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void ColorPickerTest8()
        {
            Deck.isFlipped = true;
            var color = WildSelector.PickColor(3);
            if (color != Color.Pink)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void ColorPickerTest9()
        {
            Deck.isFlipped = true;
            var color = WildSelector.PickColor(4);
            if (color != Color.Cyan)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(color.IsKnownColor);
            }
        }

        [TestMethod]
        public void WildDraw2Card()
        {
            Deck.Create();

            Pile.cards = new List<Card>() { new Card(Color.Blue, 10) };

            CardAbilities.WildDrawTwoAbility(Color.Red);

            Assert.IsTrue(Pile.cards[Pile.cards.Count - 1].Color == Color.Red);
        }

        [TestMethod]
        public void WildCard()
        {
            Deck.Create();

            Pile.cards = new List<Card>() { new Card(Color.Blue, 10) };

            CardAbilities.WildAbility(Color.Red);

            Assert.IsTrue(Pile.cards[Pile.cards.Count - 1].Color == Color.Red);
        }
    }
}
