using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDisplay
{
	internal class Display3D
	{
		#region Constructors
		protected Display3D(int width, int height, int depth, bool autoUpdate)
		{
			model = new (char, ConsoleColor, ConsoleColor?)[width, height, depth];
			screen = new (char, ConsoleColor, ConsoleColor)[width, height];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Screen[i, j] = (' ', ConsoleColor.White, ConsoleColor.Black);
					for (int k = 0; k < depth; k++)
					{
						Model[i, j, k] = (' ', ConsoleColor.White, null);
					}
				}
			}
			AutoUpdate = autoUpdate;
			if (AutoUpdate) Update();
		}
		#endregion

		#region Properties
		private static Display3D? Instance { get; set; }
		public bool AutoUpdate { get; set; }
		protected (char, ConsoleColor, ConsoleColor?)[,,] model;
		protected (char, ConsoleColor, ConsoleColor?)[,,] Model
		{
			get { return model; }
			set { model = value; }
		}
		protected (char, ConsoleColor, ConsoleColor)[,] screen;
		protected (char, ConsoleColor, ConsoleColor)[,] Screen
		{
			get { return screen; }
			set { screen = value; }
		}
		public double Distance { get; set; } = 0;
		public double Xi { get; set; } = 0;
		public double Ypsilon { get; set; } = 0;
		public double Zeta { get; set; } = 0;
		#endregion

		#region Methods
		public static Display3D NewDisplay(int width,
								 int height,
								 int depth,
								 bool autoUpdate = false)
		{
			Instance ??= new Display3D(width, height, depth, autoUpdate);
			return Instance;
		}
		public void Resize(int width,
						   int height,
						   int depth)
		{
			model = new (char, ConsoleColor, ConsoleColor?)[width, height, depth];
			screen = new (char, ConsoleColor, ConsoleColor)[width, height];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Screen[i, j] = (' ', ConsoleColor.White, ConsoleColor.Black); //transparent is projected to black
					for (int k = 0; k < depth; k++)
					{
						Model[i, j, k] = (' ', ConsoleColor.White, null); //null is transparent
					}
				}
			}
			if (AutoUpdate) Update();
		}
		public (char, ConsoleColor, ConsoleColor?) GetVoxel(int x, int y, int z) => Model[x, y, z];
		public (char, ConsoleColor, ConsoleColor) GetPixel(int x, int y) => Screen[x, y];
		public void SetPixel(int x, int y, int z, char? character = null, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
		{
			Model[x, y, z].Item1 = character ?? Model[x, y, z].Item1;
			Model[x, y, z].Item2 = foregroundColor ?? Model[x, y, z].Item2;
			Model[x, y, z].Item3 = backgroundColor;
			if (AutoUpdate) Update();
		}
		public void FillArea(int xStart, int yStart, int zStart, int xEnd, int yEnd, int zEnd, char? character = null, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
		{
			for (int x = xStart; x <= xEnd; x++)
			{
				for (int y = yStart; y <= yEnd; y++)
				{
					for (int z = zStart; z <= zEnd; z++)
					{
						Model[x, y, z].Item1 = character ?? Model[x, y, z].Item1;
						Model[x, y, z].Item2 = foregroundColor ?? Model[x, y, z].Item2;
						Model[x, y, z].Item3 = backgroundColor;
					}
				}
			}
			if (AutoUpdate) Update();
		}
		public void DrawLine(int xStart, int yStart, int zStart, int xEnd, int yEnd, int zEnd, char? character = null, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
		{
			//throw new NotImplementedException();
			//Bresenham Algorithm
			//int dx = xStart < xEnd ? xEnd - xStart : xStart - xEnd;
			//int sx = xStart < xEnd ? 1 : -1;
			//int dy = yStart < yEnd ? yStart - yEnd : yEnd - yStart;
			//int sy = yStart < yEnd ? 1 : -1;
			//int err = dx + dy;
			//int e2;
			//while (true)
			//{
			//	SetPixel(xStart, yStart, character, foregroundColor, backgroundColor);
			//	if (xStart == xEnd && yStart == yEnd) break;
			//	e2 = 2 * err;
			//	if (e2 > dy)
			//	{
			//		err += dy;
			//		xStart += sx;
			//	}
			//	if (e2 < dx)
			//	{
			//		err += dx;
			//		yStart += sy;
			//	}
			//}

			//digital differential analyser; much easier than Bresenham's Algorithm
			float x = xStart;
			float y = yStart;
			float z = zStart;
			float dx = xEnd - xStart;
			float dy = yEnd - yStart;
			float dz = zEnd - zStart;
			float step = Math.Max(Math.Max(Math.Abs(dx), Math.Abs(dy)), Math.Abs(dz));

			dx /= step;
			dy /= step;
			dz /= step;

			for (int i = 0; i <= step; i++)
			{
				SetPixel((int)(x + 0.5f), (int)(y + 0.5f), (int)(z + 0.5f), character, foregroundColor, backgroundColor);
				x += dx;
				y += dy;
				z += dz;
			}
		}
		protected void Project(ref (char, ConsoleColor, ConsoleColor)[,] screen, (char, ConsoleColor, ConsoleColor?)[,,] model, double xi, double ypsilon, double zeta)
		{
			//orthogonal projection
			//very slow; TODO: implement multithreading
			for (int x = 0; x < model.GetLength(0); x++)
			{
				for (int y = 0; y < model.GetLength(1); y++)
				{
					char currentChar = ' ';
					ConsoleColor currentForegroundColor = ConsoleColor.White;
					ConsoleColor? currentBackgroundColor = null;
					for (int z = 0; z < model.GetLength(2); z++)
					{
						if (currentBackgroundColor != null) break;
						if (currentChar == ' ')
						{
							currentChar = model[x, y, z].Item1;
							currentForegroundColor = model[x, y, z].Item2;
						}
						#if DEBUG
						currentChar = z < 10 ? z.ToString()[0] : ' ';
						currentForegroundColor = model[x, y, z].Item2;
						#endif
						currentBackgroundColor = model[x, y, z].Item3;
					}
					screen[x, y] = (currentChar, currentForegroundColor, currentBackgroundColor ?? ConsoleColor.Black);
				}
			}
		}
		protected void ParallelProject(ref (char, ConsoleColor, ConsoleColor)[,] screen, (char, ConsoleColor, ConsoleColor?)[,,] model, double xi, double ypsilon, double zeta)
		{
			for (int x = 0; x < model.GetLength(0); x++)
			{
				for (int y = 0; y < model.GetLength(1); y++)
				{
					char currentChar = ' ';
					ConsoleColor currentForegroundColor = ConsoleColor.White;
					ConsoleColor? currentBackgroundColor = null;
					screen[x, y] = (currentChar, currentForegroundColor, currentBackgroundColor ?? ConsoleColor.Black);
					for (int z = 0; z < model.GetLength(2); z++)
					{
						if (currentChar == ' ')
						{
							currentChar = model[x, y, z].Item1;
							currentForegroundColor = model[x, y, z].Item2;
						}
						currentBackgroundColor = model[x, y, z].Item3 ?? currentBackgroundColor;

						int projectedX = (int)Math.Round((x - model.GetLength(0) / 2) * Math.Cos(ypsilon) * Math.Cos(-zeta) + (z - model.GetLength(2) / 2) * Math.Sin(ypsilon) + y * Math.Sin(-zeta));
						int projectedY = (int)Math.Round((y - model.GetLength(1) / 2) * Math.Cos(-xi) * Math.Cos(zeta) + (z - model.GetLength(2) / 2) * Math.Sin(-xi) + x * Math.Sin(zeta));

						projectedX += model.GetLength(0) / 2;
						projectedY += model.GetLength(1) / 2;

						try
						{
							screen[projectedX, projectedY] = (currentChar, currentForegroundColor, currentBackgroundColor ?? ConsoleColor.Black);
						}
						catch 
						{

						}
					}
				}
			}
		}
		protected void PerspectiveProject(ref (char, ConsoleColor, ConsoleColor)[,] screen, (char, ConsoleColor, ConsoleColor?)[,,] model, double distance, double xi, double ypsilon, double zeta)
		{
			
		}
		public void Update()
		{
			ParallelProject(ref screen, model, Xi, Ypsilon, Zeta);
			Console.SetCursorPosition(0, 0);
			Console.CursorVisible = false;
			for (int j = 0; j < screen.GetLength(1); j++)
			{
				for (int i = 0; i < screen.GetLength(0); i++)
				{
					Console.ForegroundColor = screen[i, j].Item2;
					Console.BackgroundColor = screen[i, j].Item3;
					Console.Write(screen[i, j].Item1);
					Console.ForegroundColor = screen[i, j].Item3;
					Console.Write('.'); //Space does not print every time, whyever
				}
				Console.WriteLine();
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}
		public void Update(int xStart, int yStart, int xEnd, int yEnd)
		{
			ParallelProject(ref screen, model, Xi, Ypsilon, Zeta);
			Console.CursorVisible = false;
			for (int j = yStart; j <= yEnd; j++)
			{
				Console.SetCursorPosition(xStart * 2, j);
				for (int i = xStart; i <= xEnd; i++)
				{
					Console.ForegroundColor = screen[i, j].Item2;
					Console.BackgroundColor = screen[i, j].Item3;
					Console.Write(screen[i, j].Item1);
					Console.ForegroundColor = screen[i, j].Item3;
					Console.Write('.'); //Space does not print every time, whyever
				}
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}
		public void Update(int x, int y)
		{
			ParallelProject(ref screen, model, Xi, Ypsilon, Zeta);
			Console.CursorVisible = false;

			Console.SetCursorPosition(x * 2, y);

			Console.ForegroundColor = screen[x, y].Item2;
			Console.BackgroundColor = screen[x, y].Item3;
			Console.Write(screen[x, y].Item1);
			Console.Write(' ');

			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}
		#endregion
	}
}
