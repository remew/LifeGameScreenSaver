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

        private int pixelSize = 10;
        private LifeGame lg;

        #region Constructor
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
        #endregion

        private void init()
        {
            this.lg = new LifeGame(this.Bounds.Width / this.pixelSize, this.Bounds.Height / this.pixelSize);
            this.lifegameTimer.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.init();
        }

        private void lifegameTimer_Tick(object sender, EventArgs e)
        {
            this.lg.update();
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            this.lg.draw(e.Graphics, this.pixelSize);
        }

        #region exit action
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
        #endregion
    }
}
