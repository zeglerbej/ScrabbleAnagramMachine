using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anagramy
{
    public partial class Form1 : Form
    {
        Dictionary<char, int> Alphabet = new Dictionary<char, int>();
        AnagramTree ATree = new AnagramTree();
        OpenFileDialog ofd = new OpenFileDialog();
        string fullDictionary;
        string list;
        string newList;
        int wordCount = 0;
        int solved = 0;
        string wordWithNumbersAsDiacritics;
        public Form1()
        {
            InitializeComponent();
            InitializeAlphabet();
            radioButton1.Checked = true;
            radioButton4.Checked = true;
            listBox1.Sorted = true;

            textBox1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Wybierz plik z całym OSPS.", "Wybór słownika", MessageBoxButtons.OK);
            ofd.ShowDialog();
            if (ofd.FileName == null || ofd.FileName == "") return;
            fullDictionary = ofd.FileName;
            
            int count = 0;
            StreamReader sr = new StreamReader(fullDictionary, Encoding.ASCII);
            using (sr)
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    s = s.Trim();
                    ATree.Load(s, Alphabet);
                }
            }
            ofd.FileName = "";
            result = MessageBox.Show("Wybierz plik z listą anagramów.", "Wybór listy", MessageBoxButtons.OK);
            ofd.ShowDialog();
            if (ofd.FileName == null || ofd.FileName == "") return;
            list = ofd.FileName;
            sr = new StreamReader(list, Encoding.ASCII);
            using (sr)
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null) ++count;           
            }

            button2.Enabled = true;
            wordCount = count;
            ChooseWord();
            button1.Enabled = false;
            ActiveControl = button2;          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0) return;
            ATree.Search(ATree, wordWithNumbersAsDiacritics, Alphabet, listBox1);
            button3.Enabled = true;
            button2.Enabled = false;
            button4.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (wordCount == 0)
            {
                label4.Text = "Liczba słów: ";
                return;
            }
            ATree.Search(ATree, wordWithNumbersAsDiacritics, Alphabet, listBox1);

            //string[] lines = File.ReadAllLines(list);
            List<string> lines = new List<string>();
            List<string> newLines = new List<string>();
            int j = 0;
            StreamReader sr = new StreamReader(list, Encoding.ASCII);
            string s = String.Empty;
            using (sr)
            {
                while ((s = sr.ReadLine()) != null)
                {
                    lines.Add(s);
                }
            }
            for (int i=0;i<lines.Count;i++)
            {
                //if(j==listBox1.Items.Count)
                //{
                //    newLines.Add(lines[i]);
                //}
                //else
                //{
                //    if (string.Compare(lines[i], listBox1.Items[j] as string) == 0)
                //    {
                //        --wordCount;
                //        ++j;
                //    }
                //    else
                //    {
                //        newLines.Add(lines[i]);
                //    }
                //}

                bool add = true;
                foreach(string str in listBox1.Items)
                {
                    if(string.Compare(lines[i], Helpers.ConvertDiacriticsToNumber(str)) == 0)
                    {
                        --wordCount;
                        add = false;
                        break;
                    }
                }
                if(add == true) newLines.Add(lines[i]);
            }
            ++solved;
            listBox1.Items.Clear();
            //File.WriteAllLines(list, newLines);
            StreamWriter sw = new StreamWriter(list, false, Encoding.ASCII);
            //using (sw) sw.WriteLine("");
            //sw = new StreamWriter(list, true, );
            using (sw)
            {
                foreach (var line in newLines) sw.WriteLine(line);
            }
            label1.Text = "Pozostałe słowa: " + wordCount;
            label2.Text = "Rozwiązane: " + solved;
            button3.Enabled = false;
            button2.Enabled = true;
            button4.Enabled = false;

            ActiveControl = button2;
            ChooseWord();
        }

        private void ChooseWord()
        {
            if (wordCount == 0)
            {
                textBox1.Text = "";
                return;
            }
            label1.Text = "Pozostałe słowa: " + wordCount;
            Random rnd = new Random(DateTime.Now.Millisecond * wordCount);
            int chosenWord = rnd.Next(wordCount);
            StreamReader sr = new StreamReader(list, Encoding.ASCII);
            wordWithNumbersAsDiacritics = "";
            using (sr)
            {
                if (new FileInfo(list).Length == 0) return;
                for (int i = 0; i < chosenWord - 1; ++i) sr.ReadLine();
                wordWithNumbersAsDiacritics = sr.ReadLine();
                if (radioButton1.Checked == true)
                {
                    for (int i = 0; i < wordWithNumbersAsDiacritics.Length; i++)
                    {
                        int j = rnd.Next(wordWithNumbersAsDiacritics.Length);
                        char[] tmpWord = wordWithNumbersAsDiacritics.ToCharArray();
                        char tmp = tmpWord[i];
                        tmpWord[i] = tmpWord[j];
                        tmpWord[j] = tmp;
                        wordWithNumbersAsDiacritics = new string(tmpWord);
                    }
                }
                else
                {
                    for(int i =0; i< wordWithNumbersAsDiacritics.Length; ++i)
                    {
                        char c = wordWithNumbersAsDiacritics[i];

                    }
                    string sorted = String.Concat(wordWithNumbersAsDiacritics.OrderBy(c => Alphabet[c]));
                    wordWithNumbersAsDiacritics = sorted;
                }
                textBox1.Text = Helpers.ConvertNumberToDiacritics(wordWithNumbersAsDiacritics);
            }
            ATree.Search(ATree, wordWithNumbersAsDiacritics, Alphabet, listBox1);
            int count = listBox1.Items.Count;
            label4.Text = "Liczba słów: " + count.ToString();
            listBox1.Items.Clear();
        }

       

        private void InitializeAlphabet()
        {
            Alphabet.Add('A', 0);
            Alphabet.Add('1', 1);
            Alphabet.Add('B', 2);
            Alphabet.Add('C', 3);
            Alphabet.Add('2', 4);

            Alphabet.Add('D', 5);
            Alphabet.Add('E', 6);
            Alphabet.Add('3', 7);
            Alphabet.Add('F', 8);
            Alphabet.Add('G', 9);

            Alphabet.Add('H', 10);
            Alphabet.Add('I', 11);
            Alphabet.Add('J', 12);
            Alphabet.Add('K', 13);
            Alphabet.Add('L', 14);

            Alphabet.Add('4', 15);
            Alphabet.Add('M', 16);
            Alphabet.Add('N', 17);
            Alphabet.Add('5', 18);
            Alphabet.Add('O', 19);

            Alphabet.Add('6', 20);
            Alphabet.Add('P', 21);
            Alphabet.Add('R', 22);
            Alphabet.Add('S', 23);
            Alphabet.Add('7', 24);

            Alphabet.Add('T', 25);
            Alphabet.Add('U', 26);
            Alphabet.Add('W', 27);
            Alphabet.Add('Y', 28);
            Alphabet.Add('Z', 29);

            Alphabet.Add('8', 30);
            Alphabet.Add('9', 31);
        }

        private void button4_Click(object sender, EventArgs e)
        {           
            button4.Enabled = false;
            ActiveControl = button3;
            if (newList == null || newList == "") return;
            //StreamWriter sw = File.AppendText(newList);
            StreamWriter sw = new StreamWriter(newList, true);
            if (radioButton4.Checked == true)
            {
                foreach (string it in listBox1.Items)
                {
                    sw.WriteLine(it);
                }
            }
            else
            {
                sw.WriteLine(textBox1.Text);
            }
            sw.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            if (ofd.FileName == null || ofd.FileName == "") return;
            newList = ofd.FileName;
            label3.Text = "Lista do zapisu anagramów: " + newList;
        }
    }
}
