using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Sokoban
{

    public partial class Form1 : Form
    {
        Map map = new Map();
        Editor editor = new Editor();
        RecordLabel[] _record = new RecordLabel[6];
        TextBox textBox = new TextBox();


        Timer timer = new Timer();
        Label lbltimer = new Label();
        int s = 0, m = 0;

        Button[] level_buttons = new Button[Constants.LEVELBUTTONS];
        Button[] edit_buttons = new Button[Constants.EDITBUTTONS];


        static private int _floorType { get; set; }
        static private string recordPath { get; set; }
        static private string levelPath { get; set; }


        public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(400, 400);
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            RecordsButton.Click += ShowRecords;
            StartMenu();
            Controls.Remove(ContinueButton);
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lbltimer.Text = $"{m} минут {s} секунд";
            ++s;
            if (s >= 60)
            {
                m = m - 1;
                s = 0;
            }
            if (m >= 60)
            {
                m = 0;
            }
        }
        private bool CheckRecord(string name)
        {
            int time = m * 60 + s;
            bool flag = false;
            int temp;
            string tempstr;
            for (int i = 1; i < _record.Length; i++)
            {
                if (_record[i].time > time || _record[i].time == -1)
                {
                    temp = _record[i].time;
                    _record[i].time = time;
                  
                    tempstr = _record[i].name;
                    _record[i].name = name;

                    

                    name = tempstr;
                    time = temp;

                    flag = true;
                }
            }
            return flag;
        }
        private void LoadRecords()
        {
            int i = 0;
            try
            {
                FileStream fs = new FileStream(recordPath, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                _record[0] = InitRecord("", 0, ClientSize.Width / 2 - Constants.EDITBUTTONWIDTH / 2, Constants.FIRSTBUTTON_Y + 40 * (i - 1), 0);
                _record[0].body.Text = "Таблица рекордов";
                i++;
                while (br.PeekChar() > -1)
                {
                    _record[i] = InitRecord(br.ReadString(), br.ReadInt32(), ClientSize.Width / 2 - Constants.EDITBUTTONWIDTH / 2, Constants.FIRSTBUTTON_Y + 40 * (i - 1), i);
                    i++;
                }
                while (i < _record.Length)
                {
                    _record[i] = InitRecord("", 0, ClientSize.Width / 2 - Constants.EDITBUTTONWIDTH / 2, Constants.FIRSTBUTTON_Y + 40 * (i - 1), i);
                    _record[i].body.Text = "Пусто";
                    _record[i].time = -1;
                    i++;
                }
                fs.Close();
            }
            catch { }
        }
        private RecordLabel InitRecord(string name, int time, int x, int y, int i)
        {
            RecordLabel rec = new RecordLabel();
            rec.body = new Label();
            rec.name = name;
            rec.time = time;
            rec.body.Text = rec.name + rec.time;
            rec.body.Size = new Size(Constants.EDITBUTTONWIDTH, Constants.EDITBUTTONHEIGHT);
            rec.body.Location = SetPos(x, Constants.FIRSTBUTTON_Y + 40 * (i - 1));
            rec.body.BackColor = Color.White;
            rec.body.TextAlign = ContentAlignment.MiddleCenter;
            return rec;
        }
        private void ShowRecords(object sender, EventArgs e)
        {
            Controls.Clear();
            Controls.Add(_record[0].body);
            for (int i = 1; i < _record.Length; i++)
            {
                if (_record[i].time != -1)
                    _record[i].body.Text = $"{_record[i].name} {_record[i].time / 60} минут {_record[i].time - _record[i].time / 60} секунд";
                else
                    _record[i].body.Text = "Пусто";
                Controls.Add(_record[i].body);
            }
        }
        private  void RefreshRecords()
        {
            try
            {
                FileStream fs = new FileStream(recordPath, FileMode.Open);
                BinaryWriter bw = new BinaryWriter(fs);
                for (int i = 1; i < _record.Length; i++)
                {
                    bw.Write(_record[i].name + " ");
                    bw.Write(_record[i].time);
                }
                fs.Close();
            }
            catch 
            {
                MessageBox.Show("Ошибка");
            }
            
        }
        private void ClearRecords(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryWriter bw = new BinaryWriter(fs);
                for (int i = 1; i < _record.Length; i++)
                {
                    bw.Write("Пусто");
                    bw.Write(-1);
                }
                fs.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private void EndGame()
        {
            Controls.Clear();
            BackColor = Constants.MENUCOLOR;


            
            textBox.Text = "Введите имя";
            textBox.Size = new Size(150, 50);
            textBox.Location = SetPos(ClientSize.Width / 2 - 150 / 2, ClientSize.Height / 2 - 50 / 2);
            textBox.BackColor = Color.White;
            


            Button btn = new Button();
            btn = InitButton(50,150, ClientSize.Width / 2 - 150 / 2, ClientSize.Height / 2 - 50 / 2 +50,"Подтвердить");
            btn.Click += AcceptName;


            Controls.Add(textBox);
            Controls.Add(btn);
        }
        private void AcceptName(object sender, EventArgs e)
        {
            if (CheckRecord(textBox.Text))
            {
                RefreshRecords();
                s = 0;
                m = 0;
            }
            
            ShowMenu(true);
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            KeyPressed(e);
            bool flag = Box.CheckWin();
            if (flag)
            {
                timer.Stop();
                EndGame();
            }
        }


        static public Point SetPos(int x, int y)
        {   
            Point pos = new Point(x, y);
            return pos;
        }


        private void KeyPressed(KeyEventArgs e)
        {
            int i, j;
            i = Map.GetPlayer().Top / Constants.BOXSIZE;
            j = Map.GetPlayer().Left / Constants.BOXSIZE;
            Wall[,] _floor = Map.GetFloor();
            switch(e.KeyCode)
            {
                case Keys.A:
                    if (Map.GetPlayer().Left > 0 && !_floor[i, j - 1].iswall)
                        if (Box.MoveLeftBox(_floor[i, j - 1]))
                        {
                            Map.GetPlayer().Left -= Constants.BOXSIZE;
                        }
                    break;
                case Keys.D:
                    if (Map.GetPlayer().Left + Constants.BOXSIZE < ClientSize.Width && !_floor[i, j + 1].iswall)
                        if (Box.MoveRightBox(_floor[i, j + 1]))
                        {
                            Map.GetPlayer().Left += Constants.BOXSIZE;
                        }
                    break;
                case Keys.W:
                    if (Map.GetPlayer().Top > 0 && !_floor[i - 1, j].iswall)
                        if (Box.MoveUpBox(_floor[ i - 1, j]))
                        {
                            Map.GetPlayer().Top -= Constants.BOXSIZE;
                        }
                    break;
                case Keys.S:
                    if (Map.GetPlayer().Top + Constants.BOXSIZE < ClientSize.Height && !_floor[i + 1, j].iswall)
                        if (Box.MoveDownBox(_floor[i + 1, j]))
                        {
                            Map.GetPlayer().Top +=  Constants.BOXSIZE; ;
                        }

                    break;
                case Keys.Escape:
                    ClientSize = new Size(400, 400);
                    ShowMenu(false);
                    break;
                default:
                    break;
            }
            
        }


        private void StartGame()
        {
            Controls.Clear();
            timer.Start();
            InitLevel();
            BackColor = Constants.GAMECOLOR;
            
        }


        private void InitLevel()
        {
            lbltimer.Location = SetPos(300, 380);
            lbltimer.Text = $"{m} минут {s} секунд";
            lbltimer.BackColor = Color.White;
            Controls.Add(lbltimer);
            InitWalls(Map.GetFloor());
            lbltimer.BringToFront();
            InitPlayer();
            InitBases();



        }
        private void InitBases()
        {
            Wall[] _bases = Map.GetBases();
            try
            {
                for (int i = 0; i < Constants.BASEAMOUNT; i++)
                {
                    Controls.Add(_bases[i].body);
                    _bases[i].body.BringToFront();
                }
            }
            catch { };
        }
        private void InitPlayer()
        {
            try
            {
                Controls.Add(Map.GetPlayer());
                Map.GetPlayer().BringToFront();
            }
            catch { };
        }
        private void InitWalls(Wall[,] _floor)
        {
            try
            {
                for (int i = 0; i < Constants.MAPHEIGHT; i++)
                    for (int j = 0; j < Constants.MAPWIDTH; j++)
                    {
                        Controls.Add(_floor[i, j].body);
                        _floor[i, j].body.BringToFront();
                    }
            }
            catch { };
        }


        public void CreateMap()
        {
            Controls.Clear();
            ClientSize = new Size(Constants.EDITWIDTH, 400);
            BackColor = Constants.MENUCOLOR;
            Label[,] _floor = Editor.GetEditFloor();
            InitEditMenuFloor(_floor);


            for (int i = 0; i < Constants.EDITBUTTONS - 1; i++)
            {
                edit_buttons[i] = InitButton( Constants.EDITBUTTONHEIGHT, Constants.EDITBUTTONWIDTH, Constants.EDITMENUCENTER, 30 + i * 50, "");
                Controls.Add(edit_buttons[i]);
            }
            edit_buttons[Constants.EDITBUTTONS - 1] = InitButton( Constants.EDITBUTTONHEIGHT * 2, Constants.EDITBUTTONWIDTH, Constants.EDITMENUCENTER, 30 + (Constants.EDITBUTTONS - 1) * 50, "");
            Controls.Add(edit_buttons[Constants.EDITBUTTONS - 1]);


            edit_buttons[0].Text = "Удалить";
            edit_buttons[1].Text = "Стена";
            edit_buttons[2].Text = "Игрок";
            edit_buttons[3].Text = "База";
            edit_buttons[4].Text = "Ящик";
            edit_buttons[5].Text = "Сохранить и выйти";


            edit_buttons[0].Click += EditButtonSpaceClick;
            edit_buttons[1].Click += EditButtonWallClick;
            edit_buttons[2].Click += EditButtonPlayerClick;
            edit_buttons[3].Click += EditButtonBaseClick;
            edit_buttons[4].Click += EditButtonBoxClick;
            edit_buttons[5].Click += EditContinueButtonClick;
        }

        private void InitEditMenuFloor(Label[,] _floor)
        {
            for (int i = 0; i < Constants.MAPHEIGHT; i++)
                for (int j = 0; j < Constants.MAPWIDTH; j++)
                {
                    Controls.Add(_floor[i, j]);
                    _floor[i, j].BringToFront();
                }
        }


        
        private void StartMenu()
        {
            Controls.Clear();
            BackColor = Constants.MENUCOLOR;
            Heading.Location = SetPos(ClientSize.Width / 2 - ShowLevelButton.Size.Width / 2, Constants.FIRSTBUTTON_Y - Constants.DISTANCEBTWNBUTTONS);
            Heading.Text = $"Сокобан";
            ShowLevelButton.Location = SetPos(ClientSize.Width / 2 - ShowLevelButton.Size.Width / 2, Constants.FIRSTBUTTON_Y);
            ExitButton.Location = SetPos(ClientSize.Width / 2 - ExitButton.Size.Width / 2, Constants.FIRSTBUTTON_Y + Constants.DISTANCEBTWNBUTTONS );
            AddStandartBtns();
        }
        private void ShowMenu(bool iswin)
        {
            BackColor = Constants.MENUCOLOR;
            if (iswin)
            {
                ShowWinMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }
        private void ShowWinMenu()
        {
            Controls.Clear();
            Heading.Text = "Победа";
            ShowLevelButton.Location = SetPos(ClientSize.Width / 2 - ShowLevelButton.Size.Width / 2, Constants.FIRSTBUTTON_Y);
            Heading.Location = SetPos(ClientSize.Width / 2 - Heading.Size.Width / 2, Constants.FIRSTBUTTON_Y - Constants.DISTANCEBTWNBUTTONS);
            ExitButton.Location = SetPos(ClientSize.Width / 2 - ExitButton.Size.Width / 2, Constants.FIRSTBUTTON_Y + Constants.DISTANCEBTWNBUTTONS);
            AddStandartBtns();
        }
        private void ShowPauseMenu()
        {
            Controls.Clear();
            timer.Stop();
            Heading.Text = "Сокобан";
            ContinueButton.Location = SetPos(ClientSize.Width / 2 - ContinueButton.Size.Width / 2, Constants.FIRSTBUTTON_Y);
            ShowLevelButton.Location = SetPos(ClientSize.Width / 2 - ShowLevelButton.Size.Width / 2, Constants.FIRSTBUTTON_Y + Constants.DISTANCEBTWNBUTTONS);
            RecordsButton.Location = SetPos(ClientSize.Width / 2 - ExitButton.Size.Width / 2, Constants.FIRSTBUTTON_Y + Constants.DISTANCEBTWNBUTTONS * 2);
            ExitButton.Location = SetPos(ClientSize.Width / 2 - ExitButton.Size.Width / 2, Constants.FIRSTBUTTON_Y + Constants.DISTANCEBTWNBUTTONS * 3);
            AddStandartBtns();
            Controls.Add(ContinueButton);
            Controls.Add(RecordsButton);
        }
        private void AddStandartBtns()
        {
            Controls.Add(ShowLevelButton);
            Controls.Add(ExitButton);
            Controls.Add(Heading);
        }


        private Button InitButton(int height, int width, int x, int y, string text)
        {
            Button btn = new Button();
            btn.Size = new Size(width, height);
            btn.Location = new Point(x, y);
            btn.BackColor = Color.White;
            btn.Text = text;
            return btn;
        }


        private void EditButtonSpaceClick(object sender, EventArgs e)
        {
            _floorType = 0;
        }
        private void EditButtonWallClick(object sender, EventArgs e)
        {
            _floorType = 1;
        }
        private void EditButtonPlayerClick(object sender, EventArgs e)
        { 
            _floorType = 2;
        }
        private void EditButtonBaseClick(object sender, EventArgs e)
        { 
            _floorType = 3;
        }
        private void EditButtonBoxClick(object sender, EventArgs e)
        { 
            _floorType = 4;
        }
        private void EditContinueButtonClick(object sender, EventArgs e)
        {
            ClientSize = new Size(400, 400);
            int[,] _intFloor = Editor.GetIntEditFloor();
            StartMenu();
            try
            {
                FileStream fs = new FileStream(levelPath, FileMode.Open);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(10);
                bw.Write(10);
                for (int i = 0; i < Constants.MAPHEIGHT; i++)
                    for (int j = 0; j < Constants.MAPWIDTH; j++)
                        bw.Write(_intFloor[i, j]);
                fs.Close();

            }
            catch
            {
            }
        }


        public static int GetFloorType()
        { 
            return _floorType;
        }


        private void Button1Click(object sender, EventArgs e)
        {
            StartLvl($"records{1}.txt", $"level{1}.txt");
        }
        private void Button2Click(object sender, EventArgs e)
        {
            StartLvl($"records{2}.txt", $"level{2}.txt");
        }
        private void Button3Click(object sender, EventArgs e)
        {
            StartLvl($"records{3}.txt", $"level{3}.txt");
        }
        private void Button4Click(object sender, EventArgs e)
        {
            StartLvl($"records{4}.txt", $"level{4}.txt"); ;
        }
        private void Button5Click(object sender, EventArgs e)
        {
            StartLvl($"records{5}.txt", $"level{5}.txt");
        }
        private void Button6Click(object sender, EventArgs e)
        {
            ShowUserLevelsMenu();
        }
        private void StartLvl(string record, string level)
        {
            recordPath = record;
            LoadRecords();
            map.LoadMap(level);
            StartGame();
        }
        private void ChooseLevelMenu()
        {
            
            for (int i = 0; i < Constants.LEVELBUTTONS; i++)
            {
                level_buttons[i] = InitButton( Constants.EDITBUTTONHEIGHT, Constants.EDITBUTTONWIDTH, ClientSize.Width / 2 - Constants.EDITBUTTONWIDTH /2, 50 + i * 50, $"{i + 1} Уровень");
                Controls.Add(level_buttons[i]);
            }

            level_buttons[0].Click += Button1Click;
            level_buttons[1].Click += Button2Click;
            level_buttons[2].Click += Button3Click;
            level_buttons[3].Click += Button4Click;
            level_buttons[4].Click += Button5Click;

            level_buttons[5].Text = "Уровни пользователей";
            level_buttons[5].Click += Button6Click;
        }


        private void ShowUserLevelsMenu()
        {
            Controls.Clear();
            Button[] StartUserLevelButton = new Button[3];
            Button[] EditUserLevelButton = new Button[3];
            BackColor = Constants.MENUCOLOR;
            Heading.Location = SetPos(ClientSize.Width / 2 - ShowLevelButton.Size.Width / 2, Constants.FIRSTBUTTON_Y - Constants.DISTANCEBTWNBUTTONS);
            for (int i = 0; i < 3; i++)
            {
                StartUserLevelButton[i] = InitButton(40, Constants.EDITBUTTONWIDTH, 40, Constants.FIRSTBUTTON_Y + i * 50, $"Уровень {i + 1}");
                EditUserLevelButton[i] = InitButton(40, Constants.EDITBUTTONWIDTH, ClientSize.Width - 40 - Constants.EDITBUTTONWIDTH, Constants.FIRSTBUTTON_Y + i * 50, $"Редактировать");
                Controls.Add(StartUserLevelButton[i]);
                Controls.Add(EditUserLevelButton[i]);
            }
            Controls.Add(Heading);


            StartUserLevelButton[0].Click += UserButton1Click;
            StartUserLevelButton[1].Click += UserButton2Click;
            StartUserLevelButton[2].Click += UserButton3Click;

            EditUserLevelButton[0].Click += UserEditButton1Click;
            EditUserLevelButton[1].Click += UserEditButton2Click;
            EditUserLevelButton[2].Click += UserEditButton3Click;
        }
        private void UserButton1Click(object sender, EventArgs e)
        {
            StartLvl("userrecords1.txt", "userlevels1.txt");
        }
        private void UserButton2Click(object sender, EventArgs e)
        {
            StartLvl("userrecords2.txt", "userlevels2.txt");
        }
        private void UserButton3Click(object sender, EventArgs e)
        {
            StartLvl("userrecords3.txt", "userlevels3.txt");
        }
        private void UserEditButton1Click(object sender, EventArgs e)
        {
            ClearRecords("userrecords1.txt");
            levelPath = "userlevels1.txt";
            CreateMap();
        }
        private void UserEditButton2Click(object sender, EventArgs e)
        {
            ClearRecords("userrecords2.txt");
            levelPath =  "userlevels2.txt";
            CreateMap();
        }
        private void UserEditButton3Click(object sender, EventArgs e)
        {
            ClearRecords("userrecords3.txt");
            levelPath =  "userlevels3.txt";
            CreateMap();
        }


        private void ShowLevelButton_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            s = 0;
            m = 0;
            ChooseLevelMenu();
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ContinueButton_Click(object sender, EventArgs e)
        {
            timer.Start();
            StartGame();
        }
    }
    static public class Constants
    {
        public const int BOXSIZE = 40;
        public const int LEVELBUTTONS = 6;
        public const int BASEAMOUNT = 3;
        public const int EDITBUTTONS = 6;
        public const int MAPHEIGHT = 10;
        public const int MAPWIDTH = 10;
        public const int EDITWIDTH = 550;
        public const int EDITBUTTONWIDTH = 140;
        public const int EDITBUTTONHEIGHT = 30;
        public const int DISTANCEBTWNBUTTONS = 70;
        public const int EDITMENUCENTER = 400 + (EDITWIDTH - 400) / 2 - EDITBUTTONWIDTH/2;
        public const int FIRSTBUTTON_Y = 120;
        static public Color MENUCOLOR = Color.DarkGray;
        static public Color GAMECOLOR = Color.DarkSlateGray;
    }

    public class Wall
    {
        public Label body { get; set; }
        public bool iswall { get; set; }
        public bool ismovable { get; set; }
    }
    public class RecordLabel
    {
        public Label body { get; set; }
        public int time { get; set; }
        public string name { get; set; }
    }
}
