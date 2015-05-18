using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThreadsAndDragons.Keepers;

namespace ThreadsAndDragons.Tests
{
	static class SpeedTest
	{
		public static void TestKeepers()
		{
			Console.WriteLine("Setting up test");
			var words = new HashSet<string>();
			var sentences = new List<string>();
			Console.WriteLine("Reading Douglas Adams.");
			using (var reader = new StreamReader("../../Tests/So Long.txt"))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					sentences.Add(line);
					if (line == null) continue;
					foreach (var word in line.Split('.', ' ', ',', '?', '!', '(', ')'))
					{
						words.Add(word);
					}
				}
			}
			Console.WriteLine("Amazing, got {0} words", words.Count);
			Console.WriteLine("Creating word pairs");
			var pairs = GetPairs(words.ToArray());
			Console.WriteLine("Done");
			var simpleKeeper = new Keeper(sentences.ToArray());

			var sw = new Stopwatch();
			Console.WriteLine("testing simple");
			sw.Start();
			foreach (var pair in pairs)
			{
				simpleKeeper.ReplaceFirst(pair.Item1, pair.Item2);
			}
			sw.Stop();
			Console.WriteLine("simple elapsed: {0} ms", sw.ElapsedMilliseconds);
			sw.Reset();

			Console.WriteLine("testing parallel");
			var parallelKeeper = new SafeKeeper(sentences.ToArray());
			sw.Start();
			Parallel.ForEach(pairs, pair =>
			{
				parallelKeeper.ReplaceFirst(pair.Item1, pair.Item2);
			});
			sw.Stop();
			Console.WriteLine("parallel elapsed: {0} ms", sw.ElapsedMilliseconds);
			sw.Reset();

			Console.WriteLine("testing parallel AND doing it smart");
			parallelKeeper = new SafeKeeper(sentences.ToArray());
			sw.Start();
			var amountOfParts = 4;
			var parts = pairs.Split(amountOfParts);
			sw.Stop();
			Console.WriteLine("Setting up elapsed: {0} ms", sw.ElapsedMilliseconds);
			sw.Reset();
			sw.Start();
			Parallel.ForEach(parts, tuples =>
			{
				foreach (var pair in tuples)
				{
					parallelKeeper.ReplaceFirst(pair.Item1, pair.Item2);
				}
			});
			sw.Stop();
			Console.WriteLine("smart parallel elapsed: {0} ms", sw.ElapsedMilliseconds);
			sw.Reset();



			Console.WriteLine("Thank you, Douglas Adams!");
			Console.WriteLine("Done");
		}

		public static List<Tuple<string, string>> GetPairs(string[] words)
		{
			var r = new Random();
			var res = new List<Tuple<string, string>>();
			for (int i = 0; i < words.Count() / 2 - 1; i++)
			{
				res.Add(Tuple.Create(words[r.Next(words.Count() - 1)], words[r.Next(words.Count() - 1)]));
			}
			return res;
		}
	}
}
