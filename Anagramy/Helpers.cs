using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagramy
{
    public static class Helpers
    {
        public static string ConvertNumberToDiacritics(string word)
        {
            string result = "";
            for (int i = 0; i < word.Length; ++i)
            {
                switch (word[i])
                {
                    case '1':
                        result += 'Ą';
                        break;
                    case '2':
                        result += 'Ć';
                        break;
                    case '3':
                        result += 'Ę';
                        break;
                    case '4':
                        result += 'Ł';
                        break;
                    case '5':
                        result += 'Ń';
                        break;
                    case '6':
                        result += 'Ó';
                        break;
                    case '7':
                        result += 'Ś';
                        break;
                    case '8':
                        result += 'Ź';
                        break;
                    case '9':
                        result += 'Ż';
                        break;
                    default:
                        result += word[i];
                        break;
                }
            }
            return result;
        }

        public static string ConvertDiacriticsToNumber(string word)
        {
            string result = "";
            for (int i = 0; i < word.Length; ++i)
            {
                switch (word[i])
                {
                    case 'Ą':
                        result += '1';
                        break;
                    case 'Ć':
                        result += '2';
                        break;
                    case 'Ę':
                        result += '3';
                        break;
                    case 'Ł':
                        result += '4';
                        break;
                    case 'Ń':
                        result += '5';
                        break;
                    case 'Ó':
                        result += '6';
                        break;
                    case 'Ś':
                        result += '7';
                        break;
                    case 'Ź':
                        result += '8';
                        break;
                    case 'Ż':
                        result += '9';
                        break;
                    default:
                        result += word[i];
                        break;
                }
            }
            return result;
        }
    }
}
