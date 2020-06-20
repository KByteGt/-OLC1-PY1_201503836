using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class Parser
    {
        private NodoAST raiz;
        private List<Token> listaTokens;
        private List<Token> listaErrores;
        public Parser(List<Token> lista)
        {
            this.listaTokens = lista;
            this.raiz = new NodoAST();
            this.listaErrores = new List<Token>();
        }
        /// ////////////////////////////////////////////////////////////////////////
        public NodoAST Pars()
        {
            return raiz;
        }

        /// ////////////////////////////////////////////////////////////////////////

        private void insertarError(int error, int fila, int columna)
        {
            listaErrores.Add(new Token(TokenSQL.ERROR_SINTACTICO, getError(error), fila, columna));
        }

        private String getError(int error)
        {
            switch (error)
            {
                default:
                    return "Error Sintactico, token no en lugar";
            }
        }

        public List<Token> getErroes()
        {
            return listaErrores;
        }
    }
}
