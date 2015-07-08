// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TC.WinForms.Animation
{
	internal class AnimationThread
	{
		public AnimationThread()
		{
			_queue = new Queue<Action>();

			_thread = new Thread(Run);
			_thread.Name = "Animation Thread";
			_thread.IsBackground = true;
			_thread.Start();
		}

		private readonly Thread _thread;
		private readonly Queue<Action> _queue;
		private readonly object _lock = new object();

		public void Perform(Action action)
		{
			if (action != null)
				lock (_lock)
				{
					_queue.Enqueue(action);
					Monitor.Pulse(_lock);
				}
		}

		private void Run()
		{
			Action action;
			while (true)
			{
				lock (_lock)
				{
					if (_queue.Count == 0)
						Monitor.Wait(_lock);
					action = _queue.Dequeue();
				}

				Thread.Sleep(5);
				Animator.Perform(action);
			}
		}
	}
}
