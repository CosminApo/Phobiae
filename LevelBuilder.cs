using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phobiae
{
    class LevelBuilder:GameMain
    {
        public bool todespawn;
        public List<Control> platformlist;
        public Monster shadow;

        public void spawnPlatform(int x, int y, int width, int height, string tag)
        {
            

            //this is how to spawn a platform with code
            PictureBox platform = new PictureBox();
            platform.BackColor = Color.Transparent;
            if (tag == "bound")
            {
                platform.Image = Properties.Resources.alpine_landscape_platform_icy_1a_al1;
                platform.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            if (tag == "heart")
            {
                platform.Image = Properties.Resources.Heart;
                platform.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            if (tag == "save")
            {
                platform.Image = Properties.Resources.Totem;
                platform.SizeMode = PictureBoxSizeMode.StretchImage;
            }

             if (tag == "spikes")
            {
                platform.Image = Properties.Resources.SPIKESfinal;
                platform.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            if (todespawn == true)
            {
                despawnmonster();
            }
            if (tag == "shadow")
            {
                todespawn =true;
                spawnmonster();

            }


            platform.Tag = tag;
            Size size = new Size(width, height);
            platform.Size = size;
            platform.Location = new Point(x, y);
            platform.Visible = true;
          
            //Form2.ActiveForm.Controls.Add(platform);
            //platform.BringToFront();
            
            platformlist.Add(platform);
        }

        public List<Control> getplatformlist()
        {

            return platformlist;
        }

        public void spawnmonster()
        {
            shadow = new Monster();
            shadow.InitializeComponent();
            todespawn = true;
        }
        public void despawnmonster()
        {
            MessageBox.Show("s");
            shadow.despawnmonster();
            shadow.Dispose();
            
        }

        public void binarylevelbuilder(int levelcode)
        {
            platformlist = new List<Control>();
            int x;
            int width;
            int height;
            int y;
            string lvlcode = "codelevel"+Convert.ToString(levelcode);
            string tag;
           
            FileStream fsl = new FileStream(lvlcode, FileMode.Open);
            StreamReader k = new StreamReader(fsl);
            while (k.ReadLine() != null)
            {
                x = Convert.ToInt32(k.ReadLine());           
                y = Convert.ToInt32(k.ReadLine());          
                width = Convert.ToInt32(k.ReadLine());            
                height = Convert.ToInt32(k.ReadLine());            
                tag = Convert.ToString(k.ReadLine());               
               
                spawnPlatform(x, y, width, height, tag);

            }
            k.Close();

        }
    }
}
