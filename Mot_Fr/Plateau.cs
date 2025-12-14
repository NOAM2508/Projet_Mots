using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mot_Fr
{
    public class Plateau
    {
        private char[,] matrice;
        private const int lignes = 8;
        private const int colonnes = 8;

        public Plateau(string fichierLettres) 
        {
            matrice = new char[lignes,colonnes];

            List<char> sacLettres = ChargerLettres(fichierLettres);

            Random ran = new Random();

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if(sacLettres.Count > 0)
                    {
                        int ind = ran.Next(sacLettres.Count);
                        matrice[i, j] = sacLettres[ind];
                        sacLettres.RemoveAt(ind);
                    }
                    else
                    {
                        matrice[i, j] = '?';
                    }
                }
            }
        }

        public Plateau(string nom_fichier, bool sauvegarde)
        {
            matrice = new char[lignes, colonnes];
            ToRead(nom_fichier);
        }

        private List<char> ChargerLettres(string nom_fichier)
        {
            List<char> lettres = new List<char>();

            using (StreamReader sr = new StreamReader(nom_fichier))
            {
                string ligne;
                while ((ligne = sr.ReadLine()) != null)
                {
                    string[] partition = ligne.Split(',');

                    if (partition.Length == 3 )
                    {
                        char lettre = partition[0][0];
                        int qte = int.Parse(partition[1]);

                        for (int i = 0; i < qte; i++)
                        {
                            lettres.Add(lettre);
                        }
                    }
                }
            }
            return lettres;
        }




        public override string ToString()
        {
            string affichage = "   ";
            string separation = "   ";
            for (int j = 0; j < colonnes; j++)
            {
                affichage += " "+ j;
                separation += "--";
            }
            affichage += "\n"+separation +"\n";

            for (int i = 0; i< lignes; i++)
            {
                affichage+= i + " |";
                for (int j = 0; j < colonnes; j++)
                {
                    affichage += " " + matrice[i, j];
                }
                affichage += "\n";
            }


            return affichage;
        }
        public void ToFile(string nomfile)
        {

        }
        public void ToRead(string nomfile)
        {

        }
        public object Recherche_Mot(string mot)
        {
            return null;
        }
        public void Maj_Plateau(object objet)
        {

        }
    }
}
