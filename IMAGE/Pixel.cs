using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Semestre_4
{
    public class Pixel
    {
        #region Attributs
        /// <summary>
        /// Les attributs de la classe Pixel 
        /// </summary>
        byte rouge; // Un pixel Rouge
        byte vert; // Un pixel Vert
        byte bleu; // Un pixel Bleu
                   // Ces 3 couleurs permettent de, en fonction de leur intensité, utiliser tout le spectre de couleurs. 
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur de la classe Pixel 
        /// </summary>
        /// <param name="rouge"></param> Pixel Rouge
        /// <param name="vert"></param> Pixel Vert
        /// <param name="bleu"></param> Pixel Bleu
        public Pixel(byte rouge, byte vert, byte bleu)
        {
            // Un pixel est définit soit par une couleur rouge, verte ou bleue.
            this.rouge = rouge;
            this.vert = vert;
            this.bleu = bleu;
        }

        #endregion

        #region Constructeur
        /// <summary>
        /// Les propriétés de la classe Pixel 
        /// </summary>
        public byte Rouge
        {
            get { return this.rouge; }
            set { this.rouge = value; }
        }
        public byte Vert
        {
            get { return this.vert; }
            set { this.vert = value; }
        }
        public byte Bleu
        {
            get { return this.bleu; }
            set { this.bleu = value; }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Méthode qui affiche les caractéristiques de la classe Pixel 
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string result = "(" + this.rouge + "," + this.vert + "," + this.bleu + ")";
            return result;
        }

        /// <summary>
        /// Méthode qui permet de mettre les pixel en gris 
        /// </summary>
        public void gris()
        {
            int gris = (this.rouge + this.vert + this.bleu) / 3; // On recupere la valeur moyenne aux 3 pixels
            rouge = (byte)gris; // On affecte cette couleur à un des pixels puis on l'attribue aux deux pixels restants
            vert = (byte)gris;
            bleu = (byte)gris;
        }

        /// <summary>
        /// Methode qui permet de mettre une image en noir et blanc 
        /// </summary>
        public void NoirEtBlanc()
        {
            int result = (this.rouge + this.vert + this.bleu) / 3; // On recupere la valeur moyenne aux 3 pixels
            if (result >= 128) // On sait que la valeur d'un pixel varie de 0 à 255 (foncé au clair)
                               // Donc si la valeur est supérieure a 129, elle tend vers le clair donc vers le blanc
            {
                // On met donc les 3 couleurs (rouge, vert et bleu) à 255 afin d'obtenir du blanc 
                rouge = 255;
                vert = 255;
                bleu = 255;
            }
            else
            // La valeur est inférieure à 129 donc tend vers le sombre donc le noir
            {
                // On met donc les 3 couleurs (rouge, vert et bleu) à 0 afin d'obtenir du noir 
                rouge = 0;
                vert = 0;
                bleu = 0;
            }
        }
        #endregion


    }
}

