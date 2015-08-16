using System;
using System.Windows.Forms;
using System.IO;
namespace DolphinUpdater
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            String Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (File.Exists(Path + "\\config.ini") != true)
            {
                //Set Default dir to current dir.
                dir.Text = Path;
            }
            else
            {
                TextReader reader = File.OpenText(Path + "\\config.ini");
                String line = "";
                Int16 c = 0;
                while ((line = reader.ReadLine()) != null) {
                    switch (c)
                    {
                        case 0:
                            dir.Text = line;
                            break;
                        case 1:
                            //Lel
                            break;
                        case 2:
                            if(line == "1"){
                                dev.Checked = true;
                            }
                            else{
                            
                            }
                        break;

                        case 3:
                        if (line == "1")
                        {
                            show_updater.Checked = true;
                        }
                        else {
                            show_updater.Checked = false;
                        }
                        break;
                    }
                    c++;
                }
                reader.Close();
            }
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Restart();
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save Data.
            TextWriter writer = File.CreateText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\config.ini");
            writer.WriteLine(dir.Text);
            writer.WriteLine("0");
            if (dev.Checked == true)
            {
                writer.WriteLine("1");
            }
            else
            {
                writer.WriteLine("0");
            }
            if (show_updater.Checked == true)
            {
                writer.WriteLine("1");
            }
            else
            {
                writer.WriteLine("0");
            }
            writer.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextWriter writer = File.CreateText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\config.ini");
            writer.WriteLine(dir.Text);
            writer.WriteLine("0");
            if (dev.Checked == true)
            {
                writer.WriteLine("1");
            }
            else
            {
                writer.WriteLine("0");
            }
            if (show_updater.Checked == true)
            {
                writer.WriteLine("1");
            }
            else {
                writer.WriteLine("0");
            }
            writer.Close();
            Application.Exit();
        }
    }
}
