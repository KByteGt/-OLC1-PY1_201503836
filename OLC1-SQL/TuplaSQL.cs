using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OLC1_SQL.Program;

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

        /// <summary>
        /// Evaluea condición
        /// </summary>
        /// <param name="indexCol"></param>
        /// <param name="op"></param>
        /// <param name="valor"></param>
        /// <returns>True = condición correcta | False = condición incorrecta</returns>
        public bool condicion(int indexCol, TokenSQL op, Token valor)
        {
            Token dato = datos[indexCol];

            if(dato.getToken() == TokenSQL.ENTERO || dato.getToken() == TokenSQL.FLOTANTE)
            {
                double x = 0;
                double y = 0;
                try
                {
                    x = Convert.ToDouble(dato.getLexema());
                    y = Convert.ToDouble(valor.getLexema());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
                
                switch (op)
                {
                    case TokenSQL.CL_IGUAL: // =
                        return (x == y);
                    case TokenSQL.CL_DIFERENTE: //!=
                        return (x != y);
                    case TokenSQL.CL_MAYOR:
                        return (x > y);
                    case TokenSQL.CL_MAYOR_IGUAL:
                        return (x >= y);
                    case TokenSQL.CL_MENOR:
                        return (x < y);
                    case TokenSQL.CL_MENOR_IGUAL:
                        return (x <= y);
                    default:
                        return false;
                }
            } 
            else
            {
                String x = dato.getLexema();
                String y = valor.getLexema();
                switch (op)
                {
                    case TokenSQL.CL_IGUAL: // =
                        return (x.Equals(y));
                    case TokenSQL.CL_DIFERENTE: //!=
                        return !(x.Equals(y));
                    default:
                        return false;
                }
            }
        }
    }
}
