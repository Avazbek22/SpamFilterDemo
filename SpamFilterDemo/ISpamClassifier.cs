

namespace SpamFilterDemo
{
	/// <summary>Базовый контракт для любого спам-классификатора.</summary>
	public interface ISpamClassifier
	{
		void Train(TrainingSet trainingSet);
		/// <returns>Вероятность того, что документ является спамом (0..1).</returns>
		double Classify(string document);
	}
}
