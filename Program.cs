using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDisplay
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Thread.Sleep(1000);
			//ConsolePrograms.Clock();
			Display3D display = Display3D.NewDisplay(15, 15, 15, true);
			display.DrawLine(0, 0, 0, 14, 0, 0, '#', ConsoleColor.Red, ConsoleColor.Green);
			for (float i = 0; i < 62.8; i += 0.1f)
			{
				display.Zeta = i;
				display.Update();
				Thread.Sleep(10);
			}
		}
	}
}