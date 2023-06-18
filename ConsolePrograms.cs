using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDisplay
{
	public static class ConsolePrograms
	{
		public static void Clock(int size = 12)
		{
			int accuracy = 1;
			Display display = Display.NewDisplay(2 * size + 1, 2 * size + 1, backgroundFill: ConsoleColor.Black, fillSymbol: ' ', autoUpdate: true);
			display.SetPixel(size, 0, '0', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(Math.PI / 6) * size), (int)Math.Round(size - Math.Cos(Math.PI / 6) * size), '1', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(Math.PI / 3) * size), (int)Math.Round(size - Math.Cos(Math.PI / 3) * size), '2', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel(2 * size, size, '3', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(2 * Math.PI / 3) * size), (int)Math.Round(size - Math.Cos(2 * Math.PI / 3) * size), '4', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(5 * Math.PI / 6) * size), (int)Math.Round(size - Math.Cos(5 * Math.PI / 6) * size), '5', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel(size, 2 * size, '6', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(7 * Math.PI / 6) * size), (int)Math.Round(size - Math.Cos(7 * Math.PI / 6) * size), '7', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(4 * Math.PI / 3) * size), (int)Math.Round(size - Math.Cos(4 * Math.PI / 3) * size), '8', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel(0, size, '9', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(5 * Math.PI / 3) * size), (int)Math.Round(size - Math.Cos(5 * Math.PI / 3) * size), '1', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(5 * Math.PI / 3) * size) + 1, (int)Math.Round(size - Math.Cos(5 * Math.PI / 3) * size), '0', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(11 * Math.PI / 6) * size), (int)Math.Round(size - Math.Cos(11 * Math.PI / 6) * size), '1', foregroundColor: ConsoleColor.DarkRed);
			display.SetPixel((int)Math.Round(size + Math.Sin(11 * Math.PI / 6) * size) + 1, (int)Math.Round(size - Math.Cos(11 * Math.PI / 6) * size), '1', foregroundColor: ConsoleColor.DarkRed);
			//display.Update();

			double t = 2 * Math.PI / 60 * (DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600);
			int secondHand = (int)Math.Round(1f * size);
			int minuteHand = (int)Math.Round(0.75f * size);
			int hourHand = (int)Math.Round(0.5f * size);

			while (true)
			{
				display.DrawLine(size, size, size + (int)Math.Round(secondHand * Math.Sin(t)), size + (int)Math.Round(secondHand * -Math.Cos(t)), backgroundColor: ConsoleColor.White);
				display.DrawLine(size, size, size + (int)Math.Round(minuteHand * Math.Sin(t / 60)), size + (int)Math.Round(minuteHand * -Math.Cos(t / 60)), backgroundColor: ConsoleColor.Yellow);
				display.DrawLine(size, size, size + (int)Math.Round(hourHand * Math.Sin(t / 3600 * 5)), size + (int)Math.Round(hourHand * -Math.Cos(t / 3600 * 5)), backgroundColor: ConsoleColor.DarkYellow);
				//display.Update();
				Thread.Sleep(1000 / accuracy);
				display.DrawLine(size, size, size + (int)Math.Round(secondHand * Math.Sin(t)), size + (int)Math.Round(secondHand * -Math.Cos(t)), backgroundColor: ConsoleColor.Black);
				display.DrawLine(size, size, size + (int)Math.Round(minuteHand * Math.Sin(t / 60)), size + (int)Math.Round(minuteHand * -Math.Cos(t / 60)), backgroundColor: ConsoleColor.Black);
				display.DrawLine(size, size, size + (int)Math.Round(hourHand * Math.Sin(t / 3600 * 5)), size + (int)Math.Round(hourHand * -Math.Cos(t / 3600 * 5)), backgroundColor: ConsoleColor.Black);
				t = 2 * Math.PI / 60 * (DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600);
				//Console.WriteLine(t);
			}
		}
		public static void Snake(int size = 12, int minFrameTime = 150)
		{
			//Render map with walls
			Display display = Display.NewDisplay(2 * size + 1, 2 * size + 1, backgroundFill: ConsoleColor.White, autoUpdate: true);
			display.FillArea(1, 1, 2 * size - 1, 2 * size - 1, backgroundColor: ConsoleColor.Black);
			//Render/setup snakehead and Body segments
			List<int[]> segments = new() { new int[2] { size, size } };
			display.SetPixel(segments[0][0], segments[0][1], character: '#', foregroundColor: ConsoleColor.DarkYellow, backgroundColor: ConsoleColor.DarkGreen);
			//Render/setup fruit
			int[] fruitPos = new int[2];
			Random random = new();
			do
			{
				fruitPos[0] = random.Next(1, 2 * size - 1);
				fruitPos[1] = random.Next(1, 2 * size - 1);
			}
			while (fruitPos.SequenceEqual(segments[0]));
			display.SetPixel(fruitPos[0], fruitPos[1], backgroundColor: ConsoleColor.Red);
			//game loop
			int[] dPos = new int[2];
			char dir = ' ';
			while (true)
			{
				//display.Update();
				//controls
				if (Console.KeyAvailable)
				{
					char keyChar = Console.ReadKey(intercept: true).KeyChar;
					switch (keyChar)
					{
						case 'a':
							if (dir == 'a' || dir == 'd') break;
							dPos[0] = -1;
							dPos[1] = 0;
							dir = 'a';
							break;
						case 'd':
							if (dir == 'a' || dir == 'd') break;
							dPos[0] = 1;
							dPos[1] = 0;
							dir = 'd';
							break;
						case 'w':
							if (dir == 'w' || dir == 's') break;
							dPos[0] = 0;
							dPos[1] = -1;
							dir = 'w';
							break;
						case 's':
							if (dir == 'w' || dir == 's') break;
							dPos[0] = 0;
							dPos[1] = 1;
							dir = 's';
							break;
					}
				}

				//wall detection
				if (segments[0][0] + dPos[0] <= 0
					|| segments[0][0] + dPos[0] >= 2 * size
					|| segments[0][1] + dPos[1] <= 0
					|| segments[0][1] + dPos[1] >= 2 * size)
					break;

				//self intersection
				bool selfIntersection = false;
				for (int i = 1; i < segments.Count; i++)
				{
					if (segments[0][0] + dPos[0] == segments[i][0] && segments[0][1] + dPos[1] == segments[i][1])
					{
						selfIntersection = true;
						break;
					}
				}
				if (selfIntersection) break;

				//removing last segment
				if (display.GetBackgroundColor(segments.Last()[0], segments.Last()[1]) != ConsoleColor.Red)
					display.SetPixel(segments.Last()[0], segments.Last()[1], character: ' ', foregroundColor: ConsoleColor.White, backgroundColor: ConsoleColor.Black);

				//movement and fruit collision
				segments.Insert(0, new int[2] { segments[0][0] + dPos[0], segments[0][1] + dPos[1] });
				if (segments[0][0] == fruitPos[0] && segments[0][1] == fruitPos[1])
				{
					bool isSnake;
					do
					{
						fruitPos[0] = random.Next(1, 2 * size - 1);
						fruitPos[1] = random.Next(1, 2 * size - 1);
						isSnake = false;
						foreach (int[] segment in segments)
							if (fruitPos.SequenceEqual(segment))
								isSnake = true;
					}
					while (isSnake);

					display.SetPixel(fruitPos[0], fruitPos[1], backgroundColor: ConsoleColor.Red);
				}
				else
				{
					segments.RemoveAt(segments.Count - 1);
				}

				display.SetPixel(segments[0][0], segments[0][1], character: '#', foregroundColor: ConsoleColor.DarkYellow, backgroundColor: ConsoleColor.DarkGreen);

				Thread.Sleep(minFrameTime);
			}
			//display.Update();
			string msg = $" Score: {segments.Count} ";
			if (msg.Length <= 2 * size + 1)
				for (int i = 0; i < msg.Length; i++)
				{
					Thread.Sleep(100);
					display.DrawLine(size + i - msg.Length / 2, size - 1, size + i - msg.Length / 2, size + 1, character: ' ',backgroundColor: ConsoleColor.Black);
					display.SetPixel(size + i - msg.Length / 2, size, msg[i], ConsoleColor.Red);
					//display.Update(size + i - msg.Length / 2, size, size + i - msg.Length / 2, size);
				}
			Console.SetCursorPosition(0, 2 * size + 1);
		}
		public static void Pong(int size = 10, int minFrameTime = 100)
		{
			Display display = Display.NewDisplay(4 * size + 1, 2 * size + 1, autoUpdate: true, fillSymbol: ' ');
			display.DrawLine(0, 0, 4 * size, 0, backgroundColor: ConsoleColor.White);
			display.DrawLine(0, 2 * size, 4 * size, 2 * size, backgroundColor: ConsoleColor.White);

			//Initialization
			Random random = new();
			int[] score = { 0, 0 };
			int[] ballPos = { 2 * size, size };
			int[] ballDir = { random.Next(0, 2) == 0 ? -1 : 1, random.Next(0, 2) == 0 ? -1 : 1 };
			int[] paddle1Pos = { 1, size };
			int[] paddle2Pos = { 4 * size - 1, size };
			int paddleHeight = (int)Math.Round(size / 4f);

			//game loop
			while (true)
			{
				//old paddle pos
				display.AutoUpdate = false;
				display.DrawLine(paddle1Pos[0],
								 paddle1Pos[1] - paddleHeight,
								 paddle1Pos[0],
								 paddle1Pos[1] + paddleHeight,
								 backgroundColor: ConsoleColor.Black);
				display.DrawLine(paddle2Pos[0],
								 paddle2Pos[1] - paddleHeight,
								 paddle2Pos[0],
								 paddle2Pos[1] + paddleHeight,
								 backgroundColor: ConsoleColor.Black);

				//input
				while (Console.KeyAvailable)
				{
					ConsoleKey key = Console.ReadKey(intercept: true).Key;
					switch (key)
					{
						case ConsoleKey.W:
							if (paddle1Pos[1] - paddleHeight - 1 < 1) break;
							paddle1Pos[1]--;
							break;
						case ConsoleKey.S:
							if (paddle1Pos[1] + paddleHeight + 1 > 2 * size - 1) break;
							paddle1Pos[1]++;
							break;
						case ConsoleKey.UpArrow:
							if (paddle2Pos[1] - paddleHeight - 1 < 1) break;
							paddle2Pos[1]--;
							break;
						case ConsoleKey.DownArrow:
							if (paddle2Pos[1] + paddleHeight + 1 > 2 * size - 1) break;
							paddle2Pos[1]++;
							break;
						default:
							break;
					}
				}

				//new paddle pos
				display.DrawLine(paddle1Pos[0],
								 paddle1Pos[1] - paddleHeight,
								 paddle1Pos[0],
								 paddle1Pos[1] + paddleHeight,
								 backgroundColor: ConsoleColor.White);
				display.DrawLine(paddle2Pos[0],
								 paddle2Pos[1] - paddleHeight,
								 paddle2Pos[0],
								 paddle2Pos[1] + paddleHeight,
								 backgroundColor: ConsoleColor.White);
				display.Update(1, 1, 1, 2 * size - 1);
				display.Update(4 * size - 1, 1, 4 * size - 1, 2 * size - 1);
				display.AutoUpdate = true;

				display.SetPixel(ballPos[0], ballPos[1], backgroundColor: ConsoleColor.Black);

				//score
				if (ballPos[0] + ballDir[0] < 0)
				{
					score[1]++;
					ballPos = new int[] { 2 * size, size };
					//display.SetPixel(ballPos[0], ballPos[1], backgroundColor: ConsoleColor.White);
					//Thread.Sleep(1000);
					//display.SetPixel(ballPos[0], ballPos[1], backgroundColor: ConsoleColor.Black);
				}
				else if (ballPos[0] + ballDir[0] > 4 * size)
				{
					score[0]++;
					ballPos = new int[] { 2 * size, size };
					//display.SetPixel(ballPos[0], ballPos[1], backgroundColor: ConsoleColor.White);
					//Thread.Sleep(1000);
					//display.SetPixel(ballPos[0], ballPos[1], backgroundColor: ConsoleColor.Black);
				}

				//vertical reflection
				if (ballPos[1] + ballDir[1] < 1 || ballPos[1] + ballDir[1] > 2 * size - 1)
					ballDir[1] = -ballDir[1];

				//horizontal reflection
				if (ballPos[0] + ballDir[0] == 1 && Math.Abs(ballPos[1] + ballDir[1] - paddle1Pos[1]) <= paddleHeight)
					ballDir[0] = -ballDir[0];
				if (ballPos[0] + ballDir[0] == 4 * size - 1 && Math.Abs(ballPos[1] + ballDir[1] - paddle2Pos[1]) <= paddleHeight)
					ballDir[0] = -ballDir[0];

				ballPos[0] += ballDir[0];
				ballPos[1] += ballDir[1];

				display.SetPixel(ballPos[0], ballPos[1], backgroundColor: ConsoleColor.White);

				//score
				for (int i = 0; i < score[0].ToString().Length; i++)
					display.SetPixel(2 * size - score[0].ToString().Length + i - 1, 1, character: score[0].ToString()[i]);
				display.SetPixel(2 * size, 1, character: ':');
				for (int i = 0; i < score[1].ToString().Length; i++)
					display.SetPixel(2 * size + i + 2, 1, character: score[1].ToString()[i]);
				Thread.Sleep(minFrameTime);
			}
		}
	}
}
