namespace PkmGen1SaveEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlToolbar = new Panel();
            lblCurrentFile = new Label();
            btnSaveAs = new Button();
            btnOpenSave = new Button();
            grpTrainer = new GroupBox();
            txtPlayTime = new TextBox();
            numMoney = new NumericUpDown();
            txtRivalName = new TextBox();
            txtPlayerName = new TextBox();
            cmbGameVersion = new ComboBox();
            grpBadges = new GroupBox();
            chkBadgeEarth = new CheckBox();
            chkBadgeVolcano = new CheckBox();
            chkBadgeMarsh = new CheckBox();
            chkBadgeSoul = new CheckBox();
            chkBadgeRainbow = new CheckBox();
            chkBadgeThunder = new CheckBox();
            chkBadgeCascade = new CheckBox();
            chkBadgeBoulder = new CheckBox();
            statusStrip1 = new StatusStrip();
            tslStatus = new ToolStripStatusLabel();
            tslVersion = new ToolStripStatusLabel();
            pnlToolbar.SuspendLayout();
            grpTrainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMoney).BeginInit();
            grpBadges.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(lblCurrentFile);
            pnlToolbar.Controls.Add(btnSaveAs);
            pnlToolbar.Controls.Add(btnOpenSave);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(882, 60);
            pnlToolbar.TabIndex = 0;
            // 
            // lblCurrentFile
            // 
            lblCurrentFile.AutoSize = true;
            lblCurrentFile.Location = new Point(497, 18);
            lblCurrentFile.Name = "lblCurrentFile";
            lblCurrentFile.Size = new Size(144, 20);
            lblCurrentFile.TabIndex = 2;
            lblCurrentFile.Text = "Aucun fichier chargé";
            // 
            // btnSaveAs
            // 
            btnSaveAs.Enabled = false;
            btnSaveAs.Location = new Point(260, 12);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(200, 29);
            btnSaveAs.TabIndex = 1;
            btnSaveAs.Text = "Enregistrer sous...";
            btnSaveAs.UseVisualStyleBackColor = true;
            // 
            // btnOpenSave
            // 
            btnOpenSave.Location = new Point(23, 12);
            btnOpenSave.Name = "btnOpenSave";
            btnOpenSave.Size = new Size(221, 29);
            btnOpenSave.TabIndex = 0;
            btnOpenSave.Text = "Ouvrir une sauvegarde";
            btnOpenSave.UseVisualStyleBackColor = true;
            btnOpenSave.Click += btnOpenSave_Click;
            // 
            // grpTrainer
            // 
            grpTrainer.Controls.Add(txtPlayTime);
            grpTrainer.Controls.Add(numMoney);
            grpTrainer.Controls.Add(txtRivalName);
            grpTrainer.Controls.Add(txtPlayerName);
            grpTrainer.Controls.Add(cmbGameVersion);
            grpTrainer.Location = new Point(23, 66);
            grpTrainer.Name = "grpTrainer";
            grpTrainer.Size = new Size(409, 370);
            grpTrainer.TabIndex = 1;
            grpTrainer.TabStop = false;
            grpTrainer.Text = "Informations du dresseur";
            // 
            // txtPlayTime
            // 
            txtPlayTime.Enabled = false;
            txtPlayTime.Location = new Point(92, 268);
            txtPlayTime.Name = "txtPlayTime";
            txtPlayTime.Size = new Size(151, 27);
            txtPlayTime.TabIndex = 4;
            // 
            // numMoney
            // 
            numMoney.Enabled = false;
            numMoney.Location = new Point(93, 222);
            numMoney.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numMoney.Name = "numMoney";
            numMoney.Size = new Size(150, 27);
            numMoney.TabIndex = 3;
            numMoney.ThousandsSeparator = true;
            // 
            // txtRivalName
            // 
            txtRivalName.Enabled = false;
            txtRivalName.Location = new Point(92, 178);
            txtRivalName.Name = "txtRivalName";
            txtRivalName.Size = new Size(151, 27);
            txtRivalName.TabIndex = 2;
            // 
            // txtPlayerName
            // 
            txtPlayerName.Enabled = false;
            txtPlayerName.Location = new Point(92, 135);
            txtPlayerName.Name = "txtPlayerName";
            txtPlayerName.Size = new Size(151, 27);
            txtPlayerName.TabIndex = 1;
            // 
            // cmbGameVersion
            // 
            cmbGameVersion.Enabled = false;
            cmbGameVersion.FormattingEnabled = true;
            cmbGameVersion.Items.AddRange(new object[] { "Pokémon Rouge / Bleu — Français" });
            cmbGameVersion.Location = new Point(92, 86);
            cmbGameVersion.Name = "cmbGameVersion";
            cmbGameVersion.Size = new Size(151, 28);
            cmbGameVersion.TabIndex = 0;
            // 
            // grpBadges
            // 
            grpBadges.Controls.Add(chkBadgeEarth);
            grpBadges.Controls.Add(chkBadgeVolcano);
            grpBadges.Controls.Add(chkBadgeMarsh);
            grpBadges.Controls.Add(chkBadgeSoul);
            grpBadges.Controls.Add(chkBadgeRainbow);
            grpBadges.Controls.Add(chkBadgeThunder);
            grpBadges.Controls.Add(chkBadgeCascade);
            grpBadges.Controls.Add(chkBadgeBoulder);
            grpBadges.Location = new Point(454, 66);
            grpBadges.Name = "grpBadges";
            grpBadges.Size = new Size(399, 370);
            grpBadges.TabIndex = 2;
            grpBadges.TabStop = false;
            grpBadges.Text = "Badges";
            // 
            // chkBadgeEarth
            // 
            chkBadgeEarth.AutoSize = true;
            chkBadgeEarth.Location = new Point(25, 249);
            chkBadgeEarth.Name = "chkBadgeEarth";
            chkBadgeEarth.Size = new Size(101, 24);
            chkBadgeEarth.TabIndex = 7;
            chkBadgeEarth.Text = "checkBox8";
            chkBadgeEarth.UseVisualStyleBackColor = true;
            // 
            // chkBadgeVolcano
            // 
            chkBadgeVolcano.AutoSize = true;
            chkBadgeVolcano.Location = new Point(25, 219);
            chkBadgeVolcano.Name = "chkBadgeVolcano";
            chkBadgeVolcano.Size = new Size(101, 24);
            chkBadgeVolcano.TabIndex = 6;
            chkBadgeVolcano.Text = "checkBox7";
            chkBadgeVolcano.UseVisualStyleBackColor = true;
            // 
            // chkBadgeMarsh
            // 
            chkBadgeMarsh.AutoSize = true;
            chkBadgeMarsh.Location = new Point(25, 189);
            chkBadgeMarsh.Name = "chkBadgeMarsh";
            chkBadgeMarsh.Size = new Size(101, 24);
            chkBadgeMarsh.TabIndex = 5;
            chkBadgeMarsh.Text = "checkBox6";
            chkBadgeMarsh.UseVisualStyleBackColor = true;
            // 
            // chkBadgeSoul
            // 
            chkBadgeSoul.AutoSize = true;
            chkBadgeSoul.Location = new Point(25, 159);
            chkBadgeSoul.Name = "chkBadgeSoul";
            chkBadgeSoul.Size = new Size(101, 24);
            chkBadgeSoul.TabIndex = 4;
            chkBadgeSoul.Text = "checkBox5";
            chkBadgeSoul.UseVisualStyleBackColor = true;
            // 
            // chkBadgeRainbow
            // 
            chkBadgeRainbow.AutoSize = true;
            chkBadgeRainbow.Location = new Point(25, 129);
            chkBadgeRainbow.Name = "chkBadgeRainbow";
            chkBadgeRainbow.Size = new Size(101, 24);
            chkBadgeRainbow.TabIndex = 3;
            chkBadgeRainbow.Text = "checkBox4";
            chkBadgeRainbow.UseVisualStyleBackColor = true;
            // 
            // chkBadgeThunder
            // 
            chkBadgeThunder.AutoSize = true;
            chkBadgeThunder.Location = new Point(25, 99);
            chkBadgeThunder.Name = "chkBadgeThunder";
            chkBadgeThunder.Size = new Size(101, 24);
            chkBadgeThunder.TabIndex = 2;
            chkBadgeThunder.Text = "checkBox3";
            chkBadgeThunder.UseVisualStyleBackColor = true;
            // 
            // chkBadgeCascade
            // 
            chkBadgeCascade.AutoSize = true;
            chkBadgeCascade.Location = new Point(25, 69);
            chkBadgeCascade.Name = "chkBadgeCascade";
            chkBadgeCascade.Size = new Size(101, 24);
            chkBadgeCascade.TabIndex = 1;
            chkBadgeCascade.Text = "checkBox2";
            chkBadgeCascade.UseVisualStyleBackColor = true;
            // 
            // chkBadgeBoulder
            // 
            chkBadgeBoulder.AutoSize = true;
            chkBadgeBoulder.Location = new Point(25, 39);
            chkBadgeBoulder.Name = "chkBadgeBoulder";
            chkBadgeBoulder.Size = new Size(101, 24);
            chkBadgeBoulder.TabIndex = 0;
            chkBadgeBoulder.Text = "checkBox1";
            chkBadgeBoulder.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tslStatus, tslVersion });
            statusStrip1.Location = new Point(0, 477);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(882, 26);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // tslStatus
            // 
            tslStatus.Name = "tslStatus";
            tslStatus.Size = new Size(195, 20);
            tslStatus.Text = "Prêt — ouvrez un fichier .sav";
            // 
            // tslVersion
            // 
            tslVersion.Name = "tslVersion";
            tslVersion.RightToLeft = RightToLeft.No;
            tslVersion.Size = new Size(672, 20);
            tslVersion.Spring = true;
            tslVersion.Text = "Version 0.1.0";
            tslVersion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(882, 503);
            Controls.Add(statusStrip1);
            Controls.Add(grpBadges);
            Controls.Add(grpTrainer);
            Controls.Add(pnlToolbar);
            MinimumSize = new Size(750, 450);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gen1 Save Editor";
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            grpTrainer.ResumeLayout(false);
            grpTrainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMoney).EndInit();
            grpBadges.ResumeLayout(false);
            grpBadges.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlToolbar;
        private Button btnSaveAs;
        private Button btnOpenSave;
        private Label lblCurrentFile;
        private GroupBox grpTrainer;
        private TextBox txtPlayTime;
        private NumericUpDown numMoney;
        private TextBox txtRivalName;
        private TextBox txtPlayerName;
        private ComboBox cmbGameVersion;
        private GroupBox grpBadges;
        private CheckBox chkBadgeEarth;
        private CheckBox chkBadgeVolcano;
        private CheckBox chkBadgeMarsh;
        private CheckBox chkBadgeSoul;
        private CheckBox chkBadgeRainbow;
        private CheckBox chkBadgeThunder;
        private CheckBox chkBadgeCascade;
        private CheckBox chkBadgeBoulder;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tslStatus;
        private ToolStripStatusLabel tslVersion;
    }
}
