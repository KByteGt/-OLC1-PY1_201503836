using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class Archivo
    {
        String ruta = "";
        String entrada = "";

        public Archivo()
        {

        }

        public void crearHTML(String path, String archivo, String txt)
        {
            ruta = path + @"\" + archivo + ".html";
            //if (!File.Exists(ruta))
            //{
            //    using (StreamWriter sw = File.CreateText(ruta))
            //    {
            //        sw.WriteLine(txt);
            //    }
            //}
            try
            {
                StreamWriter sw = new StreamWriter(ruta, false, Encoding.UTF8);
                sw.WriteLine(txt);
                sw.Close();
            }
            catch (InvalidCastException e)
            {

            }
           
        }   
        public String leerArchivo(String path)
        {
            //using (StreamReader sr = File.OpenText(path))
            //{
            //    String s = "";
            //    while((s = sr.ReadLine()) != null)
            //    {
            //        entrada += s;
            //    }
            //}
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                entrada = sr.ReadToEnd();
                sr.Close();
            }
            catch (InvalidCastException e)
            {

            }
            

            return entrada;
        }
    }
}
