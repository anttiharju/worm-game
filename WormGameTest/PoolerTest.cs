using System;
using System.Collections;
using WormGame.Core;
using NUnit.Framework;
using WormGame.Pooling;

namespace TestWormGame.Pooling
{
	[TestFixture]
	public  class TestPooler
	{
		[Test]
		public  void TestDefragment101()
		{
			#if DEBUG
			Config testConfig = new Config();
			Pooler<PoolableObject> testPool = new Pooler<PoolableObject>(testConfig, 5);
			PoolableObject one = testPool.Enable();
			PoolableObject two = testPool.Enable();
			PoolableObject thr = testPool.Enable();
			PoolableObject fou = testPool.Enable();
			PoolableObject fiv = testPool.Enable();
			one.Disable();
			thr.Disable();
			Assert.AreEqual( one, testPool[0] , "in method Defragment, line 112");
			Assert.AreEqual( two, testPool[1] , "in method Defragment, line 113");
			Assert.AreEqual( thr, testPool[2] , "in method Defragment, line 114");
			Assert.AreEqual( fou, testPool[3] , "in method Defragment, line 115");
			Assert.AreEqual( fiv, testPool[4] , "in method Defragment, line 116");
			try
			{
			testPool[5] = fiv;
			Assert.Fail("Did not throw IndexOutOfRangeException in method Defragment on line 116");
			}
			catch (IndexOutOfRangeException)
			{
			}
			Assert.AreEqual( one, testPool[0] , "in method Defragment, line 118");
			Assert.AreEqual( 4, testPool.GetEnablingIndex() , "in method Defragment, line 119");
			Assert.AreEqual( true, testPool.Ask(2) , "in method Defragment, line 120");
			Assert.AreEqual( false, testPool.Ask(3) , "in method Defragment, line 121");
			Assert.AreEqual( 3, testPool.GetEnablingIndex() , "in method Defragment, line 122");
			Assert.AreEqual( fiv, testPool[0] , "in method Defragment, line 123");
			Assert.AreEqual( two, testPool[1] , "in method Defragment, line 124");
			Assert.AreEqual( fou, testPool[2] , "in method Defragment, line 125");
			#else
			throw new Exception("Run tests in Debug mode.");
			#endif
		}
	}
}

