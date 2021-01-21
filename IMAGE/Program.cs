using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;


namespace Projet_Semestre_4
{
    class Program
    {
        static string SaisieMot()
        {
            string result = "coco";
            while (!string.Equals(Console.ReadLine(), (result)))
            {
                Console.WriteLine("Erreur, veuillez rentrer un nom d'image valide.");

            }
            return result;
        }
        static int SaisieNombre()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Erreur, veuillez rentrer un nombre valide.");
            }
            return result;
        }
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            string Texte = "Bienvenue dans le projet info de traitement d'image. Appuyez sur une touche pour continuer.";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (Texte.Length / 2)) + "}", Texte));
            Console.ReadKey();
            Console.Clear();

            do
            {
                Console.Clear();
                Console.WriteLine("Choisissez l'image à traiter parmi les suivantes");
                Console.WriteLine(" 1 : Coco");
                Console.WriteLine(" 2 : Lena");
                int a = Convert.ToInt32(Console.ReadLine());
                string image = "coco";
                if ((a != 2) && (a != 1))
                {
                    Console.WriteLine("Vous vous êtes trompé de numéro. Appuyez sur une touche et relancer le programme");
                    Console.ReadKey();
                    break;
                }
                if (a == 2)
                {
                    image = "lena";
                }
                MyImage image_test = new MyImage(image + ".bmp");
                Console.Clear();
                Console.WriteLine("Sélectionnez l'opération à réaliser en entrant un chiffre :");
                Console.WriteLine(" 1 : Rotation de l'image ");
                Console.WriteLine(" 2 : Effet miroir ");
                Console.WriteLine(" 3 : Nuance de gris ");
                Console.WriteLine(" 4 : Noir et Blanc ");
                Console.WriteLine(" 5 : Rétrecissement ");
                Console.WriteLine(" 6 : Agrandissment ");
                Console.WriteLine(" 7 : Filtres ");
                Console.WriteLine(" 8 : Histogramme ");
                Console.WriteLine(" 9 : Créations ");
                Console.WriteLine(" 10 : Fractale");
                int numsaisi = SaisieNombre();
                switch (numsaisi)
                {
                    case 1:
                        Console.WriteLine(" De combien de degré voulez-vous faire tourner l'image ?");
                        Console.WriteLine(" 90 ?    180 ?    270 ? ");
                        int degre = SaisieNombre();
                        if (degre == 90)
                        {
                            image_test.Rotation90();
                            image_test.From_Image_To_File(image + "_90.bmp");
                            Process.Start(image + "_90.bmp");
                        }
                        if (degre == 180)
                        {
                            image_test.Rotation180();
                            image_test.From_Image_To_File(image + "_180.bmp");
                            Process.Start(image + "_180.bmp");
                        }
                        if (degre == 270)
                        {
                            image_test.Rotation270();
                            image_test.From_Image_To_File(image + "_270.bmp");
                            Process.Start(image + "_270.bmp");
                        }
                        if ((degre != 90) && (degre != 180) && (degre != 270))
                        {
                            Console.WriteLine("Veuillez entrer un angle valide");
                        }
                        break;

                    case 2:
                        image_test.EffetMiroir();
                        image_test.From_Image_To_File(image + "_miroir.bmp");
                        Process.Start(image + "_miroir.bmp");
                        break;

                    case 3:
                        image_test.Gris();
                        image_test.From_Image_To_File(image + "_gris.bmp");
                        Process.Start(image + "_gris.bmp");
                        break;

                    case 4:
                        image_test.NoirEtBlanc();
                        image_test.From_Image_To_File(image + "_noir_et_blanc.bmp");
                        Process.Start(image + "_noir_et_blanc.bmp");
                        break;

                    case 5:
                        image_test.Retrecissement();
                        image_test.From_Image_To_File(image + "_little.bmp");
                        Process.Start(image + "_little.bmp");
                        break;

                    case 6:
                        image_test.Agrandissement();
                        image_test.From_Image_To_File(image + "_big.bmp");
                        Process.Start(image + "_big.bmp");
                        break;

                    case 7:
                        Console.Clear();
                        Console.WriteLine("Quel filtre voulez-vous appliquer à l'image ? ");
                        Console.WriteLine("1 : Augmenter le contraste ");
                        Console.WriteLine("2 : Flou ");
                        Console.WriteLine("3 : Renforcement des bords ");
                        Console.WriteLine("4 : Détection des bords ");
                        Console.WriteLine("5 : Repoussage");
                        Console.WriteLine("6 : Estampage");
                        int conv = SaisieNombre();
                        switch (conv)
                        {
                            case 1:

                                int[,] augmentation_contraste = { { 0, -1, 0 }, { -1, 5, 1 }, { 0, -1, 0 } };
                                image_test.Convolution1(augmentation_contraste);
                                image_test.From_Image_To_File(image + "_augmentation_contraste.bmp");
                                Process.Start(image + "_augmentation_contraste.bmp");
                                break;
                            case 2:
                                int[,] flou = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                                image_test.Convolution1(flou);
                                image_test.From_Image_To_File(image + "_flou.bmp");
                                Process.Start(image + "_flou.bmp");
                                break;
                            case 3:
                                int[,] renforcement_des_bords = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
                                image_test.Convolution1(renforcement_des_bords);
                                image_test.From_Image_To_File(image + "_renforcement_des_bords.bmp");
                                Process.Start(image + "_renforcement_des_bords.bmp");
                                break;
                            case 4:
                                int[,] detection_des_bords = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                                image_test.Convolution1(detection_des_bords);
                                image_test.From_Image_To_File(image + "_detection_des_bords.bmp");
                                Process.Start(image + "_detection_des_bords.bmp");
                                break;
                            case 5:
                                int[,] repoussage = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                                image_test.Convolution1(repoussage);
                                image_test.From_Image_To_File(image + "_repoussage.bmp");
                                Process.Start(image + "_repoussage.bmp");
                                break;
                            case 6:
                                int[,] estampage = { { -2, 0, 0 }, { 0, 1, 0 }, { 0, 0, 2 } };
                                image_test.Convolution1(estampage);
                                image_test.From_Image_To_File(image + "_estampage.bmp");
                                Process.Start(image + "_estampage.bmp");
                                break;
                            default:
                                break;
                        }
                        break;
                    case 8:
                        image_test.Histo();
                        image_test.From_Image_To_File(image + "_histo.bmp");
                        Process.Start(image + "_histo.bmp");
                        break;
                    case 9:
                        Console.Clear();
                        Console.WriteLine("Vous êtes dans le menu création");
                        Console.WriteLine("Quelle création voulez-vous choisir ?");
                        Console.WriteLine(" ");
                        Console.WriteLine("1 : Dégradé ");
                        Console.WriteLine("2 : Miroir dégradé");
                        Console.WriteLine("3 : Image d'image");
                        int conv1 = SaisieNombre();
                                Console.WriteLine("Voici le dégradé de l'image");
                                image_test.Degradé();
                                image_test.From_Image_To_File(image + "_degra.bmp");
                                Process.Start(image + "_degra.bmp");
                                break;


                    case 10:
                        Console.WriteLine("Voici la fractale");
                        image_test.Fractale();
                        image_test.From_Image_To_File(image + "_fractale.bmp");
                        Process.Start(image + "_fractale.bmp");

                        break;

                }
                Console.WriteLine("Ce chiffre n'est pas disponible.");
                Console.WriteLine();
                Console.WriteLine("Appuyez sur Echap pour sortir ou une touche");
                cki = Console.ReadKey();
            }
            while (cki.Key != ConsoleKey.Escape);


        }
    }
}