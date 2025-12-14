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
            this.mots_trouves = null;
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

        public bool Contient(string mot)
        {
            for (int i = 0; i < this.mots_trouves.Count(); i++)
            {
                if (mots_trouves[i] == mot)
                {
                    return true;
                }
            }
            return false;
        }

        public void Add_Mot(string mot)
        {
            if(Contient(mot) == false)
            {
                this.mots_trouves.Add(mot);
            }
        }
        
        public string toString()
        {
            return $"Nom du joueur : {nom}\nScore : {score}\nmots trouvés : {mots_trouves}";
        }

        public void Add_Score(int val)
        {
            this.score = this.score + val;
        }
    }
}
