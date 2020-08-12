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
		public  void TestDefragment90()
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
			Assert.AreEqual( p1, pooler[0] , "in method Defragment, line 101");
			Assert.AreEqual( p2, pooler[1] , "in method Defragment, line 102");
			Assert.AreEqual( p3, pooler[2] , "in method Defragment, line 103");
			Assert.AreEqual( p4, pooler[3] , "in method Defragment, line 104");
			Assert.AreEqual( p5, pooler[4] , "in method Defragment, line 105");
			try
			{
			Assert.AreEqual( p5, pooler[5] , "in method Defragment, line 106");
			Assert.Fail("Did not throw IndexOutOfRangeException in method Defragment on line 105");
			}
			catch (IndexOutOfRangeException)
			{
			}
			Assert.AreEqual( 4, pooler.EnableIndex , "in method Defragment, line 107");
			pooler.Defragment();
			Assert.AreEqual( 3, pooler.EnableIndex , "in method Defragment, line 109");
			Assert.AreEqual( p5, pooler[0] , "in method Defragment, line 110");
			Assert.AreEqual( p2, pooler[1] , "in method Defragment, line 111");
			Assert.AreEqual( p4, pooler[2] , "in method Defragment, line 112");
			Assert.AreEqual( p3, pooler[3] , "in method Defragment, line 113");
			Assert.AreEqual( p1, pooler[4] , "in method Defragment, line 114");
		}
	}
}

