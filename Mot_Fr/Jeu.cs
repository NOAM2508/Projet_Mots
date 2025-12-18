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
            TimeSpan TempsEcoulePartie = DateTime.Now - this.heureDebutPartie;
            if (TempsEcoulePartie >= this.tempsMaxPartie)
            {
                Console.WriteLine("Le temps total est écoulé ! ");
                return true;
            }

            // méthode EstVide() utilisé dans la classe Plateau
            if (this.plateau.Estvide())
            {
                Console.WriteLine("\nFIN DE PARTIE : Le plateau est vide !");
                return true;
            }

            return false;
        }

        private string LireMotJoueur(string nom)
        {
            Console.Write($"\n{nom}, proposez un mot : ");
            return Console.ReadLine().ToUpper(); // Lit et met en majuscules
        }


        public void LancerPartie()
        {
            this.heureDebutPartie = DateTime.Now;

            int tourActuel = 0;
            Console.Clear();
            Console.WriteLine(" DÉBUT DE LA PARTIE ");
            Console.WriteLine($"Durée maximale de la partie : {this.tempsMaxPartie.TotalMinutes} minutes.");
            Console.WriteLine($"Temps maximum par tour : {this.tempsMaxTour.TotalSeconds} secondes.");

            while(!EstTermine())
            {
                //Méthode pour savoir à qui le tour => compte à quel tour on est et le nb de joueurs est défini par 2 => reste = 0 ou 1
                Joueur joueurCourant = this.joueurs[tourActuel % this.joueurs.Count];

                Console.WriteLine($"\n---------------------------------");
                Console.WriteLine($"AU TOUR DE {joueurCourant.Nom.ToUpper()} !");
                Console.WriteLine($"Trouvez un mot commençant sur la DERNIÈRE ligne !");

                DateTime debutTour = DateTime.Now;
                DateTime finTour = DateTime.Now;

                string mot = LireMotJoueur(joueurCourant.Nom);

                TimeSpan dureeTour = finTour - debutTour;
                if (dureeTour > tempsMaxTour)
                {
                    Console.WriteLine($"\n[TEMPS ÉCOULÉ] Trop tard ! Vous avez mis {dureeTour.TotalSeconds:F1}s (Max: {tempsMaxTour.TotalSeconds}s).");
                }
                else if (string.IsNullOrWhiteSpace(mot))
                {
                    Console.WriteLine("\n[PAS DE MOT] Vous avez passé votre tour.");
                }
                else
                {
                    TraiterMot(joueurCourant, mot);
                }
            }
        }
        private void TraiterMot(Joueur joueur, string mot)
        {
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
            int points = CalculerScore(mot);
            joueur.Add_Score(points);
            Console.WriteLine($"Scoring : +{points} points !");
        }

        private int CalculerScore(string mot)
        {

            int poidsTotal = 0;
            foreach (char c in mot)
            {
                if (Plateau.PoidsLettres.ContainsKey(c))
                {
                    poidsTotal += Plateau.PoidsLettres[c];
                }
                else
                {
                    poidsTotal += 1; // Poids par défaut
                }
            }
            return poidsTotal * mot.Length;
        }

        private void AfficherResultatsFinaux()
        {
            Console.WriteLine("\n==================================");
            Console.WriteLine("    RÉSULTATS FINAUX DU JEU    ");
            Console.WriteLine("==================================");

            // On affiche les stats de chaque joueur
            foreach (Joueur joueur in this.joueurs)
            {
                Console.WriteLine(joueur.toString()); 
            }

            // Trouver le vainqueur
            //Si joueur 0 a un score > à joueur 1 alors c'est le vainqueur 
            if (this.joueurs[0].Score > this.joueurs[1].Score)
            {
                Console.WriteLine($"VAINQUEUR : {this.joueurs[0].Nom} !");
            }
            //Si joueur 1 a un score > à joueur 0 alors c'est le vainqueur 
            else if (this.joueurs[1].Score > this.joueurs[0].Score)
            {
                Console.WriteLine($"VAINQUEUR : {this.joueurs[1].Nom} !");
            }
            //Sinon égalité
            else
            {
                Console.WriteLine("Égalité !");
            }
        }
    }

}
