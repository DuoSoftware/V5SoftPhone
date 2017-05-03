using DuoSoftware.DuoSoftPhone.UI;

//INSTANT C# NOTE: Formerly VB project-level imports:
using System.Windows.Forms;

namespace WowserBrowser
{
    public partial class AddFavorite
    {
        internal AddFavorite()
        {
            InitializeComponent();
        }

        public FormDialPad myMainForm;

        public AddFavorite(FormDialPad frm)
            : base()
        {
            this.InitializeComponent();
            myMainForm = frm;
        }

        private void OK_Button_Click(object sender, System.EventArgs e)
        {
            if (this.AddGroupRadioButton.Checked)
            {
                if (this.GroupFavoriteComboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Pease select a group and try again or use Add Single Favorite.", "No Group Specified", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void AddSingleRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (AddSingleRadioButton.Checked)
            {
                SingleTitleTextBox.Text = GroupTitleTextBox.Text;
                SingleURLTextBox.Text = GroupURLTextBox.Text;
                GroupTitleTextBox.Text = string.Empty;
                GroupURLTextBox.Text = string.Empty;
            }
            AddSingleGroupBox.Enabled = AddSingleRadioButton.Checked;
            AddGroupGroupBox.Enabled = !AddSingleRadioButton.Checked;
        }

        private void AddGroupRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (AddGroupRadioButton.Checked)
            {
                GroupTitleTextBox.Text = SingleTitleTextBox.Text;
                GroupURLTextBox.Text = SingleURLTextBox.Text;
                SingleTitleTextBox.Text = string.Empty;
                SingleURLTextBox.Text = string.Empty;
            }
            AddGroupGroupBox.Enabled = AddGroupRadioButton.Checked;
            AddSingleGroupBox.Enabled = !AddGroupRadioButton.Checked;
        }

        private void AddButton_Click(object sender, System.EventArgs e)
        {
            if (NewGroupTextBox.Text.Length > 0)
            {
                if (!myMainForm.FavoritesTreeView.Nodes.ContainsKey(NewGroupTextBox.Text))
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = NewGroupTextBox.Text;
                    tn.Text = NewGroupTextBox.Text;
                    tn.Tag = "#GROUP#";
                    myMainForm.FavoritesTreeView.Nodes.Add(tn);
                    this.GroupFavoriteComboBox.Items.Add(NewGroupTextBox.Text);
                }
            }
        }
    }
}