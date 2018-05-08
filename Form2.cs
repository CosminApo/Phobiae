using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Windows.Forms;
using System.Threading;


namespace Phobiae
{
    public partial class GameMain : Form
    {
        PictureBox slash;
        public int movespeed = 10;
        public bool Jumping = false;
        public int jumpheight = 0;
        public bool singlejump = true;
        public int framecounter = 1;
        public int hp = 3;
        public bool isHit = true;
        public int count = 0;
        public int currentlvl;
        public bool spawned = false;
        public bool hideslash = false;
        Monster mob;
        public List<Control> listofplat;
        public Label sayfull;
        public int templvl;
        public int i = 0;
        public bool isconnected = false;
        static string[] Scopes = { DriveService.Scope.Drive };
        public bool antisavespam = false;
        public bool gdrive = true;



        public GameMain()
        {
            InitializeComponent();
        }

        public void setgdrive (bool passed)
        {
            gdrive = passed;
        }
        public async void Slash()
        {      
            slash = new PictureBox();
            Size size = new Size(70, 30);
            slash.Size = size;
            slash.Image = Properties.Resources.Slash;
            slash.BackColor = Color.Transparent;
            slash.SizeMode = PictureBoxSizeMode.StretchImage;
            int y = pbox_Player.Top;
            int x = pbox_Player.Left + 55;
            slash.Location = new Point(x, y);
            slash.Visible = true;
            this.Controls.Add(slash);
            slash.BringToFront();
            await Task.Delay(300);
            this.Controls.Remove(slash);
        }

        public int getcurrentlevel()
        {
            return currentlvl;
        }


        private void spawnplatforms()
        {
           
            for (int i=0; i < listofplat.Count;i++)
            {
                this.Controls.Add(listofplat[i]);
                listofplat[i].BringToFront();
                
            }
            
        }

        private void pbox_Heart3_Click(object sender, EventArgs e)
        {

        }

        private void pbox_Heart2_Click(object sender, EventArgs e)
        {

        }

        private void pbox_Heart1_Click(object sender, EventArgs e)
        {

        }
        public PictureBox getplayer()
        {
            return pbox_Player;
        }

        private void Form2_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (pbox_YouDied.Visible == false)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        tmrRight.Start();
                        break;

                    case Keys.A:
                        tmrLeft.Start();
                        break;
                    case Keys.W:
                        if (singlejump == true)
                        {
                            singlejump = false;
                            jumpheight = 15;
                            tmrUp.Start();
                            Jumping = true;
                        }
                        break;

