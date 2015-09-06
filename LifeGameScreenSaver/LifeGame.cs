using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LifeGameScreenSaver
{
    class LifeGame
    {
        private bool[] map;
        private bool[] pre;
        private int width;
        private int height;

        public LifeGame(int width, int height)
        {
            this.createMap(width, height);
        }

        public void draw(Graphics g, int pixelSize)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (this.map[y * width + x])
                    {
                        g.FillRectangle(Brushes.Black, x * pixelSize, y * pixelSize, pixelSize, pixelSize);
                    }
                }
            }
        }

        private void createMap(int width, int height)
        {
            Random rand = new Random();
            this.map = new bool[width * height];
            this.pre = new bool[width * height];
            for (int i = 0; i < width * height; i++)
            {
                this.map[i] = this.pre[i] = rand.NextDouble() > 0.5;
            }
            this.width = width;
            this.height = height;
        }

        public void update()
        {
            bool[] tmp = this.pre;
            this.pre = this.map;
            this.map = tmp;

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    int num = this.getLeaveNum(x, y);
                    if (this.isLeave(x, y, this.pre))
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
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (this.isLeave(cx + x, cy + y, this.pre) && !(x == 0 && y == 0)) result++;
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
    }
}
