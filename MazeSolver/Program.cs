using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Maze
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Please specify a source and an output destination.");
            }
            String source = args[0];
            String output = args[1];
            String sourceExt = source.Substring(source.Length - 3);
            String outputExt = output.Substring(output.Length - 3);
            System.Diagnostics.Debug.WriteLine("Matrix has you...");
            //Console.Error.WriteLine("testing");
            //Console.WriteLine(outputExt);
            //System.Threading.Thread.Sleep(5000);
            // Keep the console window open in debug mode.
            //Hello t = new Hello();
            //Bitmap bmp = new Bitmap(args[0]);
            //Console.WriteLine(bmp.PixelFormat.ToString());
            //t.LockUnlockBitsExample(args[0]);


            //Error Handling for input and output files
            if (!sourceExt.Equals("bmp") && !sourceExt.Equals("png") && !sourceExt.Equals("jpg"))
            {
                throw new System.ArgumentException("source file extension must be format [bmp,png,jpg]", source);
            }

            if (!outputExt.Equals("bmp") && !outputExt.Equals("png") && !outputExt.Equals("jpg"))
            {
                throw new System.ArgumentException("output file extension must be correct format [bmp,png,jpg]", source);
            }

            if (!sourceExt.Equals(outputExt))
            {
                throw new System.ArgumentException("output file extension must match input file extension", source);
            }



            Bitmap bmp = new Bitmap(source);
            int width = bmp.Width;
            int height = bmp.Height;
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            //Point start = getStart(bmpData);
            //MazeNode[,] grid; //array of nodes representing the maze
            //Maze maze;
            EndPoints p = new EndPoints();
            //Tuple<Point, Point> points = p.getEndPoints(ref bmpData);
            Maze maze = p.getEndPoints(ref bmpData);
            MazeSolver s = new MazeSolver();
            s.SolveMaze(ref maze);
            if (maze.Grid[maze.Finish.X, maze.Finish.Y].Parent == null)
            {
                throw new System.ArgumentException("start point must be indicated with red pixels", source);
            }

            p.DrawSolution(ref bmpData, ref maze);
            /*int i;
            for (i = 0; i < 20; i++)
            {
                Console.WriteLine(Maze.Grid[i, i].IsWall);
            }
            System.Threading.Thread.Sleep(5000);*/

            //Point start = points.Item1;
            //Point end = points.Item2;
            Point start = maze.Start;
            Point end = maze.Finish;
            //Console.WriteLine("start and end location: " + start + ", " + end);
            //System.Threading.Thread.Sleep(5000);

            if (start.IsEmpty) { throw new System.ArgumentException("start point must be indicated with red pixels", source); }
            if (end.IsEmpty) { throw new System.ArgumentException("end point must be indicated with blue pixels", source); }
            
            //System.Threading.Thread.Sleep(5000);
            bmp.UnlockBits(bmpData);
            /*Pen redPen = new Pen(Color.Red, 3);
            Pen bluePen = new Pen(Color.Blue, 3);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawLine(bluePen, start, new Point(start.X+3, start.Y));
                graphics.DrawLine(redPen, end, new Point(end.X-3, end.Y));
            }*/

            switch (outputExt)
            {
                case "bmp":
                    bmp.Save(output, ImageFormat.Bmp);
                    break;
                case "png":
                    bmp.Save(output, ImageFormat.Png);
                    break;
                case "jpg":
                    bmp.Save(output, ImageFormat.Jpeg);
                    break;
                default:
                    throw new System.ArgumentException("output file extension must be correct format [bmp,png,jpg]", source);
            } 
        }
    }
}
