using System.Diagnostics;

namespace SpamFilterDemo
{
	public partial class MainForm : Form
	{
		// ЧЧЧ данные и две модели, обучаютс€ один раз ЧЧЧ
		private readonly TrainingSet _trainingSet = new();
		private readonly NaiveBayesClassifier _bayes = new();
		private readonly FisherClassifier _fisher = new();

		// секундомер + таймер дл€ Ђбегущегої заголовка
		private readonly Stopwatch _sw = new();
		private readonly System.Windows.Forms.Timer _uiTimer = new() { Interval = 500 };

		public MainForm()
		{
			InitializeComponent();

			btnBrowse.Click += OnBrowseClick;
			btnTrain.Click += OnTrainClick;
			btnClassify.Click += OnClassifyClick;

			// таймер каждую пол-секунды обновл€ет заголовок, пока идЄт обучение
			_uiTimer.Tick += (_, _) => Text = $"ќбучение: {_sw.Elapsed.TotalSeconds:N1} с";
		}

		// ЧЧЧ выбор папки ЧЧЧ
		private void OnBrowseClick(object? sender, EventArgs e)
		{
			using var dlg = new FolderBrowserDialog
			{
				Description = "¬ыберите папку, содержащую подкаталоги spam и ham"
			};
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				txtDatasetPath.Text = dlg.SelectedPath;
				btnTrain.Enabled = true;
			}
		}

		// ЧЧЧ ќЅ”„≈Ќ»≈ (загрузка датасета + две модели параллельно) ЧЧЧ
		private async void OnTrainClick(object? sender, EventArgs e)
		{
			// базовые проверки пути
			var root = txtDatasetPath.Text.Trim();
			if (!Directory.Exists(root) ||
				!Directory.Exists(Path.Combine(root, "spam")) ||
				!Directory.Exists(Path.Combine(root, "ham")))
			{
				MessageBox.Show("Ќекорректна€ папка: внутри должны быть подкаталоги spam и ham.",
								"¬нимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				ToggleUi(false);            // блокируем кнопки
				_sw.Restart();
				_uiTimer.Start();

				// 1) читаем все письма (I/O) без блокировки UI-потока
				await Task.Run(() => LoadDataset(root));

				// 2) обучаем две модели параллельно
				var trainBayes = Task.Run(() => _bayes.Train(_trainingSet));
				var trainFisher = Task.Run(() => _fisher.Train(_trainingSet));
				await Task.WhenAll(trainBayes, trainFisher);

				_sw.Stop();
				_uiTimer.Stop();
				Text = $"‘ильтраци€ спама Ч ќбучение зан€ло {_sw.Elapsed.TotalSeconds:N1} с";

				btnClassify.Enabled = true;
			}
			catch (Exception ex)
			{
				_uiTimer.Stop();
				MessageBox.Show($"ќшибка обучени€:\n{ex.Message}",
								"ќшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				ToggleUi(true);
				UseWaitCursor = false;
			}
		}

		// ЧЧЧ чтение датасета (словарь обнул€ем перед каждым обучением) ЧЧЧ
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

		// ЧЧЧ текуща€ выбранна€ модель ЧЧЧ
		private ISpamClassifier Current =>
			cmbAlgorithm.SelectedIndex == 0 ? _bayes : _fisher;

		// ЧЧЧ  Ћј——»‘» ј÷»я ЧЧЧ
		private void OnClassifyClick(object? sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtInput.Text))
			{
				MessageBox.Show("¬ставьте текст сообщени€ дл€ проверки.",
								"Ќет текста", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try
			{
				var probability = Current.Classify(txtInput.Text);
				lblResult.Text = probability >= 0.5
					? $"—ѕјћ ({probability:P1})"
					: $"’эм ({1 - probability:P1})";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"ќшибка классификации:\n{ex.Message}",
								"ќшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ЧЧЧ включить / выключить элементы управлени€ ЧЧЧ
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
