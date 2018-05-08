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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Net;

namespace Phobiae
{
    public partial class Phobiae : Form
    {
        System.Media.SoundPlayer startSoundPlayer = new System.Media.SoundPlayer("background.wav");
     
        System.Media.SoundPlayer endPlayer = new System.Media.SoundPlayer("gasp.wav");
        public int i = 0;
        public int currentlevel;
        public bool isconnected = false;
        public bool isonline = true;
        public bool isclicked = false;
        public bool isenabled = true;

        public bool getdrive()
        {
            return isconnected;
        }
        public Phobiae()    
        {
            InitializeComponent();
            pictureBox2.BackColor = Color.Transparent;
            startSoundPlayer.PlayLooping();
            isonline = CheckForInternetConnection();
            if (isonline == false)
            {
                isenabled = false;
                pbox_GDrive.Image = Properties.Resources.Google_Drive_icon_disabled;
                isclicked = true;
                lbl_edrive.Text = "DISABLED";
                lbl_edrive.ForeColor = Color.Red;
            }
        }

        public int getcurrentlevel()
        {
            return currentlevel;
        }

        private async void btn_NewGame_Click(object sender, EventArgs e)
        {
            startSoundPlayer.Stop();
            pbox_Fade.Image = Properties.Resources.fade;
            pbox_Fade.SizeMode = PictureBoxSizeMode.StretchImage;
            pbox_Fade.Visible = true;
            pbox_GDrive.Visible = false;
            lbl_edrive.Visible = false;
            if (isclicked == true)
            {
                endPlayer.Play();
            }
            tmr_Delay.Start();

            StreamWriter createfile = new StreamWriter("savefile1.txt",true);
            createfile.WriteLine("1");
            createfile.Close();

            currentlevel = 1;

            await Task.Delay(3300);
            this.Hide();
            GameMain frmObject = new GameMain();
            frmObject.setgdrive(isenabled);
            frmObject.setlvl(currentlevel);

            frmObject.Closed += (s, args) => this.Close();
            frmObject.Show();
            
            
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("https://www.facebook.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void btn_LoadGame_Click(object sender, EventArgs e)
        {
            

            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
           
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader filereader = new StreamReader(myStream);
                            Int32.TryParse(filereader.ReadLine(), out currentlevel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
           
            }
            else {
                currentlevel = 99;
            }
            if (currentlevel != 99)
            {
                startSoundPlayer.Stop();
                this.Hide();
                GameMain frmObject = new GameMain();
                frmObject.setgdrive(isenabled);
                frmObject.setlvl(currentlevel);

                frmObject.Closed += (s, args) => this.Close();
                frmObject.Show();
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            startSoundPlayer.Stop();
            endPlayer.Play();
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        { 
           startSoundPlayer.PlayLooping();

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            startSoundPlayer.Stop();
        }

        private void tmr_Delay_Tick(object sender, EventArgs e)
        {

        }
        
        private void pbox_GDrive_Click(object sender, EventArgs e)
        {
            if (isonline == true)
            {
                if (isclicked == false)
                {
                    isenabled = false;
                    pbox_GDrive.Image = Properties.Resources.Google_Drive_icon_disabled;
                    isclicked = true;
                    lbl_edrive.Text = "DISABLED";
                    lbl_edrive.ForeColor = Color.Red;
                }
                else
                {
                    isenabled = true;
                    pbox_GDrive.Image = Properties.Resources.Google_Drive_icon;
                    isclicked = false;
                    lbl_edrive.Text = "ENABLED";
                    lbl_edrive.ForeColor = Color.Green;
                }
            }
            else
            {
                MessageBox.Show("You cannot enable the Google Drive API because your internet connection is limited or non-existent.");
            }

        }
    }
}
