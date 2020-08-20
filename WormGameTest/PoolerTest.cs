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
		public  void TestDefragment61()
		{
			Scene scene = new Scene();
			Settings settings = new Settings();
			Pooler<Poolable> pooler = new Pooler<Poolable>(settings, scene, 5);
			Poolable p1 = pooler.Enable();
			Poolable p2 = pooler.Enable();
			Poolable p3 = pooler.Enable();
			Poolable p4 = pooler.Enable();
			Poolable p5 = pooler.Enable();
			p1.Disable();
			p3.Disable();
			Assert.AreEqual( p1, pooler[0] , "in method Defragment, line 72");
			Assert.AreEqual( p2, pooler[1] , "in method Defragment, line 73");
			Assert.AreEqual( p3, pooler[2] , "in method Defragment, line 74");
			Assert.AreEqual( p4, pooler[3] , "in method Defragment, line 75");
			Assert.AreEqual( p5, pooler[4] , "in method Defragment, line 76");
			try
			{
			Assert.AreEqual( p5, pooler[5] , "in method Defragment, line 77");
			Assert.Fail("Did not throw IndexOutOfRangeException in method Defragment on line 76");
			}
			catch (IndexOutOfRangeException)
			{
			}
			Assert.AreEqual( 4, pooler.Index , "in method Defragment, line 78");
			Assert.AreEqual( true, pooler[pooler.Index].Active , "in method Defragment, line 79");
			pooler.Defragment();
			Assert.AreEqual( 3, pooler.Index , "in method Defragment, line 81");
			Assert.AreEqual( false, pooler[pooler.Index].Active , "in method Defragment, line 82");
			Assert.AreEqual( p5, pooler[0] , "in method Defragment, line 83");
			Assert.AreEqual( p2, pooler[1] , "in method Defragment, line 84");
			Assert.AreEqual( p4, pooler[2] , "in method Defragment, line 85");
			Assert.AreEqual( p3, pooler[3] , "in method Defragment, line 86");
			Assert.AreEqual( p1, pooler[4] , "in method Defragment, line 87");
		}
	}
}

