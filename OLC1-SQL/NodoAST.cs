using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class NodoAST
    {
        private int fila, columna, id;
        private List<NodoAST> hijos;

        public NodoAST()
        {
            hijos = new List<NodoAST>();
        }

        public NodoAST(int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
            hijos = new List<NodoAST>();
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public void insertarHijo()
        {

        }
    }
}
