using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ThreadsAndDragons.Keepers
{
	class Keeper : IKeeper
	{
		public Keeper(string[] baseSentences)
		{
			sentences = new List<string>(baseSentences);
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{
			var pattern = string.Format(@"\b{0}\b", word);
			var rgx = new Regex(pattern);
			for (int i = 0; i < sentences.Count; i++)
			{
				var res = rgx.Match(sentences[i]);
				if (res.Success)
				{
					sentences[i] = rgx.Replace(sentences[i], replace, 1);
					return Tuple.Create(i, sentences[i]);
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
