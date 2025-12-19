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
            Dictionnaire dico = new Dictionnaire("Mots_Français.txt");
            dico.Tri_QuickSort(); //Tri pour la recherche dichotomique.

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
                int nbLignes;
                int nbColonnes;
                switch (choix)
                {
                    case "1":
                        do
                        {
                            Console.WriteLine("\n--- Configuration du Plateau ---\n(Pour un tableau de taille l*c = 119 maximum.)");

                            // On demande la taille
                            Console.Write("Nombre de lignes: ");
                            nbLignes = 8; // Valeur par défaut
                            int.TryParse(Console.ReadLine(), out nbLignes);

                            Console.Write("Nombre de colonnes : ");
                            nbColonnes = 8; // Valeur par défaut
                            int.TryParse(Console.ReadLine(), out nbColonnes);

                            // On appelle le nouveau constructeur
                            plateau = new Plateau("Lettre.txt", nbLignes, nbColonnes);
                            Console.Clear();
                        } while (((nbLignes * nbColonnes) >= 120)||(nbLignes<=0)||(nbColonnes <= 0));
                        break;
                    case "2":
                        // Depuis fichier
                        Console.Write("Nom du fichier CSV (ex: Test1.csv) : ");
                        string fichier = Console.ReadLine();
                        if (File.Exists(fichier))
                        {
                            plateau = new Plateau(fichier, true);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Fichier introuvable ! Retour au menu...");
                            Console.ResetColor();
                            plateau = null;
                        }
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
                    int nbJoueurs = 0;

                    // On force l'utilisateur à mettre au moins 2 joueurs
                    do
                    {
                        Console.Write("\nCombien de joueurs participent ? (min 2) : ");
                        if (!int.TryParse(Console.ReadLine(), out nbJoueurs) || nbJoueurs < 2)
                        {
                            Console.WriteLine("Erreur : Il faut au moins 2 joueurs (entier valide).");
                            nbJoueurs = 0; // On reset pour relancer la boucle
                        }
                    } while (nbJoueurs < 2);

                    // Boucle pour créer les joueurs
                    for (int i = 1; i <= nbJoueurs; i++)
                    {
                        Console.Write($"Nom du Joueur {i} : ");
                        string nom = Console.ReadLine();
                        joueurs.Add(new Joueur(nom));
                    }

                    // Temps : 5 minutes la partie, 1 minute par tour (par exemple)
                    TimeSpan tempsPartie = TimeSpan.FromMinutes(5);
                    TimeSpan tempsTour = TimeSpan.FromSeconds(60);

                    Jeu jeu = new Jeu(dico, plateau, joueurs, tempsPartie, tempsTour);

                    jeu.LancerPartie();

                    Console.WriteLine();
                    Console.Write("Voulez-vous rejouer une partie ? (O/N) : ");
                    string reponse = Console.ReadLine();


                    if (reponse != null && reponse.ToUpper() == "N")
                    {
                        continuer = false;
                        Console.WriteLine("\nMerci d'avoir joué ! À bientôt.");
                        System.Threading.Thread.Sleep(1500);
                    }
                }
                
            }
            Console.ReadKey();
        }
    }
}  