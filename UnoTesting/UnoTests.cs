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
            CardAbilities cardAbilities = new CardAbilities();
            var expexted = Player.hand.Count == 8;
            var actual = Player.hand.Count;
            Player.Draw(1);
            cardAbilities.DrawOneAbility();
            Assert.AreEqual(expexted, actual);
        }
        [TestMethod]
        public void plusFiveTest()
        {
            CardAbilities cardAbilities = new CardAbilities();
            var expexted = Player.hand.Count == 12;
            var actual = Player.hand.Count;
            Player.Draw(5);
            cardAbilities.DrawOneAbility();
            Assert.AreEqual(expexted, actual);
        }
        [TestMethod]
        public void wildTest()
        {
            CardAbilities cardAbilities = new CardAbilities();
            cardAbilities.WildAbility(Color.Red);
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
            CardAbilities cardAbilities = new CardAbilities();
            cardAbilities.WildDrawTwoAbility(Color.Red);
            var expectedColor = Color.Red;
            var actualColor = Pile.cards[Pile.cards.Count - 1].Color;
            Assert.AreEqual(expectedHand, actualHand);
            Assert.AreEqual(expectedColor, actualColor);
        }
        [TestMethod]
        public void wildDrawTillMatchTest()
        {
            CardAbilities cardAbilities = new CardAbilities();
            cardAbilities.WildAbility(Color.Red);
            var expected = Color.Red;
            var actual = Pile.cards[Pile.cards.Count - 1].Color;
            Assert.AreEqual(expected, actual);
        }
    }
}
