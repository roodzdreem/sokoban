using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Sokoban
{
    public class Map
    {
        static private Label player { get; set; }


        static private Wall[] bases = new Wall[Constants.BASEAMOUNT];
        static private Wall[,] floor = new Wall[Constants.MAPHEIGHT, Constants.MAPWIDTH];

        public Map ()
        {
            for (int i = 0; i<Constants.BASEAMOUNT;i++)
            {
                bases[i] = BuildBase(0,i);
            }
            ClearMap();
            player = InitHero(0, 0);
        }
        static public Label GetPlayer()
        {
            return player;
        }
        static public Wall[] GetBases()
        {
            return bases;
        }
        static public Wall[,] GetFloor()
        {
            return floor;
        }
        


        private static Wall PlaceMovableBox(int i, int j)
        {
            Wall box;
            box = InitFloor(i, j, false, true, Color.Yellow);
            return box;
        }
        private static Wall BuildBase(int i, int j)
        {
            Wall _base;
            _base = InitBase(i, j);
            return _base;
        }
        public static Wall BuildWall(int i, int j)
        {
            Wall wall;
            wall = InitFloor(i, j, true, false, Color.Black);
            return wall;
        }
        public static Label GetWall(int i, int j)
        {
            return floor[i, j].body;
        }
        

        public static Label InitHero(int i, int j)
        {
            Label _player;
            _player = new Label();
            _player.Location = new Point(j * Constants.BOXSIZE, i * Constants.BOXSIZE);
            _player.Size = new Size(Constants.BOXSIZE, Constants.BOXSIZE);
            _player.BackColor = Color.Blue;
            return _player;
        }
        public static Wall InitBase(int i, int j)
        {
            Wall _base = new Wall();
            _base.body = new Label();
            _base.iswall = false;
            _base.ismovable = false;
            _base.body.Location = new Point(10 + j * Constants.BOXSIZE, 10 + i * Constants.BOXSIZE);
            _base.body.Size = new Size(20, 20);
            _base.body.BackColor = Color.Green;
            return _base;
        }
        public static Wall InitFloor(int i, int j, bool _iswall, bool _ismovable, Color color)
        {
            Wall _floor = new Wall();
            _floor.body = new Label();
            _floor.iswall = _iswall;
            _floor.ismovable = _ismovable;
            _floor.body.Location = new Point(j * Constants.BOXSIZE, i * Constants.BOXSIZE);
            _floor.body.Size = new Size(Constants.BOXSIZE, Constants.BOXSIZE);
            _floor.body.BackColor = color;
            return _floor;
        }


        public void LoadMap(string path)
        {
            ClearMap();
            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                BinaryReader br = new BinaryReader(fs);
                int rows, columns, temp, i = 0, j = 0, index = 0;
                rows = br.ReadInt32();
                columns = br.ReadInt32();
                while (br.PeekChar() > -1)
                {
                    if (j == Constants.MAPWIDTH)
                    {
                        i++;
                        j = 0;
                    }
                    temp = br.ReadInt32();
                    switch (temp)
                    {
                        case 0:
                            floor[i, j] = InitFloor(i,j,false,false, Constants.GAMECOLOR);
                            break;
                        case 1:
                            floor[i,j] = BuildWall(i, j);
                            break;
                        case 2:
                            player = InitHero(i, j);
                            break;
                        case 3:
                            bases[index] = BuildBase(i, j);
                            index++;
                            break;
                        case 4:
                            floor[i,j] = PlaceMovableBox(i, j);
                            break;
                    }
                    j++;
                }  
                fs.Close();
            }
            catch
            {
            }
        }
        public void ClearMap()
        {
            for (int i = 0; i < Constants.MAPHEIGHT; i++)
                for (int j = 0; j < Constants.MAPWIDTH; j++)
                {
                    floor[i,j] = CleanFloor(i, j, Constants.GAMECOLOR);
                }
        }
        public static Wall CleanFloor(int i, int j, Color color)
        {
            Wall _floor; 
            _floor = InitFloor(i, j, false, false, color);
            return _floor;
        }
    }
}
