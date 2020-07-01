using Otter;
using NUnit.Framework;
using static WormGame.Help.Mathf;
using WormGame.Help;

namespace TestWormGame.Help
{
	[TestFixture]
	public  class TestMathf
	{
		[Test]
		public  void TestFastAbs18()
		{
			Assert.AreEqual( 1, FastAbs(1) , 0.000001, "in method FastAbs, line 19");
			Assert.AreEqual( 0, FastAbs(0) , 0.000001, "in method FastAbs, line 20");
			Assert.AreEqual( 1, FastAbs(-1) , 0.000001, "in method FastAbs, line 21");
		}
		[Test]
		public  void TestRotateCW37()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, -1), v = RotateCW(v) , "in method RotateCW, line 39");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCW(v) , "in method RotateCW, line 40");
			Assert.AreEqual( new Vector2(0, 1), v = RotateCW(v) , "in method RotateCW, line 41");
			Assert.AreEqual( new Vector2(-1, 0), v = RotateCW(v) , "in method RotateCW, line 42");
		}
		[Test]
		public  void TestRotateCCW60()
		{
			Vector2 v = new Vector2(-1, 0);
			Assert.AreEqual( new Vector2(0, 1), v = RotateCCW(v) , "in method RotateCCW, line 62");
			Assert.AreEqual( new Vector2(1, 0), v = RotateCCW(v) , "in method RotateCCW, line 63");
			Assert.AreEqual( new Vector2(0, -1), v = RotateCCW(v) , "in method RotateCCW, line 64");
			Assert.AreEqual( new Vector2(-1, 0), v = RotateCCW(v) , "in method RotateCCW, line 65");
		}
		[Test]
		public  void TestSmaller84()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 85");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 86");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 87");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 88");
		}
		[Test]
		public  void TestSmaller105()
		{
			Assert.AreEqual(  1, Smaller(1, 2)  , "in method Smaller, line 106");
			Assert.AreEqual(  1, Smaller(2, 1)  , "in method Smaller, line 107");
			Assert.AreEqual( -1, Smaller(-1, 1) , "in method Smaller, line 108");
			Assert.AreEqual(  0, Smaller(0, 0)  , "in method Smaller, line 109");
		}
		[Test]
		public  void TestBigger126()
		{
			Assert.AreEqual( 2, Bigger(1, 2) , "in method Bigger, line 127");
			Assert.AreEqual( 2, Bigger(2, 1) , "in method Bigger, line 128");
			Assert.AreEqual( 1, Bigger(-1, 1) , "in method Bigger, line 129");
			Assert.AreEqual( 0, Bigger(0, 0) , "in method Bigger, line 130");
		}
	}
}

