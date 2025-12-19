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
            

            while (!EstTermine()) 
            {

                Joueur joueurCourant = joueurs[tourActuel % joueurs.Count];
                bool tourValide = false;
                DateTime debutTour = DateTime.Now;

                while (!tourValide)
                {
                    Console.Clear();

                    Console.WriteLine("=== JEU DES MOTS GLISSÉS ===");
                    TimeSpan tempsEcoulePartie = DateTime.Now - heureDebutPartie;
                    Console.WriteLine($"Temps partie restant : {(tempsMaxPartie - tempsEcoulePartie):mm\\:ss}");

                    Console.WriteLine("\n---------------------------------");
                    Console.WriteLine($"AU TOUR DE {joueurCourant.Nom.ToUpper()} (Score : {joueurCourant.Score})");
                    Console.WriteLine("Trouvez un mot commençant sur la DERNIÈRE ligne !");
                    Console.WriteLine("---------------------------------\n");


                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(plateau.ToString());
                    Console.ResetColor();


                    TimeSpan tempsEcouleTour = DateTime.Now - debutTour;
                    TimeSpan tempsRestantTour = tempsMaxTour - tempsEcouleTour;


                    if (tempsRestantTour.TotalSeconds <= 0)
                    {
                        Console.WriteLine("\n[TEMPS ÉCOULÉ] Fin du tour !");
                        System.Threading.Thread.Sleep(2000);
                        break;
                    }


                    string mot = LireMotAvecChrono(joueurCourant.Nom, tempsRestantTour);


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

            if (mot.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERREUR] Le mot doit faire au moins 2 lettres.");
                Console.ResetColor();
                return false;
            }


            if (joueur.Contient(mot))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERREUR] Vous avez déjà trouvé le mot '{mot}' !");
                Console.ResetColor();
                return false;
            }


            if (!dictionnaire.RechDichoRecursif(mot))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERREUR] Le mot '{mot}' n'existe pas dans le dictionnaire.");
                Console.ResetColor();
                return false;
            }


            object chemin = plateau.Recherche_Mot(mot);
            if (chemin == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERREUR] Le mot '{mot}' est introuvable sur le plateau (ou ne commence pas en bas).");
                Console.ResetColor();
                return false;
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[BRAVO] Mot '{mot}' VALIDE !");
            Console.ResetColor();


            plateau.Maj_Plateau(chemin);


            joueur.Add_Mot(mot);


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


            joueurs.Sort((x, y) => y.Score.CompareTo(x.Score));


            int rang = 1;
            foreach (Joueur joueur in joueurs)
            {
                Console.WriteLine($"{rang}. {joueur.toString()}");
                rang++;
            }

            Console.WriteLine("----------------------------------");


            int meilleurScore = joueurs[0].Score;


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

                TimeSpan tempsEcoule = DateTime.Now - debutTour;
                TimeSpan restant = dureeTour - tempsEcoule;

                if (restant.TotalSeconds <= 0) return null;


                Console.Write("\r");


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


                Console.Write($"{nomJoueur}, proposez un mot : {input}");


                Console.Write("   ");
                Console.Write("\b\b\b");


                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        tourFini = true;
                        Console.WriteLine();
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {

                        if (input.Length > 0)
                        {
                            input = input.Substring(0, input.Length - 1);
                        }
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        input += keyInfo.KeyChar.ToString().ToUpper();
                    }
                }
                System.Threading.Thread.Sleep(5);
            }

            return input;
        }
    }

}
