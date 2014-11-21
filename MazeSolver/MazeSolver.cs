using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;


using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
//using Math;
//using MazeDataStructures;

namespace Maze
{
    
    public class MazeSolver
    {

        public void SolveMaze(ref Maze maze)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            HeapPriorityQueue<MazeNode> openList = new HeapPriorityQueue<MazeNode>(maze.Height * maze.Width/2); //open list
            //SortedSet<MazeNode> openList = new SortedSet<MazeNode>(); //open list
            Dictionary<UInt64, MazeNode> closedList = new Dictionary<UInt64, MazeNode>(); //closed list
            MazeNode currentNode = maze.Grid[maze.Start.X, maze.Start.Y];
            Console.WriteLine(maze.Start.X + ",");
            Console.WriteLine(maze.Start.Y + " ");
            Console.WriteLine(maze.Finish.X + ",");
            Console.WriteLine(maze.Finish.Y);
               
            openList.Enqueue(currentNode, 0);
            int i = 0;
            while (openList.Count > 0)
            {
                currentNode = openList.Dequeue();
                if (currentNode.Location == maze.Finish) break;
                closedList.Add(currentNode.Key, currentNode);

                foreach (MazeNode neighbour in GetNeighbours(currentNode, ref maze)) //Checks all the squares adjacent to the current point
                {
                    //Console.WriteLine("fgfggffg ");
                    if (closedList.ContainsKey(neighbour.Key)) continue;

                    //If parent is null, it's our first visit to the node
                        
                    UInt32 tentativeGCost = currentNode.gCost + GetCost(currentNode.Location, neighbour.Location);
                    if (neighbour.Parent == null || tentativeGCost < neighbour.gCost)
                    {
                        neighbour.Parent = currentNode; //Where it came from, final path can be found by linking parents
                        neighbour.gCost = tentativeGCost;
                            
                        if (openList.Contains(neighbour))
                        {
                            openList.UpdatePriority(neighbour, neighbour.fCost);    
                        }
                        else
                        {
                            neighbour.hCost = GetManhattan(neighbour.Location, maze.Finish); //Calculates total cost by combining the X distance by the Y
                            openList.Enqueue(neighbour, neighbour.fCost);
                            //closedList.Add(neighbour.Key, neighbour);
                        }
                    }

                }
                i++;
            }
            //Console.WriteLine("number of times in while " + i);
            stopWatch.Stop();
            System.Diagnostics.Debug.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
            return;
        }

        private MazeNode Jump(ref Maze maze, MazeNode current, Direction d)
        {
            switch (d)
            {
                case Direction.Left:
                    if ((current.X - 1) < 0 || maze.Grid[current.X - 1, current.Y].IsWall) return null;
                    current = maze.Grid[current.X - 1, current.Y];
                    if (current.Location == maze.Finish) return current;
                    break;
                case Direction.Right:
                    if ((current.X + 1) > (maze.Width - 1) || maze.Grid[current.X + 1, current.Y].IsWall) return null;
                    current = maze.Grid[current.X + 1, current.Y];
                    if (current.Location == maze.Finish) return current;
                    break;
                case Direction.Down:
                    if (current.X - 1 < 0 || current.Y + 1 > maze.Width - 1 || maze.Grid[current.X, current.Y + 1].IsWall) return null;
                    current = maze.Grid[current.X, current.Y + 1];
                    if (current.Location == maze.Finish) return current;
                    break;
                case Direction.Up:
                    if ((current.Y - 1) < 0 || maze.Grid[current.X, current.Y - 1].IsWall) return null;
                    current = maze.Grid[current.X, current.Y - 1];
                    if (current.Location == maze.Finish) return current;
                    break;
                case Direction.UpLeft:
                    if (current.X - 1 < 0 || current.Y - 1 < 0 || maze.Grid[current.X - 1, current.Y - 1].IsWall) return null;
                    current = maze.Grid[current.X - 1, current.Y - 1];
                    if (current.Location == maze.Finish) return current;
                    if (Jump(ref maze, current, Direction.Left) != null) return current;
                    if (Jump(ref maze, current, Direction.Up) != null) return current;
                    break;
                case Direction.DownLeft:
                    if (current.X - 1 < 0 || current.Y + 1 > maze.Width - 1 || maze.Grid[current.X - 1, current.Y + 1].IsWall) return null;
                    current = maze.Grid[current.X - 1, current.Y - 1];
                    if (current.Location == maze.Finish) return current;
                    if (Jump(ref maze, current, Direction.Left) != null) return current;
                    if (Jump(ref maze, current, Direction.Down) != null) return current;
                    break;
                case Direction.DownRight:
                    if ((current.X + 1) > (maze.Width - 1) || current.Y + 1 > maze.Width - 1 || maze.Grid[current.X + 1, current.Y + 1].IsWall) return null;
                    current = maze.Grid[current.X + 1, current.Y + 1];
                    if (current.Location == maze.Finish) return current;
                    if (Jump(ref maze, current, Direction.Right) != null) return current;
                    if (Jump(ref maze, current, Direction.Down) != null) return current;
                    break;
                case Direction.UpRight:
                    if ((current.X + 1) > (maze.Width - 1) || current.Y - 1 < 0 || maze.Grid[current.X + 1, current.Y - 1].IsWall) return null;
                    current = maze.Grid[current.X + 1, current.Y - 1];
                    if (current.Location == maze.Finish) return current;
                    if (Jump(ref maze, current, Direction.Right) != null) return current;
                    if (Jump(ref maze, current, Direction.Up) != null) return current;
                    break;
                case Direction.None:
                    if (current.X - 1 < 0 || current.Y + 1 > maze.Width - 1) return null;
                    break;
            }
            return Jump(ref maze, current, d);
        }
        private List<MazeNode> GetSuccessors(ref Maze maze, MazeNode parent, int x, int y, Direction d)
        {
            switch(d)
            {
                case Direction.UpLeft:
                    break;
                case Direction.Left:
                case Direction.DownLeft:
                case Direction.Down:
                case Direction.DownRight:
                case Direction.Right:
                case Direction.UpRight:
                case Direction.Up:
                case Direction.None:

            }
            /*List<MazeNode> neighbours = GetNeighbours(parent, ref maze);
            if (neighbours.Count == 8)
            {
                if (x && y)
                {
                    return Jump()
                }
                else
                {

                }
            }*/


            List<MazeNode> neighbours = new List<MazeNode>();
            if (Math.Abs(x + y) == 1) //direction is vertical or horizontal
            {
                if ()
            }
            else // direction is diagonal
            {

            }
            
            for (int i = Math.Max(0, parent.X - 1); i <= Math.Min(parent.X + 1, maze.Width-1); i++)
            {
                
                for (int j = Math.Max(0, parent.Y - 1); j <= Math.Min(parent.Y + 1, maze.Height-1); j++)
                {
                    //Console.WriteLine(j);
                    //Console.WriteLine(i);
                    if (maze.Grid[i, j].IsWall || (i == parent.X && j == parent.Y) )
                        continue;
                    neighbours.Add(maze.Grid[i, j]);
                }
            }
            return neighbours;
            //Console.WriteLine("neighjhgjfhjfg " + neighbours.First());
        }

