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
		public  void TestDefrag110()
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
			Assert.AreEqual( p1, testPool[0] , "in method Defrag, line 120");
			Assert.AreEqual( p2, testPool[1] , "in method Defrag, line 121");
			Assert.AreEqual( p3, testPool[2] , "in method Defrag, line 122");
			Assert.AreEqual( p4, testPool[3] , "in method Defrag, line 123");
			Assert.AreEqual( p5, testPool[4] , "in method Defrag, line 124");
			try
			{
			Assert.AreEqual( p5, testPool[5] , "in method Defrag, line 125");
			Assert.Fail("Did not throw IndexOutOfRangeException in method Defrag on line 124");
			}
			catch (IndexOutOfRangeException)
			{
			}
			Assert.AreEqual( 4, testPool.EnableIndex , "in method Defrag, line 126");
			Assert.AreEqual( true, testPool.Check(2) , "in method Defrag, line 127");
			Assert.AreEqual( false, testPool.Check(3) , "in method Defrag, line 128");
			Assert.AreEqual( 3, testPool.EnableIndex , "in method Defrag, line 129");
			Assert.AreEqual( p5, testPool[0] , "in method Defrag, line 130");
			Assert.AreEqual( p2, testPool[1] , "in method Defrag, line 131");
			Assert.AreEqual( p4, testPool[2] , "in method Defrag, line 132");
			Assert.AreEqual( p3, testPool[3] , "in method Defrag, line 133");
			Assert.AreEqual( p1, testPool[4] , "in method Defrag, line 134");
		}
	}
}

