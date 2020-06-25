using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class TablaSQL
    {
        private String nombre;
        private int id;
        private List<TuplaSQL> registros;
        private List<String> columnas;

        private int indexCol = 0, indexRow = 0;
        private String html;

        public TablaSQL(String nombre)
        {
            this.nombre = nombre;
            this.id = 0;//Id de tupla
            registros = new List<TuplaSQL>();
            columnas = new List<string>();
        }

        public void addColumna(String nombre)
        {   //Acción para agregar columna
            columnas.Add(nombre);
        }

        public void addTupla(List<Token> datos)
        {   //Acción para 
            registros.Add(new TuplaSQL(id, datos));
            this.id++;
        }
        /// <summary>
        /// Actualizar un o variso campos
        /// </summary>
        /// <param name="nombreCol"></param>
        /// <param name="op"></param>
        /// <param name="Valor"></param>
        /// <param name="ColUpdate"></param>
        /// <param name="valorUpdate"></param>
        public void updateDatos(String nombreCol, TokenSQL op, Token valor, List<NodoActualizar> lista)
        {   //Actualiar datos
            List<int> indexs = new List<int>();
            int indexUpdate;
            int indexCol = getIdColumna(nombreCol);
            

            Console.WriteLine("Condición actualizar: [" + indexCol + "] " + nombreCol + " " + op + " " + valor.getLexema());

            if (indexCol >= 0)
            {
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (registros[i].condicion(indexCol, op, valor))
                    {
                        indexs.Add(i);
                    }
                }

                //Actualizar Registro
                foreach (NodoActualizar dato in lista)
                {
                    indexUpdate = getIdColumna(dato.columna);
                    if (indexUpdate >= 0)
                    {
                        foreach (int i in indexs)
                        {
                            Console.WriteLine("Actualizar registro id: " + i);
                            registros[i].actualizar(indexUpdate, dato.valor);

                        }
                    }
                }
            }
        }
        /// <summary>
        /// Actualizar filas de tabla
        /// </summary>
        /// <param name="lista"></param>
        public void updateDatos(List<NodoActualizar> lista)
        {
            int indexUpdate;

            foreach(NodoActualizar dato in lista)
            {
                indexUpdate = getIdColumna(dato.columna);
                if(indexUpdate >= 0)
                {
                    foreach(TuplaSQL reg in registros)
                    {
                        reg.actualizar(indexUpdate, dato.valor);
                    }
                }
            }
        }

        /// <summary>
        /// Eliminar una tupla por medio de un condición.
        /// </summary>
        /// <param name="nombreCol"></param>
        /// <param name="op"></param>
        /// <param name="valor"></param>
        public void deleteTupla(String nombreCol, TokenSQL op, Token valor)
        {   //Eliminar tupla por id
            List<int> indexs = new List<int>();

            int indexCol = getIdColumna(nombreCol);

            Console.WriteLine("Eliminar: [" + indexCol + "] " + nombreCol + " " + op + " " + valor.getLexema());

            if(indexCol >= 0)
            {
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (registros[i].condicion(indexCol, op, valor))
                    {
                        indexs.Add(i);
                    }
                }

                //Eliminar registro 
                int eliminados = 0;
                foreach(int i in indexs)
                {
                    Console.WriteLine("Eliminando index: " + i);
                    registros.RemoveAt(i-eliminados);
                    eliminados++;

                }
            }
        }

        public int numeroRegistros()
        {
            return registros.Count();
        }

        public int numeroColumnas()
        {
            return columnas.Count();
        }

        public void imprimir()
        {
            Console.WriteLine("Tabla[" + nombre + "](Registros: " + registros.Count()+")");
            Console.Write("[ ID ]");
            foreach (String col in columnas)
            {
                Console.Write("[ " + col + " ]");
            }
            Console.WriteLine();
            foreach (TuplaSQL tupla in registros)
            {
                tupla.imprimir();
            }
            Console.WriteLine();
        }

        public String getHtml()
        {
            html = "\n\t<div class='card mb-4'>" +
                    "\n\t\t<div class='card-body'>"+
                    "\n\t\t\t<div class='d-flex justify-content-between align-items-center mb-3'>"+
                    "\n\t\t\t\t<h5 class='card-title m-0'>DB: "+ this.nombre +"</h5>"+
                    "\n\t\t\t\t<span>Registros: "+ this.registros.Count() +"</span>"+
                    "\n\t\t\t</div>"+
                    "\n\t\t\t<table class='table table-striped table-hover'>";

            html += "\n\t\t\t\t<thead>\n\t\t\t\t\t<tr>";
            foreach(String col in columnas)
            {
                html += "\n\t\t\t\t\t\t<th>"+ col +"</th>";
            }
            html += "\n\t\t\t\t\t</tr>\n\t\t\t\t</thead>";

            html += "\n\t\t\t\t<tbodky>";
            foreach(TuplaSQL reg in registros)
            {
                html += reg.getHtml();
            }
            html += "\n\t\t\t\t</tbodky>";

            html += "\n\t\t\t</table>\n\t\t</div>\n\t</div >";

            return html;
        }

        public String getNombre()
        {
            return nombre;
        }

        public int getIdColumna(String dato)
        {
            int index = -1;
            for(int i = 0; i < columnas.Count(); i++)
            {
                if (columnas[i].ToUpper().Equals(dato.ToUpper()))
                {
                    index = i;
                }
            }
            return index;
        }
    }
}