        private List<MazeNode> Prune(int x, int y, ref List<MazeNode> neighbours)
        {

        }








        private List<MazeNode> GetNeighbours(MazeNode parent, ref Maze maze)
        {
            List<MazeNode> neighbours = new List<MazeNode>();
           // Console.WriteLine("parent location");
            //Console.WriteLine(parent.X);
            //Console.WriteLine(parent.Y);
            
            for (int i = Math.Max(0, parent.X - 1); i <= Math.Min(parent.X + 1, maze.Width-1); i++)
            {
                
                for (int j = Math.Max(0, parent.Y - 1); j <= Math.Min(parent.Y + 1, maze.Height-1); j++)
                {
                    //Console.WriteLine(j);
                    //Console.WriteLine(i);
                    if (maze.Grid[i, j].IsWall || (i == parent.X && j == parent.Y) )
                        continue;
                    neighbours.Add(maze.Grid[i, j]);
                }
            }
            return neighbours;
            //Console.WriteLine("neighjhgjfhjfg " + neighbours.First());
            
            
        }

        private UInt32 GetCost(Point curr, Point neighbour)
        {
            if (Math.Abs(curr.X - neighbour.X) + Math.Abs(curr.Y - neighbour.Y) == 2)
            {
                return 14; //the cost to move diagonally
            }
            else return 10; //the cost to move horizontally or vertically
        }

        private UInt32 GetManhattan(Point curr, Point finish)
        {
            return (UInt32) (Math.Abs(finish.X - curr.X) + Math.Abs(finish.Y - curr.Y))*4;
        }
    }  
}


/*Point currentPoint = new Point(start.X, start.Y);
                SortedDictionary<UInt64, UInt32> openList = new SortedDictionary<UInt64, UInt32>(); //open list
                Dictionary<UInt64, UInt32> closedList = new Dictionary<UInt64, UInt32>(); //closed list
               
                openList.Add((((UInt64)(UInt32)x) << 32) | (UInt64)(UInt32)y, hCost(start, end)
                while (openList.Count > 0)
                {

                }
            }
            return;
        }

        private UInt32 hCost(Point current, Point end)
        {

        }*/

/*unsafe
            {
                Point start = points.Item1;
                Point end = points.Item2;
                int pixelSize = 3;
                int x = start.X;
                int xRGB = start.X * pixelSize;
                int y = start.Y;
                byte* row = (byte*)bmpData.Scan0 + (y * bmpData.Stride);
            }*/