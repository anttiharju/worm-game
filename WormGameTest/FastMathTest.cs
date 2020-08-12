using NUnit.Framework;
using static WormGame.Static.FastMath;

namespace TestWormGame.Static
{
	[TestFixture]
	public  class TestFastMath
	{
		[Test]
		public  void TestAbs16()
		{
			Assert.AreEqual( 1, Abs(1)  , 0.000001, "in method Abs, line 17");
			Assert.AreEqual( 0, Abs(0)  , 0.000001, "in method Abs, line 18");
			Assert.AreEqual( 1, Abs(-1) , 0.000001, "in method Abs, line 19");
		}
		[Test]
		public  void TestRound35()
		{
			Assert.AreEqual( 2, Round(2.0000000000000000000000001f)  , "in method Round, line 36");
			Assert.AreEqual( 1, Round(0.9999999999999999999999999f)  , "in method Round, line 37");
			Assert.AreEqual( 0, Round(0.0000000000000000000000001f)  , "in method Round, line 38");
			Assert.AreEqual( -1, Round(-0.9999999999999999999999999f) , "in method Round, line 39");
			Assert.AreEqual( -2, Round(-2.0000000000000000000000001f) , "in method Round, line 40");
		}
	}
}

