using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadsAndDragons.Keepers;

namespace ThreadsAndDragons
{
	class TaskyServer
	{
		public TaskyServer(int port, string[] sentences)
		{
			ThreadPool.SetMinThreads(8, 8);
			listener = new HttpListener();
			listener.Prefixes.Add(string.Format("http://+:{0}/", port));

			keeper = new SafeKeeper(sentences);
		}

		public void Start()
		{
			Console.WriteLine("Tasky listening.");
			listener.Start();
			while (true)
			{
				try
				{
					var context = listener.GetContext();
					try
					{
						ReplaceResponse(context);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
					finally
					{
						context.Response.Close();
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{

			var replaceTask = Task.Factory.StartNew(() => keeper.ReplaceFirst(word, replace));
			return replaceTask.Result;
		}

		public void ReplaceResponse(HttpListenerContext context)
		{
			if (context.Request.Url.Query.Length > 0)
			{
				var word = context.Request.QueryString["word"];
				var replace = context.Request.QueryString["replace"];
				context.Request.InputStream.Close();

				Thread.Sleep(1000);

				var res = ReplaceFirst(word, replace);

				var encryptedBytes = Encoding.UTF8.GetBytes(
					string.Format("Change in sentence #{0}\n{1}", res.Item1, res.Item2));

				context.Response.OutputStream.WriteAsync(encryptedBytes, 0, encryptedBytes.Length);
				context.Response.OutputStream.Close();
			}
		}

		private HttpListener listener;

		private SafeKeeper keeper;
	}
}
