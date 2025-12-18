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

        public Dictionnaire(string fichier = "Mots_Français")
        {
            List<string> tempMots = new List<string>();

            using (StreamReader sr = new StreamReader(fichier, Encoding.UTF8))
            {
                string ligne;
                while ((ligne=sr.ReadLine()) != null)
                {
                    string[] motsligne = ligne.ToUpper().Split(' ') ;
                    tempMots.AddRange(motsligne);
                }
                mots = tempMots.ToArray();
            }
        }

        public string toString()
        {
            string res = $"Dictionnaire français\nnombre de mots : {mots.Length}\nPour la lettre ";
            int[] compteur = new int[26];

            foreach (string mot in mots)
            {
                if (mot != null && mot.Length != 0)
                {
                    char premiereLettre = char.ToLower(mot[0]);
                    if (premiereLettre >= 'a' && premiereLettre <= 'z')
                    {
                        compteur[premiereLettre - 'a']++;
                    }
                }
            }
            for (int i = 0; i < 26; i++)
            {
                char lettre = (char)('A' + i);
                res += $"{lettre} : {compteur[i]} mots\n";
            }

            return res;
        }

        public bool RechDichoRecursif(string mot, int debut = 0, int fin = -2)
        {
            if (fin == -2)
            {
                if(mot==null||mot.Length==0)
                {
                    return false;
                }
                fin = mots.Length-1;
            }
            if (fin < debut)
            {
                return false;
            }

            int milieu = (debut + fin) / 2;

            int comparaison = string.Compare(mot, mots[milieu], StringComparison.OrdinalIgnoreCase);

            if (comparaison == 0)
            {
                return true;
            }

            else if (comparaison < 0)
            {
                return RechDichoRecursif(mot, debut, milieu-1);
            }
            else
            {
                return RechDichoRecursif(mot, milieu+1, fin);
            }
        }

        public void Tri_QuickSort()
        {
            if (mots != null && mots.Length > 0)
            {
                QuickSort(mots, 0, mots.Length - 1);
            }
        }

        private void QuickSort(string[] tab, int gauche, int droite)
        {
            if (gauche < droite)
            {
                // --- Début de la logique de Partition intégrée ---
                // On prend le dernier élément comme pivot
                string pivot = tab[droite];
                int i = gauche - 1;

                for (int j = gauche; j < droite; j++)
                {
                    // Si l'élément actuel est plus petit que le pivot
                    if (string.Compare(tab[j], pivot, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        i++;
                        // Échange direct (sans méthode auxiliaire Echanger)
                        string temp1 = tab[i];
                        tab[i] = tab[j];
                        tab[j] = temp1;
                    }
                }

                // On place le pivot à sa position finale (i+1)
                string temp2 = tab[i + 1];
                tab[i + 1] = tab[droite];
                tab[droite] = temp2;

                int pivotIndex = i + 1;
                // --- Fin de la Partition ---

                // Appels récursifs
                QuickSort(tab, gauche, pivotIndex - 1); // Partie gauche
                QuickSort(tab, pivotIndex + 1, droite); // Partie droite
            }
        }
    }
}
