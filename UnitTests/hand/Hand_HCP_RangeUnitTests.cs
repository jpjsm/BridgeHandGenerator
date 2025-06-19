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
    public class UnitTests_HCP_hand_range
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDefaultConstructor()
        {
            int max = HCP_hand_range.MAX_HCP;
            int min = HCP_hand_range.MIN_HCP;

            HCP_hand_range range = new HCP_hand_range();

            ClassicAssert.AreEqual(min, range.Min);
            ClassicAssert.AreEqual(max, range.Max);
        }

        [Test]
        public void TestConstructor()
        {
            HCP_hand_range range = new HCP_hand_range(10, 20);
            ClassicAssert.AreEqual(10, range.Min);
            ClassicAssert.AreEqual(20, range.Max);
        }

        [Test]
        public void TestConstructorWithNullMin()
        {
            int min = HCP_hand_range.MIN_HCP;

            HCP_hand_range range = new HCP_hand_range(null, 20);

            ClassicAssert.AreEqual(min, range.Min);
            ClassicAssert.AreEqual(20, range.Max);
        }
        
        [Test]
        public void TestConstructorWithNullMax()
        {
            int max = HCP_hand_range.MAX_HCP;

            HCP_hand_range range = new HCP_hand_range(10, null);

            ClassicAssert.AreEqual(max, range.Max);
            ClassicAssert.AreEqual(10, range.Min);
        }
        
        [Test]
        public void TestConstructorWithOutOfBoundValues()
        {
            ClassicAssert.Throws<ApplicationException>(() => new HCP_hand_range(-1, 20));
            ClassicAssert.Throws<ApplicationException>(() => new HCP_hand_range(-1, null));

            ClassicAssert.Throws<ApplicationException>(() => new HCP_hand_range(10, 40));
            ClassicAssert.Throws<ApplicationException>(() => new HCP_hand_range(null, 40));

            ClassicAssert.Throws<ApplicationException>(() => new HCP_hand_range(20, 10));
        }


        [Test]
        public void TestToString()
        {
            HCP_hand_range all_null = new HCP_hand_range();
            string all_null_expected = $"({HCP_hand_range.MIN_HCP},{HCP_hand_range.MAX_HCP})";
            ClassicAssert.AreEqual(all_null_expected, all_null.ToString());

            HCP_hand_range max_null = new HCP_hand_range(6,null);
            string max_null_expected = $"(6,{HCP_hand_range.MAX_HCP})";
            ClassicAssert.AreEqual(max_null_expected, max_null.ToString());

            HCP_hand_range min_null = new HCP_hand_range(null,6);
            string min_null_expected = $"({HCP_hand_range.MIN_HCP},6)";
            ClassicAssert.AreEqual(min_null_expected, min_null.ToString());

            HCP_hand_range min_max = new HCP_hand_range(5,6);
            string min_max_expected = "(5,6)";
            ClassicAssert.AreEqual(min_max_expected, min_max.ToString());
        }

        [Test]
        public void TestHashCode()
        {
            HCP_hand_range all_null = new HCP_hand_range();
            int all_null_hashcode = HCP_hand_range.MAX_HCP<<6;
            ClassicAssert.AreEqual(all_null_hashcode, all_null.GetHashCode());

            HCP_hand_range min_null_max_value = new HCP_hand_range(null, 8);
            int min_null_max_value_hashcode = 8<<6;
            ClassicAssert.AreEqual(min_null_max_value_hashcode, min_null_max_value.GetHashCode());

            HCP_hand_range min_value_max_null = new HCP_hand_range(4, null);
            int min_value_max_null_hashcode = (HCP_hand_range.MAX_HCP<<6) | 4;
            ClassicAssert.AreEqual(min_value_max_null_hashcode, min_value_max_null.GetHashCode());

            HCP_hand_range min_value_max_value = new HCP_hand_range(4,7);
            int min_value_max_value_hashcode = (7<<6) | 4;
            ClassicAssert.AreEqual(min_value_max_value_hashcode, min_value_max_value.GetHashCode());

        }

        [Test]
        public void TestEquals()
        {
            HCP_hand_range actual = new HCP_hand_range(3,6);
            (int max, int min) something_else = (3,6);
            ClassicAssert.IsFalse(actual.Equals(something_else));
            ClassicAssert.IsFalse(actual.Equals(null));

            #pragma warning disable CS8602
            ClassicAssert.IsTrue(actual.Equals(actual));
            #pragma warning restore CS8602


            HCP_hand_range expected = new HCP_hand_range(3,6);
            HCP_hand_range not_expected = new HCP_hand_range(2,7);
            ClassicAssert.IsTrue(actual.Equals(expected));
            ClassicAssert.IsFalse(actual.Equals(not_expected));

            HCP_hand_range full_range = new HCP_hand_range(HCP_hand_range.MIN_HCP,HCP_hand_range.MAX_HCP);
            HCP_hand_range max_null_min_null = new HCP_hand_range();
            expected = new HCP_hand_range(HCP_hand_range.MIN_HCP,HCP_hand_range.MAX_HCP);
            ClassicAssert.IsTrue(full_range.Equals(expected));
            ClassicAssert.IsTrue(max_null_min_null.Equals(expected));

            HCP_hand_range max_null_min_value = new HCP_hand_range(3,null);
            expected = new HCP_hand_range(3,HCP_hand_range.MAX_HCP);
            ClassicAssert.IsTrue(max_null_min_value.Equals(expected));

            HCP_hand_range max_value_min_null = new HCP_hand_range(null, 7);
            expected = new HCP_hand_range(HCP_hand_range.MIN_HCP,7);
            ClassicAssert.IsTrue(max_value_min_null.Equals(expected));
        }

    }
}
