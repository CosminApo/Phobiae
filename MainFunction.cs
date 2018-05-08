using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phobiae;


namespace Phobiae.PathFinding
{
    class MainFunction : GameMain
    {
        private bool[,] map;
        private Control[,] panelmap;
        private SearchParameters searchParameters;


        public void Run(PictureBox mob, Monster monster, PictureBox player)
        {
            // Start with a clear map (don't add any obstacles)
            InitializeMap(mob, player);
            AddObstacle();

            PathFinder pathFinder = new PathFinder(searchParameters);
            List<Point> path = pathFinder.FindPath();
            ShowRoute(path, mob, monster);
            RemoveMap();

        }


        public void RemoveMap()
        {
            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    GameMain.ActiveForm.Controls.Remove(panelmap[x, y]);
                }
            }
            Array.Clear(panelmap, 1, panelmap.Length - 1);
            Array.Clear(map, 1, map.Length - 1);

        }

        private void ShowRoute(IEnumerable<Point> path, PictureBox mob, Monster monster)
        {
            for (int y = this.map.GetLength(1) - 1; y >= 0; y--) 
            {
                for (int x = 0; x < this.map.GetLength(0); x++)
                {
                    if (path.Where(p => p.X == x && p.Y == y).Any())
                    {

                        int fromtop = panelmap[x, y].Top;
                        int fromleft = panelmap[x, y].Left;
                        mob.Left = fromleft;
                        mob.Top = fromtop;
                    }
                }
            }
        }

        private void InitializeMap(PictureBox mob, PictureBox player)
        {
            Point startLocation = new Point(0, 0);
            Point endLocation = new Point();
            int width = GameMain.ActiveForm.Width / 20;
            int height = GameMain.ActiveForm.Height / 10;
            int z = 0;
            int k = 0;
            this.map = new bool[25, 25];
            this.panelmap = new Control[25, 25];
            for (int y = 0; y < 25; y++)
            {
                z = 0;
                k += height;
                for (int x = 0; x < 25; x++)
                {
                    map[x, y] = true;
                    panelmap[x, y] = spawnnode(z, k);
                    z += width;
                }
            }

            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    if (startLocation == new Point(0, 0))
                    {

                        if (mob.Bounds.IntersectsWith(panelmap[x, y].Bounds))
                        {
                            startLocation = new Point(x, y);
                        }
                    }
                    if (endLocation == new Point(0, 0))
                    {
                        if (player.Bounds.IntersectsWith(panelmap[x, y].Bounds))
                        {
                            endLocation = new Point(x, y);
                        }
                    }
                }
            }
            this.searchParameters = new SearchParameters(startLocation, endLocation, map);
        }


        public Control spawnnode(int x, int y)
        {
            //this is how to dynamically spawn a platform.
            PictureBox platform = new PictureBox();
            int width = GameMain.ActiveForm.Width / 10;
            int height = GameMain.ActiveForm.Height / 10;
            platform.Tag = "pathfind";
            Size size = new Size(width, height);
            platform.Size = size;
            platform.Location = new Point(x, y);
            platform.Visible = true;

            return platform;

        }


        private void AddObstacle()
        {

            foreach (Control b in GameMain.ActiveForm.Controls)
            {

                if (b.GetType() == typeof(PictureBox))
                {

                    PictureBox pb = (PictureBox)b;
                    if (pb.Tag != null)
                    {

                        if (pb.Tag.ToString() == "bound")
                        {

                            for (int y = 0; y < 25; y++)
                            {
                                for (int x = 0; x < 25; x++)
                                {
                                    if (panelmap[x, y].Bounds.IntersectsWith(pb.Bounds))
                                    {
                                        if (!searchParameters.StartLocation.Equals(new Point(x, y)) && !searchParameters.EndLocation.Equals(new Point(x, y)))
                                        {

                                            map[x, y] = false;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }




        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
