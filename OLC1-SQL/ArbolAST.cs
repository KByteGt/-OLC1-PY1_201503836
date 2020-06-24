using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
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

        private List<Token> tupla;
        private List<TablaSQL> listaTablas;

        public ArbolAST(NodoAST raiz, List<TablaSQL> lista)
        {
            this.raiz = raiz;
            this.listaTablas = lista;
        }

        public List<TablaSQL> getTablas()
        {
            return listaTablas;
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
            //Console.WriteLine(padre.imprimir());
            foreach (NodoAST n in padre.getHijos())
            {
                //rutas += n.getRutaGraphviz();

                getDot(n);
            }
        }

        /// <summary>
        /// 
        /// </summary>

        private int estado = 0;

        private TablaSQL tabla, tempT;
        private int indexTabla;
        private bool encontrado;
        private String nombreTabla;
        public void ejecutarAcciones()
        {
            Console.WriteLine(" ** Ejecutando instrucciones **\n\n");
            accion(0,this.raiz);
        }

        private void accion(int e, NodoAST padre)
        {
            if(padre.getTipo() == TokenSQL.NO_TERMINAL)
            {   //El nodo es un No terminal
                estado = getEstado(padre.getValor());
            } 
            else
            {   //El nodo es un terminal
                ejecutar(e, padre.getTipo(), padre.getToken());
            }
            foreach (NodoAST h in padre.getHijos()) { accion(estado, h); }
        }

        private void ejecutar(int e, TokenSQL tipo, Token token)
        {
            switch (e)
            {
                case 0: //Estado Raiz
                    break;
                case 10: //CREAR TABLA ID ( L_CAMPOS ) ;
                    switch (tipo)
                    {
                        case TokenSQL.ID: //Nombre de la tabla
                            Console.WriteLine("Nombre tabla: " + token.getLexema());
                            tabla = new TablaSQL(token.getLexema());
                            break;
                        case TokenSQL.CL_FL: //Fin de linea (;)
                            Console.WriteLine("Agregando tabla " + tabla.getNombre() + " a la memoria...");
                            listaTablas.Add(tabla);
                            break;
                        case TokenSQL.CL_PARENTESIS_1: //Parentesis (
                            estado = 11;
                            break;
                        default:
                            Console.WriteLine("[" + e + "] Token: " + token.getLexema());
                            break;
                    }
                    break;
                case 11: //ID TIPO_DATO
                    switch (tipo)
                    {
                        case TokenSQL.ID: //Nombre de columna
                            Console.WriteLine("Nombre columna: " + token.getLexema());
                            tabla.addColumna(token.getLexema());
                            break;
                        case TokenSQL.CL_PARENTESIS_2: //Parentesis )
                            estado = 10;
                            break;
                        default:
                            Console.WriteLine("[" + e + "] Token: " + token.getLexema());
                            break;
                    }
                    break;
                case 20: //INSERTAR EN ID VALORES ( L_VALORES ) ;
                    switch (tipo)
                    {
                        case TokenSQL.ID: //Nombre tabla
                            nombreTabla = token.getLexema();
                            Console.WriteLine("Insertar en: " + nombreTabla);
                            break;
                        case TokenSQL.CL_PARENTESIS_1: //Parentesis (
                            tupla = new List<Token>();
                            estado = 22;
                            break;
                        case TokenSQL.CL_FL: //Fin de linea (;)
                            indexTabla = buscarTabla(nombreTabla);
                            if (encontrado)
                            {
                                listaTablas[indexTabla].addTupla(tupla);
                            }
                            Console.WriteLine("Index de tabla: " + indexTabla);
                            break;
                        default:
                            Console.WriteLine("[" + e + "] Token: " + token.getLexema());
                            break;
                    }
                    break;
                case 22: //VALOR COMA
                    switch (tipo)
                    {
                        case TokenSQL.ENTERO:
                            Console.WriteLine("Dato: " + token.getLexema());
                            tupla.Add(token);
                            break;
                        case TokenSQL.CADENA:
                            Console.WriteLine("Dato: " + token.getLexema());
                            tupla.Add(token);
                            break;
                        case TokenSQL.FECHA:
                            Console.WriteLine("Dato: " + token.getLexema());
                            tupla.Add(token);
                            break;
                        case TokenSQL.FLOTANTE:
                            Console.WriteLine("Dato: " + token.getLexema());
                            tupla.Add(token);
                            break;
                        case TokenSQL.CL_PARENTESIS_2: //Parentesis )
                            estado = 20;
                            break;
                        default:
                            Console.WriteLine("[" + e + "] Token: " + token.getLexema());
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("[" + e + "] Token: " + token.getLexema());
                    break;
            }
        }

        private int getEstado(String no_terminal)
        {
            switch (no_terminal)
            {
                case "RAIZ": return 0;
                case "INSTRUCCION CREAR TABLA": return 10;
                case "INSTRUCCION INSERTAR": return 20;
                //case "LISTA DE CAMPOS": return estado + 1;
                //case "LISTA VALORES": return estado + 2;
                default: return estado;
            }
        }

        private int buscarTabla(String nombre)
        {
            encontrado = false;
            int num = -1;
            for(int i = 0; i < listaTablas.Count(); i++)
            {
                if (listaTablas[i].getNombre().Equals(nombre))
                {
                    encontrado = true;
                    num = i;
                }
            }
            return num;
        }
    }
}
