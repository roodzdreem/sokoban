using System.Windows.Forms;
using System.Drawing;

namespace Sokoban
{
    internal class Box
    {
        public static bool CheckWin()
        {
            Wall temp;
            Wall[] _bases = Map.GetBases();
            Wall[,] _floor = Map.GetFloor();


            for( int i = 0; i < 3; i++)
            {
                
                 temp = _floor[_bases[i].body.Top / Constants.BOXSIZE, _bases[i].body.Left / Constants.BOXSIZE]; 
                

                if (!temp.ismovable || !_bases[i].body.Bounds.IntersectsWith(temp.body.Bounds))
                {
                    return false;
                }
            }
            return true;
        }


        public static bool MoveRightBox(Wall box)
        {
            Wall[,] _floor = Map.GetFloor();
            int i, j;
            i = box.body.Top / Constants.BOXSIZE;
            j = box.body.Left / Constants.BOXSIZE;
            
            
            if (box.ismovable)
            {
                if ((i < 0 || j < 0 || i >= Constants.MAPHEIGHT || (j + 1) >= Constants.MAPWIDTH) || 
                    _floor[i, j + 1].ismovable || _floor[i, j + 1].iswall)
                    return false;
                MoveBox(_floor, i, j, i, j + 1);
            }
            return true;
        }
        public static bool MoveLeftBox(Wall box)
        {
            int i,j;
            i = box.body.Top / Constants.BOXSIZE;
            j = box.body.Left / Constants.BOXSIZE;
            
            
            Wall[,] _floor = Map.GetFloor();
            if (box.ismovable)
            {
                if ((i < 0 || (j - 1) < 0 || i >= Constants.MAPHEIGHT || j  >= Constants.MAPWIDTH) || 
                    _floor[i, (j) - 1].ismovable || _floor[i, (j) - 1].iswall)
                    return false;
                MoveBox(_floor, i, j, i, j - 1);
            }
            return true;

        }
        public static bool MoveUpBox(Wall box)
        {
            Wall[,] _floor = Map.GetFloor();
            int i, j;
            i = box.body.Top / Constants.BOXSIZE;
            j = box.body.Left / Constants.BOXSIZE;
            
            if (box.ismovable)
            {
                if (((i - 1) < 0 || j < 0 || i > Constants.MAPHEIGHT || j >= Constants.MAPWIDTH) 
                    || _floor[i - 1, j].ismovable || _floor[i - 1, j].iswall)
                {
                    return false;
                }
                MoveBox(_floor, i, j, i - 1, j);
            }
            return true;
        }
        public static bool MoveDownBox(Wall box)
        {
            Wall[,] _floor = Map.GetFloor();
            int i, j;
            i = box.body.Top / Constants.BOXSIZE;
            j = box.body.Left / Constants.BOXSIZE;
            if (box.ismovable)
            {
                if ((i < 0 || j < 0 || (i + 1) >= Constants.MAPHEIGHT || j > Constants.MAPWIDTH) || _floor[i + 1, j].ismovable ||
                    _floor[i + 1, j].iswall)
                { 
                    return false;
                }
                MoveBox(_floor, i, j, i+1, j);
            }
            return true;
        }
        private static void MoveBox(Wall[,] _floor, int i, int j , int newi, int newj)
        {
            _floor[newi, newj].body = Map.GetWall(newi, newj);
            _floor[newi, newj].iswall = false;
            _floor[newi, newj].ismovable = true;
            _floor[newi, newj].body.Location = new Point(newj * Constants.BOXSIZE, newi * Constants.BOXSIZE);
            _floor[newi, newj].body.Size = new Size(Constants.BOXSIZE, Constants.BOXSIZE);
            _floor[newi, newj].body.BackColor = Color.Yellow;


            _floor[i, j].iswall = false;
            _floor[i, j].ismovable = false;
            _floor[i, j].body.Location = new Point(j * Constants.BOXSIZE, i * Constants.BOXSIZE);
            _floor[i, j].body.Size = new Size(Constants.BOXSIZE, Constants.BOXSIZE);
            _floor[i, j].body.BackColor = Constants.GAMECOLOR;
        }
    }
}
