
using System;
using System.Threading;
//automatically generated by Tuto
using uLearn;
namespace Parprog
{
	[Slide(@"QueueUserWorkItem", "f1359a20-ff7b-46c7-a928-be1b8fb62ce0")]
	public class QueueUserWorkItem
	{
		//#video O_cU5Khvtzg

		[Test]
		public void TestThreadPoolNoWait()
		{
			var methodFinshedEvent = new AutoResetEvent(false);
			ThreadPool.QueueUserWorkItem(state =>
			{
				Thread.Sleep(100);
				Console.WriteLine(1);
				methodFinshedEvent.Set();
			});
			methodFinshedEvent.WaitOne();
		}

		[Test]
		public void TestThreadPoolChain()
		{
			var firstMethodEvent = new AutoResetEvent(false);
			var secondMethodEvent = new AutoResetEvent(false);
			ThreadPool.QueueUserWorkItem(state =>
			{
				Thread.Sleep(100);
				Console.WriteLine(1);
				firstMethodEvent.Set();
			});
			ThreadPool.QueueUserWorkItem(state =>
			{
				firstMethodEvent.WaitOne();
				Console.WriteLine(2);
				secondMethodEvent.Set();
			});
			secondMethodEvent.WaitOne();
		}

		[Test]
		public void TestThreadPoolException()
		{
			ThreadPool.QueueUserWorkItem(state =>
			{
				throw new ArgumentException();
			});
			Thread.Sleep(100);
		}
	}
}