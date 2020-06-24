using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class TuplaSQL
    {
        private int id;
        private List<Token> datos;
        private String html;

        public TuplaSQL(int id, List<Token> lista)
        {
            this.id = id;
            this.datos = lista;
        }

        public void actualizar(int col, Token dato)
        {
            if(datos.Count() > col)
            {
                datos[col] = dato;
            }
        }

        public int getId()
        {
            return id;
        }

        public void imprimir()
        {
            Console.Write("[ " + id +" ]");
            foreach (Token t in datos)
            {
                Console.Write("[ " + t.getLexema() + " ]");
            }
            Console.WriteLine();
        }

        public String getHtml()
        {
            html = "\n\t\t\t\t\t<tr>";
            foreach(Token t in datos)
            {
                html += "\n\t\t\t\t\t\t<td>"+ t.getLexema() +"</td>";
            }
            html += "\n\t\t\t\t\t</tr>";
            return html;
        }
    }
}
