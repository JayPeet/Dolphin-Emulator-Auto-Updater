using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Diagnostics;

namespace DolphinUpdater
{
    public partial class DolphinLauncher : Form
    {
        Boolean Config = false;
        String DolphinDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        String DolphinVer = "0";
        String BuildType = "Stable";
        Boolean Downloading = false;
        WebClient webClient;               // Webclient which is used to download the updates.
        Stopwatch sw = new Stopwatch();    // Stopwatch for calculating download speeds
        public DolphinLauncher()
        {
            //Read Config
            String Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (File.Exists(Path + "\\HtmlAgilityPack.dll") != true)
            {
                System.IO.File.WriteAllBytes(Path + "\\HtmlAgilityPack.dll", DolphinUpdater.Properties.Resources.HtmlAgilityPack);
                Application.Restart();
            }
            if (File.Exists(Path + "\\config.ini") != true)
            {
                //File.Create(Path + "\\config.ini");
                ConfigForm c_form = new ConfigForm();
                this.Hide();
                c_form.ShowDialog();
                Config = true;
            }
            else
            {

                //Check if user is holding down shift. If so, show config menu.
                if (Control.ModifierKeys == Keys.Shift)
                {
                    ConfigForm c_form = new ConfigForm();
                    this.Hide();
                    c_form.ShowDialog();
                    Config = true;
                    return;
                }

                //Load Config
                TextReader reader = File.OpenText(Path + "\\config.ini");
                String line = "";
                Int16 c = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    switch (c)
                    {
                        case 0:
                            DolphinDir = line;
                            break;
                        case 1:
                            DolphinVer = line;
                            break;
                        case 2:
                            BuildType = line;
                            break;
                        case 3:
                            if (line == "1")
                            {
                                this.Visible = true;
                            }
                            else
                            {
                                this.Visible = false;
                            }
                            break;
                    }
                    c++;
                }
                reader.Close();
            }
            InitializeComponent();
            Thread1.RunWorkerAsync();
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!IsHandleCreated && value)
            {
                value = false;
                CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        private void Thread1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Check For Update.
            String WebsiteUrl = "https://dolphin-emu.org/download/";
            String Download_Link = "";
            HtmlWeb hw = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = hw.Load(WebsiteUrl);
            String Version = "0";
            if (BuildType == "1")
            {
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@id='download-dev']/table/tbody/tr[@class='infos']/td[@class='version always-ltr']/a[@href]"))
                {
                    Version = link.InnerHtml;
                    break;
                }
            }
            else {
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@id='download-stable']/h1/table/tbody/tr[@class='infos']/td[@class='version always-ltr']"))
                {
                    
                    String Temp = link.InnerHtml;
                    Temp = Temp.Replace("Dolphin ", "");
                    Version = Temp;
                    break;
                }
            }

            //Check if update required.
            if(DolphinVer != Version){

            //Get download link.
            if (BuildType == "1")
            {
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@id='download-dev']/table/tbody/tr[@class='download']/td[@class='download-links']/a[@class='btn always-ltr btn-info x64']"))
                {
                    Download_Link = link.Attributes["href"].Value;
                    break;
                }

                //Get update information.
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@id='download-dev']/table/tbody/tr[@class='infos']/td[@class='description always-ltr']"))
                {
                    Update_Text.Text = "Downloading Version : " + Version + "" + System.Environment.NewLine + link.InnerText;
                    break;
                }
                Download(Download_Link, DolphinDir + "\\update.7z");
                Downloading = true;
            }
            else {
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@id='download-stable']/h1/table/tbody/tr[@class='download']/td[@class='download-links']/a[@class='btn always-ltr btn-primary x64']"))
                {
                    Download_Link = link.Attributes["href"].Value;
                    break;
                }
                Update_Text.Text = "Downloading New Stable Build Version : " + Version;
                Download(Download_Link, DolphinDir + "\\update.exe");
                Downloading = true;
            }
            do
            {

            } while (Downloading == true);
                //Extract Update.
            if (BuildType == "1")
            {
                //Extract Zip
                //Put CMD 7Z on drive.
                System.IO.File.WriteAllBytes(DolphinDir + "\\7z.exe", DolphinUpdater.Properties.Resources._7za);
                //Process.Start(DolphinDir + "7z e archive.zip");
                ProcessStartInfo zipper = new ProcessStartInfo(DolphinDir + "\\7z.exe");
                zipper.Arguments = string.Format("x {0}.7z -y", "update");
                zipper.RedirectStandardInput = true;
                zipper.UseShellExecute = false;
                zipper.CreateNoWindow = true;
                zipper.WindowStyle = ProcessWindowStyle.Hidden;
                Process process = Process.Start(zipper);
                process.WaitForExit();
                DirectoryInfo Dir1 = new DirectoryInfo(DolphinDir + "\\Dolphin-x64");
                DirectoryInfo Dir2 = new DirectoryInfo(DolphinDir);
                CopyRecursively(Dir1,Dir2);
                String ThisPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                System.IO.File.Delete(ThisPath + "\\7z.exe");
                System.IO.File.Delete(DolphinDir + "\\update.7z");
                StreamWriter writer = new StreamWriter(ThisPath + "\\config.ini",false);
                writer.WriteLine(DolphinDir);
                writer.WriteLine(Version);
                writer.WriteLine(BuildType);
                writer.Close();
                Directory.Delete(DolphinDir + "\\Dolphin-x64",true);
                Process.Start(DolphinDir + "\\Dolphin.exe");
                Environment.Exit(0);

                }else{
                    /*
                     * This WOULD be used for extracting the EXE installer for stable builds, however the 7zip CMD wont extract it
                     * For some reason...
                     * 
                    String ThisPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    System.IO.File.WriteAllBytes(DolphinDir + "\\7z.exe", DolphinUpdater.Properties.Resources._7za);
                    MessageBox.Show("Test");
                    ProcessStartInfo zipper = new ProcessStartInfo(DolphinDir + "\\7z.exe");
                    zipper.Arguments = string.Format("x {0}.exe -y", "update");
                    zipper.RedirectStandardInput = true;
                    zipper.UseShellExecute = false;
                    zipper.CreateNoWindow = true;
                    zipper.WindowStyle = ProcessWindowStyle.Hidden;
                    Process process = Process.Start(zipper);
                    MessageBox.Show("Test2");
                    process.WaitForExit();
                    MessageBox.Show("Test3");
                    if (Directory.Exists(DolphinDir + "\\Languages") != true)
                    {
                        Directory.CreateDirectory(DolphinDir + "\\Languages");
                    }
                    if (Directory.Exists(DolphinDir + "\\Sys") != true)
                    {
                        Directory.CreateDirectory(DolphinDir + "\\Sys");
                    }
                    
                    DirectoryInfo Dir1 = new DirectoryInfo(DolphinDir + "\\update\\$_OUTDIR\\Sys");
                    DirectoryInfo Dir2 = new DirectoryInfo(DolphinDir + "\\Sys");
                    CopyRecursively(Dir1, Dir2);
                    Dir1 = new DirectoryInfo(DolphinDir + "\\update\\$_OUTDIR\\Languages");
                    Dir2 = new DirectoryInfo(DolphinDir + "\\Languages");
                    CopyRecursively(Dir1, Dir2);
                    Directory.Delete(DolphinDir + "\\update\\$_OUTDIR");
                    Directory.Delete(DolphinDir + "\\update\\$PLUGINSDIR");
                    
                    Dir1 = new DirectoryInfo(DolphinDir + "\\update");
                    Dir2 = new DirectoryInfo(DolphinDir);
                    CopyRecursively(Dir1, Dir2);

                    StreamWriter writer = new StreamWriter(ThisPath + "\\config.ini", false);
                    writer.WriteLine(DolphinDir);
                    writer.WriteLine(Version);
                    writer.WriteLine(BuildType);
                    writer.Close();
                    Directory.Delete(DolphinDir + "\\update");
                    File.Delete(DolphinDir + "\\update.exe");
                    Process.Start(DolphinDir + "\\Dolphin.exe");
                    Environment.Exit(0);
                     * 
                     * Instead we just run the installer. Nothing I can do about that for now.
                    */
                    Process.Start(DolphinDir + "\\update.exe");
                    //Then we write the new version number into the config file, before closing.
                    String ThisPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    StreamWriter writer = new StreamWriter(ThisPath + "\\config.ini", false);
                    writer.WriteLine(DolphinDir);
                    writer.WriteLine(Version);
                    writer.WriteLine(BuildType);
                    writer.Close();
                    Environment.Exit(0);
                }
            }
            else
            {
                ProcessStartInfo Dolphin = new ProcessStartInfo(DolphinDir + "\\Dolphin.exe");
                Dolphin.CreateNoWindow = false;
                Dolphin.WindowStyle = ProcessWindowStyle.Normal;
                Process process = Process.Start(Dolphin);
                Environment.Exit(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigForm c_form = new ConfigForm();
            this.Hide();
            c_form.ShowDialog();
        }

        public void Download(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(FinishedDownload);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Progress);
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);
                sw.Start();

                try
                {
                    webClient.DownloadFileAsync(URL, location);
                }
                catch (Exception ex)
                {
                    Downloading = false;
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Progress(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                speed.Text = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
            });
           this.Invoke((MethodInvoker)delegate
           {
               DownloadBar.Value = e.ProgressPercentage;
           });
           this.Invoke((MethodInvoker)delegate
           {
               Downloaded.Text = string.Format("{0} MB's / {1} MB's",
                 (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
           });

        }
        private void FinishedDownload(object sender, AsyncCompletedEventArgs e)
        {
            sw.Reset();

            if (e.Cancelled == true)
            {
                Downloading = false;
            }
            else
            {
                Downloading = false;
            }
        }

        public static void CopyRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name),true);
                
        }
    }
}
