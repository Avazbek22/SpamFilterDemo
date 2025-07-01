
namespace SpamFilterDemo
{
	/// <summary>Хранит подсчитанные частоты слов для спама и хэма.</summary>
	public class TrainingSet
	{
		public Dictionary<string, int> SpamCounts { get; } = new();
		public Dictionary<string, int> HamCounts { get; } = new();

		public int SpamDocuments { get; private set; }
		public int HamDocuments { get; private set; }

		/// <summary>Добавить документ в обучающую выборку.</summary>
		public void AddDocument(string document, bool isSpam)
		{
			var bag = isSpam ? SpamCounts : HamCounts;
			foreach (var token in Tokenizer.Tokenize(document))
				bag[token] = bag.GetValueOrDefault(token) + 1;

			if (isSpam) SpamDocuments++; else HamDocuments++;
		}

		/// <summary>Частота слова среди спама/хэма.</summary>
		internal int GetTokenCount(string token, bool spam) =>
			(spam ? SpamCounts : HamCounts).GetValueOrDefault(token);
	}
}
