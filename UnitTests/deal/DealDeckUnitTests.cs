using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework.Legacy;

using BridgeHandGenerator;
using BridgeHandGenerator.cards;
using BridgeHandGenerator.deal;
using BridgeHandGenerator.hand;

namespace BridgeHandGenerator_UnitTests.deal
{
    public class UnitTests_Table_Deck
    {
        public const int MAXTESTS = 1000000;
        public const double MAX_REPETITION_PCT = 0.001 / 100.0; // eq. 1/100,000

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDefaultConstructor()
        {
            Deck deck = new Deck();
            Cards[] expected_cards = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToArray();
            Cards[] cards = deck.GetDeck;

            CollectionAssert.AreEqual(expected_cards, cards);   
        }

        [Test]
        public void TestShuffle()
        {
            Deck deck = new Deck();
            deck.Shuffle();
            Cards[] expected_cards = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToArray();
            Cards[] cards = deck.GetDeck;

            CollectionAssert.AreNotEqual(expected_cards, cards);
        }


        [Test]
        public void TestShuffleMAXTESTS()
        {
            HashSet<ulong> hands = new HashSet<ulong>();
            int repetitions = 0;
            int n_repetitions = 0;
            int e_repetitions = 0;
            int s_repetitions = 0;
            int w_repetitions = 0;

            for (int i = 0; i < MAXTESTS; i++) 
            {
                Deck deck = new Deck();

                Players_Cards positions = deck.Deal();
                Hand n = positions.North;
                Hand e = positions.East;
                Hand s = positions.South;
                Hand w = positions.West;

                if (!hands.Add(n.Compact_hand))
                {
                    repetitions++;
                    n_repetitions++;
                }

                if (!hands.Add(e.Compact_hand))
                {
                    repetitions++;
                    e_repetitions++;
                }

                if (!hands.Add(s.Compact_hand))
                {
                    repetitions++;
                    s_repetitions++;
                }

                if (!hands.Add(w.Compact_hand))
                {
                    repetitions++;
                    w_repetitions++;
                }
            }

            double confidence_max = 4 * MAXTESTS * MAX_REPETITION_PCT;

            Assert.That(repetitions < confidence_max, $"repetitions={repetitions} < {confidence_max} = 4 * {MAXTESTS:N0} * {MAX_REPETITION_PCT} | Position repetitions: N: {n_repetitions}, E:{e_repetitions}, S: {s_repetitions}, W: {w_repetitions}");
        }
        [Test]
        public void TestDealNoArguments()
        {
            Deck deck = new Deck();

            Hand expected_north = new Hand([
                Cards.Clubs_2, Cards.Clubs_6, Cards.Clubs_10, Cards.Clubs_Ace,
                Cards.Diamonds_5, Cards.Diamonds_9, Cards.Diamonds_King,
                Cards.Hearts_4, Cards.Hearts_8, Cards.Hearts_Queen,
                Cards.Spades_3, Cards.Spades_7, Cards.Spades_Jack]);
            Hand expected_east = new Hand([
                Cards.Clubs_3, Cards.Clubs_7, Cards.Clubs_Jack,
                Cards.Diamonds_2, Cards.Diamonds_6, Cards.Diamonds_10, Cards.Diamonds_Ace,
                Cards.Hearts_5, Cards.Hearts_9, Cards.Hearts_King,
                Cards.Spades_4, Cards.Spades_8, Cards.Spades_Queen]);
            Hand expected_south = new Hand([
                Cards.Clubs_4, Cards.Clubs_8, Cards.Clubs_Queen,
                Cards.Diamonds_3, Cards.Diamonds_7, Cards.Diamonds_Jack,
                Cards.Hearts_2, Cards.Hearts_6, Cards.Hearts_10, Cards.Hearts_Ace,
                Cards.Spades_5, Cards.Spades_9, Cards.Spades_King]);
            Hand expected_west = new Hand([
                Cards.Clubs_5, Cards.Clubs_9, Cards.Clubs_King,
                Cards.Diamonds_4, Cards.Diamonds_8, Cards.Diamonds_Queen,
                Cards.Hearts_3, Cards.Hearts_7, Cards.Hearts_Jack,
                Cards.Spades_2, Cards.Spades_6, Cards.Spades_10, Cards.Spades_Ace]);

            Players_Cards actual = deck.Deal();

            ClassicAssert.IsFalse(expected_north.Equals(actual.North));
            ClassicAssert.IsFalse(expected_east.Equals(actual.East));
            ClassicAssert.IsFalse(expected_south.Equals(actual.South));
            ClassicAssert.IsFalse(expected_west.Equals(actual.West));

            deck = new Deck();
            actual = deck.Deal(shuffle: false);
            ClassicAssert.IsTrue(expected_north.Equals(actual.North));
            ClassicAssert.IsTrue(expected_east.Equals(actual.East));
            ClassicAssert.IsTrue(expected_south.Equals(actual.South));
            ClassicAssert.IsTrue(expected_west.Equals(actual.West));
        }

        [Test]
        public void TestDealWithConstraints()
        {
            Hand_constraints North_constraints = new Hand_constraints("North       : (13,15)  sssss ¦ hh_ ¦ dd_ ¦ cc_ ");
            Hand_constraints East_constraints = new Hand_constraints("East        : (7,10)  ss_ ¦ hh__ ¦ dd__ ¦ cc__ ");
            Hand_constraints South_constraints = new Hand_constraints("South       : (8,9)  sss_ ¦ hh_ ¦ dd__ ¦ cc__ ");

            Deck deck = new Deck();

            List<Hand_constraints> hand_constraints = new List<Hand_constraints>() { North_constraints, East_constraints, South_constraints };

            Players_Cards actual = deck.Deal(hand_constraints);

            // Check poinsts and distribution
            ClassicAssert.IsTrue(actual.North.HCP_INITIAL >= North_constraints.Points.Min && actual.North.HCP_INITIAL <= North_constraints.Points.Max);

        }

        [Test]
        public void TestReset()
        {
            // Create deck, deal, and verify deck is empty
            Deck deck = new Deck();
            deck.Deal();
            Cards[] cards = deck.GetDeck;
            ClassicAssert.That(cards.Length == 0);

            // Reset deck, verify deck is full and not shuffled
            deck.Reset(shuffle: false);
            Cards[] expected_cards = Enum.GetValues(typeof(Cards)).Cast<Cards>().ToArray();
            cards = deck.GetDeck;

            CollectionAssert.AreEqual(expected_cards, cards);

            // empty deck
            deck.Deal();
            cards = deck.GetDeck;
            ClassicAssert.That(cards.Length == 0);

            // Reset deck, verify deck is full and shuffled
            deck.Reset(shuffle: true);
            cards = deck.GetDeck;

            CollectionAssert.AreNotEqual(expected_cards, cards);
        }
    }   
}
