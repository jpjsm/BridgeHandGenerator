using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Legacy;

using BridgeHandGenerator;
using BridgeHandGenerator.cards;
using BridgeHandGenerator.deal;
using BridgeHandGenerator.hand;

namespace BridgeHandGenerator_UnitTests.hand
{
    public class UnitTests_Hand_constraints
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void TestPointsAndDistributionConstructor()
        {
            HCP_hand_range minimum_opening_hand_points = new HCP_hand_range(13, 15);
            Hand_suits_distribution opening_spades_simple = new Hand_suits_distribution( 
                spades: new Suit_length(5,5),
                hearts: new Suit_length(2,3),
                diamonds: new Suit_length(2,3),
                clubs: new Suit_length(2,3)
            );

            Hand_constraints opener = new Hand_constraints(minimum_opening_hand_points, opening_spades_simple, Positions.North);
            string opener_str = opener.ToString();
            ClassicAssert.AreEqual("North       : (13,15)  sssss ¦ hh_ ¦ dd_ ¦ cc_ ", opener_str);
            ClassicAssert.AreEqual(19, opener.Constraint_level);
            ClassicAssert.AreEqual(minimum_opening_hand_points, opener.Points);
            ClassicAssert.AreEqual(opening_spades_simple, opener.Shape);
            ClassicAssert.AreEqual(Positions.North, opener.Position);

            HCP_hand_range minimum_responder_hand_points = new HCP_hand_range(6, 7);
            Hand_suits_distribution responder_spades_simple = new Hand_suits_distribution( 
                spades: new Suit_length(3,3),
                hearts: new Suit_length(null,3),
                diamonds: new Suit_length(null,null),
                clubs: new Suit_length(null,null)
            );

            Hand_constraints responder = new Hand_constraints(minimum_responder_hand_points, responder_spades_simple, Positions.North);
            ClassicAssert.AreEqual(7, responder.Constraint_level);
            ClassicAssert.AreEqual(minimum_responder_hand_points, responder.Points);
            ClassicAssert.AreEqual(responder_spades_simple, responder.Shape);
        }

        [Test]
        public void TestStringConstructor()
        {
            HCP_hand_range minimum_opening_hand_points = new HCP_hand_range(13, 15);
            Hand_suits_distribution opening_spades_simple = new Hand_suits_distribution( 
                spades: new Suit_length(5,5),
                hearts: new Suit_length(2,3),
                diamonds: new Suit_length(2,3),
                clubs: new Suit_length(2,3)
            );

            Hand_constraints expected = new Hand_constraints(minimum_opening_hand_points, opening_spades_simple, Positions.North);
            Hand_constraints actual = new Hand_constraints(expected.ToString());
            ClassicAssert.AreEqual(expected.Constraint_level, actual.Constraint_level);
            ClassicAssert.AreEqual(expected.Points, actual.Points);
            ClassicAssert.IsTrue(expected.Shape.Equals(actual.Shape));
            ClassicAssert.AreEqual(expected.Position, actual.Position);
        }

        [Test]
        public void TestConstructorExceptions()
        {
            HCP_hand_range points = new HCP_hand_range();

            #pragma warning disable CS8600
            HCP_hand_range null_points = null;
            #pragma warning restore CS8600

            Suit_length undefined_suit_length = new Suit_length();
            Hand_suits_distribution shape = new Hand_suits_distribution(undefined_suit_length, undefined_suit_length, undefined_suit_length, undefined_suit_length);

            #pragma warning disable CS8600
            Hand_suits_distribution null_shape = null;
            #pragma warning restore CS8600

            #pragma warning disable CS8604
            ClassicAssert.Throws<ApplicationException>(() => new Hand_constraints(null_points, shape, Positions.North));
            ClassicAssert.Throws<ApplicationException>(() => new Hand_constraints(points, null_shape, Positions.North));
            ClassicAssert.Throws<ApplicationException>(() => new Hand_constraints(null_points, null_shape, Positions.North));
            #pragma warning restore CS8604
        }

    }
}