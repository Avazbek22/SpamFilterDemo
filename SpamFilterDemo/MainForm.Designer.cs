// UI/MainForm.Designer.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SpamFilterDemo
{
	partial class MainForm
	{
		private IContainer components = null!;

		private TextBox txtDatasetPath = null!;
		private Button btnBrowse = null!;
		private Button btnTrain = null!;
		private ComboBox cmbAlgorithm = null!;
		private TextBox txtInput = null!;
		private Button btnClassify = null!;
		private Label lblResult = null!;

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components is not null)
				components.Dispose();
			base.Dispose(disposing);
		}

		/// <summary>Метод, требуемый конструктором формы — не изменяйте вручную!</summary>
		private void InitializeComponent()
		{
			components = new Container();
			txtDatasetPath = new TextBox();
			btnBrowse = new Button();
			btnTrain = new Button();
			cmbAlgorithm = new ComboBox();
			txtInput = new TextBox();
			btnClassify = new Button();
			lblResult = new Label();

			SuspendLayout();
			// -----------------------------------------------------------------
			// txtDatasetPath
			// -----------------------------------------------------------------
			txtDatasetPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtDatasetPath.Location = new Point(12, 12);
#if NET8_0_OR_GREATER
			txtDatasetPath.PlaceholderText = "Путь к папке с датасетом (spam / ham)";
#endif
			txtDatasetPath.Size = new Size(420, 27);
			txtDatasetPath.TabIndex = 0;

			// -----------------------------------------------------------------
			// btnBrowse
			// -----------------------------------------------------------------
			btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnBrowse.Location = new Point(438, 11);
			btnBrowse.Size = new Size(94, 29);
			btnBrowse.Text = "Обзор…";
			btnBrowse.TabIndex = 1;

			// -----------------------------------------------------------------
			// btnTrain
			// -----------------------------------------------------------------
			btnTrain.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnTrain.Enabled = false;
			btnTrain.Location = new Point(538, 11);
			btnTrain.Size = new Size(94, 29);
			btnTrain.Text = "Обучить";
			btnTrain.TabIndex = 2;

			// -----------------------------------------------------------------
			// cmbAlgorithm
			// -----------------------------------------------------------------
			cmbAlgorithm.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			cmbAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbAlgorithm.Items.AddRange(new object[] { "Наивный Байес", "Метод Фишера" });
			cmbAlgorithm.Location = new Point(12, 54);
			cmbAlgorithm.Size = new Size(180, 28);
			cmbAlgorithm.SelectedIndex = 0;
			cmbAlgorithm.TabIndex = 3;

			// -----------------------------------------------------------------
			// btnClassify
			// -----------------------------------------------------------------
			btnClassify.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			btnClassify.Enabled = false;
			btnClassify.Location = new Point(198, 52);
			btnClassify.Size = new Size(155, 32);
			btnClassify.Text = "Классифицировать";
			btnClassify.TabIndex = 5;

			// -----------------------------------------------------------------
			// lblResult
			// -----------------------------------------------------------------
			lblResult.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			lblResult.AutoSize = true;
			lblResult.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
			lblResult.Location = new Point(359, 58);
			lblResult.TabIndex = 6;

			// -----------------------------------------------------------------
			// txtInput
			// -----------------------------------------------------------------
			txtInput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			txtInput.Location = new Point(12, 98);
			txtInput.Multiline = true;
#if NET8_0_OR_GREATER
			txtInput.PlaceholderText = "Вставьте текст письма…";
#endif
			txtInput.ScrollBars = ScrollBars.Vertical;
			txtInput.Size = new Size(620, 466);
			txtInput.TabIndex = 4;

			// -----------------------------------------------------------------
			// MainForm
			// -----------------------------------------------------------------
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(644, 576);
			MinimumSize = new Size(500, 400);
			Controls.AddRange(new Control[]
			{
				lblResult, btnClassify, txtInput,
				cmbAlgorithm, btnTrain, btnBrowse, txtDatasetPath
			});
			Name = "MainForm";
			Text = "Фильтрация спама";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
