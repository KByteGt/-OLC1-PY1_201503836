using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class ArbolAST
    {
        private NodoAST raiz;
        private String dot = "";
        private String nodos = "", rutas = "";

        public ArbolAST(NodoAST raiz)
        {
            this.raiz = raiz;
        }

        public void graficarArbol()
        {
            Console.WriteLine("-- Graficando el árbol AST");

            getDot(this.raiz);

            dot = "digraph G {\n\t" + nodos + "\n\t" + rutas + "\n}";
            Console.WriteLine(dot);

            Archivo a = new Archivo();
            a.crearDot(pathCarpeta, dot);
            a.crearImg(pathCarpeta);
        }

        private void getDot(NodoAST padre)
        {
            nodos += padre.getNodoGraphviz();
            rutas += padre.getRutaGraphviz();
            Console.WriteLine(padre.imprimir());
            foreach (NodoAST n in padre.getHijos())
            {
                //rutas += n.getRutaGraphviz();

                getDot(n);
            }
        }

        public void ejecutarAcciones()
        {

        }
    }
}
