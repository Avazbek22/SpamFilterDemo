
namespace SpamFilterDemo
{
	/// <summary>Наивный байесовский классификатор с добавлением Лапласа (α=1).</summary>
	public class NaiveBayesClassifier : ISpamClassifier
	{
		private TrainingSet? _training;
		private const double Alpha = 1.0;   // сглаживание

		public void Train(TrainingSet trainingSet) => _training = trainingSet;

		public double Classify(string document)
		{
			if (_training is null)
				throw new InvalidOperationException("Классификатор не обучен.");

			var spamProbLog = 0.0;
			var hamProbLog = 0.0;

			var spamDenominator = _training.SpamCounts.Values.Sum() + Alpha * _training.SpamCounts.Count;
			var hamDenominator = _training.HamCounts.Values.Sum() + Alpha * _training.HamCounts.Count;

			foreach (var token in Tokenizer.Tokenize(document))
			{
				var spamCount = _training.GetTokenCount(token, spam: true) + Alpha;
				var hamCount = _training.GetTokenCount(token, spam: false) + Alpha;

				spamProbLog += Math.Log(spamCount / spamDenominator);
				hamProbLog += Math.Log(hamCount / hamDenominator);
			}

			spamProbLog += Math.Log((double)_training.SpamDocuments / (_training.SpamDocuments + _training.HamDocuments));
			hamProbLog += Math.Log((double)_training.HamDocuments / (_training.SpamDocuments + _training.HamDocuments));

			var odds = Math.Exp(spamProbLog - hamProbLog);
			return odds / (1 + odds);   // 0..1
		}
	}
}
