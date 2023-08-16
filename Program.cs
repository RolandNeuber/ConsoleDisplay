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
			Thread.Sleep(1000);
			ConsolePrograms.Clock();
			//Display3D display = Display3D.NewDisplay(15, 10, 5, true);
			//display.DrawLine(5, 2, 3, 6, 9, 4, '#', ConsoleColor.Red, ConsoleColor.Green);
		}
	}
}