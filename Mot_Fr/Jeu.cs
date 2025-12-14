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
                Console.WriteLine("Le temps totale est écoulé ! ");
                return true;
            }

            // Rajouter méthode dans la classe Plateau
            //if (this.plateau.Estvide())
            //{
              //  Console.WriteLine("\nFIN DE PARTIE : Le plateau est vide !");
                //return true;
            //}

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
            Console.WriteLine(" DÉBUT DE LA PARTIE ");
            Console.WriteLine($"Durée maximale de la partie : {this.tempsMaxPartie.TotalMinutes} minutes.");
            Console.WriteLine($"Temps maximum par tour : {this.tempsMaxTour.TotalSeconds} secondes.");

            while(!EstTermine())
            {
                //Méthode pour savoir à qui le tour => compte à quel tour on est et le nb de joueurs est défini par 2 => reste = 0 ou 1
                Joueur joueurCourant = this.joueurs[tourActuel % this.joueurs.Count];

                Console.WriteLine($"\n---------------------------------");
                Console.WriteLine($"AU TOUR DE {joueurCourant.Nom.ToUpper()} !");

                DateTime heureDebutTour = DateTime.Now;

                string motSaisi = LireMotJoueur(joueurCourant.Nom);
            }
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
            if (this.joueurs[0].ScoreTotal > this.joueurs[1].ScoreTotal)
            {
                Console.WriteLine($"VAINQUEUR : {this.joueurs[0].Nom} !");
            }
            //Si joueur 1 a un score > à joueur 0 alors c'est le vainqueur 
            else if (this.joueurs[1].ScoreTotal > this.joueurs[0].ScoreTotal)
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
