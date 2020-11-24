using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anagramy
{
    class AnagramTree
    {
        public AnagramTree[] Nodes;
        public List<string> Words;

        public AnagramTree()
        {
            Words = new List<string>();
            Nodes = new AnagramTree[32];
        }

        public void Load(string s, Dictionary<char, int> Alphabet)
        {
            s = s.ToUpper();
            string sorted = String.Concat(s.OrderBy(c => Alphabet[c]));
            AnagramTree p = this;
            int length = s.Length;
            for (int i = 0; i < length - 1; i++)
            {
                if (p.Nodes[Alphabet[sorted[i]]] == null)
                {
                    p.Nodes[Alphabet[sorted[i]]] = new AnagramTree();
                }
                p = p.Nodes[Alphabet[sorted[i]]];
                if (p.Nodes == null) p.Nodes = new AnagramTree[32];
            }

            if (p.Nodes[Alphabet[sorted[length - 1]]] == null)
            {
                p.Nodes[Alphabet[sorted[length - 1]]] = new AnagramTree();
                p.Nodes[Alphabet[sorted[length - 1]]].Words.Add(s);
            }
            else
            {
                p.Nodes[Alphabet[sorted[length - 1]]].Words.Add(s);
            }
        }

        public void Search(AnagramTree at, string s, Dictionary<char, int> Alphabet, ListBox lb)
        {
            s = s.ToUpper();
            string sorted = String.Concat(s.OrderBy(c => Alphabet[c]));
            AnagramTree p = at;
            int length = sorted.Length;
            int i;
            for (i = 0; i < length; i++)
            {
                if (sorted[i] == '.')
                {
                    string newString;
                    foreach (var e in Alphabet)
                    {
                        //if (p.Nodes == null || p.Nodes[e.Value] == null) continue;
                        newString = "";
                        newString += e.Key;
                        newString += sorted.Substring(i + 1);
                        newString = String.Concat(newString.OrderBy(c => c));
                        Search(p, newString, Alphabet, lb);
                    }
                    return;
                }
                /*else if (sorted[i] == '@')
                {
                    string rest = "", news = "";
                    while (news.Length <= remainder - i)
                    {
                        news = rest + sorted.Substring(i + 1);
                        news = String.Concat(news.OrderBy(c => c));
                        Search(p, news, Alphabet, lb, remainder - i);
                        rest += ".";
                    }
                    return;
                }*/
                else if (Alphabet.ContainsKey(sorted[i]) == false) return;
                if (p.Nodes != null && p.Nodes[Alphabet[sorted[i]]] != null) p = p.Nodes[Alphabet[sorted[i]]];
                else return;
            }

            if (p != null && p.Words != null)
            {
                foreach (string w in p.Words)
                {
                    if (lb.Items.Contains(Helpers.ConvertNumberToDiacritics(w)) == false) lb.Items.Add(Helpers.ConvertNumberToDiacritics(w));
                }
            }
        }

        public string FindAnagramsWithBlank(string s, Dictionary<char, int> Alphabet)
        {
            AnagramTree p = this;
            s = s.ToUpper();
            string letters = "";
            string newString;
            int length = s.Length + 1;

            foreach (var c in Alphabet.Keys)
            {
                p = this;
                newString = c + s;
                newString = String.Concat(newString.OrderBy(x => x));
                for (int i = 0; i < length; i++)
                {
                    if (p != null && p.Nodes != null && p.Nodes[Alphabet[newString[i]]] != null) p = p.Nodes[Alphabet[newString[i]]];
                    else break;
                }
                if (p != null && p.Words != null && p.Words.Count > 0 && p.Words[0].Length == length) letters += c;
            }
            return letters;
        }

        public int FindNumberOfAnagrams(string s, Dictionary<char, int> Alphabet)
        {
            AnagramTree p = this;
            string newString = "";
            int length = s.Length;
            s = s.ToUpper();
            newString = s;
            newString = String.Concat(newString.OrderBy(x => x));
            for (int i = 0; i < length; i++)
            {
                if (p != null && p.Nodes != null && p.Nodes[Alphabet[newString[i]]] != null) p = p.Nodes[Alphabet[newString[i]]];
                else break;
            }
            if (p != null && p.Words != null && p.Words.Count > 0 && p.Words[0].Length == length) return p.Words.Count;
            else return -1;
        }
    }
}
