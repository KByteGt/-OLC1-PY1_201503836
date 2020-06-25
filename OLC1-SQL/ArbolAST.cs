using System;
using System.Collections.Generic;
using System.Linq;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class NodoActualizar
    {
        public String columna { get; set; }
        public Token valor { get; set; }

        public NodoActualizar(String col, Token t)
        {
            this.columna = col;
            this.valor = t;
        }
    }
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

        public void graficarArbol(String pathArchivo)
        {
            Console.WriteLine("-- Graficando el árbol AST");

            getDot(this.raiz);

            dot = "digraph G {\n\t" + nodos + "\n\t" + rutas + "\n}";
            Console.WriteLine(dot);

            Archivo a = new Archivo();
            a.crearDot(pathArchivo, dot);
            a.crearImg(pathArchivo);
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
        private int indexTabla, indexColumna;
        private bool encontrado;
        private String nombreTabla;
        //Eliminiar
        private bool condicionDonde;
        private String condicionColumna1;
        private TokenSQL condicionComparador1;
        private Token condicionValor1;
        //Actualizar
        private List<NodoActualizar> listaUpdate;
        private String nombreColumna;
        private Token nuevoValor;

        public void ejecutarAcciones()
        {
            Console.WriteLine(" ** Ejecutando instrucciones **\n\n");
            accion(0, this.raiz);
        }

        private void accion(int e, NodoAST padre)
        {
            if (padre.getTipo() == TokenSQL.NO_TERMINAL)
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
                case 30:// ELIMINAR DE ID;| ELIMINAR DE ID DONDE ID = VALOR;
                    switch (tipo)
                    {
                        case TokenSQL.ID: //Nombre tabla
                            nombreTabla = token.getLexema();
                            condicionDonde = false;
                            break;
                        case TokenSQL.PR_DONDE: //PR DONDE
                            estado = 31;
                            condicionDonde = true;
                            break;
                        case TokenSQL.CL_FL: //Fin de linea (;)
                            //Ejecutar acción a eliminar
                            Console.WriteLine("Eliminar de: " + nombreTabla);
                            eliminarEnTabla();
                            break;
                        default:
                            Console.WriteLine("[" + e + "] Token: " + token.getLexema());
                            break;
                    }
                    break;
                case 31: // Condicion DONDE - ELIMINAR: DONDE ID OP VALOR
                    switch (tipo)
                    {
                        case TokenSQL.ID:
                            condicionColumna1 = token.getLexema();
                            Console.WriteLine("Columna condición: " + condicionColumna1);
                            break;
                        case TokenSQL.CL_IGUAL: // =
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_DIFERENTE: // !=
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MAYOR: // >
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MAYOR_IGUAL: // >=
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MENOR: // <
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MENOR_IGUAL: // <=
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.ENTERO: // entero
                            condicionValor1 = token;
                            estado = 30;
                            break;
                        case TokenSQL.CADENA: // cadena
                            condicionValor1 = token;
                            estado = 30;
                            break;
                        case TokenSQL.FLOTANTE: // flotante
                            condicionValor1 = token;
                            estado = 30;
                            break;
                        case TokenSQL.FECHA: // fecha
                            condicionValor1 = token;
                            estado = 30;
                            break;
                        default:

                            break;
                    }
                    break;
                case 40: // ACTUALIZAR ID ESTABLCER ( L_ESTABLECER ) DONDE L_LISTA ;
                    switch (tipo)
                    {
                        case TokenSQL.ID:
                            nombreTabla = token.getLexema();
                            listaUpdate = new List<NodoActualizar>();
                            condicionDonde = false;
                            break;
                        case TokenSQL.CL_PARENTESIS_1:
                            estado = 41;
                            break;
                        case TokenSQL.PR_DONDE:
                            condicionDonde = true;
                            estado = 42;
                            break;
                        case TokenSQL.CL_FL:
                            actualizarDato();
                            break;
                    }
                    break;
                case 41: //L_ESTABLECER = ID = VALOR , ID = VALOR
                    switch (tipo)
                    {
                        case TokenSQL.ID:
                            nombreColumna = token.getLexema();
                            break;
                        case TokenSQL.CADENA:
                            listaUpdate.Add(new NodoActualizar(nombreColumna, token));
                            break;
                        case TokenSQL.FECHA:
                            listaUpdate.Add(new NodoActualizar(nombreColumna, token));
                            break;
                        case TokenSQL.ENTERO:
                            listaUpdate.Add(new NodoActualizar(nombreColumna, token));
                            break;
                        case TokenSQL.FLOTANTE:
                            listaUpdate.Add(new NodoActualizar(nombreColumna, token));
                            break;
                        case TokenSQL.CL_PARENTESIS_2:
                            estado = 40;
                            break;
                    }
                    break;
                case 42:
                    switch (tipo)
                    {
                        case TokenSQL.ID:
                            condicionColumna1 = token.getLexema();
                            Console.WriteLine("Columna condición: " + condicionColumna1);
                            break;
                        case TokenSQL.CL_IGUAL: // =
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_DIFERENTE: // !=
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MAYOR: // >
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MAYOR_IGUAL: // >=
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MENOR: // <
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.CL_MENOR_IGUAL: // <=
                            condicionComparador1 = tipo;
                            break;
                        case TokenSQL.ENTERO: // entero
                            condicionValor1 = token;
                            estado = 40;
                            break;
                        case TokenSQL.CADENA: // cadena
                            condicionValor1 = token;
                            estado = 40;
                            break;
                        case TokenSQL.FLOTANTE: // flotante
                            condicionValor1 = token;
                            estado = 40;
                            break;
                        case TokenSQL.FECHA: // fecha
                            condicionValor1 = token;
                            estado = 40;
                            break;
                        default:

                            break;
                    }
                    break;
                case 50:

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
                case "INSTRUCCION ELIMINAR": return 30;
                case "INSTRUCCION ACTUALIZAR": return 40;
                default: return estado;
            }
        }

        private int buscarTabla(String nombre)
        {
            encontrado = false;
            int num = -1;
            for (int i = 0; i < listaTablas.Count(); i++)
            {
                if (listaTablas[i].getNombre().Equals(nombre))
                {
                    encontrado = true;
                    num = i;
                }
            }
            return num;
        }

        private void eliminarEnTabla()
        {

            indexTabla = buscarTabla(nombreTabla);
            if (condicionDonde)
            {   //Ejecutar eliminción de tupla por coincidencia
                listaTablas[indexTabla].deleteTupla(condicionColumna1, condicionComparador1, condicionValor1);
            }
            else
            {   //Eliminar tabla
                listaTablas.RemoveAt(indexTabla);
            }
        }

        private void actualizarDato()
        {
            indexTabla = buscarTabla(nombreTabla);
            Console.WriteLine("Actualizar [" + indexTabla + "] " + nombreTabla + " - " + condicionDonde.ToString());
            if (condicionDonde)
            {
                listaTablas[indexTabla].updateDatos(condicionColumna1, condicionComparador1, condicionValor1, listaUpdate);
            }
            else
            {
                listaTablas[indexTabla].updateDatos(listaUpdate);
            }

        }
    }
}
