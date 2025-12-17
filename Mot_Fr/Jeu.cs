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
                Console.WriteLine("Le temps totale est écoulé ! ");
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
            return Console.ReadLine().ToUpper(); // Lit et met en majuscules
        }

        public void LancerPartie()
        {
            heureDebutPartie = DateTime.Now;
            int tourActuel = 0;

            Console.WriteLine(" DÉBUT DE LA PARTIE ");
            Console.WriteLine($"Durée maximale de la partie : {tempsMaxPartie.TotalMinutes} minutes.");
            Console.WriteLine($"Temps maximum par tour : {tempsMaxTour.TotalSeconds} secondes.");
            Console.WriteLine(plateau.toString()); // On affiche le plateau au début !

            // TANT QUE le jeu n'est pas fini
            while (!EstTermine()) 
            {
                // On détermine c'est à qui de jouer
                Joueur joueurCourant = joueurs[tourActuel % joueurs.Count]; //Donne soit 0 ou 1 

                Console.WriteLine($"\n---------------------------------");
                Console.WriteLine($"AU TOUR DE {joueurCourant.Nom.ToUpper()} !");
                Console.WriteLine($"Trouvez un mot commençant sur la DERNIÈRE ligne !");
                

                // On prend l'heure du début du tour
                DateTime heureDebutTour = DateTime.Now;

                // Le joueur saisit un mot
                string motSaisi = LireMotJoueur(joueurCourant.Nom);

                // Vérification du Temps du tour
                TimeSpan tempsPris = DateTime.Now - heureDebutTour;
                if (tempsPris > this.tempsMaxTour)
                {
                    Console.WriteLine($"Trop tard ! Vous avez mis {tempsPris.TotalSeconds:F1}s (Max: {tempsMaxTour.TotalSeconds}s).");
                }
                else
                {
                    // VERIFICATION DU MOT 

                    // Est-ce que le mot appartient au dictionnaire
                    if (dictionnaire.RechDichoRecursif(motSaisi))
                    {
                        // Est-ce que le joueur l'a déjà trouvé ?
                        if (!joueurCourant.Contient(motSaisi))
                        {
                            // Est-ce que le mot est sur le plateau ?
                            object chemin = plateau.Recherche_Mot(motSaisi);

                            if (chemin != null)
                            {
                                Console.WriteLine("Mot valide et trouvé sur le plateau !");

                                // Ajout du mot et du score
                                joueurCourant.Add_Mot(motSaisi);
                                joueurCourant.Add_Score(motSaisi.Length); // Score = longueur du mot

                                // Mise à jour du plateau (faire tomber les lettres)
                                this.plateau.Maj_Plateau(chemin);

                                // On réaffiche le plateau modifié
                                Console.WriteLine(plateau.toString());
                            }
                            else
                            {
                                Console.WriteLine("Le mot n'est pas présent sur la grille.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous avez déjà trouvé ce mot !");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ce mot n'existe pas dans le dictionnaire.");
                    }
                }

                //On passe au tour suivant
                tourActuel++;
            }

            //Affichage des résultats
            AfficherResultatsFinaux();
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
        }
    }

}
