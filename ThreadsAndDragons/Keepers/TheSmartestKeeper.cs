using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace ThreadsAndDragons.Keepers
{
	class TheSmartestKeeper:IKeeper
	{
		public TheSmartestKeeper(string[] baseSentences)
		{
			sentences = baseSentences.Select(x =>new SmartWrapper(x)).ToList();
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{
			var pattern = string.Format(@"\b{0}\b", word);
			var rgx = new Regex(pattern);
			for (int i = 0; i < sentences.Count; i++)
			{
				lock (sentences[i])
				{
					var wrappedString = sentences[i].WrappedString;
					var res = rgx.Match(wrappedString);
					if (res.Success)
					{
						wrappedString = rgx.Replace(wrappedString, replace, 1);
						return Tuple.Create(i, wrappedString);
					}
				}
			}
			return null;
		}

		public string[] GetWords()
		{
			return sentences.Select(x => x.WrappedString).ToArray();
		}

		private List<SmartWrapper> sentences;

	}

	public class SmartWrapper
	{
		public SmartWrapper(string sentence)
		{
			WrappedString = sentence;
		}

		public string WrappedString { get; set; }
	}
}
