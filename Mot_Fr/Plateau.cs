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
        public void ToFile(string nom_fichier)
        {
            using (StreamWriter sw = new StreamWriter(nom_fichier))
            {
                for (int i = 0; i < lignes; i++)
                {
                    string ligne = "";
                    for (int j = 0; j < colonnes; j++)
                    {
                        ligne += matrice[i, j];
                        if (j < colonnes - 1)
                        {
                            ligne += ";";
                        }
                        sw.WriteLine(ligne);
                    }
                }
            }
            Console.WriteLine($"Plateau sauvegardé avec succès dans {nom_fichier}");
        }
        public void ToRead(string nom_fichier)
        {
            if(!File.Exists(nom_fichier))
            {
                Console.WriteLine($"Fichier {nom_fichier} introuvable.");
            }

            using(StreamReader sr = new StreamReader(nom_fichier))
            {
                string ligne;
                int i = 0;
                while((ligne = sr.ReadLine()) != null && i< lignes)
                {
                    string[] lettres = ligne.Split(';');

                    for(int j = 0;j < colonnes;j++)
                    {
                        if (lettres[j].Length > 0)
                        {
                            matrice[i, j] = lettres[j][0];
                        }
                        else
                        {
                            matrice[i, j] = ' ';
                        }
                    }
                    i++;
                }
                Console.WriteLine("Plateau chargé avec succès");
            }
        }
        public object Recherche_Mot(string mot)
        {
            if(mot == null || mot.Length == 0)
            {
                return null;
            }

            mot = mot.Trim();
            mot = mot.ToUpper();

            int ligne_base = lignes - 1;

            for (int j = 0; j < colonnes; j++)
            {
                if (matrice[ligne_base, j] == mot[0])
                {
                    List<int[]> chemin = new List<int[]>();
                    bool[,] visite = new bool[lignes, colonnes]; 

                    if(RechercheRecursive(mot, 0, ligne_base, j, chemin, visite))
                    {
                        return chemin;
                    }
                }
            }
            return null;
        }
        private bool RechercheRecursive(string mot, int indexLettre, int l, int c, List<int[]> chemin, bool[,] visite)
        {
            if (l<0 || l>= colonnes || c<0 || c>= lignes || visite[l,c])
            {
                return false;
            }

            if (matrice[l,c] != mot[indexLettre])
            {
                return false;
            }

            chemin.Add(new int[] { l, c });
            visite[l, c] = true;

            if(indexLettre == mot.Length-1)
            {
                return true;
            }

            int[] dL = {-1, -1, -1, 0, 0};
            int[] dC = {0, -1, 1, -1, 1};

            for (int i = 0; i < dL.Length; i++)
            {
                if (RechercheRecursive(mot, indexLettre + 1, l + dL[i], c + dC[i], chemin, visite))
                {
                    return true;
                }
            }
            visite[l,c] = false;
            chemin.RemoveAt(chemin.Count-1);
            return false;
        }

        public void Maj_Plateau(object objet)
        {
            if(objet == null)
            {
                return;
            }

            List<int[]> chemin = (List<int[]>)objet;
            foreach (int[] coordonnées in chemin)
            {
                matrice[coordonnées[0], coordonnées[1]] = ' ';
            }

            for (int j =0; j < colonnes; j++)
            {
                List<char> reste_colonne = new List<char>();

                for (int i = 0;i < lignes;i++)
                {
                    if(matrice[i,j] != ' ')
                    {
                        reste_colonne.Add(matrice[i,j]);
                    }
                }

                int indexLettre = reste_colonne.Count - 1;

                for (int i = lignes-1; i >= 0;i--)
                {
                    if (indexLettre >= 0)
                    {
                        matrice[i, j] = reste_colonne[indexLettre];
                        indexLettre--;
                    }
                    else
                    {
                        matrice[i, j] = ' ';
                    }
                }
            }
        }
    }
}
