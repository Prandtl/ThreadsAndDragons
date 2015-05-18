using System;

namespace ThreadsAndDragons
{
	public interface IKeeper
	{
		Tuple<int, string> ReplaceFirst(string word, string replace);

		string[] GetWords();
	}
}