using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Mot_Fr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chargement du dictionnaire...");
            // Assurez-vous que le fichier MotsFrançais.txt s'appelle bien comme ça
            // Si c'est "MotsFrancais.txt" (sans ç), changez le nom ici !
            Dictionnaire dico = new Dictionnaire("Mots_Français.txt");
            dico.Tri_QuickSort(); // Important : on trie pour la recherche dichotomique

            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== JEU DES MOTS GLISSÉS ===");
                Console.WriteLine("1. Jouer (Plateau Aléatoire)");
                Console.WriteLine("2. Jouer (À partir d'un fichier CSV)");
                Console.WriteLine("3. Quitter");
                Console.Write("\nVotre choix : ");

                string choix = Console.ReadLine();

                Plateau plateau = null;

                switch (choix)
                {
                    case "1":
                        // Génération aléatoire
                        plateau = new Plateau("Lettre.txt");
                        break;
                    case "2":
                        // Depuis fichier
                        Console.Write("Nom du fichier CSV (ex: Test1.csv) : ");
                        string fichier = Console.ReadLine();
                        plateau = new Plateau(fichier, true); // Appel du constructeur de chargement
                        break;
                    case "3":
                        continuer = false;
                        continue;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }

                if (plateau != null)
                {
                    // Configuration de la partie
                    List<Joueur> joueurs = new List<Joueur>();
                    Console.Write("\nNom du Joueur 1 : ");
                    joueurs.Add(new Joueur(Console.ReadLine()));
                    Console.Write("Nom du Joueur 2 : ");
                    joueurs.Add(new Joueur(Console.ReadLine()));

                    // Temps : 5 minutes la partie, 1 minute par tour (par exemple)
                    TimeSpan tempsPartie = TimeSpan.FromMinutes(5);
                    TimeSpan tempsTour = TimeSpan.FromSeconds(60);

                    Jeu jeu = new Jeu(dico, plateau, joueurs, tempsPartie, tempsTour);
                    jeu.LancerPartie();
                }
            }
        }
    }
}