using System;
using System.Net;
using ThreadsAndDragons.Keepers;

namespace ThreadsAndDragons
{
	class Server
	{
		public Server(int port, Action<HttpListenerContext> callback, string[] sentences)
		{
			listener = new HttpListener();
			listener.Prefixes.Add(string.Format("http://+:{0}/", port));
			callbackAction = callback;

			keeper=new Keeper(sentences);
		}

		public void Start()
		{
			listener.Start();
			while (true)
			{
				try
				{
					var context = listener.GetContext();
					callbackAction(context);
					context.Response.Close();
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		public Tuple<int, string> ReplaceFirst(string word, string replace)
		{
			return keeper.ReplaceFirst(word, replace);
		}

		private Action<HttpListenerContext> callbackAction;
		private HttpListener listener;


		private Keeper keeper;
	}
}
