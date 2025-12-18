using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mot_Fr
{
    public class Jeu
    {
        private Dictionnaire dictionnaire;
        private Plateau plateau;
        private List<Joueur> joueurs;

        private TimeSpan tempsMaxPartie;
        private TimeSpan tempsMaxTour;
        private DateTime heureDebutPartie;

        public Jeu(Dictionnaire dictionnaire, Plateau plateau, List<Joueur> joueurs, TimeSpan tempsMaxPartie, TimeSpan tempsMaxTour)
        {
            this.dictionnaire = dictionnaire;
            this.plateau = plateau;
            this.joueurs = joueurs;
            this.tempsMaxPartie = tempsMaxPartie;
            this.tempsMaxTour = tempsMaxTour;
        }

        public bool EstTermine()
        {
            TimeSpan TempsEcoulePartie = DateTime.Now - heureDebutPartie;
            if (TempsEcoulePartie >= tempsMaxPartie)
            {
                Console.WriteLine("Le temps total est écoulé !\n");
                Console.WriteLine("Appuyez sur une touche pour voir les résultats");
                Console.ReadKey();
                return true;
            }

            // méthode EstVide() utilisé dans la classe Plateau
            if (plateau.Estvide())
            {
                Console.WriteLine("\nFIN DE PARTIE : Le plateau est vide !");
                return true;
            }

            return false;
        }


        private string LireMotJoueur(string nom)
        {
            Console.Write($"\n{nom}, proposez un mot : ");
            return Console.ReadLine().ToUpper();
        }

        public void LancerPartie()
        {
            heureDebutPartie = DateTime.Now;
            int tourActuel = 0;
            Console.Clear();
            Console.WriteLine(" DÉBUT DE LA PARTIE ");
            Console.WriteLine($"Durée maximale de la partie : {tempsMaxPartie.TotalMinutes} minutes.");
            Console.WriteLine($"Temps maximum par tour : {tempsMaxTour.TotalSeconds} secondes.");
            Console.WriteLine($"\n---------------------------------\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(plateau.ToString());
            Console.ResetColor();
            
            // On affiche le plateau au début

            // TANT QUE le jeu n'est pas fini
            while (!EstTermine()) 
            {
                // On détermine c'est à qui de jouer
                Joueur joueurCourant = joueurs[tourActuel % joueurs.Count]; //Donne soit 0 ou 1 
                Console.WriteLine($"\n---------------------------------\n");
                Console.WriteLine($"AU TOUR DE {joueurCourant.Nom.ToUpper()} (Score actuel : {joueurCourant.Score})");
                Console.WriteLine($"Trouvez un mot commençant sur la DERNIÈRE ligne !");

                DateTime debutTour = DateTime.Now;                

                string mot = LireMotAvecChrono(joueurCourant.Nom, tempsMaxTour);

                DateTime finTour = DateTime.Now;

                if (EstTermine())
                {
                    break;
                }
                Console.Clear();
                TimeSpan dureeTour = finTour - debutTour;
                if (dureeTour > tempsMaxTour)
                {
                    Console.WriteLine($"\n[TEMPS ÉCOULÉ] Trop tard ! Vous avez dépassé {tempsMaxTour.TotalSeconds}s.");
                }
                else if (string.IsNullOrWhiteSpace(mot))
                {
                    Console.WriteLine("\n[PAS DE MOT] Vous avez passé votre tour.");
                }
                else
                {
                    TraiterMot(joueurCourant, mot);
                }
                Console.WriteLine($"\n---------------------------------\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(plateau.ToString());
                Console.ResetColor();

                tourActuel++;
            }
            Console.Clear();
            AfficherResultatsFinaux();
        }
        private void TraiterMot(Joueur joueur, string mot)
        {
            Console.Clear();
            //A.Vérification de la taille
            if (mot.Length < 2)
            {
                Console.WriteLine("[ERREUR] Le mot doit faire au moins 2 lettres.");
                return;
            }

            // B. Vérification si déjà trouvé
            if (joueur.Contient(mot))
            {
                Console.WriteLine($"[ERREUR] Vous avez déjà trouvé le mot '{mot}' !");
                return;
            }

            // C. Vérification Dictionnaire
            if (!dictionnaire.RechDichoRecursif(mot))
            {
                Console.WriteLine($"[ERREUR] Le mot '{mot}' n'existe pas dans le dictionnaire.");
                return;
            }

            // D. Vérification Plateau (Recherche chemin)
            object chemin = plateau.Recherche_Mot(mot);
            if (chemin == null)
            {
                Console.WriteLine($"[ERREUR] Le mot '{mot}' est introuvable sur le plateau (ou ne commence pas en bas).");
                return;
            }

            // SI TOUT EST BON :
            Console.WriteLine($"\n[BRAVO] Mot '{mot}' VALIDE !");

            // 1. Mise à jour du plateau (lettres tombent)
            plateau.Maj_Plateau(chemin);

            // 2. Ajout du mot au joueur
            joueur.Add_Mot(mot);

            // 3. Calcul et ajout du score
            int points = plateau.CalculerScore(mot);
            joueur.Add_Score(points);
            Console.WriteLine($"Scoring : +{points} points !");
        }

        private void AfficherResultatsFinaux()
        {
            Console.WriteLine("\n==================================");
            Console.WriteLine("    RÉSULTATS FINAUX DU JEU    ");
            Console.WriteLine("==================================");

            // On affiche les stats de chaque joueur
            foreach (Joueur joueur in joueurs)
            {
                Console.WriteLine(joueur.toString()); 
            }

            // Trouver le vainqueur
            //Si joueur 0 a un score > à joueur 1 alors c'est le vainqueur 
            if (joueurs[0].Score > joueurs[1].Score)
            {
                Console.WriteLine($"VAINQUEUR : {joueurs[0].Nom} !");
            }
            //Si joueur 1 a un score > à joueur 0 alors c'est le vainqueur 
            else if (joueurs[1].Score > joueurs[0].Score)
            {
                Console.WriteLine($"VAINQUEUR : {joueurs[1].Nom} !");
            }
            //Sinon égalité
            else
            {
                Console.WriteLine("Égalité !");
            }

            Console.WriteLine("\nAppuyez sur une touche pour quitter la partie...");
            Console.ReadKey();
        }
        // À ajouter dans Jeu.cs

        private string LireMotAvecChrono(string nomJoueur, TimeSpan dureeTour)
        {
            StringBuilder input = new StringBuilder();
            DateTime debutTour = DateTime.Now;
            bool tourFini = false;

            Console.Write($"\n{nomJoueur}, proposez un mot : ");

            // On sauvegarde la position du curseur là où le joueur écrit
            int curseurX = Console.CursorLeft;
            int curseurY = Console.CursorTop;

            while (!tourFini)
            {
                // 1. Calcul du temps restant
                TimeSpan tempsEcoule = DateTime.Now - debutTour;
                TimeSpan restant = dureeTour - tempsEcoule;

                if (restant.TotalSeconds <= 0)
                {
                    return null; // Le temps est écoulé !
                }

                // 2. Affichage du chrono (en haut à droite par exemple, ou juste au dessus)
                // On sauvegarde la position actuelle du curseur
                int currentLeft = Console.CursorLeft;
                int currentTop = Console.CursorTop;

                // On va écrire le temps restant en haut à droite (Ligne 0, à droite)
                int posX = Console.WindowWidth - 20;
                if (posX < 0) posX = 0;
                Console.SetCursorPosition(posX, 0);

                Console.ForegroundColor = restant.TotalSeconds <= 10 ? ConsoleColor.Red : ConsoleColor.Green;
                Console.Write($"Temps: {restant:mm\\:ss}");
                Console.ResetColor();

                // On remet le curseur à sa place pour que le joueur continue d'écrire
                Console.SetCursorPosition(currentLeft, currentTop);

                // 3. Gestion de la saisie sans bloquer (KeyAvailable)
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true = ne pas afficher la lettre automatiquement

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        tourFini = true;
                        Console.WriteLine(); // Saut de ligne pour valider visuellement
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (input.Length > 0)
                        {
                            input.Remove(input.Length - 1, 1);
                            // Effacer le caractère à l'écran
                            Console.Write("\b \b");
                        }
                    }
                    else if (!char.IsControl(keyInfo.KeyChar)) // Si c'est une lettre ou un chiffre
                    {
                        input.Append(keyInfo.KeyChar);
                        Console.Write(keyInfo.KeyChar.ToString().ToUpper()); // On affiche en majuscule
                    }
                }

                // Petite pause pour ne pas surcharger le processeur
                System.Threading.Thread.Sleep(50);
            }

            return input.ToString().ToUpper();
        }
    }

}
