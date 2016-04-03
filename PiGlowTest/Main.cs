using System;
using RPi.PiGlow;
using System.Threading;

namespace PiGlowTest
{
	class MainClass
	{
		private static Thread _thread; 

		private static bool _go = true;

		public static void Main (string[] args)
		{
			_thread = new Thread (() => {
			
				int i = 0;
				while (_go == true)
				{
					Thread.Sleep (1000);
					PiGlow.Instance.AllOff ();
					PiGlow.Instance.SetLEDs (PiGlow.Arms[i], .01f);

					i++;

					if (i >= 3)
						i = 0;
				}

				PiGlow.Instance.AllOff ();
			});

			_thread.Start ();

			Console.WriteLine ("Hello World!");
			Console.ReadKey ();
			_go = false;
		}
	}
}
