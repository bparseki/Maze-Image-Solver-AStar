using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Maze
{
    public enum Direction { UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight, Up, None };
    public class MazeNode : IComparable<MazeNode>
    {
        //private int status;

        public Direction Dir{ get; set; }
        public MazeNode Parent { get; set; }
        public UInt32 gCost{ get; set;}
        public UInt32 hCost { get; set; }
        public UInt32 fCost{ get {return (gCost + hCost);}}
        public bool IsWall { get; set; }

        public double Priority{ get; set; }
        public long InsertionIndex { get; set; }
        public int QueueIndex { get; set; }
        public Point Location { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    
        public MazeNode(int x, int y, bool isWall)
        {
            Location = new Point(x, y);
            IsWall = isWall;
            Parent = null;
            gCost = 0;
            hCost = 0;
            X = x;
            Y = y;
        }


        public UInt64 Key { get { return (((UInt64)(UInt32)X) << 32) | (UInt64)(UInt32)Y; } }
        


        public int CompareTo(MazeNode other)
        {
            if (this.fCost < other.fCost) return -1;
            else if (this.fCost == other.fCost) return 0;
            else return 1;
        }

    }
}
