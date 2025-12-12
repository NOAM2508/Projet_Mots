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
            this.nom = nom;
            this.score = 0;
            this.mots_trouves = null;
        }

        public void Add_Mot(string mot)
        {

        }
        
        public string toString()
        {
            return $"Joueur : {nom}\nScore : {score}\nmots trouvés : {mots_trouves}";
        }

        public void Add_Score(int val)
        {

        }

        public bool Contient(string mot)
        {
            return false;
        }
    }
}
