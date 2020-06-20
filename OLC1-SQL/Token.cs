using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class Token
    {
        private TokenSQL token { get; }
        private String lexema { get; }
        private int fila { get; }
        private int columna { get; }

        public Token(TokenSQL token, String lexema, int fila, int columna)
        {
            this.token = token;
            this.lexema = lexema;
            this.fila = fila;
            this.columna = columna;
        }

        public String toString()
        {
            return "Token: [" + (int)token + "] Tipo: " + token + ", Lexema: " + lexema + " (" + fila + "," + columna + ")";
        }

        public String getHTML(int i)
        {
            return "<tr><td>" + i + "</td><td>" + (int)token + "</td><td>" + token.ToString() + "</td><td>" + lexema + "</td><td>" + fila + "</td><td>" + columna + "</td></tr>";
        }

        public TokenSQL getToken()
        {
            return this.token;
        }

        public String getLexema()
        {
            return this.lexema;
        }

        public int getFila()
        {
            return this.fila;
        }

        public int getColumna()
        {
            return this.columna;
        }
    }
}
