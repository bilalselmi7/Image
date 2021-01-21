using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Semestre_4
{
    /// <summary>
    /// Classe complexe utilisé par la fractale 
    /// </summary>
    public class Complexe
    {
        #region Attributs
        public double a;
        public double b;
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur de la classe Complexe 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Complexe(double a, double b)
        {
            this.a = a;
            this.b = b;
        }
        #endregion

        #region Méthodes
        public void Square()
        {
            double temp = (a * a) - (b * b);
            b = 2.0 * a * b;
            a = temp;
        }
        public double Magnitude()
        {
            return Math.Sqrt((a * a) + (b * b));
        }
        public void Add(Complexe c)
        {
            a += c.a;
            b += c.b;
        }
        #endregion
    }
}
