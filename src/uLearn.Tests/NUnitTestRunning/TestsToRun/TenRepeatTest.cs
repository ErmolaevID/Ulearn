﻿using NUnit.Framework;

namespace uLearn.NUnitTestRunning.TestsToRun
{
	public class TenRepeatTest
	{
		public static int Counter { get; set; }

		[Repeat(10)]
		[Test]
		public void Test()
		{
			Counter++;
		}
	}
}
