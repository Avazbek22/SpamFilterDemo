using System.Text.RegularExpressions;

namespace SpamFilterDemo
{
	/// <summary>Простейший токенизатор: разбивает текст на слова, фильтрует короткие.</summary>
	public static class Tokenizer
	{
		private static readonly Regex WordRegex = new(@"[А-ЯA-ZЁ]?[а-яa-zё]{2,}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static IEnumerable<string> Tokenize(string text) => WordRegex.Matches(text).Select(m => m.Value.ToLowerInvariant());
	}
}
