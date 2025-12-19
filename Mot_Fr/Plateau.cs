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
        private int lignes = 8;
        private int colonnes = 8;
        public static Dictionary<char, int> poidsLettres = new Dictionary<char, int>();





        public Plateau(string fichierLettres, int lignes, int colonnes) 
        {
            this.lignes = lignes;
            this.colonnes = colonnes;

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
            poidsLettres = new Dictionary<char, int>();
            ChargerLettres(nom_fichier);
            ToRead(nom_fichier);
        }









        /// <summary>
        /// Calcule le score d'un mot en fonction du poids des lettres et de sa longueur.
        /// </summary>
        public int CalculerScore(string mot)
        {
            if (string.IsNullOrEmpty(mot)) return 0;

            int sommePoids = 0;
            mot = mot.ToUpper();

            foreach (char lettre in mot)
            {
                if (poidsLettres.ContainsKey(lettre))
                {
                    if (poidsLettres[lettre] > 0)
                    {
                        sommePoids += poidsLettres[lettre];
                    }
                    else
                    {
                        sommePoids += 1;
                    }
                }
                else
                {
                    sommePoids += 1;
                }
            }
            return sommePoids * mot.Length;
        }









        /// <summary>
        /// Récupère les informations liées à chaque lettre (Quantité,Poids).
        /// </summary>
        private List<char> ChargerLettres(string nom_fichier)
        {
            List<char> lettres = new List<char>();
            poidsLettres.Clear(); // On vide les poids avant de recharger

            if (File.Exists(nom_fichier))
            {
                using (StreamReader sr = new StreamReader(nom_fichier))
                {
                    string ligne;
                    while ((ligne = sr.ReadLine()) != null)
                    {
                        string[] partition = ligne.Split(',');

                        
                        if (partition.Length >= 3 && partition[0].Length > 0)
                        {
                            char lettre = partition[0][0];

                            if (int.TryParse(partition[1], out int qte) && int.TryParse(partition[2], out int poids)) // On vérifie la validité du contenu.
                            {
                                if (!poidsLettres.ContainsKey(lettre))
                                {
                                    poidsLettres.Add(lettre, poids);
                                }
                                for (int i = 0; i < qte; i++)
                                {
                                    lettres.Add(lettre);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Erreur dans le chargement des lettres.");
            }
            return lettres;
        }













        /// <summary>
        /// Affichage du plateau.
        /// </summary>
        public override string ToString()
        {
            string affichage = "  |";
            string separation = "----";
            for (int j = 0; j < colonnes; j++)
            {
                if(j<10)
                {
                    affichage += "  " + j;
                }
                else
                {
                    affichage += " " + j;
                }
                separation += "---";
            }
            affichage += "\n"+separation +"\n";

            for (int i = 0; i< lignes; i++)
            {
                if (i < 10)
                {
                    affichage += i + " |";
                }
                else
                {
                    affichage += i + "|";
                }
                for (int j = 0; j < colonnes; j++)
                {
                    affichage += "  " + matrice[i, j];
                }
                affichage += "\n";
            }


            return affichage;
        }














        /// <summary>
        /// Sauvegarde le plateau dans un fichier.
        /// </summary>
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

















        /// <summary>
        /// Récupère un plateau à partir d'un fichier.
        /// </summary>
        public void ToRead(string nom_fichier)
        {
            if(!File.Exists(nom_fichier))
            {
                Console.WriteLine($"Fichier {nom_fichier} introuvable.");
                return;
            }

            string[] ligne = File.ReadAllLines(nom_fichier);

            if (ligne.Length > 0)
            {
                this.lignes = ligne.Length;
                this.colonnes = ligne[0].Split(';').Length;

                matrice = new char[lignes, colonnes];

                for (int i = 0; i < lignes; i++)
                {
                    string[] lettres = ligne[i].Split(',');
                    for (int j = 0; j < colonnes; j++)
                    {
                        if (j < lettres.Length && lettres[j].Length > 0)
                            matrice[i, j] = lettres[j][0];
                        else
                            matrice[i, j] = ' ';
                    }
                }
                Console.WriteLine($"Plateau chargé avec succès ({lignes}x{colonnes})");
            }
        }













        /// <summary>
        /// Recherche le mot saisi dans le plateau.
        /// </summary>
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












        /// <summary>
        /// Méthode récursive de recherche du mot.
        /// </summary>
        /// <param name="indexLettre">Indice de la lettre dans le mot</param>
        /// <param name="l">Ligne</param>
        /// <param name="c">Colonne</param>
        /// <param name="chemin">Liste d'indices des cases visitées</param>
        /// <param name="visite">Si la case a été visitée ou non</param>

        private bool RechercheRecursive(string mot, int indexLettre, int l, int c, List<int[]> chemin, bool[,] visite)
        {
            if (l<0 || l>= lignes || c<0 || c>= colonnes || visite[l,c])
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














        /// <summary>
        /// Permet de faire glisser verticalement les lettres du plateau.
        /// </summary>
        /// <param name="objet">Liste ordonnée d'indices des lettres eliminées</param>
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
        
















        /// <summary>
        /// Permet de vérifier la présence d'une lettre ou non dans une case.
        /// </summary>
        public bool Estvide()
        {
            for(int i=0; i< lignes; i++)
            {
                for(int j = 0;j < colonnes; j++)
                {
                    if (this.matrice[i,j] != ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
