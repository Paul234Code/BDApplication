using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BDApplication
{
    public class Program
    {
        readonly Connection objetConnection = new Connection();
        readonly List<Animal> listeOfAnimal = new List<Animal>();
        static void Main()
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("Bienvenue dans l'application gestion clinique Veterinaire");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Program program = new();
            Console.WriteLine();
            AfficherMenu();
            program.StartTheMachine();
        }
        // On demande a l'utilisateur de faire son choix tant que ce dernier est invalide
        private void SelectChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    AjouterAnimal();
                    break;
                case "2":
                    VoirListeAnimauxPension();
                    break;
                case "3":
                    VoirListePropriétaire();
                    break;
                case "4":
                    VoirNombreTotalAnimaux();
                    break;
                case "5":
                    VoirPoidsTotalAnimaux();
                    break;
                case "6":
                    ExtraireAnimauxSelonCouleurs();
                    break;
                case "7":
                    RetirerUnAnimalDeListe();
                    break;
                case "8":
                    Modifier();
                    break;
                case "9":
                    Environment.Exit(0);
                    break;
                default:
                    AfficherMessageErreur("Le choix n'est pas valide.......");
                    break;
            }
        }
        // Fonction qui affiche le menu principal de l'application
        private static void AfficherMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1- Ajouter un animal");
            Console.WriteLine("2- Voir la liste de tous les animaux en pension");
            Console.WriteLine("3- Voir la liste de tous les proprietaires");
            Console.WriteLine("4- Voir le nombre Total d'animaux en pension");
            Console.WriteLine("5- Voir le poids total de tous les animaux en pensions");
            Console.WriteLine("6- Voir la liste des animmaux d'une couleur(rouge,bleu ou violet)");
            Console.WriteLine("7- Retirer un anamal de la liste");
            Console.WriteLine("8- Modifier un animal");
            Console.WriteLine("9- Quitter l'application");
            Console.WriteLine();
        }
        // Fonction qui demarre l'Application  et qui redirrige le programme
        // en fonction de l'Entree au clavier
        private void StartTheMachine()
        {
            string choice;
            while (true)
            {
                Console.WriteLine("Enter votre choix ou taper CTRL+C pour quitter");
                choice = Console.ReadLine();
                SelectChoice(choice);
                AfficherMenu();
            }
        }
        // Fonction qui affiche un message d'errur sur la console
        private static void AfficherMessageErreur(string message)
        {
            Console.WriteLine(message);
        }
        // Fonction qui permet d'ajouter un animal dans le tableau 
        // En demandant a l'utilisateur de saisir les informations d'un animal
        private void AjouterAnimal()
        {
            Console.WriteLine("Veuillez saisir le type de l'animal: ");
            var type = Console.ReadLine();
            Console.WriteLine("Veuillez saisir le nom de l'animal: ");
            var nomAnimal = Console.ReadLine();
            Console.WriteLine("Veuillez saisir l'age de l'animal: ");
            var ageAnimal = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Veuillez saisir le poids de l'animal:");
            var poidsAnimal = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Veuillez saisir la couleur de l'animal: ");
            var couleurAnimal = Console.ReadLine().ToLower();
            if (!ValidationCouleur(couleurAnimal))
                AfficherMessageErreur("votre couleur n'est pas valide.........");
            while (!ValidationCouleur(couleurAnimal))
            {
                Console.WriteLine("Veuillez saisir la couleur de l'animal: ");
                couleurAnimal = Console.ReadLine().ToLower();
                if (!ValidationCouleur(couleurAnimal))
                    AfficherMessageErreur("votre couleur n'est pas valide.........");
            }
            Console.WriteLine("Veuillez saisir le nom du proprietaire de l'animal: ");
            var nomProprietaireAnimal = Console.ReadLine();
            var animal = new Animal() { Type = type, Nom = nomAnimal, Age = ageAnimal, Poids = poidsAnimal, Couleur = couleurAnimal, Proprietaire = nomProprietaireAnimal };
            listeOfAnimal.Add(animal);
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "INSERT INTO animal(Type,Nom,Age,Poids,Couleur,Proprietaire)" +
                             "VALUES(@Type,@Nom,@Age,@Poids,@Couleur,@Proprietaire)";
            MySqlCommand mySqlCommand = new MySqlCommand(request, connection);
            mySqlCommand.Parameters.AddWithValue("@Type", animal.Type);
            mySqlCommand.Parameters.AddWithValue("@Nom", animal.Nom);
            mySqlCommand.Parameters.AddWithValue("@Age", animal.Age);
            mySqlCommand.Parameters.AddWithValue("@Poids", animal.Poids);
            mySqlCommand.Parameters.AddWithValue("@Couleur", animal.Couleur);
            mySqlCommand.Parameters.AddWithValue("@Proprietaire", animal.Proprietaire);
            mySqlCommand.ExecuteReader();
            connection.Close();
            Console.WriteLine("Requete INSERT INTO terminé ");
        }
        // Fonction qui affiche la liste des animaux en pensions(choix 2)
        private void VoirListeAnimauxPension()
        {
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("ID\t|\tTYPE ANIMAL\t|\tNOM\t|\tAGE\t|\tPOIDS\t|\tCOULEUR\t|\tPROPRIETAIRE |");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "SELECT * FROM animal";
            MySqlCommand command = new(request, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetInt32(0)} \t\t{reader.GetString(1)}\t\t\t{reader.GetString(2)}\t\t{reader.GetInt32(3)} \t\t{reader.GetInt32(4)}\t\t{reader.GetString(5)}\t\t{reader.GetString(6)}");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            connection.Close();
        }      
         // Une fonction  qui affiche la liste des proprietaires des animaux pensionnaires        
        private void VoirListePropriétaire()
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("|\tPROPRIETAIRE\t|");
            Console.WriteLine("-------------------------");
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "SELECT Proprietaire FROM animal";
            MySqlCommand command = new MySqlCommand(request, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"\t{reader.GetString(0)}\t");
                    }
                }
            }
            connection.Close();
        }
        // fonction qui affiche  le nombre total des animaux  pensionnaires
        private void VoirNombreTotalAnimaux()
        {
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine("|\tNOMBRE ANIMAUX\t|");
            Console.WriteLine("----------------------------------------------------------------------");
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "SELECT COUNT(Id) AS NombreTotal FROM animal";
            MySqlCommand mySqlCommand = new MySqlCommand(request, connection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"\t{reader.GetInt32(0)}\t");
                    }
                }
            }
            connection.Close();
        }
        // Fonction qui modifie un animal dans la base de donnée
        private void Modifier()
        {
            Console.WriteLine("Entrer le nouveau nom");
            string nom = Console.ReadLine();
            Console.WriteLine("Entrer l'age ");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter le nouveau poids");
            int poids = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Entrer la  nouvelle couleur");
            string couleur = Console.ReadLine();
            Console.WriteLine("Entrer le nouveau proprietaire");
            string proprietaire = Console.ReadLine();
            Console.WriteLine("Entrer l'Identifiant de l'animal");
            int Id = Convert.ToInt32(Console.ReadLine());
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "UPDATE animal SET Nom = @nom, Age = @age,Poids = @poids,Couleur = @couleur,Proprietaire = @proprietaire WHERE Id = @Id";
            MySqlCommand mySqlCommand = new MySqlCommand(request, connection);
            mySqlCommand.Parameters.AddWithValue("@Nom", nom);
            mySqlCommand.Parameters.AddWithValue("@Age", age);
            mySqlCommand.Parameters.AddWithValue("@Poids", poids);
            mySqlCommand.Parameters.AddWithValue("@Couleur", couleur);
            mySqlCommand.Parameters.AddWithValue("@Proprietaire", proprietaire);
            mySqlCommand.Parameters.AddWithValue("@Id", Id);
            mySqlCommand.ExecuteReader();
            connection.Close();
            Console.WriteLine("Requete UPDATE terminé ");
        }
        // Fonction qui calcule et  affiche le poids total de l'ensemble des  animaux
        // pensionnaires 
        private void VoirPoidsTotalAnimaux()
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("|\tPOIDS TOTAL\t|");
            Console.WriteLine("---------------------------");
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "SELECT SUM(Poids) AS Total FROM animal";
            MySqlCommand mySqlCommand = new MySqlCommand(request, connection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"\t{reader.GetInt32(0)}\t");
                    }
                }
            }
            connection.Close();
        }
        // Fonction qui permet d'extraire une sous-liste d'animaux suivant leur couleur
        private void ExtraireAnimauxSelonCouleurs()
        {
            Console.WriteLine("VEUILLEZ SAISIR LA COULEUR DE RECHERCHE : ");
            string couleur = Console.ReadLine();
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "SELECT * FROM animal WHERE Couleur = @Couleur";
            MySqlCommand mySqlCommand = new MySqlCommand(request, connection);
            mySqlCommand.Parameters.AddWithValue("@Couleur", couleur);
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("ID\t|\tTYPE ANIMAL\t|\tNOM\t|\tAGE\t|\tPOIDS\t|\tCOULEUR\t|\tPROPRIETAIRE |");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetInt32(0)} \t\t{reader.GetString(1)}\t\t\t{reader.GetString(2)}\t\t{reader.GetInt32(3)} \t\t{reader.GetInt32(4)}\t\t{reader.GetString(5)}\t\t{reader.GetString(6)}");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            connection.Close();
        }
        // Une fonction qui permet de retirer un animal dans la liste
        // En connaissant son ID
        private void RetirerUnAnimalDeListe()
        {
            Console.WriteLine("VEUILLEZ SAISIR LE ID DE L'ANIMAL: ");
            int numeroID = Convert.ToInt32(Console.ReadLine());
            MySqlConnection connection = objetConnection.ConnectToDatabase();
            string request = "DELETE FROM animal WHERE Id = @numeroID";
            MySqlCommand mySqlCommand = new MySqlCommand(request, connection);
            mySqlCommand.Parameters.AddWithValue("@numeroID", numeroID);
            mySqlCommand.ExecuteReader();
            connection.Close();
            Console.WriteLine("Suppression terminé avec sucess");
        }
        // Fonction qui valide la couleur de l'animal
        private bool ValidationCouleur(string couleur)
        {
            return couleur == "bleu" || couleur == "rouge" || couleur == "violet";
        }
    }
}
