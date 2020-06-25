using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        String nombreDot = "arbolAST.dot";
        String nombreImg = "arbolAST.jpg";

        public Archivo()
        {

        }

        public void crearHTML(String path, String archivo, String txt)
        {
            ruta = path + @"\" + archivo + ".html";
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
        
        public void crearArchivo(String path, String txt)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
                sw.WriteLine(txt);
                sw.Close();
            }
            catch (InvalidCastException e)
            {
                
            }
        }

        public void crearDot(String path, String txt)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path+"\\"+nombreDot, false, Encoding.UTF8);
                sw.WriteLine(txt);
                sw.Close();
            }
            catch (InvalidCastException e)
            {

            }
        }

        public void crearImg(String path)
        {
            Console.WriteLine("Generadno imagen ....");
            Console.WriteLine("dot -Tjpeg " + path + "\\" + nombreDot + " -o " + path + "\\" + nombreImg);
            ProcessStartInfo si = new ProcessStartInfo(@"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe");
            si.Arguments = "-Tjpg " + path + "\\" + nombreDot + " -o " + path + "\\" + nombreImg;
            try
            {
                Process.Start(si);
            }
            catch (Exception)
            {

            }
            
        }
    }
}
