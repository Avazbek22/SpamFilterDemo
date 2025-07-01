using System.Diagnostics;

namespace SpamFilterDemo
{
	public partial class MainForm : Form
	{
		// ��� ������ � ��� ������, ��������� ���� ��� ���
		private readonly TrainingSet _trainingSet = new();
		private readonly NaiveBayesClassifier _bayes = new();
		private readonly FisherClassifier _fisher = new();

		// ���������� + ������ ��� ��������� ���������
		private readonly Stopwatch _sw = new();
		private readonly System.Windows.Forms.Timer _uiTimer = new() { Interval = 500 };

		public MainForm()
		{
			InitializeComponent();

			btnBrowse.Click += OnBrowseClick;
			btnTrain.Click += OnTrainClick;
			btnClassify.Click += OnClassifyClick;

			// ������ ������ ���-������� ��������� ���������, ���� ��� ��������
			_uiTimer.Tick += (_, _) => Text = $"��������: {_sw.Elapsed.TotalSeconds:N1} �";
		}

		// ��� ����� ����� ���
		private void OnBrowseClick(object? sender, EventArgs e)
		{
			using var dlg = new FolderBrowserDialog
			{
				Description = "�������� �����, ���������� ����������� spam � ham"
			};
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				txtDatasetPath.Text = dlg.SelectedPath;
				btnTrain.Enabled = true;
			}
		}

		// ��� �������� (�������� �������� + ��� ������ �����������) ���
		private async void OnTrainClick(object? sender, EventArgs e)
		{
			// ������� �������� ����
			var root = txtDatasetPath.Text.Trim();
			if (!Directory.Exists(root) ||
				!Directory.Exists(Path.Combine(root, "spam")) ||
				!Directory.Exists(Path.Combine(root, "ham")))
			{
				MessageBox.Show("������������ �����: ������ ������ ���� ����������� spam � ham.",
								"��������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				ToggleUi(false);            // ��������� ������
				_sw.Restart();
				_uiTimer.Start();

				// 1) ������ ��� ������ (I/O) ��� ���������� UI-������
				await Task.Run(() => LoadDataset(root));

				// 2) ������� ��� ������ �����������
				var trainBayes = Task.Run(() => _bayes.Train(_trainingSet));
				var trainFisher = Task.Run(() => _fisher.Train(_trainingSet));
				await Task.WhenAll(trainBayes, trainFisher);

				_sw.Stop();
				_uiTimer.Stop();
				Text = $"���������� ����� � �������� ������ {_sw.Elapsed.TotalSeconds:N1} �";

				btnClassify.Enabled = true;
			}
			catch (Exception ex)
			{
				_uiTimer.Stop();
				MessageBox.Show($"������ ��������:\n{ex.Message}",
								"������", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				ToggleUi(true);
				UseWaitCursor = false;
			}
		}

		// ��� ������ �������� (������� �������� ����� ������ ���������) ���
		private void LoadDataset(string root)
		{
			_trainingSet.SpamCounts.Clear();
			_trainingSet.HamCounts.Clear();

			foreach (var path in Directory.EnumerateFiles(Path.Combine(root, "spam"), "*.txt",
														  SearchOption.TopDirectoryOnly))
				_trainingSet.AddDocument(File.ReadAllText(path), isSpam: true);

			foreach (var path in Directory.EnumerateFiles(Path.Combine(root, "ham"), "*.txt",
														  SearchOption.TopDirectoryOnly))
				_trainingSet.AddDocument(File.ReadAllText(path), isSpam: false);
		}

		// ��� ������� ��������� ������ ���
		private ISpamClassifier Current =>
			cmbAlgorithm.SelectedIndex == 0 ? _bayes : _fisher;

		// ��� ������������� ���
		private void OnClassifyClick(object? sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtInput.Text))
			{
				MessageBox.Show("�������� ����� ��������� ��� ��������.",
								"��� ������", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try
			{
				var probability = Current.Classify(txtInput.Text);
				lblResult.Text = probability >= 0.5
					? $"���� ({probability:P1})"
					: $"��� ({1 - probability:P1})";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"������ �������������:\n{ex.Message}",
								"������", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ��� �������� / ��������� �������� ���������� ���
		private void ToggleUi(bool enabled)
		{
			UseWaitCursor = !enabled;
			btnBrowse.Enabled = enabled;
			btnTrain.Enabled = enabled;
			btnClassify.Enabled = enabled && _trainingSet.SpamDocuments + _trainingSet.HamDocuments > 0;
			cmbAlgorithm.Enabled = enabled;
		}
	}
}
