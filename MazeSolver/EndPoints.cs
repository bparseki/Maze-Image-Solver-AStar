using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

/*namespace Maze
{
    public class EndPoints
    {
        public Tuple<Point, Point> getEndPoints(ref BitmapData bmpData)
        {
            int pixelSize = 3;
            int height = bmpData.Height;
            int width = bmpData.Width;
            Point start = new Point();
            Point end = new Point();
            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                int bytes = Math.Abs(bmpData.Stride) * height;
                //byte[] rgbValues = new byte[bytes];

                //Console.WriteLine("bytes = " + bytes);

                for (int y = 0; y < height; y++)
                {
                    //Console.WriteLine("y " +y);
                    byte* row = (byte*)bmpData.Scan0 + (y * Math.Abs(bmpData.Stride));

                    //loop through image bit sequence
                    for (int x = 0; x < width * pixelSize - 2; x += pixelSize)
                    {
                        //Console.WriteLine("x " + x);
                        if (row[x + 1] < 50)  //check if pixel is not green
                        {
                            //check if pixel is red
                            if (start.IsEmpty && row[x] < 70 && row[x + 2] > 130)
                            {
                                start.X = x/pixelSize;
                                start.Y = y;
                            }

                            //check if pixel is blue
                            if (end.IsEmpty && row[x] > 130 && row[x + 2] < 70)
                            {
                                end.X = x/pixelSize;
                                end.Y = y;
                            }
                        }
                    }
                }
            }
            //return empty points if start and end does not exist
            return new Tuple<Point, Point>(start, end); 
        }
    }
}*/

namespace Maze
{
    public class EndPoints
    {
        /// <summary>
        /// Creates an array that represents the maze as walled and non-walled Nodes 
        /// Returns 2 points representing the start and the end of the maze
        /// </summary>
        public Maze getEndPoints(ref BitmapData bmpData)
        {
            int pixelSize = 3;
            int height = bmpData.Height;
            int width = bmpData.Width;
            Point start = new Point();
            Point finish = new Point();
            Maze maze = new Maze(height, width, start, finish);
            //grid = new MazeNode[height, width];
            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                int bytes = Math.Abs(bmpData.Stride) * height;
                //byte[] rgbValues = new byte[bytes];

                Console.WriteLine("bytes = " + bytes);
                Console.WriteLine("height = " + height);
                Console.WriteLine("width = " + width);


                for (int y = 0; y < height; y++)
                {
                    //Console.WriteLine(y);
                    byte* row = (byte*)bmpData.Scan0 + (y * bmpData.Stride);

                    //loop through image bit sequence
                    for (int x = 0; x < width * pixelSize - 2; x += pixelSize)
                    {
                        //Console.WriteLine(x);
                        
                        //check if pixel is red (start)
                        if (start.IsEmpty && (row[x] < 60) && (row[x + 2] > 180) && (row[x + 1] < 60))
                        {
                            Console.WriteLine("red pixel" );
                            start.X = x / pixelSize;
                            start.Y = y;
                            maze.Grid[x / pixelSize, y] = new MazeNode(x / pixelSize, y, false);
                        }

                        //check if pixel is blue (end)
                        else if (finish.IsEmpty && (row[x] > 180) && (row[x + 2] < 60) && (row[x + 1] < 60))
                        {
                            Console.WriteLine("blue pixel");
                            finish.X = x / pixelSize;
                            finish.Y = y;
                            maze.Grid[x / pixelSize, y] = new MazeNode(x / pixelSize, y, false);
                        }

                        // check if pixel is black (wall)
                        else if ((row[x] < 50) && (row[x + 1] < 60) && (row[x + 2] < 60))
                        {
                            maze.Grid[x / 3, y] = new MazeNode(x / pixelSize, y, true);
                        }

                        // pixel is part of maze floor
                        else 
                            maze.Grid[x / pixelSize, y] = new MazeNode(x / pixelSize, y, false); 
                    }
                }
            }
            //return new Tuple<Point, Point>(start, end);
            maze.Start = start;
            maze.Finish = finish;
            Console.WriteLine("start and end location: " + start + ", " + finish);
            //System.Threading.Thread.Sleep(5000);
            return maze;
        }

        public void DrawSolution(ref BitmapData bmpData, ref Maze maze)
        {
            //int pixelSize = 3;
            //int height = bmpData.Height;
            //int width = bmpData.Width;
            MazeNode currentNode = maze.Grid[maze.Finish.X, maze.Finish.Y];
            unsafe
            {
                byte* row;
                while ((currentNode = currentNode.Parent) != null)
                {
                    row = (byte*)bmpData.Scan0 + (currentNode.Y * bmpData.Stride);
                    row[currentNode.X*3] = 48; //B
                    row[currentNode.X*+1] = 202; //G
                    row[currentNode.X*3+2] = 25; //R

                }
            }
            
        }
    }
}

