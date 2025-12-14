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
        private List<string> motstrouves;
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
            get { return motstrouves; }
        }

        public int Score
        {
            get { return score; }
        }

        public bool Contient(string mot)
        {
            for (int i = 0; i < this.motstrouves.Count(); i++)
            {
                if (motstrouves[i] == mot)
                {
                    return true;
                }sz
            }
            return false;
        }

        public void Add_Mot(string mot)
        {
            if(Contient(mot) == false)
            {
                this.motstrouves.Add(mot);
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
