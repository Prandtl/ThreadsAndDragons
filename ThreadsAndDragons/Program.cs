using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using ThreadsAndDragons.Tests;

namespace ThreadsAndDragons
{
	class Program
	{
		private static void Main()
		{
			SpeedTest.TestKeepers();
			//	var sentences = new List<string>();
			//	using (var reader = new StreamReader("../../Tests/So Long.txt"))
			//	{
			//		while (!reader.EndOfStream)
			//		{
			//			sentences.Add(reader.ReadLine());
			//		}
			//	}

			//	server = new Server(port, ReplaceResponse, sentences.ToArray());
			//	server.Start();
			//}
		}

		private const int port = 20000;

		public static void ReturnResponse(HttpListenerContext context)
		{
			var word = context.Request.QueryString["word"];
			var replace = context.Request.QueryString["replace"];
			context.Request.InputStream.Close();

			Thread.Sleep(1000);

			var encryptedBytes = Encoding.UTF8.GetBytes(word + " " + replace);

			context.Response.OutputStream.WriteAsync(encryptedBytes, 0, encryptedBytes.Length);
			context.Response.OutputStream.Close();
		}

		public static void ReplaceResponse(HttpListenerContext context)
		{
			var word = context.Request.QueryString["word"];
			var replace = context.Request.QueryString["replace"];
			context.Request.InputStream.Close();

			Thread.Sleep(1000);

			var res = server.ReplaceFirst(word, replace);

			var encryptedBytes = Encoding.UTF8.GetBytes(
				string.Format("Change in sentence #{0}\n{1}", res.Item1, res.Item2));

			context.Response.OutputStream.WriteAsync(encryptedBytes, 0, encryptedBytes.Length);
			context.Response.OutputStream.Close();
		}


		private static Server server;
	}
}
