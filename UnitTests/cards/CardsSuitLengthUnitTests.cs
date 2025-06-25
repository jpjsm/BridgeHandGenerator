using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Legacy;

using BridgeHandGenerator;
using BridgeHandGenerator.cards;


namespace BridgeHandGenerator_UnitTests.cards
{
    public class UnitTests_Suit_length
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDefaultConstructor()
        {
            int max = Suit_length.MAX_SUIT_LENGTH;
            int min = Suit_length.MIN_SUIT_LENGTH;

            Suit_length suit = new Suit_length();  

            ClassicAssert.AreEqual(min, suit.Min);
            ClassicAssert.AreEqual(max, suit.Max); 
        }

        [Test]
        public void TestConstructor()
        {
            Suit_length suit = new Suit_length(4, 6);
            ClassicAssert.AreEqual(4, suit.Min);
            ClassicAssert.AreEqual(6, suit.Max);
        }

        [Test]
        public void TestConstructorWithNullMin()
        {
            int min = Suit_length.MIN_SUIT_LENGTH;

            Suit_length suit = new Suit_length(null, 7);

            ClassicAssert.AreEqual(min, suit.Min);
            ClassicAssert.AreEqual(7, suit.Max);
        }
        
        [Test]
        public void TestConstructorWithNullMax()
        {
            int max = Suit_length.MAX_SUIT_LENGTH;

            Suit_length suit = new Suit_length(3, null);
            
            ClassicAssert.AreEqual(max, suit.Max);
            ClassicAssert.AreEqual(3, suit.Min);
        }
        
        [Test]
        public void TestConstructorWithOutOfBoundValues()
        {
            ClassicAssert.Throws<ApplicationException>(() => new Suit_length(-1, 5));
            ClassicAssert.Throws<ApplicationException>(() => new Suit_length(-1, null));

            ClassicAssert.Throws<ApplicationException>(() => new Suit_length(3, 14));
            ClassicAssert.Throws<ApplicationException>(() => new Suit_length(null, 14));

            ClassicAssert.Throws<ApplicationException>(() => new Suit_length(5, 3));
        }

        [Test]
        public void TestToString()
        {
            Suit_length all_null = new Suit_length();
            string all_null_expected = $"({Suit_length.MIN_SUIT_LENGTH},{Suit_length.MAX_SUIT_LENGTH})";
            ClassicAssert.AreEqual(all_null_expected, all_null.ToString());

            Suit_length max_null = new Suit_length(6,null);
            string max_null_expected = $"(6,{Suit_length.MAX_SUIT_LENGTH})";
            ClassicAssert.AreEqual(max_null_expected, max_null.ToString());

            Suit_length min_null = new Suit_length(null,6);
            string min_null_expected = $"({Suit_length.MIN_SUIT_LENGTH},6)";
            ClassicAssert.AreEqual(min_null_expected, min_null.ToString());

            Suit_length min_max = new Suit_length(5,6);
            string min_max_expected = "(5,6)";
            ClassicAssert.AreEqual(min_max_expected, min_max.ToString());
        }

        [Test]
        public void TestHashCode()
        {
            Suit_length all_null = new Suit_length();
            int all_null_hashcode = Suit_length.MAX_SUIT_LENGTH<<4;
            ClassicAssert.AreEqual(all_null_hashcode, all_null.GetHashCode());

            Suit_length min_null_max_value = new Suit_length(null, 8);
            int min_null_max_value_hashcode = 8<<4;
            ClassicAssert.AreEqual(min_null_max_value_hashcode, min_null_max_value.GetHashCode());

            Suit_length min_value_max_null = new Suit_length(4, null);
            int min_value_max_null_hashcode = (Suit_length.MAX_SUIT_LENGTH<<4) | 4;
            ClassicAssert.AreEqual(min_value_max_null_hashcode, min_value_max_null.GetHashCode());

            Suit_length min_value_max_value = new Suit_length(4,7);
            int min_value_max_value_hashcode = (7<<4) | 4;
            ClassicAssert.AreEqual(min_value_max_value_hashcode, min_value_max_value.GetHashCode());

        }

        [Test]
        public void TestEquals()
        {
            Suit_length actual = new Suit_length(3,6);
            (int max, int min) something_else = (3,6);
            ClassicAssert.IsFalse(actual.Equals(something_else));
            ClassicAssert.IsFalse(actual.Equals(null));

            #pragma warning disable CS8602
            ClassicAssert.IsTrue(actual.Equals(actual));
            #pragma warning restore CS8602

            Suit_length expected = new Suit_length(3,6);
            Suit_length not_expected = new Suit_length(2,7);
            ClassicAssert.IsTrue(actual.Equals(expected));
            ClassicAssert.IsFalse(actual.Equals(not_expected));

            Suit_length full_range = new Suit_length(Suit_length.MIN_SUIT_LENGTH,Suit_length.MAX_SUIT_LENGTH);
            Suit_length max_null_min_null = new Suit_length();
            expected = new Suit_length(Suit_length.MIN_SUIT_LENGTH,Suit_length.MAX_SUIT_LENGTH);
            ClassicAssert.IsTrue(full_range.Equals(expected));
            ClassicAssert.IsTrue(max_null_min_null.Equals(expected));

            Suit_length max_null_min_value = new Suit_length(3,null);
            expected = new Suit_length(3,Suit_length.MAX_SUIT_LENGTH);
            ClassicAssert.IsTrue(max_null_min_value.Equals(expected));

            Suit_length max_value_min_null = new Suit_length(null, 7);
            expected = new Suit_length(Suit_length.MIN_SUIT_LENGTH,7);
            ClassicAssert.IsTrue(max_value_min_null.Equals(expected));
        }
    }
}
