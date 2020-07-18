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
		public  void TestBigger19()
		{
			Assert.AreEqual( 2, Bigger(1, 2) , "in method Bigger, line 20");
			Assert.AreEqual( 2, Bigger(2, 1) , "in method Bigger, line 21");
			Assert.AreEqual( 1, Bigger(-1, 1) , "in method Bigger, line 22");
			Assert.AreEqual( 0, Bigger(0, 0) , "in method Bigger, line 23");
			Assert.AreEqual( -1, Bigger(-1, -2) , "in method Bigger, line 24");
			Assert.AreEqual( -1, Bigger(-2, -1) , "in method Bigger, line 25");
		}
		[Test]
		public  void TestFastAbs41()
		{
			Assert.AreEqual( 1, FastAbs(1) , 0.000001, "in method FastAbs, line 42");
			Assert.AreEqual( 0, FastAbs(0) , 0.000001, "in method FastAbs, line 43");
			Assert.AreEqual( 1, FastAbs(-1) , 0.000001, "in method FastAbs, line 44");
		}
		[Test]
		public  void TestFastRound60()
		{
			Assert.AreEqual( 2, FastRound(2.0000000000000000000000001f) , "in method FastRound, line 61");
			Assert.AreEqual( 1, FastRound(0.9999999999999999999999999f) , "in method FastRound, line 62");
			Assert.AreEqual( 0, FastRound(0.0000000000000000000000001f) , "in method FastRound, line 63");
			Assert.AreEqual( -1, FastRound(-0.9999999999999999999999999f) , "in method FastRound, line 64");
			Assert.AreEqual( -2, FastRound(-2.0000000000000000000000001f) , "in method FastRound, line 65");
		}
		[Test]
		public  void TestRotateCCW82()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, 1), v = RotateCCW(v) , "in method RotateCCW, line 84");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCCW(v) , "in method RotateCCW, line 85");
			Assert.AreEqual( new Vector2(0, -1), v = RotateCCW(v) , "in method RotateCCW, line 86");
			Assert.AreEqual( new Vector2(-1, 0), RotateCCW(v) , "in method RotateCCW, line 87");
		}
		[Test]
		public  void TestRotateCW105()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, -1), v = RotateCW(v) , "in method RotateCW, line 107");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCW(v) , "in method RotateCW, line 108");
			Assert.AreEqual( new Vector2(0, 1), v = RotateCW(v) , "in method RotateCW, line 109");
			Assert.AreEqual( new Vector2(-1, 0), RotateCW(v) , "in method RotateCW, line 110");
		}
		[Test]
		public  void TestSmaller129()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 130");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 131");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 132");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 133");
		}
		[Test]
		public  void TestSmaller150()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 151");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 152");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 153");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 154");
		}
	}
}

