using System;
using WormGame.Core;
using NUnit.Framework;
using WormGame.Pooling;

namespace TestWormGame.Pooling
{
	[TestFixture]
	public  class TestPooler
	{
		[Test]
		public  void TestSort114()
		{
			Config testConfig = new Config();
			Pooler<PoolableObject> testPool = new Pooler<PoolableObject>(testConfig, 5);
			PoolableObject p1 = testPool.Enable();
			PoolableObject p2 = testPool.Enable();
			PoolableObject p3 = testPool.Enable();
			PoolableObject p4 = testPool.Enable();
			PoolableObject p5 = testPool.Enable();
			p1.Disable();
			p3.Disable();
			Assert.AreEqual( p1, testPool[0] , "in method Sort, line 124");
			Assert.AreEqual( p2, testPool[1] , "in method Sort, line 125");
			Assert.AreEqual( p3, testPool[2] , "in method Sort, line 126");
			Assert.AreEqual( p4, testPool[3] , "in method Sort, line 127");
			Assert.AreEqual( p5, testPool[4] , "in method Sort, line 128");
			try
			{
			Assert.AreEqual( p5, testPool[5] , "in method Sort, line 129");
			Assert.Fail("Did not throw IndexOutOfRangeException in method Sort on line 128");
			}
			catch (IndexOutOfRangeException)
			{
			}
			Assert.AreEqual( 4, testPool.EnableIndex , "in method Sort, line 130");
			Assert.AreEqual( true, testPool.HasAvailable(2) , "in method Sort, line 131");
			Assert.AreEqual( false, testPool.HasAvailable(3) , "in method Sort, line 132");
			Assert.AreEqual( 3, testPool.EnableIndex , "in method Sort, line 133");
			Assert.AreEqual( p5, testPool[0] , "in method Sort, line 134");
			Assert.AreEqual( p2, testPool[1] , "in method Sort, line 135");
			Assert.AreEqual( p4, testPool[2] , "in method Sort, line 136");
			Assert.AreEqual( p3, testPool[3] , "in method Sort, line 137");
			Assert.AreEqual( p1, testPool[4] , "in method Sort, line 138");
		}
	}
}

