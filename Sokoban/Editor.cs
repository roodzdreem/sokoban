using System.Drawing;
using System.Windows.Forms;

namespace Sokoban
{
    public class Editor
    {
        private static Label[,] _editFloor = new Label[Constants.MAPHEIGHT, Constants.MAPWIDTH];
        private static int[,] _intEditFloor = new int[Constants.MAPHEIGHT, Constants.MAPWIDTH];

        
        static int index = 0;
        static bool _heroAdded = false;

        public Editor()
        {
            InitEditFloor();
        }
        private static void InitEditFloor()
        { 
            for (int i = 0; i < Constants.MAPHEIGHT; i++)
                for (int j = 0; j < Constants.MAPWIDTH; j++)
                {
                    _editFloor[i, j] = CLeanEditFloor(i, j, Constants.GAMECOLOR);
                    _intEditFloor[i, j] = 0;
                }
        }
        

        public static Label CLeanEditFloor(int i, int j, Color color)
        {
            Label _floor = new Label();
            _floor.Location = new Point(j * Constants.BOXSIZE, i * Constants.BOXSIZE);
            _floor.Size = new Size(Constants.BOXSIZE, Constants.BOXSIZE);
            _floor.BackColor = color;
            _floor.MouseClick += DrawFloor;
            return _floor;
        }
        static public Label[,] GetEditFloor()
        {
            return _editFloor;
        }
        static public int[,] GetIntEditFloor()
        {
            return _intEditFloor;
        }
        private static void DrawFloor(object sender, MouseEventArgs e)
        {
            Control ctrl = sender as Control;
            int i, j;
            j = ctrl.Location.X / Constants.BOXSIZE;
            i = ctrl.Location.Y / Constants.BOXSIZE;


            switch (Form1.GetFloorType())
            {
                case 0:
                    if (_intEditFloor[i, j] == 2)
                        _heroAdded = false;
                    if (_intEditFloor[i, j] == 3)
                        --index;
                    
                    ctrl.BackColor = Constants.GAMECOLOR;
                    _intEditFloor[i, j] = 0;
                    
                    break;
                case 1:
                    ctrl.BackColor = Color.Black;
                    _intEditFloor[i, j] = 1;
                    break;
                case 2:
                    if (!_heroAdded)
                    {
                        ctrl.BackColor = Color.Blue;
                        _heroAdded = true;
                        _intEditFloor[i, j] = 2;
                    }
                    break;
                case 3:
                    if (index < Constants.BASEAMOUNT && ctrl.BackColor != Color.Green)
                    {
                        ctrl.BackColor = Color.Green;
                        index += 1;
                        _intEditFloor[i, j] = 3;
                    }
                    break;
                case 4:
                    ctrl.BackColor = Color.Yellow;
                    _intEditFloor[i, j] = 4;
                    break;
            }

        }
    }

    
}
