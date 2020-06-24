using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void updateDato(Token col, Token valor)
        {
            indexCol = columnas.BinarySearch(col.getLexema());

            if(indexCol > 0)
            {

            }
        }

        public void deleteTupla()
        {   //Eliminar tupla por id
            
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
    }
}
