using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class NodoAST
    {
        private int id;
        TokenSQL tipo;
        String valor;
        private List<NodoAST> hijos;

        public NodoAST(int id, String noTerminal)
        {
            this.tipo = TokenSQL.NO_TERMINAL;
            this.valor = noTerminal;
            hijos = new List<NodoAST>();

            setId(noTerminal);
        }

        public NodoAST(Token token)
        {
            this.tipo = token.getToken();
            this.valor = token.getLexema();
            hijos = new List<NodoAST>();

            setId(token);
        }


        public int getId()
        {
            return id;
        }

        public void setId(Token token)
        {
            if(token != null)
            {
                this.id = ((int)token.getToken() * 1000 + token.getFila()) * 1000 + token.getColumna();
            } else
            {
                this.id = -1;
            }
        }

        public void setId(String noTerminal)
        {
            int x = noTerminal.GetHashCode() * 100 + id;
            if (x < 0)
            {
                x = x * -1;
            }

            this.id = x;
        }

        public TokenSQL getTipo()
        {
            return tipo;
        }

        public void setTipo(TokenSQL tipo)
        {
            this.tipo = tipo;
        }

        public String getValor()
        {
            return valor;
        }

        public void setValor(String valor)
        {
            this.valor = valor;
        }

        public List<NodoAST> getHijos()
        {
            return hijos;
        }

        public void insertarHijo(NodoAST nodo)
        {
            this.hijos.Add(nodo);
        }

        public String imprimir()
        {
            return "Nodo[" + id + "] (token: " + tipo + ", valor: " + valor + ")";
        }

        public String getNodoGraphviz()
        {
            return "node"+id+" [label = \""+valor+"\"];\n\t";
        }

        public String getRutaGraphviz()
        {
            String temp = "";

            foreach(NodoAST n in hijos)
            {
                temp += "node" + id + " -> node" + n.getId() + ";\n\t";
            }

            return temp;
        }
    }
}
