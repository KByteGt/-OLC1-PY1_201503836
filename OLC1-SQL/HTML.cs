using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class HTML
    {
        String nombrePestaña, titulo, descripcion, html;
        List<Token> lista;
        public HTML(String nombre, String titulo, String descripcion)
        {
            this.nombrePestaña = nombre;
            this.titulo = titulo;
            this.descripcion = descripcion;
        }

        public String crear(List<Token> lista)
        {
            this.lista = lista;

            html = getHeader();
            html += getMain();
            html += getTabla();
            html += getFooter();

            return html;
        }

        private String getHeader()
        {
            return  "<!doctype html>\n" +
                    "<html lang = \"es\">\n" +
                    "\t<head>\n" +
                    "\t\t<meta charset = \"utf-8\">\n" +
                    "\t\t<meta name = \"viewport\" content = \"width=device-width, initial-scale=1, shrink-to-fit=no\">\n" +
                    "\t\t<link rel = \"stylesheet\" href = \"https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css\" integrity = \"sha384-9aIt2nRpC12Uk9gS9baDl411NQApFmC26EwAOH8WgZl5MYYxFfc+NcPb1dKGj7Sk\" crossorigin = \"anonymous\">\n" +
                    "\t\t<title> PY1 - "+nombrePestaña+ "</title>\n" +
                    "\t</head>\n" +
                    "<body>\n";
        }
        
        private String getMain()
        {
            return "\t\t\t<div class=\"container\">\n" +
                    "\t\t\t\t<h1 class=\"text-center text-dark\">" + titulo + "</h1>\n" +
                    "\t\t\t\t\t<p class=\"text-center text-muted\">" + descripcion + "</p>\n" +
                    "\t\t\t\t\t<table class=\"table table-hover\">\n" +
                    "\t\t\t\t\t<thead>\n" +
                    "\t\t\t\t\t\t<tr>\n" +
                    "\t\t\t\t\t\t\t<th scope = \"col\" >#</th>\n" +
                    "\t\t\t\t\t\t\t<th scope=\"col\">Token</th>\n" +
                    "\t\t\t\t\t\t\t<th scope = \"col\" > Tipo </th >\n" +
                    "\t\t\t\t\t\t\t<th scope=\"col\">Lexema</th>\n" +
                    "\t\t\t\t\t\t\t<th scope = \"col\" > Fila </th >\n" +
                    "\t\t\t\t\t\t\t<th scope=\"col\">Columna</th>\n" +
                    "\t\t\t\t\t\t</tr>\n" +
                    "\t\t\t\t\t</thead>\n";
        }
        private String getTabla()
        {
            String txt = "\t\t\t\t\t<tbody>\n";
            if(lista.Count > 0)
            {
                int i = 1;
                foreach (Token t in lista)
                {
                    txt += "\n\t\t\t\t\t\t" + t.getHTML(i);
                    i++;
                }
            }
            else
            {
                txt += "<tr><td colspan=\"6\"><p class=\"text-center\">No hay datos...</p></td></tr>";
            }
            
            txt += "\t\t\t\t\t</tbody>\n\t\t\t\t</table>\n\t\t\t</div>\n";

            return txt;
        }

        private String getFooter()
        {
            return "\t\t<footer class=\"bg - light p - 4\">\n" +
                    "\t\t\t<p class=\"text-center text-muted\">José Daniel López Gonzalez | 201503836<br/>Facultad de Ingenieria - Organización de lenguajes y compiladores 1<br>Universidad de San Carlos De Guatemala</p>\n" +
                    "\t\t</footer>\n" +
                    "\t\t<script src= \"https://code.jquery.com/jquery-3.5.1.slim.min.js \" integrity= \"sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj\" crossorigin= \"anonymous\" ></ script >\n" +
                    "\t\t<script src= \"https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js \" integrity= \"sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo\" crossorigin= \"anonymous\" ></ script >\n" +
                    "\t\t<script src= \"https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js \" integrity= \"sha384-OgVRvuATP1z7JjHLkuOU7Xw704+h835Lr+6QL9UvYjZE3Ipu6Tp75j7Bh/kR0JKI\" crossorigin= \"anonymous\" ></ script >\n" +
                    "\t</ body>\n" +
                    "</ html>";
        }
    }
}
