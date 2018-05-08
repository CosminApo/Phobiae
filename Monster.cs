using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Phobiae.PathFinding;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Phobiae
{
    class Monster : PictureBox
    {
        private System.Windows.Forms.Timer timer1;
        private IContainer components;
        private int monsterhp = 1;
        private string tag = "monster1";
        private int x = 300;
        MainFunction pf;
        private int y = 400;
        private System.Windows.Forms.Timer timer2;
        private bool tospawn = true;
        GameMain form2obj;
        Monster mob;

        PictureBox monster;
        PictureBox player;

        public int getx()
        {
            return x;

        }
        public int gety()
        {
            return y;

        }
        public void setx(int y)
        {
            x = y;
        }
        public void sety(int x)
        {
            y = x;
        }
        public void incrementx(int y)
        {
            x += y;
        }
        public void incrementy(int x)
        {
            y += x;
        }

        public void sethp(int x)
        {
            monsterhp += x;

        }
        public int gethp()
        {
            return monsterhp;
        }


        public void checkhp(int hp, Monster mob)
        {
            if (hp <= 0)
            {
                GameMain.ActiveForm.Controls.Remove(mob);
            }
        }

        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            mob = new Monster();
            player = new PictureBox();
            pf = new MainFunction();
            form2obj = new GameMain();


        }
        public void despawnmonster()
        {
            timer1.Stop();
            timer2.Stop();
            GameMain.ActiveForm.Controls.Remove(mob);
            mob.Dispose();
            form2obj.Dispose();
            pf.Dispose();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tospawn)
            {
                monster = new PictureBox();
                Size size = new Size(50, 65);
                monster.Size = size;
                monster.Image = Properties.Resources.Lostyghost_2B;
                monster.BackColor = Color.Transparent;
                monster.SizeMode = PictureBoxSizeMode.StretchImage;
                monster.Location = new Point(x, y);
                monster.Visible = true;
                monster.Tag = tag;
                GameMain.ActiveForm.Controls.Add(monster);
                monster.BringToFront();
                tospawn = false;
            }
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (Control b in GameMain.ActiveForm.Controls)
            {
                if (b.GetType() == typeof(PictureBox))
                {
                    PictureBox pb = (PictureBox)b;
                    if (pb.Tag != null)
                    {

                        if (pb.Tag.ToString() == "player")
                        {

                            player = pb;
                        }
                    }
                }
            }

            pf.Run(monster, mob, player);
            pf.Dispose();

        }
    }
}
