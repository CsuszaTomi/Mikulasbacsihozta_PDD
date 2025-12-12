using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mikulásbácsihozta_PDD.Models
{
    internal class User
    {
        public User()
        {

        }

        public User(int id, string nev, int pont1, double ido1, int pont2, double ido2, int pont3, double ido3, int legjobbpont, double legjobbido)
        {
            Id = id;
            Nev = nev;
            this.pont1 = pont1;
            this.ido1 = ido1;
            this.pont2 = pont2;
            this.ido2 = ido2;
            this.pont3 = pont3;
            this.ido3 = ido3;
            Legjobbpont = legjobbpont;
            Legjobbido = legjobbido;
        }

        public int Id { get; private set; }
        public string Nev { get; set; }
        public int pont1 { get; set; }
        public double ido1 { get; set; }
        public int pont2 { get; set; }
        public double ido2 { get; set; }
        public int pont3 { get; set; }
        public double ido3 { get; set; }
        public int Legjobbpont { get; set; }
        public double Legjobbido { get; set; }

    }
}
