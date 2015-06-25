namespace DolphinUpdater
{
    partial class DolphinLauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Thread1 = new System.ComponentModel.BackgroundWorker();
            this.DownloadBar = new System.Windows.Forms.ProgressBar();
            this.speed = new System.Windows.Forms.Label();
            this.Downloaded = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Thread1
            // 
            this.Thread1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Thread1_DoWork);
            // 
            // DownloadBar
            // 
            this.DownloadBar.Location = new System.Drawing.Point(17, 12);
            this.DownloadBar.Name = "DownloadBar";
            this.DownloadBar.Size = new System.Drawing.Size(534, 23);
            this.DownloadBar.TabIndex = 3;
            // 
            // speed
            // 
            this.speed.AutoSize = true;
            this.speed.Location = new System.Drawing.Point(476, 38);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(0, 17);
            this.speed.TabIndex = 4;
            // 
            // Downloaded
            // 
            this.Downloaded.AutoSize = true;
            this.Downloaded.Location = new System.Drawing.Point(14, 38);
            this.Downloaded.Name = "Downloaded";
            this.Downloaded.Size = new System.Drawing.Size(0, 17);
            this.Downloaded.TabIndex = 5;
            // 
            // DolphinLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 61);
            this.Controls.Add(this.Downloaded);
            this.Controls.Add(this.speed);
            this.Controls.Add(this.DownloadBar);
            this.Name = "DolphinLauncher";
            this.Text = "Dolphin Launcher";
            this.Load += new System.EventHandler(this.DolphinLauncher_Load);
            this.Shown += new System.EventHandler(this.DolphinLauncher_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker Thread1;
        private System.Windows.Forms.ProgressBar DownloadBar;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.Label Downloaded;
    }
}

