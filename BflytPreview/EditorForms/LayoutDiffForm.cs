﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SARCExt;
using SwitchThemes.Common;

namespace BflytPreview.EditorForms
{
	public partial class LayoutDiffForm : Form
	{
		public LayoutDiffForm(SarcData _Original = null, SarcData _Edited = null)
		{
			InitializeComponent();
			if (_Original != null)
			{
				textBox1.Text = "<From file>";
				ClearOriginal = false;
				button1.Enabled = false;
				textBox1.Enabled = false;
				Original = _Original;
			}
			if (_Edited != null)
			{
				textBox2.Text = "<From file>";
				ClearEdited = false;
				button2.Enabled = false;
				textBox2.Enabled = false;
				Edited = _Edited;
			}
		}

		bool ClearOriginal = true;
		SarcData Original = null;
		bool ClearEdited = true;
		SarcData Edited = null;

		private void button1_Click(object sender, EventArgs e)
			=> SelectFile(ref textBox1);

		private void button2_Click(object sender, EventArgs e)
			=> SelectFile(ref textBox2);

		void SelectFile(ref TextBox target)
		{
			OpenFileDialog opn = new OpenFileDialog() { Filter = "szs files|*.szs" };
			if (opn.ShowDialog() != DialogResult.OK) return;
			target.Text = opn.FileName;
		}

		private void LayoutDiffForm_Load(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (Original == null)
				Original = SARCExt.SARC.UnpackRamN(ManagedYaz0.Decompress(File.ReadAllBytes(textBox1.Text)));
			if (Edited == null)
				Edited = SARCExt.SARC.UnpackRamN(ManagedYaz0.Decompress(File.ReadAllBytes(textBox2.Text)));
			try
			{
				var (res,msg) = SwitchThemes.Common.LayoutDiff.Diff(Original, Edited);
				if (msg != null)
					MessageBox.Show(msg);
				if (res != null)
				{
					SaveFileDialog sav = new SaveFileDialog() { Filter = "json file|*.json" };
					if (sav.ShowDialog() != DialogResult.OK) return;
					File.WriteAllText(sav.FileName, res.AsJson());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			if (ClearEdited)
				Edited = null;
			if (ClearOriginal)
				Original = null;
		}
	}
}
