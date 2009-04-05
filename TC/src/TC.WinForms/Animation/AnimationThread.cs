// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

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
			fQueue = new Queue<Action>();

			fThread = new Thread(Run);
			fThread.Name = "Animation Thread";
			fThread.IsBackground = true;
			fThread.Start();
		}

		private readonly Thread fThread;
		private readonly Queue<Action> fQueue;
		private readonly object fLock = new object();

		public void Perform(Action action)
		{
			if (action != null)
				lock (fLock)
				{
					fQueue.Enqueue(action);
					Monitor.Pulse(fLock);
				}
		}

		private void Run()
		{
			Action lAction;
			while (true)
			{
				lock (fLock)
				{
					if (fQueue.Count == 0)
						Monitor.Wait(fLock);
					lAction = fQueue.Dequeue();
				}

				Thread.Sleep(5);
				Animator.Perform(lAction);
			}
		}
	}
}
