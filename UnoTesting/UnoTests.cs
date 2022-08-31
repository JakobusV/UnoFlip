using Microsoft.VisualStudio.TestTools.UnitTesting;
using UNO;
using System;
using Karty.Components;
using System.Drawing;

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

            var cards1 = Deck.cards;
            Deck.Shuffle();
            var cards2 = Deck.cards;
            Assert.AreNotEqual(cards1, cards2);
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
            if(color != Color.Red)
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
        public void PlayerWinTest()
        {

        }
        [TestMethod]
        public void ShuffleCardsTest()
        {
            
        }
        [TestMethod]
        public void CreateGameTest()
        {
            
        }
        [TestMethod]
        public void mouseClickTestTest()
        {
            
        }
        [TestMethod]
        public void SkipTest()
        {
            /*var expexted = Player.canPlay;
            var actual = !Player.canPlay;
            Assert.AreNotEqual(expexted, actual);*/
            
        }
        [TestMethod]
        public void reverseTest()
        {
            
        }
        [TestMethod]
        public void skipAllTest()
        {
            
        }
        [TestMethod]
        public void flipTest()
        {
            
        }
        [TestMethod]
        public void plusOneTest()
        {
            var expexted = Player.hand.Count == 8;
            var actual = Player.hand.Count;
            Player.Draw(1);
            CardAbilities.DrawOneAbility();
            Assert.AreEqual(expexted, actual);
        }
        [TestMethod]
        public void plusFiveTest()
        {
            var expexted = Player.hand.Count == 12;
            var actual = Player.hand.Count;
            Player.Draw(5);
            CardAbilities.DrawOneAbility();
            Assert.AreEqual(expexted, actual);
        }
        [TestMethod]
        public void wildTest()
        {            
            CardAbilities.WildAbility(Color.Red);
            var expected = Color.Red;
            var actual = Pile.cards[Pile.cards.Count - 1].Color;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void wildDrawTwoTest()
        {
            var expectedHand = Player.hand.Count == 9;
            Player.Draw(2);
            var actualHand = Player.hand.Count;
            CardAbilities.WildDrawTwoAbility(Color.Red);
            var expectedColor = Color.Red;
            var actualColor = Pile.cards[Pile.cards.Count - 1].Color;
            Assert.AreEqual(expectedHand, actualHand);
            Assert.AreEqual(expectedColor, actualColor);
        }
        [TestMethod]
        public void wildDrawTillMatchTest()
        {
            CardAbilities.WildAbility(Color.Red);
            var expected = Color.Red;
            var actual = Pile.cards[Pile.cards.Count - 1].Color;
            Assert.AreEqual(expected, actual);
        }
    }
}
