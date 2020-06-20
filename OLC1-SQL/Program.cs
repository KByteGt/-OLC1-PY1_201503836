using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_SQL
{
    static class Program
    {
        public enum TokenSQL
        {
            EOF = 0,                    //Fin de archivo
            ERROR_LEXICO = 1,           //ERROR LEXICO
            ERROR_SINTACTICO = 2,       //ERROR SINTACTICO
            LFCR = 3,                   //LF - Nueva linea, CR - salto de linea
            WS = 4,                     //Espacio en blanco
            TAB = 5,                    //Tab

            ENTERO = 10,                //Tipo de dato entero, 10
            CADENA = 11,                //Tipo de dato cadena, "Hola"
            FLOTANTE = 12,              //Tipo de dato flotante, 10.5
            FECHA = 13,                 //Tipo de dato fecha, '02/07/1997'
            ID = 14,                    //Tipo de dato id, var_1

            PR_CREAR = 20,              //Palabra reservada CREAR   
            PR_TABLA = 21,              //Palabra reservada TABLA
            PR_INSERTAR = 22,           //Palabra reservada INSERTAR
            PR_EN = 23,                 //Palabra reservada EN
            PR_VALORES = 24,            //Palabra reservada VALORES
            PR_SELECCIONAR = 25,        //Palabra reservada SELECCIONAR
            PR_DE = 26,                 //Palabra reservada DE
            PR_DONDE = 27,              //Palabra reservada DONDE
            PR_Y = 28,                  //Palabra reservada Y
            PR_O = 29,                  //Palabra reservada O
            PR_ACTUALIZAR = 30,         //Palabra reservada ACTUALIZAR
            PR_ESTABLECER = 31,         //Palabra reservada ESTABLECER
            PR_COMO = 32,               //Palabra reservada COMO
            PR_ENTERO = 33,             //Palabra reservada ENTERO
            PR_CADENA = 34,             //Palabra reservada CADENA
            PR_FLOTANTE = 35,           //Palabra reservada FLOTANTE
            PR_FECHA = 36,              //Palabra reservada FECHA

            CL_PARENTESIS_1 = 40,       // (
            CL_PARENTESIS_2 = 41,       // )
            CL_COMA = 44,               // ,
            CL_FL = 59,                 // ;
            CL_POR = 42,                // *
            CL_MAYOR = 62,              // >
            CL_MAYOR_IGUAL = 63,        // >=
            CL_MENOR = 60,              // <
            CL_MENOR_IGUAL = 64,        // <=
            CL_IGUAL = 61,              // =
            CL_DIFERENTE = 65,          // !=
            CL_PUNTO = 46,              // .

            COMENTARIO_LINEA = 100,     // -- C* LFCR
            COMENTARIO_BLOQUE = 101     // /* C* */
        }

        static List<Token> listaTokens;
        static List<Token> listaErroresLexicos;
        static List<Token> listaErroresSintacticos;

        static Color morado;
        static Color naranja;
        static Color negro;
        static Color azul;
        static Color verde;
        static Color gris;
        static Color rojo;
        static Color cafe;

        static String ruta;
       

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            listaTokens = new List<Token>();

            //prueba();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //listaTokens.Add(new Token(TokenSQL.PR_CREAR,"CREAR",3,4));
            //Token t = new Token(TokenSQL.ID, "var_1", 0, 0);
            //listaTokens.Add(t);


            //foreach (Token token in listaTokens)
            //{
            //    Console.WriteLine(token.toString());
            //}
            
        }

        private static void prueba()
        {
            String txto = "CREAR TABLA Departamento( $Id_departamento entero, nombre cadena);INSERTAR EN Departamento VALORES(0,\"Alta Verapaz\");INSERTAR EN Departamento VALORES(1,\"Baja Verapaz\");INSERTAR EN Departamento VALORES(2,\"Chimaltenango\");INSERTAR EN Departamento VALORES(3#,\"Chiquimula\");%&# ";

            Scanner sc = new Scanner(txto);

            listaTokens = sc.Scan();
            listaErroresLexicos = sc.getErrores();

            foreach(Token t in listaTokens)
            {
                Console.WriteLine(t.toString());
            }

            foreach(Token t in listaErroresLexicos)
            {
                Console.WriteLine(t.toString());
            }

            ruta = @"C:\Users\JOSED\Documents\Reportes\SQL-es";

            String descripcion = "Reporte de todos los toquens reconocidos por el programa al ejecutar el Scanner()";
            Archivo a = new Archivo();
            HTML html = new HTML("Reporte Tokens","Reporte de Tokens - SQL-es",descripcion);
            a.crearHTML(ruta, "reporteTokens", html.crear(listaTokens));

            html = new HTML("Reporte De Errores", "Reporte de Errores - SQL-es", "Errores encontrados...");
            a.crearHTML(ruta, "reporteErr", html.crear(listaErroresLexicos));
        }

    }
}
