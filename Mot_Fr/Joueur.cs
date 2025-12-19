using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mot_Fr
{
    public class Joueur
    {
        private string nom;
        private List<string> mots_trouves;
        private int score;

        public Joueur(string nom)
        {
            if(nom == null || nom == "")
            {
                this.nom = "anonyme"; 
            }
            else
            {
                this.nom = nom;
            }
            this.score = 0;
            this.mots_trouves = new List<string>();
        }

        public string Nom 
        {
            get { return nom; }
        }

        public List<string> Motstrouves 
        {
            get { return mots_trouves; }
        }

        public int Score
        {
            get { return score; }
        }












        /// <summary>
        /// Permet de savoir si le joueur à déjà trouvé le mot.
        /// </summary>
        public bool Contient(string mot)
        {
            for (int i = 0; i < this.mots_trouves.Count; i++)
            {
                if (mots_trouves[i] == mot)
                {
                    return true;
                }
            }
            return false;
        }












        /// <summary>
        /// Permet d'ajouter le mot à la liste des mots trouvés.
        /// </summary>
        /// <param name="mot"></param>
        public void Add_Mot(string mot)
        {
            if(Contient(mot) == false)
            {
                this.mots_trouves.Add(mot);
            }
        }













        /// <summary>
        /// Présentation du joueur.
        /// </summary>
        public string toString()
        {
            string listeMots = (mots_trouves != null && mots_trouves.Count > 0)
                               ? string.Join(", ", mots_trouves)
                               : "Aucun";

            return $"Nom : {nom} | Score : {score} | Mots trouvés : {listeMots}";
        }














        /// <summary>
        /// Permet d'augmenter le score total.
        /// </summary>
        /// <param name="val">Valeur à ajouter au score total</param>
        public void Add_Score(int val)
        {
            this.score = this.score + val;
        }
    }
}
