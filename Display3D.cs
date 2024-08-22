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
		public (char, ConsoleColor, ConsoleColor?)[,,] Model
		{
			get { return model; }
			protected set { model = value; }
		}
		protected (char, ConsoleColor, ConsoleColor)[,] screen;
		public (char, ConsoleColor, ConsoleColor)[,] Screen
		{
			get { return screen; }
			protected set { screen = value; }
		}
		public double Distance { get; set; } = 0;
		public double Alpha { get; set; } = 0;
		public double Beta { get; set; } = 0;
		public double Gamma { get; set; } = 0;
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
		protected void Project(ref (char, ConsoleColor, ConsoleColor)[,] screen, (char, ConsoleColor, ConsoleColor?)[,,] model, Axis axis = Axis.x_Axis)
		{
			//orthogonal projection
			//very slow; TODO: implement multithreading
			if (axis == Axis.x_Axis)
			{
			}
			else if (axis == Axis.y_Axis)
			{
				
			}
			else
			{
				int maxX = model.GetLength(0);
				int maxY = model.GetLength(1);
				int maxZ = model.GetLength(2);
			}

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
		protected void Project(ref (char, ConsoleColor, ConsoleColor)[,] screen, (char, ConsoleColor, ConsoleColor?)[,,] model, double alpha, double beta, double gamma)
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
		protected void ParallelProject(ref (char, ConsoleColor, ConsoleColor)[,] screen, (char, ConsoleColor, ConsoleColor?)[,,] model, double alpha, double beta, double gamma)
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

						//projectedX = x * cos(ypsilon) * cos(-zeta) + z * sin(ypsilon) + y * sin(-zeta)
						double projectedX = (x - model.GetLength(0) / 2f) * Math.Cos(beta) * Math.Cos(gamma) + (z - model.GetLength(2) / 2f) * Math.Sin(beta) + (y - model.GetLength(1) / 2f) * Math.Sin(-gamma);
						//prejectedY = y * cos(-xi) * cos(zeta) + z * sin(-xi) + x * sin(zeta)
						double projectedY = (y - model.GetLength(1) / 2f) * Math.Cos(-alpha) * Math.Cos(gamma) + (z - model.GetLength(2) / 2f) * Math.Sin(-alpha) + (x - model.GetLength(0) / 2f) * Math.Sin(gamma);
						

						projectedX += model.GetLength(0) / 2f;
						projectedY += model.GetLength(1) / 2f;

						try
						{
							screen[(int)Math.Round(projectedX), (int)Math.Round(projectedY)] = (currentChar, currentForegroundColor, currentBackgroundColor ?? ConsoleColor.Black);
						}
						catch 
						{

						}
					}
				}
			}
		}
		protected void PerspectiveProject(ref (char, ConsoleColor, ConsoleColor)[,] screen, (char, ConsoleColor, ConsoleColor?)[,,] model, double distance, double alpha, double beta, double gamma)
		{
			
		}
		public void Update()
		{
			ParallelProject(ref screen, model, Alpha, Beta, Gamma);
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
			ParallelProject(ref screen, model, Alpha, Beta, Gamma);
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
			ParallelProject(ref screen, model, Alpha, Beta, Gamma);
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