                    case Keys.K:
                        Slash();
                        break;
                }
            }
            if (pbox_YouDied.Visible == true)
            {
                if (e.KeyCode == Keys.R)
                {
                    pbox_YouDied.Visible = false;
                    pbox_Player.Location = new Point(80, 10);
                    pbox_Heart1.Visible = true;
                    pbox_Heart2.Visible = true;
                    pbox_Heart3.Visible = true;
                    hp = 3;
                    lbl_dead.Visible = false;
                }
                if (e.KeyCode == Keys.Escape)
                {
                    Application.Exit();
                }
            }
        }

        private void tmrRight_Tick(object sender, EventArgs e)

        {
            if (pbox_Player.Left < 990)
            {
                pbox_Player.Left += movespeed;
                tmr_animation.Start();
            }
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    framecounter = 1;
                    pbox_Player.Image = Properties.Resources.frame_Stand;
                    tmr_animation.Stop();
                    tmrRight.Stop();
                    break;

                case Keys.A:
                    framecounter = 1;
                    pbox_Player.Image = Properties.Resources.frame_Stand;
                    tmr_animationleft.Stop();
                    tmrLeft.Stop();
                    break;

                case Keys.W:
                    tmrUp.Stop();
                    //singlejump = true;
                    Jumping = false;
                    break;
            }
        }
        
        private void savefileonline()
        {
            UserCredential credential;
            credential = GetCredentials();
            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Phobiae",
            });
            UploadFile("SaveFiles\\SaveFile1.txt", service);
        }

        private static void UploadFile(string path, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = "text/plain";
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "text/plain");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
        }

        private static UserCredential GetCredentials()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;      
            }
            return credential;
        }
    
        private void tmrGameLoop_Tick(object sender, EventArgs e)
        {
        
            if (pbox_Player.Bounds.IntersectsWith(pbox_Air.Bounds) && Jumping == false)
            {
                tmrGravity.Start();
            }

            if (spawned == false)
            {
                spawned = true;
            }

            foreach (Control b in this.Controls)
            {
                if (b.GetType() == typeof(PictureBox))
                {

                    PictureBox pb = (PictureBox)b;


                    if (pb.Tag != null)
                    {
                        if (pb.Tag.ToString() == "bound")
                        {
                            if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                            {
                                tmrGravity.Stop();
                                singlejump = true;
                            }
                        }

                        if (pb.Tag.ToString() == "save")
                        {
                            
                            if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                            {
                                if (antisavespam == false)
                                {
                                    antisavespam = true;
                                    tmr_SpamDelay.Start();
                                    pbox_Player.Left += 100;
                                    StreamWriter saver = new StreamWriter("SaveFiles\\SaveFile1.txt", false);
                                    saver.WriteLine(Convert.ToString(currentlvl));
                                    saver.Close();

                                    if(gdrive==true)
                                    {
                                        savefileonline();
                                    }
                                }
                            }
                        }
                        if (pb.Tag.ToString() == "NextLevel")
                        {
                            if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                            {
                                if (currentlvl < 4)
                                {
                                    currentlvl++;
                                    nextlevel();
                                }
                            }
                        }
                        if (pb.Tag.ToString() == "PreviousLevel" && currentlvl>1)
                        {
                            if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                            {
                                templvl = currentlvl;
                                currentlvl--;
                                nextlevel();
                            }
                        }
                        if (pb.Tag.ToString() == "heart" && hp<3)
                        {
                            if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                            {
                                if (hp == 2)
                                {
                                    Controls.Remove(pb);
                                    hp++;
                                    pbox_Heart3.Visible = true;
                                }
                                if (hp == 1)
                                {
                                    Controls.Remove(pb);
                                    hp++;
                                    pbox_Heart2.Visible = true;
                                }
                            }
                        }
                        if (pb.Tag.ToString() == "heart" && hp == 3)
                        {
                            if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                            {
                                tmr_FullHP.Start();
                            }
                        }

                        if (pb.Tag.ToString() == "spikes")
                        {
                            if (isHit == true && hp > 0)
                                if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                                {
                                    hp--;
                                    isHit = false;
                                    tmrIFrameOnHit.Start();

                                }
                        }
                        if (pb.Tag.ToString() == "monster1")
                        {
                            if (isHit == true && hp > 0)
                                if (pbox_Player.Bounds.IntersectsWith(b.Bounds))
                                {
                                    hp--;
                                    isHit = false;
                                    tmrIFrameOnHit.Start();


                                }
                            //if (slash.Bounds.IntersectsWith(b.Bounds))
                            //{
                            //    mob.sethp(-1);
                            //    mob.checkhp(mob.gethp(), mob);
                            //    if (mob.gethp() <= 0)
                            //    {
                            //        Form2.ActiveForm.Controls.Remove(pb);
                            //    }
                            //}

                        }

                    }
                }
            }


            switch (hp)
            {
                case 0:
                    pbox_Heart1.Visible = false;
                    pbox_YouDied.Visible = true;
                    lbl_dead.Visible = true;
                    break;
                case 1:
                    pbox_Heart2.Visible = false;
                    break;
                case 2:
                    pbox_Heart3.Visible = false;
                    break;
                case 3:
                    break;
            }
        }


        public void nextlevel()
        {
    
            LevelBuilder builder = new LevelBuilder();

            int i = 0;
            foreach (Control b in this.Controls)
            {
                if (b.GetType() == typeof(PictureBox))
                {
                    i++;
                }
            }
            
            while (i > 7)
            {
                foreach (Control b in this.Controls)
                {
                    if (b.GetType() == typeof(PictureBox))
                    {

                        PictureBox pb = (PictureBox)b;
                        if (pb.Tag != null)
                        {
                            if (pb.Tag.ToString() != "player" && pb.Tag.ToString() != "hp" && pb.Tag.ToString() != "dead" && pb.Tag.ToString() != "NextLevel" && pb.Tag.ToString() != "PreviousLevel" && pb.Tag.ToString() != "Air")
                            {
                                Controls.Remove(pb);
                                i--;
                            }
                        }
                    }
                }
            }


            if (i == 7)
            {
                builder.binarylevelbuilder(currentlvl);
            }
            listofplat = builder.getplatformlist();
            spawnplatforms();
            //mob = builder.spawnmonster("monster1");
            if (templvl > currentlvl)
            {
                pbox_Player.Location = new Point(750, 250);
            }
            else
            {
                pbox_Player.Location = new Point(50, 250);
            }
            pbox_Player.BringToFront();
            
        }

        private void tmrGravity_Tick(object sender, EventArgs e)
        {
            pbox_Player.Top += movespeed + 3;
        }

        private void tmrIFrameOnHit_Tick(object sender, EventArgs e)
        {
            count++;
            if (count == 50)
            {
                isHit = true;
                count = 0;
                tmrIFrameOnHit.Stop();
            }

            if (count % 2 == 1)
            {
                pbox_Player.Visible = false;
            }
            if (count % 2 == 0)
            {
                pbox_Player.Visible = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            nextlevel();
            tmrGameLoop.Start();
        }

        public void setlvl(int level)
        {
            currentlvl = level;
        }

        private void tmrLeft_Tick_1(object sender, EventArgs e)
        {
            if (pbox_Player.Left >= 1)
            {
                pbox_Player.Left -= movespeed;
                tmr_animationleft.Start();
            }
        }

        private void tmrUp_Tick_1(object sender, EventArgs e)
        {
            pbox_Player.Top -= jumpheight;
            jumpheight--;

            if (jumpheight < 0)
            {
                jumpheight = 0;
            }
            if (jumpheight == 0)
            {
                tmrGravity.Start();
            }
        }

        private void tmr_animation_Tick(object sender, EventArgs e)
        {
            string image = "Character\\frame" + Convert.ToString(framecounter) + "_Right.png";
            pbox_Player.Image = Image.FromFile(image);
            pbox_Player.SizeMode = PictureBoxSizeMode.StretchImage;
            framecounter++;
            if (framecounter >= 8)
            {
                framecounter = 1;
            }
        }

        private void tmr_animationleft_Tick(object sender, EventArgs e)
        {
            string image = "Character\\frame" + Convert.ToString(framecounter) + "_Left.png";
            pbox_Player.Image = Image.FromFile(image);
            pbox_Player.SizeMode = PictureBoxSizeMode.StretchImage;
            framecounter++;
            if (framecounter >= 8)
            {
                framecounter = 1;
            }
        }

        private void tmr_FullHP_Tick(object sender, EventArgs e)
        {
           
            if (i == 0)
            {
                sayfull = new Label();
                sayfull.Text = "Full HP";
                sayfull.ForeColor = Color.White;
                sayfull.BackColor = Color.Black;
                Point asd = new Point(pbox_Player.Left + 100, pbox_Player.Top - 50);
                sayfull.Location = asd;
                sayfull.Visible = true;
                Controls.Add(sayfull);
            }
            i++;
            if (i == 10)
            {
                Controls.Remove(sayfull);
                tmr_FullHP.Stop();
                i = 0;
            }
        }


        public int timercount = 0;
        private void tmr_SpamDelay_Tick(object sender, EventArgs e)
        {
            timercount++;
            if (timercount==100)
            {
                timercount = 0;
                antisavespam = false;
                tmr_SpamDelay.Stop();
            }

        }
    }
}
