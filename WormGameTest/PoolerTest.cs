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
		public  void TestSort110()
		{
			#if DEBUG
			Config testConfig = new Config();
			Pooler<PoolableObject> testPool = new Pooler<PoolableObject>(testConfig, 5);
			PoolableObject a = testPool.Enable();
			PoolableObject b = testPool.Enable();
			PoolableObject c = testPool.Enable();
			PoolableObject d = testPool.Enable();
			PoolableObject e = testPool.Enable();
			a.Disable();
			c.Disable();
			Assert.AreEqual( a, testPool[0] , "in method Sort, line 121");
			Assert.AreEqual( b, testPool[1] , "in method Sort, line 122");
			Assert.AreEqual( c, testPool[2] , "in method Sort, line 123");
			Assert.AreEqual( d, testPool[3] , "in method Sort, line 124");
			Assert.AreEqual( e, testPool[4] , "in method Sort, line 125");
			try
			{
			testPool[5] = d;
			Assert.Fail("Did not throw IndexOutOfRangeException in method Sort on line 125");
			}
			catch (IndexOutOfRangeException)
			{
			}
			Assert.AreEqual( 4, testPool.GetEnablingIndex() , "in method Sort, line 127");
			Assert.AreEqual( true, testPool.Check(2) , "in method Sort, line 128");
			Assert.AreEqual( false, testPool.Check(3) , "in method Sort, line 129");
			Assert.AreEqual( 3, testPool.GetEnablingIndex() , "in method Sort, line 130");
			Assert.AreEqual( e, testPool[0] , "in method Sort, line 131");
			Assert.AreEqual( b, testPool[1] , "in method Sort, line 132");
			Assert.AreEqual( d, testPool[2] , "in method Sort, line 133");
			#else
			throw new Exception("Run tests in Debug mode.");
			#endif
		}
	}
}

