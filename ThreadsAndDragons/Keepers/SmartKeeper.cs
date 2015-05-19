using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ThreadsAndDragons.Keepers
{
	class SmartKeeper : IKeeper
	{
		public SmartKeeper(string[] baseSentences)
		{
			sentences = new List<string>(baseSentences);
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{
			var pattern = string.Format(@"\b{0}\b", word);
			var rgx = new Regex(pattern);
			for (int i = 0; i < sentences.Count; i++)
			{
				var sentence = sentences[i];
				lock (sentence)
				{
					var res = rgx.Match(sentence);
					if (res.Success)
					{
						sentences[i] = rgx.Replace(sentence, replace, 1);
						return Tuple.Create(i, sentence);
					}
				}
			}
			return null;
		}

		public string[] GetWords()
		{
			return sentences.ToArray();
		}

		private List<string> sentences;
	}

}
