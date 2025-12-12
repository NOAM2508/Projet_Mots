using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mot_Fr
{
    public class Dictionnaire
    {
        private string[] mots;

        public Dictionnaire(string fichier)
        {
            List<string> tempMots = new List<string>();

            using (StreamReader sr = new StreamReader(fichier, Encoding.UTF8))
            {
                string ligne = sr.ReadLine();
                while (ligne != null)
                {
                    string[] motsligne = ligne.Split(' ');
                    tempMots.AddRange(motsligne);
                }
                mots = tempMots.ToArray();
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
