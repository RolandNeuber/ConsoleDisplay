using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDisplay
{
	public class Display
	{
		#region Constructors
		protected Display(int width, int height, char fillSymbol, ConsoleColor foregroundFill, ConsoleColor backgroundFill, bool autoUpdate)
		{
			character = new char[width, height];
			foregroundColor = new ConsoleColor[width, height];
			backgroundColor = new ConsoleColor[width, height];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Character[i, j] = fillSymbol;
					ForegroundColor[i, j] = foregroundFill;
					BackgroundColor[i, j] = backgroundFill;
				}
			}
			AutoUpdate = autoUpdate;
			if (AutoUpdate) Update();
		}
		#endregion

		#region Properties
		private static Display? Instance { get; set; }
		public bool AutoUpdate { get; set; }
		protected char[,] character;
		protected char[,] Character
		{
			get => character;
			set
			{
				character = value;
				if (AutoUpdate) Update();
			}
		}
		protected ConsoleColor[,] foregroundColor;
		protected ConsoleColor[,] ForegroundColor
		{
			get => foregroundColor;
			set
			{
				foregroundColor = value;
				if (AutoUpdate) Update();
			}
		}
		protected ConsoleColor[,] backgroundColor;
		protected ConsoleColor[,] BackgroundColor
		{
			get => backgroundColor;
			set
			{
				backgroundColor = value;
				if (AutoUpdate) Update();
			}
		}
		#endregion

		#region Methods
		public static Display NewDisplay(int width,
										 int height,
										 char fillSymbol = ' ',
										 ConsoleColor foregroundFill = ConsoleColor.White,
										 ConsoleColor backgroundFill = ConsoleColor.Black,
										 bool autoUpdate = false)
		{
			if (Instance == null)
			{
				return new Display(width, height, fillSymbol, foregroundFill, backgroundFill, autoUpdate);
			}
			return Instance;
		}
		public void Resize(int width,
						   int height,
						   char fillSymbol = ' ',
						   ConsoleColor foregroundFill = ConsoleColor.White,
						   ConsoleColor backgroundFill = ConsoleColor.Black)
		{
			character = new char[width, height];
			foregroundColor = new ConsoleColor[width, height];
			backgroundColor = new ConsoleColor[width, height];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Character[i, j] = fillSymbol;
					ForegroundColor[i, j] = foregroundFill;
					BackgroundColor[i, j] = backgroundFill;
				}
			}
			if (AutoUpdate) Update();
		}
		public char GetCharacter(int x, int y) => Character[x, y];
		public ConsoleColor GetForegroundColor(int x, int y) => ForegroundColor[x, y];
		public ConsoleColor GetBackgroundColor(int x, int y) => BackgroundColor[x, y];
		public void SetPixel(int x, int y, char? character = null, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
		{
			Character[x, y] = character ?? Character[x, y];
			ForegroundColor[x, y] = foregroundColor ?? ForegroundColor[x, y];
			BackgroundColor[x, y] = backgroundColor ?? BackgroundColor[x, y];
			if (AutoUpdate) Update(x, y);
		}
		public void FillArea(int xStart, int yStart, int xEnd, int yEnd, char? character = null, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
		{
			for (int x = xStart; x <= xEnd; x++)
			{
				for (int y = yStart; y <= yEnd; y++)
				{
					Character[x, y] = character ?? Character[x, y];
					ForegroundColor[x, y] = foregroundColor ?? ForegroundColor[x, y];
					BackgroundColor[x, y] = backgroundColor ?? BackgroundColor[x, y];
				}
			}
			if (AutoUpdate) Update(xStart, yStart, xEnd, yEnd);
			//if (AutoUpdate) Update();
		}
		public void DrawLine(int xStart, int yStart, int xEnd, int yEnd, char? character = null, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
		{
			//Bresenham Algorithm
			int dx = xStart < xEnd ? xEnd - xStart : xStart - xEnd;
			int sx = xStart < xEnd ? 1 : -1;
			int dy = yStart < yEnd ? yStart - yEnd : yEnd - yStart;
			int sy = yStart < yEnd ? 1 : -1;
			int err = dx + dy;
			int e2;
			while (true)
			{
				SetPixel(xStart, yStart, character, foregroundColor, backgroundColor);
				if (xStart == xEnd && yStart == yEnd) break;
				e2 = 2 * err;
				if (e2 > dy)
				{
					err += dy;
					xStart += sx;
				}
				if (e2 < dx)
				{
					err += dx;
					yStart += sy;
				}
			}
		}
		public void Update()
		{
			Console.SetCursorPosition(0, 0);
			Console.CursorVisible = false;
			for (int j = 0; j < Character.GetLength(1); j++)
			{
				for (int i = 0; i < Character.GetLength(0); i++)
				{
					Console.ForegroundColor = ForegroundColor[i, j];
					Console.BackgroundColor = BackgroundColor[i, j];
					Console.Write(Character[i, j]);
					Console.ForegroundColor = BackgroundColor[i, j];
					Console.Write('.'); //Space does not print every time, whyever
				}
				Console.WriteLine();
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}

		public void Update(int xStart, int yStart, int xEnd, int yEnd)
		{
			Console.CursorVisible = false;
			for (int j = yStart; j <= yEnd; j++)
			{
				Console.SetCursorPosition(xStart * 2, j);
				for (int i = xStart; i <= xEnd; i++)
				{
					Console.ForegroundColor = ForegroundColor[i, j];
					Console.BackgroundColor = BackgroundColor[i, j];
					Console.Write(Character[i, j]);
					Console.ForegroundColor = BackgroundColor[i, j];
					Console.Write('.'); //Space does not print every time, whyever
				}
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}
		public void Update(int x, int y)
		{
			Console.CursorVisible = false;
			
			Console.SetCursorPosition(x * 2, y);
			
			Console.ForegroundColor = ForegroundColor[x, y];
			Console.BackgroundColor = BackgroundColor[x, y];
			Console.Write(Character[x, y]);
			Console.Write(' ');

			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}
		#endregion
	}
}
