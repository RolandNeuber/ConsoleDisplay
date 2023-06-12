using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDisplay
{
    public static class ConsolePrograms
    {
        public static void Clock(int size = 8)
        {
            int accuracy = 1;
            Display display = Display.NewDisplay(2 * size + 1, 2 * size + 1, backgroundFill: ConsoleColor.Black, fillSymbol: ' ');
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
            display.Update();

            double t = 2 * Math.PI / 60 * (DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600);
            int secondHand = (int)Math.Round(1f * size);
            int minuteHand = (int)Math.Round(0.75f * size);
            int hourHand = (int)Math.Round(0.5f * size);

            while (true)
            {
                display.DrawLine(size, size, size + (int)Math.Round(secondHand * Math.Sin(t)), size + (int)Math.Round(secondHand * -Math.Cos(t)), backgroundColor: ConsoleColor.White);
                display.DrawLine(size, size, size + (int)Math.Round(minuteHand * Math.Sin(t / 60)), size + (int)Math.Round(minuteHand * -Math.Cos(t / 60)), backgroundColor: ConsoleColor.Yellow);
                display.DrawLine(size, size, size + (int)Math.Round(hourHand * Math.Sin(t / 3600 * 5)), size + (int)Math.Round(hourHand * -Math.Cos(t / 3600 * 5)), backgroundColor: ConsoleColor.DarkYellow);
                display.Update();
                Thread.Sleep(1000 / accuracy);
                display.DrawLine(size, size, size + (int)Math.Round(secondHand * Math.Sin(t)), size + (int)Math.Round(secondHand * -Math.Cos(t)), backgroundColor: ConsoleColor.Black);
                display.DrawLine(size, size, size + (int)Math.Round(minuteHand * Math.Sin(t / 60)), size + (int)Math.Round(minuteHand * -Math.Cos(t / 60)), backgroundColor: ConsoleColor.Black);
                display.DrawLine(size, size, size + (int)Math.Round(hourHand * Math.Sin(t / 3600 * 5)), size + (int)Math.Round(hourHand * -Math.Cos(t / 3600 * 5)), backgroundColor: ConsoleColor.Black);
                t = 2 * Math.PI / 60 * (DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600);
                //Console.WriteLine(t);
            }
        }
        public static void Snake(int size = 12, int minFrameTime = 100)
        {
            //Render map with walls
            Display display = Display.NewDisplay(2 * size + 1, 2 * size + 1, backgroundFill: ConsoleColor.White);
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
            while (fruitPos == segments[0]);
            display.SetPixel(fruitPos[0], fruitPos[1], backgroundColor: ConsoleColor.Red);
            //game loop
            int[] dPos = new int[2];
            char dir = ' ';
            while (true)
            {
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

                display.Update();
                Thread.Sleep(minFrameTime);
            }
            display.Update();
            string msg = $"Score: {segments.Count}";
            if (msg.Length <= 2 * size + 1)
                for (int i = 0; i < msg.Length; i++)
                {
                    Thread.Sleep(100);
                    display.SetPixel(size + i - msg.Length / 2, size, msg[i], ConsoleColor.Red, ConsoleColor.Black);
                    display.Update(size + i - msg.Length / 2, size, size + i - msg.Length / 2, size);
                }
            Console.SetCursorPosition(0, 2 * size + 1);
        }
    }
}
