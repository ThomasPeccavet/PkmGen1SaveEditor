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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            pnlToolbar = new Panel();
            lblCurrentFile = new Label();
            btnSaveAs = new Button();
            btnOpenSave = new Button();
            statusStrip1 = new StatusStrip();
            tslStatus = new ToolStripStatusLabel();
            tslVersion = new ToolStripStatusLabel();
            grpTrainer = new PokemonGroupBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtPlayTime = new TextBox();
            numMoney = new NumericUpDown();
            txtRivalName = new TextBox();
            txtPlayerName = new TextBox();
            cmbGameVersion = new ComboBox();
            grpBadges = new PokemonGroupBox();
            chkBadgeEarth = new CheckBox();
            chkBadgeVolcano = new CheckBox();
            chkBadgeMarsh = new CheckBox();
            chkBadgeSoul = new CheckBox();
            chkBadgeRainbow = new CheckBox();
            chkBadgeThunder = new CheckBox();
            chkBadgeCascade = new CheckBox();
            chkBadgeBoulder = new CheckBox();
            btnViewParty = new Button();
            pnlToolbar.SuspendLayout();
            statusStrip1.SuspendLayout();
            grpTrainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMoney).BeginInit();
            grpBadges.SuspendLayout();
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
            btnSaveAs.Click += btnSaveAs_Click;
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
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { tslStatus, tslVersion });
            statusStrip1.Location = new Point(0, 430);
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
            // grpTrainer
            // 
            grpTrainer.BackColor = Color.FromArgb(232, 239, 199);
            grpTrainer.Controls.Add(label5);
            grpTrainer.Controls.Add(label4);
            grpTrainer.Controls.Add(label3);
            grpTrainer.Controls.Add(label2);
            grpTrainer.Controls.Add(label1);
            grpTrainer.Controls.Add(txtPlayTime);
            grpTrainer.Controls.Add(numMoney);
            grpTrainer.Controls.Add(txtRivalName);
            grpTrainer.Controls.Add(txtPlayerName);
            grpTrainer.Controls.Add(cmbGameVersion);
            grpTrainer.ForeColor = Color.FromArgb(26, 39, 27);
            grpTrainer.Location = new Point(0, 66);
            grpTrainer.Name = "grpTrainer";
            grpTrainer.Padding = new Padding(42, 48, 42, 42);
            grpTrainer.Size = new Size(416, 293);
            grpTrainer.TabIndex = 4;
            grpTrainer.TabStop = false;
            grpTrainer.Text = "Informations du dresseur";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(38, 229);
            label5.Name = "label5";
            label5.Size = new Size(104, 20);
            label5.TabIndex = 19;
            label5.Text = "Temps de jeu :";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Location = new Point(81, 186);
            label4.Name = "label4";
            label4.Size = new Size(61, 20);
            label4.TabIndex = 18;
            label4.Text = "Argent :";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(94, 132);
            label3.Name = "label3";
            label3.Size = new Size(48, 20);
            label3.TabIndex = 17;
            label3.Text = "Rival :";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(93, 87);
            label2.Name = "label2";
            label2.Size = new Size(49, 20);
            label2.TabIndex = 16;
            label2.Text = "Nom :";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(105, 43);
            label1.Name = "label1";
            label1.Size = new Size(37, 20);
            label1.TabIndex = 15;
            label1.Text = "Jeu :";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtPlayTime
            // 
            txtPlayTime.Enabled = false;
            txtPlayTime.Location = new Point(169, 226);
            txtPlayTime.Name = "txtPlayTime";
            txtPlayTime.Size = new Size(209, 27);
            txtPlayTime.TabIndex = 14;
            // 
            // numMoney
            // 
            numMoney.Enabled = false;
            numMoney.Location = new Point(170, 183);
            numMoney.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numMoney.Name = "numMoney";
            numMoney.Size = new Size(208, 27);
            numMoney.TabIndex = 13;
            numMoney.ThousandsSeparator = true;
            // 
            // txtRivalName
            // 
            txtRivalName.Enabled = false;
            txtRivalName.Location = new Point(169, 132);
            txtRivalName.Name = "txtRivalName";
            txtRivalName.Size = new Size(209, 27);
            txtRivalName.TabIndex = 12;
            // 
            // txtPlayerName
            // 
            txtPlayerName.Enabled = false;
            txtPlayerName.Location = new Point(169, 84);
            txtPlayerName.Name = "txtPlayerName";
            txtPlayerName.Size = new Size(209, 27);
            txtPlayerName.TabIndex = 11;
            // 
            // cmbGameVersion
            // 
            cmbGameVersion.Enabled = false;
            cmbGameVersion.FormattingEnabled = true;
            cmbGameVersion.Items.AddRange(new object[] { "Pokémon Rouge / Bleu — Français" });
            cmbGameVersion.Location = new Point(169, 40);
            cmbGameVersion.Name = "cmbGameVersion";
            cmbGameVersion.Size = new Size(209, 28);
            cmbGameVersion.TabIndex = 10;
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
            grpBadges.ForeColor = Color.FromArgb(26, 39, 27);
            grpBadges.Location = new Point(439, 66);
            grpBadges.Name = "grpBadges";
            grpBadges.Padding = new Padding(42, 48, 42, 42);
            grpBadges.Size = new Size(431, 293);
            grpBadges.TabIndex = 5;
            grpBadges.TabStop = false;
            grpBadges.Text = "Badges";
            // 
            // chkBadgeEarth
            // 
            chkBadgeEarth.AutoSize = true;
            chkBadgeEarth.Location = new Point(233, 174);
            chkBadgeEarth.Name = "chkBadgeEarth";
            chkBadgeEarth.Size = new Size(64, 24);
            chkBadgeEarth.TabIndex = 15;
            chkBadgeEarth.Text = "Terre";
            chkBadgeEarth.UseVisualStyleBackColor = true;
            // 
            // chkBadgeVolcano
            // 
            chkBadgeVolcano.AutoSize = true;
            chkBadgeVolcano.Location = new Point(233, 144);
            chkBadgeVolcano.Name = "chkBadgeVolcano";
            chkBadgeVolcano.Size = new Size(75, 24);
            chkBadgeVolcano.TabIndex = 14;
            chkBadgeVolcano.Text = "Volcan";
            chkBadgeVolcano.UseVisualStyleBackColor = true;
            // 
            // chkBadgeMarsh
            // 
            chkBadgeMarsh.AutoSize = true;
            chkBadgeMarsh.Location = new Point(233, 114);
            chkBadgeMarsh.Name = "chkBadgeMarsh";
            chkBadgeMarsh.Size = new Size(75, 24);
            chkBadgeMarsh.TabIndex = 13;
            chkBadgeMarsh.Text = "Marais";
            chkBadgeMarsh.UseVisualStyleBackColor = true;
            // 
            // chkBadgeSoul
            // 
            chkBadgeSoul.AutoSize = true;
            chkBadgeSoul.Location = new Point(233, 84);
            chkBadgeSoul.Name = "chkBadgeSoul";
            chkBadgeSoul.Size = new Size(62, 24);
            chkBadgeSoul.TabIndex = 12;
            chkBadgeSoul.Text = "Âme";
            chkBadgeSoul.UseVisualStyleBackColor = true;
            // 
            // chkBadgeRainbow
            // 
            chkBadgeRainbow.AutoSize = true;
            chkBadgeRainbow.Location = new Point(123, 174);
            chkBadgeRainbow.Name = "chkBadgeRainbow";
            chkBadgeRainbow.Size = new Size(75, 24);
            chkBadgeRainbow.TabIndex = 11;
            chkBadgeRainbow.Text = "Prisme";
            chkBadgeRainbow.UseVisualStyleBackColor = true;
            // 
            // chkBadgeThunder
            // 
            chkBadgeThunder.AutoSize = true;
            chkBadgeThunder.Location = new Point(123, 144);
            chkBadgeThunder.Name = "chkBadgeThunder";
            chkBadgeThunder.Size = new Size(77, 24);
            chkBadgeThunder.TabIndex = 10;
            chkBadgeThunder.Text = "Foudre";
            chkBadgeThunder.UseVisualStyleBackColor = true;
            // 
            // chkBadgeCascade
            // 
            chkBadgeCascade.AutoSize = true;
            chkBadgeCascade.Location = new Point(123, 114);
            chkBadgeCascade.Name = "chkBadgeCascade";
            chkBadgeCascade.Size = new Size(86, 24);
            chkBadgeCascade.TabIndex = 9;
            chkBadgeCascade.Text = "Cascade";
            chkBadgeCascade.UseVisualStyleBackColor = true;
            // 
            // chkBadgeBoulder
            // 
            chkBadgeBoulder.AutoSize = true;
            chkBadgeBoulder.Location = new Point(123, 84);
            chkBadgeBoulder.Name = "chkBadgeBoulder";
            chkBadgeBoulder.Size = new Size(72, 24);
            chkBadgeBoulder.TabIndex = 8;
            chkBadgeBoulder.Text = "Roche";
            chkBadgeBoulder.UseVisualStyleBackColor = true;
            // 
            // btnViewParty
            // 
            btnViewParty.Enabled = false;
            btnViewParty.Location = new Point(23, 379);
            btnViewParty.Name = "btnViewParty";
            btnViewParty.Size = new Size(221, 29);
            btnViewParty.TabIndex = 6;
            btnViewParty.Text = "Voir l'équipe";
            btnViewParty.UseVisualStyleBackColor = true;
            btnViewParty.Click += btnViewParty_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(882, 456);
            Controls.Add(btnViewParty);
            Controls.Add(grpBadges);
            Controls.Add(grpTrainer);
            Controls.Add(statusStrip1);
            Controls.Add(pnlToolbar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(750, 450);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gen1 Save Editor";
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            grpTrainer.ResumeLayout(false);
            grpTrainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMoney).EndInit();
            grpBadges.ResumeLayout(false);
            grpBadges.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlToolbar;
        private Button btnSaveAs;
        private Button btnOpenSave;
        private Label lblCurrentFile;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tslStatus;
        private ToolStripStatusLabel tslVersion;
        private PokemonGroupBox grpTrainer;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtPlayTime;
        private NumericUpDown numMoney;
        private TextBox txtRivalName;
        private TextBox txtPlayerName;
        private ComboBox cmbGameVersion;
        private PokemonGroupBox grpBadges;
        private CheckBox chkBadgeEarth;
        private CheckBox chkBadgeVolcano;
        private CheckBox chkBadgeMarsh;
        private CheckBox chkBadgeSoul;
        private CheckBox chkBadgeRainbow;
        private CheckBox chkBadgeThunder;
        private CheckBox chkBadgeCascade;
        private CheckBox chkBadgeBoulder;
        private Button btnViewParty;
    }
}
