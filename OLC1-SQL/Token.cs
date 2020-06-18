using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class Token
    {
        private int tipo;
        private String lexema;
        private double valor;
        private int fila;
        private int columna;

        Token(int tipo, String lexema, int fila, int columna)
        {
            this.tipo = tipo;
            this.lexema = lexema;
            this.fila = fila;
            this.columna = columna;
        }

        private void setValor()
        {
            if(this.tipo == T_INT || this.tipo == T_FLOAT)
            {

            }
        }
    }
}
