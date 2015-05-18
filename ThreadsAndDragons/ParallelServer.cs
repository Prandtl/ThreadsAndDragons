using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ThreadsAndDragons.Keepers;

namespace ThreadsAndDragons
{
	class ParallelServer
	{
		public ParallelServer(int port, Action<HttpListenerContext> callback, string[] sentences)
		{
			ThreadPool.SetMinThreads(8, 8);
			listener = new HttpListener();
			listener.Prefixes.Add(string.Format("http://+:{0}/", port));
			callbackAction = callback;

			keeper = new SafeKeeper(sentences);
		}

		public void Start()
		{
			listener.Start();
			while (true)
			{
				try
				{
					var context = listener.GetContext();
					Task.Run(
						() =>
						{
							try
							{
								callbackAction(context);
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
						);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{
			var replaceTask = Task.Run(() => keeper.ReplaceFirst(word, replace));
			return replaceTask.Result;
		}

		private Action<HttpListenerContext> callbackAction;
		private HttpListener listener;


		private SafeKeeper keeper;
	}
}
