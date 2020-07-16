using System;
using Otter.Utility.MonoGame;
using NUnit.Framework;
using static WormGame.Static.Mathf;
using WormGame.Static;

namespace TestWormGame.Static
{
	[TestFixture]
	public  class TestMathf
	{
		[Test]
		public  void TestBigger20()
		{
			Assert.AreEqual( 2, Bigger(1, 2) , "in method Bigger, line 21");
			Assert.AreEqual( 2, Bigger(2, 1) , "in method Bigger, line 22");
			Assert.AreEqual( 1, Bigger(-1, 1) , "in method Bigger, line 23");
			Assert.AreEqual( 0, Bigger(0, 0) , "in method Bigger, line 24");
			Assert.AreEqual( -1, Bigger(-1, -2) , "in method Bigger, line 25");
			Assert.AreEqual( -1, Bigger(-2, -1) , "in method Bigger, line 26");
		}
		[Test]
		public  void TestFastAbs42()
		{
			Assert.AreEqual( 1, FastAbs(1) , 0.000001, "in method FastAbs, line 43");
			Assert.AreEqual( 0, FastAbs(0) , 0.000001, "in method FastAbs, line 44");
			Assert.AreEqual( 1, FastAbs(-1) , 0.000001, "in method FastAbs, line 45");
		}
		[Test]
		public  void TestFastRound61()
		{
			Assert.AreEqual( 2, FastRound(1.9f) , "in method FastRound, line 62");
			Assert.AreEqual( 4, FastRound(4.1f) , "in method FastRound, line 63");
			Assert.AreEqual( 0, FastRound(0.1f) , "in method FastRound, line 64");
			Assert.AreEqual( 0, FastRound(-0.1f) , "in method FastRound, line 65");
			Assert.AreEqual( -3, FastRound(-2.9f) , "in method FastRound, line 66");
			Assert.AreEqual( -4, FastRound(-3.9f) , "in method FastRound, line 67");
		}
		[Test]
		public  void TestRotateCCW84()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, 1), v = RotateCCW(v) , "in method RotateCCW, line 86");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCCW(v) , "in method RotateCCW, line 87");
			Assert.AreEqual( new Vector2(0, -1), v = RotateCCW(v) , "in method RotateCCW, line 88");
			Assert.AreEqual( new Vector2(-1, 0), RotateCCW(v) , "in method RotateCCW, line 89");
		}
		[Test]
		public  void TestRotateCW107()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, -1), v = RotateCW(v) , "in method RotateCW, line 109");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCW(v) , "in method RotateCW, line 110");
			Assert.AreEqual( new Vector2(0, 1), v = RotateCW(v) , "in method RotateCW, line 111");
			Assert.AreEqual( new Vector2(-1, 0), RotateCW(v) , "in method RotateCW, line 112");
		}
		[Test]
		public  void TestSmaller131()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 132");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 133");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 134");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 135");
		}
		[Test]
		public  void TestSmaller152()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 153");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 154");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 155");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 156");
		}
	}
}

