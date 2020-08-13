using System;
using Otter.Core;
using WormGame.Core;
using NUnit.Framework;
using WormGame.Pooling;

namespace TestWormGame.Pooling
{
	[TestFixture]
	public  class TestPooler
	{
		[Test]
		public  void TestDefragment49()
		{
			Scene scene = new Scene();
			Config config = new Config();
			Pooler<Poolable> pooler = new Pooler<Poolable>(scene, config, 5);
			Poolable p1 = pooler.Enable();
			Poolable p2 = pooler.Enable();
			Poolable p3 = pooler.Enable();
			Poolable p4 = pooler.Enable();
			Poolable p5 = pooler.Enable();
			p1.Disable();
			p3.Disable();
			Assert.AreEqual( p1, pooler[0] , "in method Defragment, line 60");
			Assert.AreEqual( p2, pooler[1] , "in method Defragment, line 61");
			Assert.AreEqual( p3, pooler[2] , "in method Defragment, line 62");
			Assert.AreEqual( p4, pooler[3] , "in method Defragment, line 63");
			Assert.AreEqual( p5, pooler[4] , "in method Defragment, line 64");
			try
			{
			Assert.AreEqual( p5, pooler[5] , "in method Defragment, line 65");
			Assert.Fail("Did not throw IndexOutOfRangeException in method Defragment on line 64");
			}
			catch (IndexOutOfRangeException)
			{
			}
			Assert.AreEqual( 4, pooler.Index , "in method Defragment, line 66");
			Assert.AreEqual( true, pooler[pooler.Index].Active , "in method Defragment, line 67");
			pooler.Defragment();
			Assert.AreEqual( 3, pooler.Index , "in method Defragment, line 69");
			Assert.AreEqual( false, pooler[pooler.Index].Active , "in method Defragment, line 70");
			Assert.AreEqual( p5, pooler[0] , "in method Defragment, line 71");
			Assert.AreEqual( p2, pooler[1] , "in method Defragment, line 72");
			Assert.AreEqual( p4, pooler[2] , "in method Defragment, line 73");
			Assert.AreEqual( p3, pooler[3] , "in method Defragment, line 74");
			Assert.AreEqual( p1, pooler[4] , "in method Defragment, line 75");
		}
	}
}

