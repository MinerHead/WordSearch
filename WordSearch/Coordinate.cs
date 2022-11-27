using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace WordSearch
{
    internal struct Coordinate
    {
        public int X;
        public int Y;

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void MoveToNext(Coordinate direction)
        {
            this.X += direction.X;
            this.Y += direction.Y;
        }

        public Coordinate GetNext(Coordinate direction)
        {
            return new Coordinate(this.X + direction.X, this.Y + direction.Y);
        }

        public static bool IsValidDistance(Coordinate coordOriginal, Coordinate coordNext, out Coordinate direction)
        {
            // Check if second coordinate is within valid range of the first one and output the direction vector
            direction = new Coordinate(coordNext.X - coordOriginal.X, coordNext.Y - coordOriginal.Y);
            var diffY = Math.Abs(direction.Y);
            var diffX = Math.Abs(direction.X);
            if (diffY > 1 || diffX > 1)
            {
                return false;
            }
            else if (diffX == 0 && diffY == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
