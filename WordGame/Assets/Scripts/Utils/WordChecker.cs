using System.Collections.Generic;

namespace Utils
{
    public class WordChecker
    {
        private Dictionary<string, bool> _dictionary;

        public WordChecker(Dictionary<string, bool> dictionary)
        {
            this._dictionary = dictionary;
        }

        public bool IsWordValid(string word)
        {
            word = word.ToLower();
            return word.Length >= 2 && _dictionary.ContainsKey(word);
        }
    }
}