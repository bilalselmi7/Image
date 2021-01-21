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
    public class MyImage
    {
        #region Attributs
        /// <summary>
        /// Les attributs de la classe MyImage. La classe contient les attributs suivants : 
        /// </summary>
        Pixel[,] matricepixel; // La matrice de pixels contenant les informations des pixels de l'image
        int typeImage; // Un tableau de bytes contenant les octets determinant le type du fichier
        int tailleFichier; // La taille du fichier
        int largeurImage; // La largeur de l'image
        int hauteurImage; // La hauteur de l'image
        int nombreBits; // Le nombre de bits par couleur 
        int[,] image; // Une matrice de bytes contenant les informations en rapport avec l'image (pixel)
        int headerInfo; // Un tableau de bytes qui prend en compte les informations du Header
        int tailleOffest; // La taille du Offset
        byte[] offset1 = new byte[8];
        byte[] offset2 = new byte[2];
        byte[] offset3 = new byte[24];
        #endregion

        #region Constructeur
        /// <summary>
        /// MyImage permet de transcrire les informations d'une image dans des tableaux de bytes
        /// </summary>
        /// <param name="myfile"></param>
        public MyImage(string myfile)
        {
            byte[] filename = File.ReadAllBytes(myfile); // On crée un tableau de bytes qui va recuperer tous les octets du fichier myfile (l'image)
            byte[] largeurIm = new byte[4]; // On crée un tableau de bytes qui va recevoir les informations concernant la largeur de l'image
            byte[] hauteurIm = new byte[4]; // On crée un tableau de bytes qui va recevoir les informations concernant la hauteur de l'image
            byte[] tailleFich = new byte[4]; // On crée un tableau de bytes qui va recevoir les informations concernant la taille du fichier
            byte[] typeIm = new byte[2];
            byte[] headerInf = new byte[4];
            byte[] nombreB = new byte[2]; // On crée un tableau de bytes qui va recevoir les informations concernant le nombre de bits nécessaire à la couleur
                                          // On crée ensuite des compteurs necessaire pour incrémenter chacun des tableaux et matrices 

            int compteur = 0;
            // On fait ensuite défiler les index du tableau filename contenant les informations de l'image
            for (int i = 0; i < 2; i++)
            {
                typeIm[i] = filename[compteur]; // On attribue ensuite l'information au tableau 
                compteur++; // On augmente le conteur pour faire défiler les index du tableau
            }
            typeImage = Convertir_Endian_To_Int(typeIm);
            // Les deux premiers bytes

            for (int i = 0; i < 4; i++)
            {
                tailleFich[i] = filename[compteur];
                compteur++; // On est obligé d'initalisé un index à 0 et non mettre "i" car si l'on met "i", les deux premiers cases du tableau TailleFichier serait vide. 
            }
            // bytes 2 à 5
            tailleFichier = Convertir_Endian_To_Int(tailleFich); // Cela permet de convertir TailleFichier en int. On pourra ainsi récupérer ainsi plus facilement sa valeur afin de l'utiliser à l'avenir dans d'autres méthodes 
            for (int i = 0; i < 8; i++)
            {
                offset1[i] = filename[compteur];
                compteur++;
            }

            for (int i = 0; i < 4; i++)
            {
                headerInf[i] = filename[compteur];
                compteur++;
            }
            headerInfo = Convertir_Endian_To_Int(headerInf);
            // bytes 14 et 15

            for (int i = 0; i < 4; i++)
            {

                largeurIm[i] = filename[compteur];
                compteur++;
            }
            // bytes 18 à 21.
            largeurImage = Convertir_Endian_To_Int(largeurIm); // Cela permet de convertir LargeurImage en int. On pourra ainsi récupérer ainsi plus facilement sa valeur afin de l'utiliser à l'avenir dans d'autres méthodes 

            for (int i = 0; i < 4; i++)
            {
                hauteurIm[i] = filename[compteur];
                compteur++;
            }
            // bytes 22 à 25
            hauteurImage = Convertir_Endian_To_Int(hauteurIm); // Cela permet de convertir HauteurImage en int. On pourra ainsi récupérer ainsi plus facilement sa valeur afin de l'utiliser à l'avenir dans d'autres méthodes 

            for (int i = 0; i < 2; i++)
            {
                offset2[i] = filename[compteur];
                compteur++;
            }
            for (int i = 0; i < 2; i++)
            {
                nombreB[i] = filename[compteur];
                compteur++;
            }
            // bytes 28 et 29
            nombreBits = Convertir_Endian_To_Int(nombreB); // Cela permet de convertir nombreBits en int. On pourra ainsi récupérer ainsi plus facilement sa valeur afin de l'utiliser à l'avenir dans d'autres méthodes 

            for (int i = 0; i < 24; i++)
            {
                offset3[i] = filename[compteur];
                compteur++;
            }
            int compteur1 = 54;
            image = new int[hauteurImage, largeurImage * 3];
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++) // On crée une matrice qui a pour taille [largeurImage, hauteurImage] afin de récuperer les informations de l'image (pixel) et les mettre dans une matrice que l'on pourra utiliser plus tard
                {

                    image[i, j] = Convert.ToInt32(filename[compteur1]); // On démarre g à 54 car l'information pour l'image commence au 54ème byte.
                    compteur1++; // On incrémente g pour passer au byte suivant

                }

            }
            // On définit la matrice de Pixel que l'on a crée dans les attributs. Elle prend pour dimensions la largeur de l'image et la hauteur de l'image. 

            matricepixel = new Pixel[hauteurImage, largeurImage];
            int k = 0;
            for (int i = 0; i < hauteurImage; i++)
            {
                k = 0;
                for (int j = 0; j < largeurImage; j++)
                {
                    matricepixel[i, j] = new Pixel(Convert.ToByte(image[i, k]), Convert.ToByte(image[i, k + 1]), Convert.ToByte(image[i, k + 2])); // Prends les valeurs des pixels

                    k = k + 3;
                }

            }
            // Bout de code qui permet d'afficher les valeurs des pixels. 

            //Console.WriteLine();
            //for (int i = 0; i < largeurImage; i++)
            //{
            //    for (int j = 0; j < hauteurImage; j++)
            //    {
            //        Console.Write(matricepixel[i, j].Rouge);
            //        Console.Write(matricepixel[i, j].Vert);
            //        Console.Write(matricepixel[i, j].Bleu + "|");
            //    }
            //}

        }
        #endregion

        #region proprietes
        /// <summary>
        /// Proprité de TypeImage
        /// </summary>
        public int TypeImage
        {
            get { return this.typeImage; }
            set { this.typeImage = value; }
        }
        /// <summary>
        /// Propriété TailleFichier
        /// </summary>
        public int TailleFichier
        {
            get { return this.tailleFichier; }
            set { this.tailleFichier = value; }
        }
        /// <summary>
        /// Propriété LargeurImage
        /// </summary>
        public int LargeurImage
        {
            get { return this.largeurImage; }
            set { this.largeurImage = value; }
        }
        /// <summary>
        /// Propriété HauteurImage
        /// </summary>
        public int HauteurImage
        {
            get { return this.hauteurImage; }
            set { this.hauteurImage = value; }
        }
        /// <summary>
        /// Propriété NombreBits
        /// </summary>
        public int NombreBits
        {
            get { return this.nombreBits; }
            set
            {
                this.nombreBits = value;
            }
        }
        /// <summary>
        /// Propriété de l'Image
        /// </summary>
        public int[,] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }
        /// <summary>
        /// Propriété du Header Info
        /// </summary>
        public int HeaderInfo
        {
            get { return this.headerInfo; }
            set { this.headerInfo = value; }
        }
        /// <summary>
        /// Propriété de la taille de l'Offset
        /// </summary>
        public int TailleOffset
        {
            get { return this.tailleOffest; }
            set { this.tailleOffest = value; }
        }
        // Les offest seront les tableaux servant à recueillir les informations/valeurs qui resteront inchangées de notre part.
        public byte[] Offset1
        {
            get { return this.offset1; }
            set { this.offset1 = value; }
        }
        public byte[] Offset2
        {
            get { return this.offset2; }
            set { this.offset2 = value; }
        }
        public byte[] Offset3
        {
            get { return this.offset3; }
            set { this.offset3 = value; }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Methode permettant de passer d'une image en un tableau en convertissant les int en endian
        /// </summary>
        /// <param name="file"></param>
        public void From_Image_To_File(string file)
        {
            byte[] tab = new byte[tailleFichier]; // On crée un tableau ayant la taille de l'image afin de reccueillir toute l'information du fichier
            // Puis on attribue chaque valeur de chaque attribut au tableau crée précédemment. 
            for (int i = 0; i < 2; i++)
            {
                tab[i] = Convertir_Int_To_Endian(typeImage, 2)[i]; // Contient l'information sur le type de l'image
            }

            for (int i = 2; i < 6; i++)
            {
                tab[i] = Convertir_Int_To_Endian(tailleFichier, 4)[i - 2]; // Contient l'information sur la taille de l'image
                // On revient à i-2 car on veut remplir le tableau à partir de l'index 0. Par conséquent on fait -2 pour ne pas qu'il prenne l'information se situant dans l'index 2 
            }

            for (int i = 6; i < 14; i++)
            {
                tab[i] = offset1[i - 6]; // Offest donne l'information qui est dite "inutile"
            }
            for (int i = 14; i < 18; i++)
            {
                tab[i] = Convertir_Int_To_Endian(headerInfo, 4)[i - 14]; // Contient l'information sur le header info
            }
            for (int i = 18; i < 22; i++)
            {
                tab[i] = Convertir_Int_To_Endian(largeurImage, 4)[i - 18]; // Contient l'information sur la largueur de l'Image

            }
            for (int i = 22; i < 26; i++)
            {
                tab[i] = Convertir_Int_To_Endian(hauteurImage, 4)[i - 22]; // Contient l'information sur la hauteur de l'image
            }
            for (int i = 26; i < 28; i++)
            {
                tab[i] = offset2[i - 26]; // Deuxième offset
            }
            for (int i = 28; i < 30; i++)
            {
                tab[i] = Convertir_Int_To_Endian(nombreBits, 2)[i - 28]; // Contient l'information sur le nombre de Bits
            }
            for (int i = 30; i < 34; i++)
            {
                tab[i] = offset3[i - 30]; // Troisième offset
            }
            for (int i = 34; i < 38; i++)
            {
                tab[i] = Convertir_Int_To_Endian(largeurImage * hauteurImage * 3, 4)[i - 34]; // Contient l'information sur la taille de l'image. Permettra de pouvoir agrandir ou retrecir la photo
            }
            for (int i = 38; i < 54; i++)
            {
                tab[i] = offset3[i - 30]; // Quatrième et dernier offset
            }
            int c = 54; // On crée un compteur qui débute à 54. 54 correspond à l'index où commence l'information sur les pixels de l'image (matrice de pixel)
            for (int i = 0; i < hauteurImage; i++) // La ligne correspond à la largeur de l'image
            {
                for (int j = 0; j < largeurImage; j++) // La ligne correspond à la hauteur de l'image
                {
                    // On sait que pour un tableau de Pixel, un index contient 3 pixels correspondant au 3 couleurs. Ce qui veut dire que un index de la matrice de pixel vaut 3 index du tableau "tab". 
                    tab[c] = matricepixel[i, j].Rouge; // On attribue au premier index la valeur du pixel rouge
                    tab[c + 1] = matricepixel[i, j].Vert; // On attribue au premier index la valeur du pixel vert
                    tab[c + 2] = matricepixel[i, j].Bleu; // On attribue au premier index la valeur du pixel bleu
                    c += 3; // le compteur augmente de 3 pour passer à un autre pixel
                }
            }
            File.WriteAllBytes(file, tab); // on écrit tout dans la matrice. 

        }

        /// <summary>
        /// Methode qui permet de convertir les endian en int
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            // On sait que un octet permet de représenter 28 nombres, soit 256 valeurs différentes. De plus, en little indian, on lit de droite à gauche. Par conséquent, la plus petite valeur se trouve à l'indice le plus petit
            int result = 0; // On crée un résultat qui va être la valeur en int de la conversion. On le met au début à 0
            for (int i = 0; i < tab.Length; i++) // On fait défiler les index du tableau à convertir
            {
                result += tab[i] * Convert.ToInt32(Math.Pow(256, i)); // Le resultat va correspondre à la somme de la valeur de chaque index multiplié par 256 (2 puissance 8 bits) à la puissance correspondant à l'index
                // On pourra ainsi obtenir une valeur comprise dans la variable "resultat" qui correspond à la conversion du little endian en int. 

            }
            return result;
        }
        /// <summary>
        /// Méthode qui permet de convertir les int en little endian
        /// </summary>
        /// <param name="val"></param>
        /// <param name="t1"></param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(int val, int t1)
        {
            // On entre en paramètre la valeur que l'on veut convertir ainsi que "t1", la taille du tableau
            byte[] tab1 = new byte[t1]; // On crée un tableau tampon qui a pour taille, la taille rentrer en paramètre
            if ((t1 == 2) || (t1 == 4)) // On verfie si la taille du tableau est égal à 2 ou 4 (car on travaille sur des tableaux à 2 ou 4 bytes)
            {

                for (int i = tab1.Length - 1; i >= 0; i--) // on effectue l'inverse de ce qui a été fait plus haut. Cette fois-ci, on part du dernier index du tableau pour aller au premier
                {
                    tab1[i] = Convert.ToByte(val / Convert.ToInt32((Math.Pow(256, i)))); // Le tableau prend la valeur de la valeur entré en paramètre divisé par 256 élevé à la puissance de l'index
                    val %= Convert.ToInt32((Math.Pow(256, i))); // Il faut ensuite que la valeur soit égale à la valeur - la valeur qui a été rentré dans l'index. On peut donc utiliser le reste de la division

                    // Ainsi, lors de la convertion inverse, on obtiendra pas une valeur immensément grande
                }

            }
            return tab1; // On retourne à la fin le tableau en question

        }

        /// <summary>
        /// Méthode qui permet de faire une rotation de l'image
        /// </summary>
        public void Rotation90()
        {

            Pixel[,] matricepixel2 = new Pixel[largeurImage, hauteurImage]; // On crée un tableau de pixel ayant pour première dimension les colonnes, et en deuxième dimension les lignes
                                                                            // En effet, l'image effectue une rotation donc les dimensions s'inversent

            // On fait défiler la matrice
            int ligne = 0; // On se fixe sur la ligne 0
            int colonne = largeurImage - 1; // On se place sur la dernière colonne
            for (int i = 0; i < largeurImage; i++)
            {
                ligne = 0;
                for (int j = 0; j < hauteurImage; j++)
                {
                    matricepixel2[i, j] = matricepixel[ligne, colonne]; // La ligne ne bouge pas, cependant la colonne devient la taille de la colonne - la colonne en question-1 (par rapport à la symétrie)
                    // On incrémente ensuite la ligne pour remplir toute la colonne
                    ligne++;
                }
                colonne--;
                //Puis on passe à la colonne précdente et on repète la même action 
            }
            //On n'oublie pas de remettre les bonnes dimensions
            matricepixel = matricepixel2;
            int temp = largeurImage;
            largeurImage = hauteurImage;
            hauteurImage = temp;


        }
        /// <summary>
        /// Méthode qui effectue une rotation a 180 degrés de l'image
        /// </summary>
        public void Rotation180()
        {
            int a = 0;
            do
            {
                Pixel[,] matricepixel2 = new Pixel[largeurImage, hauteurImage]; // On crée un tableau de pixel ayant pour première dimension les colonnes, et en deuxième dimension les lignes
                                                                                // En effet, l'image effectue une rotation donc les dimensions s'inversent

                // On fait défiler la matrice
                int ligne = 0;
                int colonne = largeurImage - 1;
                for (int i = 0; i < largeurImage; i++)
                {
                    ligne = 0;
                    for (int j = 0; j < hauteurImage; j++)
                    {
                        matricepixel2[i, j] = matricepixel[ligne, colonne]; // La ligne ne bouge pas, cependant la colonne devient la taille de la colonne - la colonne en question-1 (par rapport à la symétrie)
                        ligne++;
                    }
                    colonne--;
                }

                matricepixel = matricepixel2;
                int temp = largeurImage;
                largeurImage = hauteurImage;
                hauteurImage = temp;
                a++;


            } while (a != 2);


        }
        /// <summary>
        /// Méthode qui effectue une rotation a 270 degrés de l'image
        /// </summary>
        public void Rotation270()
        {
            // On réalise trois fois la méthode rotation 90 degrés
            int a = 0;
            do
            {
                Pixel[,] matricepixel2 = new Pixel[largeurImage, hauteurImage];

                // On fait défiler la matrice
                int ligne = 0;
                int colonne = largeurImage - 1;
                for (int i = 0; i < largeurImage; i++)
                {
                    ligne = 0;
                    for (int j = 0; j < hauteurImage; j++)
                    {
                        matricepixel2[i, j] = matricepixel[ligne, colonne]; // La ligne ne bouge pas, cependant la colonne devient la taille de la colonne - la colonne en question-1 (par rapport à la symétrie)
                        ligne++;
                    }
                    colonne--;
                }

                matricepixel = matricepixel2;
                int temp = largeurImage;
                largeurImage = hauteurImage;
                hauteurImage = temp;

                a++;

            } while (a != 3);
        }
        /// <summary>
        /// Méthode qui permet de faire l'image avec un effet miroir
        /// </summary>
        public void EffetMiroir()
        {
            // Les dimensions de la matrice sont les même
            Pixel[,] matricepixel3 = new Pixel[hauteurImage, largeurImage];
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < (largeurImage); j++)
                {
                    // La ligne reste la même car on inverse juste les colonnes
                    // La colonne devient la colonne maximale moi la colonne initiale 
                    matricepixel3[i, (largeurImage - j - 1)] = matricepixel[i, j];

                }

            }
            matricepixel = matricepixel3;

        }
        /// <summary>
        /// Méthode permettant de mettre l'image en nuance de gris
        /// </summary>
        public void Gris()
        {
            // On parcourt toute la matrice Image
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    matricepixel[i, j].gris(); // On fait le gris pour chacun des index contenant les pixel 
                }
            }
        }
        /// <summary>
        /// Méthode permettant de mettre l'image en noir et blanc
        /// </summary>
        public void NoirEtBlanc()
        {
            // On parcourt la matrice
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    matricepixel[i, j].NoirEtBlanc(); // On attribue la méthode Noir et Blanc à la matrice afin d'obtenir une image noire et blanche. 
                }
            }
        }
        /// <summary>
        /// Méthode permettant de rétrécir l'image
        /// </summary>
        public void Retrecissement()
        {
            Console.WriteLine("Combien de fois voulez-vous réduire l'image");
            int retrecissement = Convert.ToInt32(Console.ReadLine()); // L'utilisateur entre un entier qui correspond au nombre de fois que l'image sera retrecie 
            if (retrecissement % 2 != 0)
            {
                retrecissement++;
            }
            Pixel[,] little = new Pixel[hauteurImage / retrecissement, largeurImage / retrecissement]; // On crée une nouvelle matrice de pixel avec pour dimensions la hauteur et la largeur de l'image initiale; le tout divisé par le rétrecissement voulu
            int a = 0; // on crée deux variables qui serivront à parcourir l'image
            int b = 0;
            for (int i = 0; i < hauteurImage; i += retrecissement) // i s'incréemente avec un pas correspond au rétrecissement
            {
                for (int j = 0; j < largeurImage; j += retrecissement) // j de même
                {
                    little[a, b] = matricepixel[i, j]; // L'image rétrecie est égale à la matrice initiale en sachant que cette dernière s'incrémente plus vite que la matrice little ce qui va permettre le rétrecissement de l'image.  
                    b++; // On incrémente les colonnes uniquement
                }
                b = 0; // On reveint à la première colonne....
                a++; // ... Puis on incrémente les lignes ce qui permettra de parcourir toute l'image
            }
            matricepixel = little; // On remplace notre matrice initiale par la nouvelle matrice rétrécie 
            largeurImage /= retrecissement; // Dont la largeur est l'initiale divisée par le rétrécissement
            hauteurImage /= retrecissement; // De même pour la hauteur
        }
        /// <summary>
        /// Méthode permettant d'agrandir une image
        /// </summary>
        public void Agrandissement()
        {
            Console.WriteLine("Combien de fois voulez-vous agrandir l'image ?");
            int agrandissement = Convert.ToInt32(Console.ReadLine());
            //On agrandi la matrice avec un facteur determiné par l'utilisateur
            Pixel[,] big = new Pixel[hauteurImage * agrandissement, largeurImage * agrandissement];
            // On fait défiler les indices de la matrice
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    // On s'est donc situé sur un index de la matrice 

                    for (int a = i * agrandissement; a < i * agrandissement + agrandissement; a++)
                    {
                        for (int b = j * agrandissement; b < j * agrandissement + agrandissement; b++)
                        {
                            big[a, b] = matricepixel[i, j];
                        }
                    }
                }
            }
            matricepixel = big;
            largeurImage *= agrandissement;
            hauteurImage *= agrandissement;
            tailleFichier = largeurImage * hauteurImage * 3 + 54;
        }

        /// <summary>
        /// Méthode permettant d'appliquer un filtre à une image grâce à la matrice de convolution
        /// </summary>
        /// <param name="filtre"></param>matrice de convolution (filtre)
        public void Convolution(int[,] filtre)
        {
            Pixel[,] mat_filtre = new Pixel[hauteurImage, largeurImage];
            int diviseur = 0;
            for (int i = 0; i < filtre.GetLength(0); i++)
            {
                for (int j = 0; j < filtre.GetLength(1); j++)
                {
                    diviseur = diviseur + filtre[i, j];
                }
            }
            if (diviseur == 0)
            {
                diviseur = 1;
            }
            Pixel[,] mat_temporaire = new Pixel[3, 3]; // On crée une matrice temporaire pour stocker les valeurs du produit de la matrice avec le filtre (matrice de convolution)
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    mat_filtre[i, j] = new Pixel(0, 0, 0); // Pour les bords (qui n'ont pas de voisins) on crée des bords noirs
                }
            }
            for (int i = 1; i < hauteurImage - 1; i++) // On parcourt la grande matrice
            {
                for (int j = 1; j < largeurImage - 1; j++)
                {
                    int a = -1; // On crée 2 indexs temporaires pour appliquer l'opération à chaque fois
                    int b = -1;
                    for (int c = i - 1; c < i + 2; c++) // On parcourt les voisins de chaque pixel de l'image
                    {
                        a++; // Dans cette boucle, on parcourt juste les lignes et les colonnes reviennent à zéro (cf cahier quand on arrive au bout d'une colonne on revient à la première et on incrémente les lignes)
                        b = 0;
                        for (int d = j - 1; d < j + 2; d++)
                        {
                            //b++; // On incrémente aussi les colonnes cette fois
                            // try
                            {
                                mat_temporaire[a, b] = new Pixel(Convert.ToByte(matricepixel[c, d].Rouge * filtre[a, b]), Convert.ToByte(matricepixel[c, d].Vert * filtre[a, b]), Convert.ToByte(matricepixel[c, d].Bleu * filtre[a, b])); // On ne peut pas multiplier un pixel par un nombre donc on prend les valeurs RVB de chaque pixel que l'on multiplie par le filtre
                                // On convertir en byte les valeurs avec Convert.ToByte()
                            }
                            /*  catch(IndexOutOfRangeException) // Si il y a une erreur quelconque, on ne la prend pas en compte
                             {
                                  throw;
                              }*/
                            b++; // On incrémente aussi les colonnes cette fois
                        } // Flou outofrange autre overflow
                        // faire matrice de int plutot que byte pour ne pas être limité par le byte (<255)
                    }
                    mat_filtre[i, j] = Somme_matrice(mat_temporaire, diviseur);
                }
            }
            matricepixel = mat_filtre; // On applique la nouvelle matrice à la matricepixel initiale 
        }
        /// <summary>
        /// On crée une autre fonction pour transformer la matrice calculée en un pixel qu'on mettra dans une nouvelle matrice et qui permettra d'avoir la matrice filtrée
        /// </summary>
        /// <param name="mat"></param>matrice de pixel
        /// <param name="diviseur"></param> correspond à la somme des coefficients de la matrice de convolution (filtre) par lequel on va diviser la valeur obtenue avec le produit
        /// <returns></returns>
        public Pixel Somme_matrice(Pixel[,] mat, int diviseur)
        {
            int r = 0; // On initialise les couleurs à 0
            int v = 0;
            int b = 0;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    r += mat[i, j].Rouge;
                    v += mat[i, j].Vert;
                    b += mat[i, j].Bleu;
                }
            }
            r = r / diviseur;
            v = v / diviseur;
            b = b / diviseur;
            return new Pixel(Convert.ToByte(r), Convert.ToByte(v), Convert.ToByte(b));
            // On a ainsi une fonction qui transforme une matrice en un pixel en faisant aussi la division par la somme des coefficients
        }
        /// <summary>
        /// Méthode permettant d'appliquer un filtre à une image grâce à la matrice de convolution
        /// </summary>
        /// <param name="filtre"></param> matrice de convolution (filtre)
        public void Convolution1(int[,] filtre) // Nouvelle méthode matrice de convolution
        {
            Pixel[,] Newmatricepixel = new Pixel[hauteurImage, largeurImage]; // On crée une nouvelle matrice de Pixel que l'on va pouvoir modifier
            int diviseur = 0; // Le diviseur correspond à la valeur avec laquelle on va diviser la somme des pixels pour le filtre
            for (int i = 0; i < filtre.GetLength(0); i++) // On fait défiler la matrice du filtre
            {
                for (int j = 0; j < filtre.GetLength(1); j++)
                {
                    diviseur = diviseur + filtre[i, j]; // On dit que le diviseur est égale à la somme des valeurs de chacun des index de la matrice filtre
                }
            }
            if (diviseur == 0) // Afin d'eviter le cas ou le dvisieur est égale à 0 et ainsi ne pas se retrouver par une division par 0 (ce qui n'existe pas)
            {
                diviseur = 1; // On dit que le diviseur est égal à 0
            }
            for (int i = 0; i < hauteurImage; i++) // On fait ensuite défiler les index de la nouvelle matrice de Pixel
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    try
                    {
                        Newmatricepixel[i, j] = new Pixel(0, 0, 0); // On remplie la matrice de 0
                    }
                    catch (OutOfMemoryException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            // On s'intéresse ensuite aux indexs qui ne se situent pas sur les bords
            for (int i = 1; i < hauteurImage - 1; i++) // On va donc de la ligne 1 a la taille -1
            {
                for (int j = 1; j < largeurImage - 1; j++) // On va donc de la colonne a la taille -1
                {
                    // On initialise la somme des pixels qui vont nous servir pour faire la somme à diviser par le diviseur
                    int sommerouge = 0;
                    int sommevert = 0;
                    int sommebleu = 0;
                    bool result = false;
                    for (int a = -1; a < 2; a++) // On part de la ligne avant l'index d'étude jusqu'a la ligne qui se trouve apres celle de l'index d'étude
                    {
                        for (int b = -1; b < 2; b++) // On part de la colonne avant l'index d'étude jusqu'a la colonne qui se trouve apres celle de l'index d'étude
                        {

                            if ((i + a < 0) || (i + a > hauteurImage - 1) || (j + b < 0) || (j + b > largeurImage - 1)) // on regarde si la condition aux bords de la matrice est respecté 
                            {

                                result = true;

                            }
                            else
                            {
                                // On dit que la somme de chacun des pixels est égale à l'index de la matrice pixel ou on se situe + la somme de ses voisins
                                //Que l'on mutliplie au valeur des index de la matrice filtre
                                sommerouge += (matricepixel[i + a, j + b].Rouge * filtre[a + 1, b + 1]);
                                sommevert += (matricepixel[i + a, j + b].Vert * filtre[a + 1, b + 1]);
                                sommebleu += (matricepixel[i + a, j + b].Bleu * filtre[a + 1, b + 1]);

                            }

                        }
                    }

                    if (result == true)
                    {
                        Newmatricepixel[i, j] = new Pixel(0, 0, 0); // Si on se situe sur les bords, on met à 0 la valeur
                    }
                    else
                    {
                        // Sinon on procède à la division des sommes obtenues par le diviseur
                        sommerouge = sommerouge / diviseur;
                        sommevert = sommevert / diviseur;
                        sommebleu = sommebleu / diviseur;
                        // On est sur une matrice de int que l'on doit convertir en byte
                        // Une valeur en byte varie de 0 à 255
                        // Il faut donc que la valeur soit au minimum 0 ou au maximum 255 pour éviter les problèmes System.Overflow
                        // On borne donc entre 0 et 255
                        sommerouge = Math.Max(0, Math.Min(255, sommerouge));
                        sommevert = Math.Max(0, Math.Min(255, sommevert));
                        sommebleu = Math.Max(0, Math.Min(255, sommebleu));
                        // Puis on attribue la somme à la nouvelle matrice 
                        Newmatricepixel[i, j] = new Pixel(Convert.ToByte(sommerouge), Convert.ToByte(sommevert), Convert.ToByte(sommebleu));
                    }
                }
            }
            // Puis ont attribue ces valeurs à la matrice de Pixel initale
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    matricepixel[i, j].Rouge = Newmatricepixel[i, j].Rouge;
                    matricepixel[i, j].Vert = Newmatricepixel[i, j].Vert;
                    matricepixel[i, j].Bleu = Newmatricepixel[i, j].Bleu;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de déterminer la valeur maximale présente au sein d'une matrice
        /// </summary>
        /// <param name="matrice"></param>
        /// <returns></returns>
        public int ValeurMaximum(int[,] matrice)
        {
            // on crée une variable qui va être égale à la valeur maximale 
            int result = 0;
            // On parcoure les index de la matrice
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    // Si la valeur de result est inférieur à la valeur de l'index suivant de la matrice, result prend cette valeur
                    if (matrice[i, j] > result)
                    {
                        result = matrice[i, j];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Méthode permettant de prendre en compte les valeurs de la somme pour faire un histogramme
        /// </summary>
        public void Histogramme1()
        {
            Pixel[,] matricepixel2 = new Pixel[largeurImage, hauteurImage]; // On crée une matrice de pixel 
            int a = 0;
            int[][] tab = new int[2][]; // On crée un double tableau
            // Un tableau comprendre les valeurs de 0 à 255
            tab[0] = new int[256];
            for (int i = 0; i < 256; i++)
            {
                tab[0][i] = a;
                a++;
                tab[1][i] = 0;
            }
            for (int i = 0; i < matricepixel2.GetLength(0); i++)
            {
                for (int j = 0; j < matricepixel2.GetLength(1); j++)
                {
                    tab[1][matricepixel2[i, j].Rouge]++;
                    tab[1][matricepixel2[i, j].Vert]++;
                    tab[1][matricepixel2[i, j].Bleu]++;


                }
            }

        }
        /// <summary>
        /// Méthode permettant de prendre en compte les valeurs de la somme pour faire un histogramme
        /// </summary>
        /// <returns></returns>
        public int[,] MatriceHistogramme()
        {
            // On crée une matrice avec 3 lignes et 256 colonnes
            // 3 lignes pour RVB
            // 256 colonnes pour les valeurs comprises entre 0 et 255
            int[,] matriceHistogramme = new int[3, 256];
            for (int i = 0; i < matricepixel.GetLength(0); i++)
            {
                for (int j = 0; j < matricepixel.GetLength(1); j++)
                {
                    int rouge = matricepixel[i, j].Rouge; // On attribue la valeur du pixel rouge
                    int vert = matricepixel[i, j].Vert; // On attribue la valeur du pixel vert
                    int bleu = matricepixel[i, j].Bleu; // On attribue la valeur du pixel bleu
                    //On incrémente l'index correspondant à la ligne représentant la couleur associé du pixel pour voir le nombre de valeur
                    matriceHistogramme[0, rouge]++;
                    matriceHistogramme[1, vert]++;
                    matriceHistogramme[2, bleu]++;

                }
            }
            return matriceHistogramme;
        }
        /// <summary>
        /// Matrice de l'histogramme
        /// </summary>
        public void Histo()
        {
            // On crée une matrice qui va nous sevir d'histogramme
            //On donne donc les valeurs de MatriceHistogramme à cette matrice
            int[,] matriceHistogramme = MatriceHistogramme();
            //Notre histogramme aura une valeur max qui sera donné par un des trois pixels
            // On fait donc en sorte que la hauteur de la matrice histogramme soit plus grande que cette valeur afin de ne pas avoir la dernière valeur sur le bord
            // En ce qui concerne la largeur (les colonnes) on fait 256*3 car il y'a a 3 histogrammes prenant une valeur de 0 à 255. On ajoute +40 pour avoir un écart entre ces histogrammes
            int max = ValeurMaximum(matriceHistogramme) / 4;
            Pixel[,] matricepixeltemporaire = new Pixel[(max * 4) + 20, (256 * 3) + 40];
            // On fait défiler les index de la matrice
            for (int i = 0; i < matricepixeltemporaire.GetLength(0); i++)
            {
                for (int j = 0; j < matricepixeltemporaire.GetLength(1); j++)
                {
                    matricepixeltemporaire[i, j] = new Pixel(255, 255, 255); // On initialse l'histogramme à blanc pour avoir un fond permettant ainsi d'afficher les histogrammes en couleur

                }
            }
            // On va de 0 à 255 pour le premier histogramme
            for (int j = 0; j < 256; j++)
            {
                for (int i = 0; i <= matriceHistogramme[0, j]; i++)
                {
                    //On met le vert et le bleu à 0. On a eu un bug ce qui fait que l'on doit mettre .Rouge à la place de .Bleu
                    matricepixeltemporaire[i, j].Rouge = 0;
                    matricepixeltemporaire[i, j].Vert = 0;
                }
            }
            //On part de 266 pour avoir un écart de 10 entre les deux histogrammes
            for (int j = 266; j < 522; j++)
            {
                for (int i = 0; i <= matriceHistogramme[1, j - 266]; i++)
                {
                    //On met rouge et bleu à 0 pour afficher le vert
                    matricepixeltemporaire[i, j].Rouge = 0;
                    matricepixeltemporaire[i, j].Bleu = 0;
                }
            }
            //On part de 532 pour avoir un écart de 10 entre les deux histogrammes
            for (int j = 532; j < 788; j++)
            {
                for (int i = 0; i <= matriceHistogramme[2, j - 532]; i++)
                {
                    //On met le bleu et le vert à 0 ce qui nous donne du rouge (contradictoire mais fonctionne pour afficher le bleu)
                    matricepixeltemporaire[i, j].Bleu = 0;
                    matricepixeltemporaire[i, j].Vert = 0;
                }
            }
            // On n'oublie pas de faire les changements pour les dimensions 
            matricepixel = matricepixeltemporaire;
            largeurImage = matricepixeltemporaire.GetLength(1);
            hauteurImage = matricepixeltemporaire.GetLength(0);
            tailleFichier = (largeurImage * hauteurImage * 3) + 54;
        }
        /// <summary>
        /// Méthode qui créer un filtre faisant un dégré de couleur
        /// </summary>
        public void Degradé()
        {
            Pixel[,] newmatricepi = new Pixel[hauteurImage, largeurImage]; // On crée une matrice temporaire ayant les mêmes dimensions que la matrice pixel
            int couleur = 1; // On fixe la couleur a 1
            // On fait défilé les index de la matrice
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    // on fixe une valeur pour le pixel rouge et on fait varier les couleurs bleus et verts avec une différence de 1 pour créer le dégradé de couleurs
                    newmatricepi[i, j] = new Pixel(20, Convert.ToByte(255 - couleur), Convert.ToByte(couleur));
                }
                couleur++;
                //On fait attention que couleur ne dépasse pas 255 (Valeur max du type byte)
                if (couleur == 256)
                {
                    couleur = 1;
                }
            }
            // On applique ensuite une superposition des deux images
            // Matricepixel aura donc les valeurs moyennes des pixels entre matricepixel et la matrice intermediaire afin d'appliquer le filtre dégradé à l'image
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    matricepixel[i, j].Rouge = (Convert.ToByte((matricepixel[i, j].Rouge + newmatricepi[i, j].Rouge) / 2));
                    matricepixel[i, j].Vert = (Convert.ToByte((matricepixel[i, j].Vert + newmatricepi[i, j].Vert) / 2));
                    matricepixel[i, j].Bleu = (Convert.ToByte((matricepixel[i, j].Bleu + newmatricepi[i, j].Bleu) / 2));
                }
            }
        }

        /// <summary>
        /// Fractale utilisant la classe Complexes
        /// </summary>
        public void Fractale()
        {
            Pixel[,] fractale = new Pixel[1000, 1000];
            matricepixel = fractale;
            tailleFichier = 54 + largeurImage * hauteurImage * 3;
            for (int x = 0; x < hauteurImage; x++)
            {
                for (int y = 0; y < largeurImage; y++)
                {
                    double a = (double)(x - (hauteurImage / 2)) / (double)(hauteurImage / 4);
                    double b = (double)(y - (largeurImage / 2)) / (double)(largeurImage / 4);
                    Complexe c = new Complexe(a, b);
                    Complexe z = new Complexe(0, 0);
                    int it = 0;
                    while (it < 100)
                    {
                        it++;
                        z.Square();
                        z.Add(c);
                        if (z.Magnitude() > 2) break;
                    }
                    if (it < 100)
                    {
                        matricepixel[x, y] = new Pixel(0, 0, 0);
                    }
                    else
                        matricepixel[x, y] = new Pixel(255, 255, 255);

                }

            }
            //    From_Image_To_File("fractale.bmp");
        }


    }
    #endregion

}


