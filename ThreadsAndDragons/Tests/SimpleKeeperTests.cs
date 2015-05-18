using System;
using System.Threading;
using NUnit.Framework;
using ThreadsAndDragons.Keepers;

namespace ThreadsAndDragons.Tests
{
	[TestFixture]
	static class SimpleKeeperTests
	{
		[SetUp]
		public static void SetUp()
		{
			_keeper = new TheSmartestKeeper(sentences);
		}

		[Test]
		public static void Toast()
		{
			Assert.AreEqual(0, 0);
		}

		[Test]
		public static void ReplaceFirstWorks()
		{
			_keeper.ReplaceFirst("really", "probably");
			Assert.AreEqual("Roses are red. Violets are blue. I probably hate you.", _keeper.GetWords()[0]);
		}

		[Test]
		public static void PartOneOfAnswerIsCorrect()
		{
			var res = _keeper.ReplaceFirst("really", "probably");
			Assert.AreEqual("Roses are red. Violets are blue. I probably hate you.", _keeper.GetWords()[res.Item1]);
		}

		[Test]
		public static void PartTwoOfAnswerIsCorrect()
		{
			var res = _keeper.ReplaceFirst("really", "probably");
			Assert.AreEqual(res.Item2, _keeper.GetWords()[0]);
		}

		[Test]
		public static void ThisOneShouldCrash()
		{
			for (int i = 0; i < 50; i++)
			{
				SetUp();
				var res = Tuple.Create(0, "");
				var firstThread = new Thread(() =>
				{
					res = _keeper.ReplaceFirst("really", ",Jason,");
				});
				var secondThread = new Thread(() =>
				{
					res = _keeper.ReplaceFirst("really", ",John,");
				});
				firstThread.Start();
				secondThread.Start();
				Thread.Sleep(500);
				var expected = res.Item2 == "Roses are red. Violets are blue. I ,Jason, hate you."
					? "Roses are red. Violets are blue. I ,John, hate you."
					: "Roses are red. Violets are blue. I ,Jason, hate you.";
				Assert.AreEqual(expected, _keeper.GetWords()[0]);
			}
		}

		private static IKeeper _keeper;
		private static string[] sentences =
			{
				"Roses are red. Violets are blue. I really hate you.",
				"Chet Baker is really a great musician.",
				"Really really really.",
			};
	}
}
