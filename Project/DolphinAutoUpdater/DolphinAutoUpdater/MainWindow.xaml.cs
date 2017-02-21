using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Xml;

namespace DolphinAutoUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            info_label.Content = "Initializing...";
            LoadConfig();
        }

        /// <summary>
        /// Loads the configuration file, or creates one.
        /// </summary>
        private void LoadConfig()
        {
            string xmlConfig = "";
            //Create config file, if missing.
            if (!File.Exists("config.xml"))
            {
                //Open new config file and fill with data.
                StreamWriter w = new StreamWriter("config.xml");
                xmlConfig = "<config><installDir>" + AppDomain.CurrentDomain.BaseDirectory + "</installDir><installedVersion>0</installedVersion><devBuilds>0</devBuilds></config>";

                w.Write(xmlConfig);
                w.Close();
            }

            //Load config from file.
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load("config.xml");
            info_label.Content = configDoc["config"]["installDir"].InnerText;

        }
    }
}
