using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Maze
{
    public class Maze
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Point Start { get; set; }
        public Point Finish { get; set; }
        public MazeNode[,] Grid { get; set; }

        public Maze(int height, int width, Point start, Point finish)
        {
            this.Grid = new MazeNode[height, width];
            this.Height = height;
            this.Width = width;
            this.Start = start;
            this.Finish = finish;
        }

        
    }
}
