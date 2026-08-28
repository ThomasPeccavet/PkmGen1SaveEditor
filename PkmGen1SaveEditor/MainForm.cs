namespace PkmGen1SaveEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenSave_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new()
            {
                Title = "Ouvrir une sauvegarde Pokémon",
                Filter = "Sauvegardes Game Boy (*.sav)|*.sav|Tous les fichiers (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            FileInfo saveFile = new(dialog.FileName);

            lblCurrentFile.Text = saveFile.Name;
            tslStatus.Text = $"Fichier chargé — {saveFile.Length:N0} octets";

            btnSaveAs.Enabled = true;

            cmbGameVersion.Enabled = true;
            txtPlayerName.Enabled = true;
            txtRivalName.Enabled = true;
            numMoney.Enabled = true;
            txtPlayTime.Enabled = true;

            cmbGameVersion.SelectedIndex = 0;
        }
    }
}
