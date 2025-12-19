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















        /// <summary>
        /// Vérifie si la partie est terminée ou non.
        /// </summary>
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















        /// <summary>
        /// Méthode gérant le jeu.
        /// </summary>
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
                Joueur joueurCourant = joueurs[tourActuel % joueurs.Count];
                bool tourValide = false;
                DateTime debutTour = DateTime.Now;

                while (!tourValide)
                {
                    Console.Clear();
                    // Infos générales
                    Console.WriteLine("=== JEU DES MOTS GLISSÉS ===");
                    TimeSpan tempsEcoulePartie = DateTime.Now - heureDebutPartie;
                    Console.WriteLine($"Temps partie restant : {(tempsMaxPartie - tempsEcoulePartie):mm\\:ss}");

                    Console.WriteLine("\n---------------------------------");
                    Console.WriteLine($"AU TOUR DE {joueurCourant.Nom.ToUpper()} (Score : {joueurCourant.Score})");
                    Console.WriteLine("Trouvez un mot commençant sur la DERNIÈRE ligne !");
                    Console.WriteLine("---------------------------------\n");

                    // Affichage du plateau
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(plateau.ToString());
                    Console.ResetColor();

                    // Calcul du temps restant pour CE tour spécifiquement
                    TimeSpan tempsEcouleTour = DateTime.Now - debutTour;
                    TimeSpan tempsRestantTour = tempsMaxTour - tempsEcouleTour;

                    // Si le temps est écoulé avant même de taper, on arrête tout de suite
                    if (tempsRestantTour.TotalSeconds <= 0)
                    {
                        Console.WriteLine("\n[TEMPS ÉCOULÉ] Fin du tour !");
                        System.Threading.Thread.Sleep(2000); // Pause pour lire
                        break; // On sort de la boucle de retry, on passe au joueur suivant
                    }

                    // Saisie du mot avec le temps restant mis à jour
                    string mot = LireMotAvecChrono(joueurCourant.Nom, tempsRestantTour);

                    // Si mot est null, c'est que le temps s'est écoulé PENDANT la saisie
                    if (mot == null)
                    {
                        Console.WriteLine("\n\n[TEMPS ÉCOULÉ] Trop tard !");
                        System.Threading.Thread.Sleep(2000);
                        break;
                    }



                    if (TraiterMot(joueurCourant, mot))
                    {
                        tourValide = true;
                        Console.WriteLine("\nAppuyez sur une touche pour passer au joueur suivant...");
                        Console.ReadKey();
                    }
                    else
                    {
                        // Erreur : on laisse le message affiché 2 secondes puis on efface l'écran et on recommence
                        Console.WriteLine("\nEssayez encore !");
                        System.Threading.Thread.Sleep(2000);
                    }

                }
                tourActuel++;
            }
            Console.Clear();
            AfficherResultatsFinaux();
        }
















        /// <summary>
        /// Gère le mot saisi par l'utilisateur.
        /// </summary>
        private bool TraiterMot(Joueur joueur, string mot)
        {
            //A.Vérification de la taille
            if (mot.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERREUR] Le mot doit faire au moins 2 lettres.");
                Console.ResetColor();
                return false;
            }

            // B. Vérification si déjà trouvé
            if (joueur.Contient(mot))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERREUR] Vous avez déjà trouvé le mot '{mot}' !");
                Console.ResetColor();
                return false;
            }

            // C. Vérification Dictionnaire
            if (!dictionnaire.RechDichoRecursif(mot))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERREUR] Le mot '{mot}' n'existe pas dans le dictionnaire.");
                Console.ResetColor();
                return false;
            }

            // D. Vérification Plateau (Recherche chemin)
            object chemin = plateau.Recherche_Mot(mot);
            if (chemin == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERREUR] Le mot '{mot}' est introuvable sur le plateau (ou ne commence pas en bas).");
                Console.ResetColor();
                return false;
            }

            // SI TOUT EST BON :
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[BRAVO] Mot '{mot}' VALIDE !");
            Console.ResetColor();

            // 1. Mise à jour du plateau (lettres tombent)
            plateau.Maj_Plateau(chemin);

            // 2. Ajout du mot au joueur
            joueur.Add_Mot(mot);

            // 3. Calcul et ajout du score
            int points = plateau.CalculerScore(mot);
            joueur.Add_Score(points);
            Console.WriteLine($"Scoring : +{points} points !");
            return true;
        }
















        /// <summary>
        /// Affiche les scores finaux et le gagnant.
        /// </summary>
        private void AfficherResultatsFinaux()
        {
            Console.WriteLine("\n==================================");
            Console.WriteLine("    RÉSULTATS FINAUX DU JEU    ");
            Console.WriteLine("==================================");

            // 1. On trie les joueurs par score décroissant (du plus grand au plus petit)
            // La méthode Sort utilise une fonction lambda pour comparer les scores
            joueurs.Sort((x, y) => y.Score.CompareTo(x.Score));

            // 2. On affiche le classement
            int rang = 1;
            foreach (Joueur joueur in joueurs)
            {
                Console.WriteLine($"{rang}. {joueur.toString()}");
                rang++;
            }

            Console.WriteLine("----------------------------------");

            // 3. Gestion du vainqueur et des égalités
            int meilleurScore = joueurs[0].Score;

            // On vérifie s'il y a une égalité pour la première place
            // On compte combien de joueurs ont le même score que le premier
            int nbGagnants = joueurs.Count(j => j.Score == meilleurScore);

            if (nbGagnants > 1)
            {
                Console.WriteLine("\nIl y a une ÉGALITÉ entre les premiers joueurs !");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nVAINQUEUR : {joueurs[0].Nom.ToUpper()} avec {meilleurScore} points !");
                Console.ResetColor();
            }

            Console.WriteLine("\nAppuyez sur une touche pour quitter la partie...");
            Console.ReadKey();
        }
















        /// <summary>
        /// Récupère le mot saisi par l'utilisateur et affiche le temps de tour restant.
        /// </summary>
        private string LireMotAvecChrono(string nomJoueur, TimeSpan dureeTour)
        {
            string input = "";
            DateTime debutTour = DateTime.Now;
            bool tourFini = false;

            while (!tourFini)
            {
                // 1. Calcul du temps
                TimeSpan tempsEcoule = DateTime.Now - debutTour;
                TimeSpan restant = dureeTour - tempsEcoule;

                if (restant.TotalSeconds <= 0) return null;

                // 2. Affichage complet de la ligne avec \r (Retour début de ligne)
                Console.Write("\r");

                // A. Le Chrono
                if (restant.TotalSeconds <= 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write($"[{restant:ss}s] ");
                Console.ResetColor();

                // B. Le Joueur et sa saisie
                Console.Write($"{nomJoueur}, proposez un mot : {input}");

                // C. Nettoyage et repositionnement du curseur
                Console.Write("   ");     // Espaces pour effacer les lettres supprimées
                Console.Write("\b\b\b");  // On recule pour écrire à la suite du mot

                // 3. Gestion de la saisie
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true = ne pas afficher la touche automatiquement

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        tourFini = true;
                        Console.WriteLine(); // On valide visuellement par un saut de ligne
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        // Si on appuie sur effacer et que le mot n'est pas vide
                        if (input.Length > 0)
                        {
                            // On garde tout sauf le dernier caractère (Substring)
                            input = input.Substring(0, input.Length - 1);
                        }
                    }
                    // Si c'est une lettre ou un chiffre (pas une touche spéciale comme F1, Echap...)
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        // On ajoute la lettre à la fin (Concaténation)
                        input += keyInfo.KeyChar.ToString().ToUpper();
                    }
                }

                // Petite pause
                System.Threading.Thread.Sleep(5);
            }

            return input;
        }
    }

}
