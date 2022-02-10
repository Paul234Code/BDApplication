using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDApplication
{
    public class Animal
    {
        public string Type { get; set; }
        public string Nom { get; set; }
        public int Age { get; set; }
        public int Poids { get; set; }
        public string Couleur { get; set; }
        public string Proprietaire { get; set; }
        // le constructeur par par defaut
        public Animal()
        {

        }
        // Fonction qui affiche un animal
        public void DisplayAnimal()
        {
            Console.WriteLine($"Type : {Type}\nNom : {Nom}\nAge : {Age}\nPoids : {Poids}\nCouleur : {Couleur}\nProprietaire : {Proprietaire}");
        }

    }
}
