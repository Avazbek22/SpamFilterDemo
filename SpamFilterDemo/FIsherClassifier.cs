using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamFilterDemo
{
	/// <summary>Классификатор по методу Фишера (похож на подход Грэма в SpamBayes).</summary>
	public class FisherClassifier : ISpamClassifier
	{
		private TrainingSet? _training;
		private const double Epsilon = 1e-9;

		public void Train(TrainingSet trainingSet) => _training = trainingSet;

		public double Classify(string document)
		{
			if (_training is null)
				throw new InvalidOperationException("Классификатор не обучен.");

			var fisherScore = -2.0 *
				Tokenizer.Tokenize(document)
						 .Distinct()
						 .Select(TokenScore)
						 .Where(p => p > 0)
						 .Select(p => Math.Log(p))
						 .Sum();

			// преобразуем статистику χ² в вероятность
			var probability = ChiSquareCdf(fisherScore, 2 * _training.SpamCounts.Count);
			return 1.0 - probability;   // чем выше, тем спам
		}

		private double TokenScore(string token)
		{
			double spamFreq = (double)_training!.GetTokenCount(token, true);
			double hamFreq = (double)_training!.GetTokenCount(token, false);

			if (spamFreq + hamFreq < 1) return 0; // неизвестное слово

			double prob = spamFreq / (_training.SpamDocuments + Epsilon);
			double expected = (spamFreq + hamFreq) / (_training.SpamDocuments + _training.HamDocuments + Epsilon);
			return expected < Epsilon ? 0 : prob / expected;
		}

		/// <summary>Наблюдаемая вероятность для χ²-распределения (upper tail).</summary>
		private static double ChiSquareCdf(double chi, int degreesOfFreedom)
		{
			// Используем неполную гамма-функцию через Series expansion (достаточно для учебных целей)
			double k = degreesOfFreedom / 2.0;
			double term = Math.Exp(-chi / 2);
			double sum = term;
			for (int i = 1; i < k; i++)
			{
				term *= chi / (2 * i);
				sum += term;
			}
			return Math.Min(1, sum);
		}
	}
}
