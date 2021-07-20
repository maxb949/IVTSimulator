using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvtSimulator.Models
{
    public class SimModel
    {
        private Random rnd;
        public SimModel()
        {
            G = 9.81;
            K1 = K2 = K3 = K4 = K5 = 0;
            rnd = new Random(DateTime.Now.Millisecond);
            L1 = new SimVariable("L1", rnd.NextDouble());
            L2 = new SimVariable("L2", rnd.NextDouble());
        }

        public double G { get; } //Гравитационная составляющая

        public double P1 { get; set; }
        public double P2 { get; set; }
        public double P3 { get; set; }
        public double P4 { get; set; }

        public double Hydrostatic1 { get; set; }
        public double Hydrostatic2 { get; set; }

        //Избыточные давления в емкостях
        public double PB1 { get; set; }
        public double PB2 { get; set; }

        //Коэффициенты проходного сечения клапанов
        public double K1 { get; set; }
        public double K2 { get; set; }
        public double K3 { get; set; }
        public double K4 { get; set; }
        public double K5 { get; set; }

        //Уровни в емкостях Е-1 и Е-2
        public SimVariable L1 { get; set; }
        public SimVariable L2 { get; set; }

        /// <summary>
        /// Метод вычисления текущей итерации
        /// </summary>
        internal void Iteration()
        {
            L1.Value = rnd.NextDouble() * 100;
            L2.Value = rnd.NextDouble() * 100;
        }
    }
}
