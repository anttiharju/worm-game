using Otter.Utility.MonoGame;
using System;
using NUnit.Framework;
using static WormGame.Static.Mathf;
using WormGame.Static;

namespace TestWormGame.Static
{
	[TestFixture]
	public  class TestMathf
	{
		[Test]
		public  void TestFastAbs19()
		{
			Assert.AreEqual( 1, FastAbs(1) , 0.000001, "in method FastAbs, line 20");
			Assert.AreEqual( 0, FastAbs(0) , 0.000001, "in method FastAbs, line 21");
			Assert.AreEqual( 1, FastAbs(-1) , 0.000001, "in method FastAbs, line 22");
		}
		[Test]
		public  void TestFastRound39()
		{
			Assert.AreEqual( 2, FastRound(1.9f) , "in method FastRound, line 40");
			Assert.AreEqual( 4, FastRound(4.1f) , "in method FastRound, line 41");
			Assert.AreEqual( -3, FastRound(-2.9f) , "in method FastRound, line 42");
			Assert.AreEqual( -6, FastRound(-6.1f) , "in method FastRound, line 43");
		}
		[Test]
		public  void TestRotateCW65()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, -1), v = RotateCW(v) , "in method RotateCW, line 67");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCW(v) , "in method RotateCW, line 68");
			Assert.AreEqual( new Vector2(0, 1), v = RotateCW(v) , "in method RotateCW, line 69");
			Assert.AreEqual( new Vector2(-1, 0), v = RotateCW(v) , "in method RotateCW, line 70");
		}
		[Test]
		public  void TestRotateCCW88()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, 1), v = RotateCCW(v) , "in method RotateCCW, line 90");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCCW(v) , "in method RotateCCW, line 91");
			Assert.AreEqual( new Vector2(0, -1), v = RotateCCW(v) , "in method RotateCCW, line 92");
			Assert.AreEqual( new Vector2(-1, 0), v = RotateCCW(v) , "in method RotateCCW, line 93");
		}
		[Test]
		public  void TestSmaller112()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 113");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 114");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 115");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 116");
		}
		[Test]
		public  void TestSmaller133()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 134");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 135");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 136");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 137");
		}
		[Test]
		public  void TestBigger154()
		{
			Assert.AreEqual( 2, Bigger(1, 2) , "in method Bigger, line 155");
			Assert.AreEqual( 2, Bigger(2, 1) , "in method Bigger, line 156");
			Assert.AreEqual( 1, Bigger(-1, 1) , "in method Bigger, line 157");
			Assert.AreEqual( 0, Bigger(0, 0) , "in method Bigger, line 158");
		}
	}
}

