using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThreadsAndDragons.Keepers
{
	class TheSmartestKeeper : IKeeper
	{
		public TheSmartestKeeper(string[] baseSentences)
		{
			sentences = baseSentences.Select(x => new SmartWrapper(x)).ToArray();
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{
			var pattern = string.Format(@"\b{0}\b", word);
			var rgx = new Regex(pattern);
			for (int i = 0; i < sentences.Length; i++)
			{
				var sentence = sentences[i];
				lock (sentence)
				{
					var res = rgx.Match(sentence.WrappedString);
					if (res.Success)
					{
						sentence.WrappedString = rgx.Replace(sentence.WrappedString, replace, 1);
						return Tuple.Create(i, sentence.WrappedString);
					}
				}
			}
			return null;
		}

		public string[] GetWords()
		{
			return sentences.Select(x => x.WrappedString).ToArray();
		}

		private SmartWrapper[] sentences;

		private class SmartWrapper
		{
			public SmartWrapper(string sentence)
			{
				WrappedString = sentence;
			}

			public string WrappedString { get; set; }
		}

	}

}
