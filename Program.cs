using System;
using System.Collections.Generic;
using System.Data;
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
			Display3D display3D = Display3D.NewDisplay(11, 11, 11);
			//display3D.Beta = Math.PI;
			display3D.DrawLine(0, 0, 0, 10, 5, 5, '#', ConsoleColor.Red, ConsoleColor.White);
			display3D.DrawLine(5, 0, 10, 5, 10, 10, '#', ConsoleColor.Green, ConsoleColor.White);
			while (true)
			{
				display3D.Update();
				display3D.Gamma += 0.01;
			}
        }
	}
}