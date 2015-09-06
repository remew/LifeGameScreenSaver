using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace LifeGameScreenSaver
{
    public partial class MainForm : Form
    {
        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private bool isPreviewMode = false;
        private Point origin = new Point(int.MaxValue, int.MaxValue);

        private bool[] map;
        private bool[] preMap;
        private int width;
        private int height;

        private int pixelRatio = 5;

        public MainForm()
        {
            InitializeComponent();
        }

        //for show screensaver.
        public MainForm(Rectangle bounds)
        {
            InitializeComponent();
            this.Bounds = bounds;
            Cursor.Hide();
            this.TransparencyKey = this.BackColor;
        }

        //for preview screensaver.
        public MainForm(IntPtr previewHandle)
        {
            InitializeComponent();
            SetParent(this.Handle, previewHandle);

            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            Rectangle parentRect;
            GetClientRect(previewHandle, out parentRect);
            this.Size = parentRect.Size;
            this.Location = new Point(0, 0);
            this.isPreviewMode = true;
        }

        private void init()
        {
            this.createMap(this.Bounds.Width / this.pixelRatio, this.Bounds.Height / this.pixelRatio);
            this.lifegameTimer.Start();
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            if( !this.isPreviewMode )
            {
                System.Environment.Exit(0);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if( !this.isPreviewMode )
            {
                System.Environment.Exit(0);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if( !this.isPreviewMode )
            {
                if( this.origin.X == int.MaxValue && origin.Y == int.MaxValue )
                {
                    this.origin = e.Location;
                }
                if( Math.Abs(e.X - this.origin.X) > 20 || Math.Abs(e.Y - this.origin.Y) > 20 )
                {
                    System.Environment.Exit(0);
                }
            }
        }

        private void lifegameTimer_Tick(object sender, EventArgs e)
        {
            this.updateMap();
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            this.drawMap(e.Graphics);
        }

        private void createMap(int width, int height)
        {
            Random rand = new Random();
            this.map = new bool[width * height];
            this.preMap = new bool[width * height];
            for (int i = 0; i < width * height; i++ )
            {
                this.map[i] = rand.NextDouble() > 0.5;
            }
            this.width = width;
            this.height = height;
        }

        private void updateMap()
        {
            //swap the map.
            bool[] tmp = this.preMap;
            this.preMap = this.map;
            this.map = tmp;

            for (int y = 0; y < this.height; y++ )
            {
                for(int x = 0; x < this.width; x++)
                {
                    int num = this.getLeaveNum(x, y);
                    if (this.isLeave(x, y, this.preMap))
                    {
                        this.setLeave(x, y, num == 2 || num == 3, this.map);
                    }
                    else
                    {
                        this.setLeave(x, y, num == 3, this.map);
                    }
                }
            }
        }

        private int getLeaveNum(int cx, int cy)
        {
            int result = 0;
            for(int y = -1; y <= 1; y++)
            {
                for(int x = -1; x <= 1; x++)
                {
                    if (this.isLeave(cx + x, cy + y, this.preMap) && !(x == 0 && y == 0)) result++;
                }
            }
            return result;
        }

        private void setLeave(int x, int y, bool isLeave, bool[] map)
        {
            if (x < 0 || x >= this.width || y < 0 || y >= this.height) return;
            map[y * this.width + x] = isLeave;
        }

        private bool isLeave(int x, int y, bool[] map)
        {
            if (x < 0 || x >= this.width || y < 0 || y >= this.height) return false;
            return map[y * this.width + x];
        }

        private void drawMap(Graphics g)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if(this.map[y * width + x])
                    {
                        g.FillRectangle(Brushes.Black, (float)x * this.pixelRatio, (float)y * this.pixelRatio, (float)this.Bounds.Width / this.width, (float)this.Bounds.Height / this.height);
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.init(); 
        }
    }
}
