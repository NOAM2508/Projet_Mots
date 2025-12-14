using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace Mot_Fr
{
    public class Dictionnaire
    {
        private string[] mots;

        public Dictionnaire(string fichier)
        {
            List<string> tempMots = new List<string>()[26];
            for (int i = 0; i < 26; i++)
            {
                mots[i] = new List<string>();
            }

            using (StreamReader sr = new StreamReader(fichier, Encoding.UTF8))
            {
                string ligne;
                int i = 0;
                while ((sr.ReadLine()) != null && i < 26)
                {
                    string[] motsligne = ligne.Split(' ');
                    foreach (string mot in motsligne)
                    {
                        if (mot != motsligne)
                        {
                            this.mots[i].Add(mot.ToUpper());
                        }
                    }
                    i++;
                }
            }

        public string toString()
        {
            return "";
        }
        public bool RechDichoRecursif(string mot)
        {
            return false;
        }
        public void Tri_XXX()
        {

        }
    }
}
