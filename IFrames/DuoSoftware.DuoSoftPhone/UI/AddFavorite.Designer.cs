//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace WowserBrowser
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	public partial class AddFavorite : System.Windows.Forms.Form
	{
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.AddSingleGroupBox = new System.Windows.Forms.GroupBox();
            this.SingleURLTextBox = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.SingleTitleTextBox = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.AddGroupGroupBox = new System.Windows.Forms.GroupBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.NewGroupTextBox = new System.Windows.Forms.TextBox();
            this.GroupFavoriteComboBox = new System.Windows.Forms.ComboBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.GroupURLTextBox = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.GroupTitleTextBox = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.AddSingleRadioButton = new System.Windows.Forms.RadioButton();
            this.AddGroupRadioButton = new System.Windows.Forms.RadioButton();
            this.TableLayoutPanel1.SuspendLayout();
            this.AddSingleGroupBox.SuspendLayout();
            this.AddGroupGroupBox.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.ColumnCount = 2;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Controls.Add(this.OK_Button, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.Cancel_Button, 1, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(277, 263);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 1;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(146, 29);
            this.TableLayoutPanel1.TabIndex = 0;
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK_Button.Location = new System.Drawing.Point(3, 3);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(67, 23);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "OK";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(76, 3);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(67, 23);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // AddSingleGroupBox
            // 
            this.AddSingleGroupBox.Controls.Add(this.SingleURLTextBox);
            this.AddSingleGroupBox.Controls.Add(this.Label2);
            this.AddSingleGroupBox.Controls.Add(this.SingleTitleTextBox);
            this.AddSingleGroupBox.Controls.Add(this.Label1);
            this.AddSingleGroupBox.Location = new System.Drawing.Point(32, 12);
            this.AddSingleGroupBox.Name = "AddSingleGroupBox";
            this.AddSingleGroupBox.Size = new System.Drawing.Size(391, 84);
            this.AddSingleGroupBox.TabIndex = 1;
            this.AddSingleGroupBox.TabStop = false;
            this.AddSingleGroupBox.Text = "Add Single Favorite";
            // 
            // SingleURLTextBox
            // 
            this.SingleURLTextBox.Location = new System.Drawing.Point(40, 48);
            this.SingleURLTextBox.Name = "SingleURLTextBox";
            this.SingleURLTextBox.Size = new System.Drawing.Size(345, 20);
            this.SingleURLTextBox.TabIndex = 3;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(7, 51);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(29, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "URL";
            // 
            // SingleTitleTextBox
            // 
            this.SingleTitleTextBox.Location = new System.Drawing.Point(41, 20);
            this.SingleTitleTextBox.Name = "SingleTitleTextBox";
            this.SingleTitleTextBox.Size = new System.Drawing.Size(344, 20);
            this.SingleTitleTextBox.TabIndex = 1;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(8, 23);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(27, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Title";
            // 
            // AddGroupGroupBox
            // 
            this.AddGroupGroupBox.Controls.Add(this.GroupBox3);
            this.AddGroupGroupBox.Controls.Add(this.GroupFavoriteComboBox);
            this.AddGroupGroupBox.Controls.Add(this.Label5);
            this.AddGroupGroupBox.Controls.Add(this.GroupURLTextBox);
            this.AddGroupGroupBox.Controls.Add(this.Label3);
            this.AddGroupGroupBox.Controls.Add(this.GroupTitleTextBox);
            this.AddGroupGroupBox.Controls.Add(this.Label4);
            this.AddGroupGroupBox.Enabled = false;
            this.AddGroupGroupBox.Location = new System.Drawing.Point(32, 102);
            this.AddGroupGroupBox.Name = "AddGroupGroupBox";
            this.AddGroupGroupBox.Size = new System.Drawing.Size(391, 153);
            this.AddGroupGroupBox.TabIndex = 2;
            this.AddGroupGroupBox.TabStop = false;
            this.AddGroupGroupBox.Text = "Add to Favorite Group";
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.AddButton);
            this.GroupBox3.Controls.Add(this.NewGroupTextBox);
            this.GroupBox3.Location = new System.Drawing.Point(6, 102);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(379, 45);
            this.GroupBox3.TabIndex = 6;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "New Favorite Group";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(298, 17);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 20);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // NewGroupTextBox
            // 
            this.NewGroupTextBox.Location = new System.Drawing.Point(7, 17);
            this.NewGroupTextBox.Name = "NewGroupTextBox";
            this.NewGroupTextBox.Size = new System.Drawing.Size(285, 20);
            this.NewGroupTextBox.TabIndex = 0;
            // 
            // GroupFavoriteComboBox
            // 
            this.GroupFavoriteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GroupFavoriteComboBox.FormattingEnabled = true;
            this.GroupFavoriteComboBox.Location = new System.Drawing.Point(92, 75);
            this.GroupFavoriteComboBox.Name = "GroupFavoriteComboBox";
            this.GroupFavoriteComboBox.Size = new System.Drawing.Size(293, 21);
            this.GroupFavoriteComboBox.TabIndex = 5;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(7, 78);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(77, 13);
            this.Label5.TabIndex = 4;
            this.Label5.Text = "Favorite Group";
            // 
            // GroupURLTextBox
            // 
            this.GroupURLTextBox.Location = new System.Drawing.Point(40, 48);
            this.GroupURLTextBox.Name = "GroupURLTextBox";
            this.GroupURLTextBox.Size = new System.Drawing.Size(345, 20);
            this.GroupURLTextBox.TabIndex = 3;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(7, 51);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(29, 13);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "URL";
            // 
            // GroupTitleTextBox
            // 
            this.GroupTitleTextBox.Location = new System.Drawing.Point(41, 20);
            this.GroupTitleTextBox.Name = "GroupTitleTextBox";
            this.GroupTitleTextBox.Size = new System.Drawing.Size(344, 20);
            this.GroupTitleTextBox.TabIndex = 1;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(8, 23);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(27, 13);
            this.Label4.TabIndex = 0;
            this.Label4.Text = "Title";
            // 
            // AddSingleRadioButton
            // 
            this.AddSingleRadioButton.AutoSize = true;
            this.AddSingleRadioButton.Checked = true;
            this.AddSingleRadioButton.Location = new System.Drawing.Point(12, 13);
            this.AddSingleRadioButton.Name = "AddSingleRadioButton";
            this.AddSingleRadioButton.Size = new System.Drawing.Size(14, 13);
            this.AddSingleRadioButton.TabIndex = 3;
            this.AddSingleRadioButton.TabStop = true;
            this.AddSingleRadioButton.UseVisualStyleBackColor = true;
            this.AddSingleRadioButton.CheckedChanged += new System.EventHandler(this.AddSingleRadioButton_CheckedChanged);
            // 
            // AddGroupRadioButton
            // 
            this.AddGroupRadioButton.AutoSize = true;
            this.AddGroupRadioButton.Location = new System.Drawing.Point(12, 102);
            this.AddGroupRadioButton.Name = "AddGroupRadioButton";
            this.AddGroupRadioButton.Size = new System.Drawing.Size(14, 13);
            this.AddGroupRadioButton.TabIndex = 4;
            this.AddGroupRadioButton.UseVisualStyleBackColor = true;
            this.AddGroupRadioButton.CheckedChanged += new System.EventHandler(this.AddGroupRadioButton_CheckedChanged);
            // 
            // AddFavorite
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(435, 304);
            this.Controls.Add(this.AddGroupRadioButton);
            this.Controls.Add(this.AddSingleRadioButton);
            this.Controls.Add(this.AddGroupGroupBox);
            this.Controls.Add(this.AddSingleGroupBox);
            this.Controls.Add(this.TableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddFavorite";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Favorite";
            this.TableLayoutPanel1.ResumeLayout(false);
            this.AddSingleGroupBox.ResumeLayout(false);
            this.AddSingleGroupBox.PerformLayout();
            this.AddGroupGroupBox.ResumeLayout(false);
            this.AddGroupGroupBox.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
		internal System.Windows.Forms.Button OK_Button;
		internal System.Windows.Forms.Button Cancel_Button;
		internal System.Windows.Forms.GroupBox AddSingleGroupBox;
		internal System.Windows.Forms.TextBox SingleURLTextBox;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox SingleTitleTextBox;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.GroupBox AddGroupGroupBox;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.TextBox NewGroupTextBox;
		internal System.Windows.Forms.ComboBox GroupFavoriteComboBox;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.TextBox GroupURLTextBox;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.TextBox GroupTitleTextBox;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.Button AddButton;
		internal System.Windows.Forms.RadioButton AddSingleRadioButton;
		internal System.Windows.Forms.RadioButton AddGroupRadioButton;

	}

}