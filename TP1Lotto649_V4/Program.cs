/* Titre: Lotto649
 * 
 * Description:
 * Ce programme permet de simuler des tirages de 6/49.
 * 1- Il demande à l'utilisateur le nombre de combinaisons désirées (entre 10 à 200 combinaisons), 
 *    génère et affiche les combinaisons de l’utilisateur.
 * 2- Le programme génère ensuitela combinaison gagnante de 6 chiffres avec le chiffre complémentaire.
 * 3- Ensuite, il affiche les combinaisons gagnantes de l'utilisateur (les combinaisons qui donnent des gains - 3 numéros sur 6 et plus).
 * 4- A la fin, on retrouve les statistiques indiquant le nombre de tirages et le nombre d’apparitions 
 *    que chaque nombre a eu à l’intérieur des combinaisons gagnantes, 
 *    ainsi que le % de combinaisons pour chaque famille de résultats.
 * 
 * Fait par : Alex Morissette
 * Le: 6 mars 2021
 * Dernière révision le : 31 mars 2021
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto649
{
    class Program
    {
        static void Main(string[] args)
        {
            // Déclaration des variables

            int nbr_tirages = 1;
            // Instanciation du tableau qui accueillera les combinaisons du joueur
            int[,] tab_combines = new int[0, 6];

            // Pour la combinaison gagnante
            int[,] CombineGagnante = new int[0, 7];

            // Pour le tableau de stats, dénombrement des numéros tirés dans les combines gagnantes
            int[,] statsApparitions = new int[50, 3];
            Array.Clear(statsApparitions, 0, statsApparitions.Length);

            //Pour le tableau de stats des catégories des combines, leur nombre et le pourcentage.
            int[,] statsCategories = new int[10, 3];
            Array.Clear(statsCategories, 0, statsCategories.Length);

            int cpt_comb_total = 0;

            int nbr_combines_joueur = 0;

            //Boucle des tirages
            do
            {
                Console.Clear();

                // Afficher un message de bienvenue
                Bienvenue();

                // Demander au joueur le nombre de combinaisons qu'il désire tirer
                nbr_combines_joueur = DemanderNbrCombines(nbr_tirages);
                cpt_comb_total += nbr_combines_joueur;

                // Instanciation du tableau qui accueillera les combinaisons du joueur
                tab_combines = new int[nbr_combines_joueur, 6];

                // GÉNÉRER LES COMBINAISONS DU JOUEUR
                GenererCombinesJoueur(tab_combines);

                // Afficher les toutes combinaisons du joueur
                AfficherCombines(tab_combines);

                // Roulements de tambour...
                Console.WriteLine("\n\tAppuyez sur ENTRÉE pour connaitre le résultat du tirage no {0} ...", nbr_tirages);
                Pause();
                Console.Clear();

                // Générer la combinaison gagnante avec le complémentaire
                CombineGagnante = GenererCombineGagnante();

                // Comparer les combinaisons du joueur avec la combinaison gagnante et compile les statistiques pour le tableau des catégories de combinaisons
                CompilerStats(tab_combines, CombineGagnante, statsCategories);

                //Afficher la combinaison gagnante avec le complémentaire
                AfficherCombineGagnante(CombineGagnante, nbr_tirages);

                // Afficher les combinaisons gagnantes du joueur et compiler les statistiques pour les apparitions
                ComparerCombinaisons(tab_combines, CombineGagnante, statsApparitions);

            } while (Recommencer(ref nbr_tirages));

            // Afficher les statistques des tirages
            AfficherStats(statsApparitions, statsCategories, cpt_comb_total, nbr_tirages);
            Pause();

        }

        /// <summary>
        /// Affiche un message de bienvenue au joueur
        /// </summary>
        public static void Bienvenue()
        {
            Console.WriteLine();
            Console.SetCursorPosition(3, 2);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Pour une meilleure expérience d'utilisation, mettre la fenêtre de la console en plein écran.");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.SetCursorPosition(26, 4);
            Console.WriteLine("  ****** BIENVENUE À LOTTO 6/49 ! ******  ");
            Console.WriteLine();
            AfficherLigne(80);

        }

        /// <summary>
        /// Fonction qui demande le nombre de combinaisons que l'utilisateur désire effectuer pour le tirage en cours. 
        /// </summary>
        public static int DemanderNbrCombines(int nb_tirages)
        {
            int nbr_combines = 0;
            string str_nbr_combines;
            Console.Write("\n\tCombien de combinaisons voulez-vous pour le tirage no. {0} ? Entre 10 et 200 : ", nb_tirages);
            str_nbr_combines = Console.ReadLine();

            while (int.TryParse(str_nbr_combines, out nbr_combines) == false || (nbr_combines < 10 || nbr_combines > 200))
            {
                Console.WriteLine("\n\tEntrée invalide!");
                Console.Write("\n\tCombien de combinaisons voulez-vous pour le tirage no. {0}? Entre 10 et 200 inclusivement : ", nb_tirages);
                str_nbr_combines = Console.ReadLine();
            }
            Console.WriteLine();
            return nbr_combines;
        }

        /// <summary>
        /// Fonction qui génère un tableau int[,] des combinaisons du joueur selon le nombre spécifié par celui-ci.
        /// </summary>
        /// <param name="tab_combines">Tableau 2 dimensions int[,]</param>
        /// <returns>Tableau 2 dimensions int[,]</returns>
        public static void GenererCombinesJoueur(int[,] tab_combines)
        {
            Random hasard = new Random();
            int i, j;

            for (i = 0; i <= tab_combines.GetUpperBound(0); i++) // Générer la liste des combinaisons selon le nombre spécifié par le joueur
            {
                bool[] sorti = new bool[49]; // Éviter les doublons.                              
                Array.Clear(sorti, 0, sorti.Length);  // initialisation de sorti[]
                int no; // Pour stocker le numéro unique généré pour chaque combine    
                for (j = 0; j <= tab_combines.GetUpperBound(1); j++) //Générer une combinaison de 6 chiffres
                {
                    // Générer un nombre aléatoire
                    do
                    {
                        no = hasard.Next(49); //(max value - 1)
                    } while (sorti[no] == true);

                    sorti[no] = true;
                    tab_combines[i, j] = no + 1; // + 1 pour accéder à 49
                }
            }
            TrierCombine(tab_combines);
        }

        /// <summary>
        /// Procédure qui trie en ordre croissant les valeurs de chaque rangée individuellement des tableaux à 2 dimensions
        /// </summary>
        /// <param name="tab">Tableau à trier int[,]</param>
        public static void TrierCombine(int[,] tab)
        {
            int h, i, j;
            int temp;

            // Tri-bulle
            for (h = 0; h <= tab.GetUpperBound(0); h++)
            {
                for (i = 0; i <= 5; i++)
                {
                    for (j = 0; j <= 5 - i - 1; j++)
                    {
                        if (tab[h, j] > tab[h, j + 1])
                        {
                            temp = tab[h, j];
                            tab[h, j] = tab[h, j + 1];
                            tab[h, j + 1] = temp;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Procédure qui affiche le tableau de toutes les combinaisons du joueur
        /// </summary>
        /// <param name="tab">Tableau 2 dimensions int[,]</param>
        public static void AfficherCombines(int[,] tab)
        {
            int i, j;
            for (i = 0; i <= tab.GetUpperBound(0); i++)
            {
                Console.Write("\tCombine #{0} =>\t", i + 1);
                for (j = 0; j <= tab.GetUpperBound(1); j++)
                {
                    Console.Write("{0, 2}  ", tab[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            AfficherLigne(80);
        }

        /// <summary>
        /// Fonction qui génère la combinaison gagnante
        /// </summary>
        /// <returns>Un tableau 2 dimensions de la combinaison gagnante int[,]. </returns>
        public static int[,] GenererCombineGagnante()
        {
            // Déclaration des variables
            Random hasard = new Random();
            int[,] combine_gagnante = new int[1, 7]; // Tableau à 2 dimensions de 1 ligne par 7 colonnes pour accueillir le complémentaire. 
                                                     // Pour que la comparaison avec le tableau des combines du joueur puisse être réalisé 
                                                     // dans la méthode ComparerCombinaisons().
            bool[] sorti = new bool[49];
            Array.Clear(sorti, 0, sorti.Length);
            int no;
            int i = 0;

            //Générer une combinaison de 7 chiffres
            for (i = 0; i < combine_gagnante.Length; i++)
            {
                if (i == 6)
                {
                    // Générer un nombre aléatoire pour complémentaire
                    do
                    {
                        no = hasard.Next(49);
                    } while (sorti[no] == true);
                    sorti[no] = true;
                    combine_gagnante[0, i] = no + 1;
                }

                // Générer un nombre aléatoire
                do
                {
                    no = hasard.Next(49);              
                } while (sorti[no] == true);
                sorti[no] = true;
                combine_gagnante[0, i] = no + 1;           
            }
            TrierCombine(combine_gagnante);
            return combine_gagnante;
        }

        
        /// <summary>
        /// Procédure qui compare les combinaisons du joueur avec la combinaison gagnante afin de compiler les statistques des catégories de combinaisons
        /// </summary>
        /// <param name="tab_j">Tableau des combinaisons du joueur</param>
        /// <param name="tab_g">Tableau de la combinaison gagnante</param>
        /// <param name="statsCat">Tableau pour les statistiques des catégories</param>
        public static void CompilerStats(int[,] tab_j, int[,] tab_g, int[,] statsCat)
        {
            int h = 0,
                i = 0,
                g = 0;
            int cpt_no_gagnant = 0;
            bool[,] no_gagnant = new bool[tab_j.GetLength(0), 6];
            Array.Clear(no_gagnant, 0, no_gagnant.Length);
            bool comp_gagnant = false;

            for (h = 0; h < tab_j.GetLength(0); h++) // Pour chaque ligne du tableau des combinaisons du joueur
            {
                for (i = 0; i < tab_j.GetLength(1); i++) // Pour chaque numéro d'une combinaison joueur
                {
                    for (g = 0; g < tab_g.GetLength(1); g++) // Comparer avec chaque numéro de la combinaison gagnante
                    {
                        if (tab_j[h, i] == tab_g[0, g]) // Compiler chaque numéro gagnant
                        {
                            no_gagnant[h, i] = true;
                            cpt_no_gagnant++;

                            if (tab_j[h, i] == tab_g[0, 6]) // Compiler le complémentaire
                            {
                                comp_gagnant = true;
                                cpt_no_gagnant--;
                            }
                        }
                    }
                }

                // Compiler dans le tableau pour les stats des catégories
                switch (cpt_no_gagnant)
                {
                    case 0:
                        statsCat[0, 1]++;
                        break;
                    case 1:
                        statsCat[1, 1]++;
                        break;
                    case 2:
                        statsCat[2, 1]++;
                        break;
                    case 3:
                        if (comp_gagnant == true)
                            statsCat[4, 1]++;
                        else statsCat[3, 1]++;
                        break;
                    case 4:
                        if (comp_gagnant == true)
                            statsCat[6, 1]++;
                        else statsCat[5, 1]++;
                        break;
                    case 5:
                        if (comp_gagnant == true)
                            statsCat[8, 1]++;
                        else statsCat[7, 1]++;
                        break;
                    case 6:
                        statsCat[9, 1]++;
                        break;
                    default:
                        break;
                }
                cpt_no_gagnant = 0;
                comp_gagnant = false;
            }
        }

        /// <summary>
        /// Procédure qui compare les combinaisons du joueur avec la combinaison gagnante pour afficher les combinaisons gagnantes du joueur.
        /// </summary>
        /// <param name="tab_j">Tableau 2 dimensions de la combinaison gagnante</param>
        /// <param name="tab_g">Tableau de la combinaison gagnante</param>
        /// <param name="stats_app">Tableau pour les statistiques d'apparitions des numéros gagnants</param>
        public static void ComparerCombinaisons(int[,] tab_j, int[,] tab_g, int[,] stats_app)
        {
            Console.WriteLine("\n\tVos combinaisons gagnantes sont : ");

            // Déclaration et initialisation des variables
            int h = 0,
                i = 0,
                g = 0;
            int cpt_no_gagnant = 0; // Pour compter le nombre de numéros gagnants à l'intérieur d'une combinaison
            bool[,] no_gagnant = new bool[tab_j.GetLength(0), 6]; // Pour stocker vrai ou faux, si le numéro de la combine du joueur correspond 
                                                                  // au numéro de la combine gagnante.
            Array.Clear(no_gagnant, 0, no_gagnant.Length);
            bool comp_gagnant = false;  // Pour identifier si le complémentaire correspond à un numéro d'une combine du joueur. 
            bool combine_pas_de_gain = true;

            for (h = 0; h < tab_j.GetLength(0); h++) // Pour chaque ligne du tableau des combinaisons du joueur
            {
                for (i = 0; i < tab_j.GetLength(1); i++) // Pour chaque numéro d'une combinaison joueur
                {
                    for (g = 0; g < tab_g.GetLength(1); g++) // Comparer avec chaque numéro de la combinaison gagnante
                    {
                        if (tab_j[h, i] == tab_g[0, g]) // Compiler chaque numéro gagnant de la combine joueur actuelle
                        {
                            no_gagnant[h, i] = true;
                            cpt_no_gagnant++;
                            if (tab_j[h, i] == tab_g[0, 6]) // Compiler le complémentaire si tiré.
                            {
                                comp_gagnant = true;
                                cpt_no_gagnant--; // Pour ne pas compter en double le numéro.
                            }
                        }
                    }
                }

                if (cpt_no_gagnant > 2) // Condition pour afficher les combinaisons qui donnent des gains
                {
                    combine_pas_de_gain = false;
                    AfficherCombinesGagnantes(h, cpt_no_gagnant, comp_gagnant, tab_j, tab_g, stats_app, no_gagnant);
                }

                // Réinitialisation des variables pour la prochaine combinaison (h => ligne du tableau)
                cpt_no_gagnant = 0;
                comp_gagnant = false;
            }
            if (combine_pas_de_gain == true)
            {
                Console.WriteLine("\n\t--- Désolé, aucune combinaison n'a apporté de gain pour ce tirage. ---");
            }
        }

        /// <summary>
        /// Procédure qui affiche la combinaison gagnante du tirage en cours
        /// </summary>
        /// <param name="tab_g">Tableau 2 dimensions de la combinaison gagnante</param>
        /// <param name="tirages">Pour indiquer au joueur le numéro du tirage en cours</param>
        public static void AfficherCombineGagnante(int[,] tab_g, int tirages)
        {
            int i = 0;

            Console.WriteLine();
            Console.WriteLine("\tLa combinaison gagnante du tirage no. {0} du LOTTO 6/49 est : ", tirages);
            Console.WriteLine();
            for (i = 0; i < tab_g.Length; i++)
            {
                if (i < tab_g.Length - 1)
                {
                    Console.Write("\t{0} ", tab_g[0, i]);
                    Console.Write("  -");
                }
                else
                {
                    Console.Write("\t");
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("{0}", tab_g[0, i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            Console.Write("  ");
            Console.WriteLine();
        }

        /// <summary>
        /// Procédure qui affiche les combinaisons gagnantes du joueur.
        /// </summary>
        /// <param name="h">Nombre de lignes du tableau des combinaisons joueur</param>
        /// <param name="cpt_no_gagnant">Nombre de numéros concordant avec la combine gagnante du croupier</param>
        /// <param name="comp_gagnant">Vrai si le complémentaire est parmi la combine du joueur</param>
        /// <param name="tab_j">Tableau des combines du joueur</param>
        /// <param name="tab_g">Tableau de la combine gagnante</param>
        /// <param name="stats_app">Tableau pour les statistques d'appartition des numéros gagnants</param>
        /// <param name="no_gagnant">Tableau bool qui indique vrai pour les numéros gagnants</param>
        public static void AfficherCombinesGagnantes(int h, int cpt_no_gagnant, bool comp_gagnant, int[,] tab_j, int[,] tab_g, int[,] stats_app, bool[,] no_gagnant)
        {
            Console.Write("\a"); // Signal sonore pour les combines gagnantes

            // Afficher les combines gagnantes et compiler les statistiques pour l'apparition des numéros
            int no_app = 0;
            Console.Write("\n\tCombine #{0} =>", h + 1);
            for (int i = 0; i < tab_j.GetLength(1); i++) // Pour chaque numéro d'une combinaison
            {
                CompilerStats(h, i, no_app, tab_j, comp_gagnant, tab_g, stats_app);

                // Afficher les combines
                if (no_gagnant[h, i] == true && tab_j[h, i] != tab_g[0, 6]) //Highlight les no gagnants
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("\t{0, 2}    ", tab_j[h, i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (no_gagnant[h, i] && tab_j[h, i] == tab_g[0, 6]) // Highlight le complémentaire
                {
                    Console.Write("\t");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("{0, 2}", tab_j[h, i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("    ");
                }
                else                                                    // Autres numéros
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("\t{0, 2}    ", tab_j[h, i]);
                }
            }
            // Reset couleurs
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Write("<= {0} sur 6 {1}", cpt_no_gagnant, comp_gagnant == true ? " +" : "");
            comp_gagnant = false;
        }

        /// <summary>
        /// Procédure qui compile les statistques du nombre d'appartitions des numéros dans les combinaisons gagnantes du joueur
        /// </summary>
        /// <param name="h">Indice de la ligne du tableau des combines joueur</param>
        /// <param name="i">Indice de la colonne du tableau des combines joueur</param>
        /// <param name="no_app">Pour stocker l'indice des numéros de 1 à 49</param>
        /// <param name="tab_j">Tableau des combines du joueur</param>
        /// <param name="comp_gagnant">Vrai si le complémentaire est parmi la combine du joueur</param>
        /// <param name="tab_g">Tableau de la combine gagnante</param>
        /// <param name="stats_apparitions">Tableau pour les statistques d'appartition des numéros gagnants</param>
        public static void CompilerStats(int h, int  i, int no_app, int[,] tab_j, bool comp_gagnant, int[,] tab_g, int[,] stats_apparitions)
        {
            // Compiler les statistiques - apparition des numéros dans les combines gagnantes
            no_app = tab_j[h, i];
            stats_apparitions[no_app, 1]++;

            // Compter l'apparition du complémentaire
            if (comp_gagnant == true && tab_j[h, i] == tab_g[0, 6])
            {
                stats_apparitions[no_app, 2]++;
            }
        }

        /// <summary>
        /// Boucle des tirages
        /// </summary>
        /// <returns>Vrai ou Faux selon la réponse de l'utilisateur, s'il veut faire un autre tirage ou non.</returns>
        public static bool Recommencer(ref int tirages)
        {
            string str_reponse;
            char reponse;

            Console.WriteLine();
            //Demander à l'utilisateur s'il veut faire un nouveau tirage
            Console.Write("\n\tVoulez-vous faire un autre tirage? O/N : ");
            str_reponse = Console.ReadLine().ToUpper();

            while (!(char.TryParse(str_reponse, out reponse)) || (reponse != 'O' && reponse != 'N'))
            {
                Console.WriteLine("\n\tEntrée invalide!");
                Console.Write("\n\tVoulez-vous faire un autre tirage? O/N : ");
                str_reponse = Console.ReadLine().ToUpper();
            }
            if (reponse == 'O')
            {
                tirages++;
            }
            return reponse == 'O';
        }

        /// <summary>
        /// Procédure qui affiche les statistiques de tous les tirages effectués 
        /// </summary>
        /// <param name="tab_app">Tableau des statistiques d'apparitions des numéros gagnants</param>
        /// <param name="tab_cat">Tableau pour les statistiques des catégories</param>
        /// <param name="nbrCombTotal">Nombre total de combinaisons gagnantes du joueur</param>
        public static void AfficherStats(int[,] tab_app, int[,] tab_cat, int nbrCombTotal, int tirages)
        {
            // Introduction aux statistiques
            Console.WriteLine();
            AfficherLigne(80);
            Console.WriteLine("\n\tVous avez fait {0} tirage{1}.", tirages, tirages > 1 ? "s" : "");
            Console.WriteLine("\n\tContinuez pour voir les statistiques {0} tirage{1}...", tirages < 2 ? "du" : "des", tirages >= 2 ? "s" : "");
            Pause();

            // Afficher le tableau des statistques du nombre d'apparitions de chaque numéro des combinaisons gagnantes
            // En-tête
            AfficherLigne(80);
            Console.WriteLine("\n\tStatistiques de l'apparition des numéros des combinaisons gagnantes");
            Console.WriteLine();
            Console.Write("\t╔");
            AfficherCaractere('═', 60);
            Console.WriteLine("╗");
            Console.WriteLine("\t║   Numéros\t│\tApparitions\t│   Complémentaires  ║");
            Console.Write("\t╟");
            AfficherCaractere('─', 60);
            Console.Write("╢");

            Console.WriteLine("\t");

            // Corps du tableau - résultats apparitions
            for (int h = 1; h < tab_app.GetLength(0); h++)
            {
                Console.WriteLine("\t║{0,10}  \t│  {1,10}\t\t│{2,10}\t     ║", h, tab_app[h, 1], tab_app[h, 2]);
            }
            Console.Write("\t");
            Console.Write('╚');
            AfficherCaractere('═', 60);
            Console.Write('╝');

            // Pause
            Console.WriteLine();
            AfficherLigne(80);
            Console.WriteLine("\n\tAppuyer sur ENTRÉE pour voir le tableau des statistiques des catégories...");
            Pause();

            // Afficher les statistiques pour les catégories de combines

            // En-tête
            AfficherLigne(80);
            Console.WriteLine("\n\tStatistiques du total des combinaisons sorties par catégories");
            Console.WriteLine();

            Console.Write("\t╔");
            AfficherCaractere('═', 55);
            Console.WriteLine("╗");
            Console.WriteLine("\t║ Catégories\t│\tNombre\t│\tPourcentage\t║");
            Console.Write("\t╟");
            AfficherCaractere('─', 55);
            Console.Write("╢");

            // déclaration et initialisation des variables


            // Corps du tableau
            float pourcent = 0;
            int somme_comb = 0;
            float total_pourcent = 0;

            Console.WriteLine("\t");
            for (int h = 0; h < tab_cat.GetLength(0); h++)
            {
                switch (h)
                {
                    case 0:
                    case 1:
                    case 2:
                        pourcent = CalculerStatsCat(h, tab_cat, nbrCombTotal, somme_comb, ref total_pourcent);
                        Console.WriteLine("\t║  {0} sur 6\t│\t{1, 4}  \t│\t{2,6:F2}%\t\t║", h, tab_cat[h, 1], pourcent);
                        break;
                    case 3:
                    case 4:
                        pourcent = CalculerStatsCat(h, tab_cat, nbrCombTotal, somme_comb, ref total_pourcent);
                        Console.WriteLine("\t║  3 sur 6 {0}\t│\t{1, 4}  \t│\t{2,6:F2}%\t\t║", h == 4 ? "+" : "", tab_cat[h, 1], pourcent);
                        break;
                    case 5:
                    case 6:
                        pourcent = CalculerStatsCat(h, tab_cat, nbrCombTotal, somme_comb, ref total_pourcent);
                        Console.WriteLine("\t║  4 sur 6 {0}\t│\t{1, 4}  \t│\t{2,6:F2}%\t\t║", h == 6 ? "+" : "", tab_cat[h, 1], pourcent);
                        break;
                    case 7:
                    case 8:
                        pourcent = CalculerStatsCat(h, tab_cat, nbrCombTotal, somme_comb, ref total_pourcent);
                        Console.WriteLine("\t║  5 sur 6 {0}\t│\t{1, 4}  \t│\t{2,6:F2}%\t\t║", h == 8 ? "+" : "", tab_cat[h, 1], pourcent);
                        break;
                    case 9:
                        pourcent = CalculerStatsCat(h, tab_cat, nbrCombTotal, somme_comb, ref total_pourcent);
                        Console.WriteLine("\t║  6 sur 6\t│\t{0, 4}  \t│\t{1,6:F2}%\t\t║", tab_cat[h, 1], pourcent);
                        break;
                    default:
                        break;
                }
            }

            Console.Write("\t");
            Console.Write('║');
            AfficherCaractere('-', 55);
            Console.Write('║');
            Console.WriteLine();
            Console.Write("\t║  Total\t│\t{0, 4}  \t│\t{1:F2}%\t\t║", nbrCombTotal, total_pourcent);
            Console.WriteLine();
            Console.Write("\t");
            Console.Write('╚');
            AfficherCaractere('═', 55);
            Console.Write('╝');
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("\n\tMerci d'avoir participé et au revoir!");
            AfficherLigne(80);

        }

        /// <summary>
        /// Fonction qui calcule le nombre de combinaisons sorties et leur pourcentage selon la catégorie
        /// </summary>
        /// <param name="h">Indice de la ligne du tableau des categories</param>
        /// <param name="tab">Tableau des catégories</param>
        /// <param name="nbrTot">Nombre total de combinaisons de tous les tirages</param>
        /// <param name="sommeCombCat">Nombre total de combinaisons de chaque catégorie</param>
        /// <param name="totalPourcent">Pourcentage total de toutes les catégories, doit donner 100%</param>
        /// <returns>Le pourcentage du nombre de combinaisons chaque une catégorie</returns>
        public static float CalculerStatsCat(int h, int[,] tab, int nbrTot, int sommeCombCat, ref float totalPourcent)
        {
            float pourcentage = 0;

            sommeCombCat += tab[h, 1];
            pourcentage = tab[h, 1] * 100 / (float)nbrTot;
            totalPourcent += pourcentage;

            return pourcentage;
        }

        /// <summary>
        /// Procédure qui affiche un caractère spécifié, le nombre de fois spécifié.
        /// </summary>
        /// <param name="c">Caractère</param>
        /// <param name="nbr">Nombre de fois qu'il s'affiche (Console.Write)</param>
        public static void AfficherCaractere(char c, int nbr)
        {
            for (int i = 0; i < nbr; i++)
            {
                Console.Write(c);
            }
        }

        /// <summary>
        /// Procédure qui affiche une ligne bleue de la longueur spécifée
        /// </summary>
        /// <param name="nbr">Nombre de fois que l'on veut faire afficher le caractère spécifié.</param>
        public static void AfficherLigne(int nbr)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("\t");
            for (int i = 0; i < nbr; i++)
            {
                Console.Write('─');
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        /// <summary>
        /// Fait une pause et attend que l'utilisateur appuie sur une touche
        /// </summary>
        public static void Pause()
        {
            Console.ReadLine();
        }
    }
}
