using System.Collections.Generic;
using System.IO;

namespace ThreadsAndDragons
{
	class Program
	{
		private static void Main()
		{
			var sentences = new List<string>();
			using (var reader = new StreamReader("../../Tests/So Long.txt"))
			{
				while (!reader.EndOfStream)
				{
					sentences.Add(reader.ReadLine());
				}
			}

			server = new Server(port, sentences.ToArray());
			taskyServer = new TaskyServer(taskyPort, sentences.ToArray());
			threadyServer = new ThreadyServer(threadyPort, sentences.ToArray());
			//server.Start();
			//taskyServer.Start();
			threadyServer.Start();
		}

		private const int port = 20000;
		private static Server server;

		private const int taskyPort = 20001;
		private static TaskyServer taskyServer;


		private const int threadyPort = 20002;
		private static ThreadyServer threadyServer;
	}
}
