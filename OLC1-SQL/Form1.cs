using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    public partial class Form1 : Form
    {
        //Variables globales
        int contadorPos, altura, numFila, col, row, temp, x;
        String txt_consola = " Consola: \n\n ";
        bool flagArchivo = false;
        bool flagCarpeta = false;
        bool flagEditado = false;
        String pathArchivo = "";
        //String pathCarpeta = "";
        String nombreArchivo = "Sin_titulo";

        Archivo archivo = new Archivo();
        SaveFileDialog saveFileDialog1;
        OpenFileDialog openFileDialogSQLE;

        List<Token> listaTokens;
        List<Token> listaErroresLexicos;
        List<Token> listaErroresSintacticos;
        List<TablaSQL> listaTablas;
        NodoAST raiz;


        private void nueboToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Btn - Nuevo
            limpiarEntrada();  
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Abrir archivo
            

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                abrirArchivo(openFileDialog1.FileName);
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Guardar
            guardar();
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Guardar como
            guardarComo();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Salir 
            salir();            
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Acerca de
            MessageBox.Show("José Daniel López Gonzalez - 201503836\nOrganización de Lenguajes y Compiladores 1", "Proyecto 1 - SQL español");
        }

        private void cargarTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Cargar tablas
            if (openFileDialogSQLE.ShowDialog() == DialogResult.OK)
            {
                abrirArchivo(openFileDialogSQLE.FileName);
            }
        }

        private void ejecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Ejecutar analisis léxico y sintáctico
            ejecutarAnalisis();
        }

        private void mostrarTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Mostrar reporte de tokens
            openFile(pathCarpeta + "\\reporteTokens.html");
        }

        private void mostrarErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Mostrar reporte de errores
            openFile(pathCarpeta + "\\reporteErr.html");
        }

        private void verTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {   //Mostrar tablas
            mostrarTablas();
        }

        public Form1()
        {
            ///////////////////////////////////////////////
            InitializeComponent();
            timer1.Interval = 10;
            timer1.Start();

            //timer2.Interval = 1200;
            //timer2.Start();

            statusProgresBar.Value = 0;

            listaTokens = new List<Token>();
            listaErroresLexicos = new List<Token>();
            listaErroresSintacticos = new List<Token>();
            listaTablas = new List<TablaSQL>();

            // File Dialog
            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Seleccionear un archivo SQL-es",
                Filter = "SQL JDLG (*.jdlg)|*.jdlg",
                Title = "Abrir archivo SQL-es"
            };

            saveFileDialog1 = new SaveFileDialog()
            {
                FileName = "Sin_titulo",
                Filter = "SQL JDLG (*.jdlg)|*.jdlg|SQLE (*.sqle)|*.sqle",
                Title = "Guardar como archivo SQL-es"
            };

            openFileDialogSQLE = new OpenFileDialog()
            {
                FileName = "Seleccionear un archivo SQLE",
                Filter = "SQLE (*.sqle)|*.sqle",
                Title = "Abrir archivo SQLE"
            };


            ///////////////////////////////////////////////
        }

        /// <summary>
        /// Hilo para actualización de líneas del RichTextBox y pintado de palabras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            numLineTxt.Refresh();
            labelRowColUpdate();
            //pintarPalabras();

            if (entrada.Modified)
            {
                flagEditado = true;
            }
          
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pintarPalabras();
        }


        private void numLineTxt_Paint(object sender, PaintEventArgs e)
        {
            contadorPos = 0;

            altura = entrada.GetPositionFromCharIndex(0).Y;

            if (entrada.Lines.Length > 0)
            {
                for(int i = 0; i < entrada.Lines.Length; i++)
                {
                    numFila = i + 1;
                    e.Graphics.DrawString(numFila.ToString(), entrada.Font, Brushes.Silver, numLineTxt.Width - (e.Graphics.MeasureString(numFila.ToString(), entrada.Font).Width + 10), altura);
                    contadorPos += entrada.Lines.ElementAt(i).Length + 1;

                    altura = entrada.GetPositionFromCharIndex(contadorPos).Y;
                }
            } 
            else
            {
                e.Graphics.DrawString("1", entrada.Font, Brushes.Silver, numLineTxt.Width - (e.Graphics.MeasureString("1", entrada.Font).Width + 10), altura);
            }

        }

        private void labelRowColUpdate()
        {
            temp = entrada.SelectionStart;
            row = entrada.GetLineFromCharIndex(temp);
            col = temp - entrada.GetFirstCharIndexOfCurrentLine();

            label_FilaColumna.Text = "Fila " + (row + 1) + ", Columna " + col;

        }

        /// <summary>
        /// Propios métodos
        /// </summary>
        private void limpiarEntrada()
        {   //Limpiar área de texto
            flagArchivo = false;
            pathArchivo = "";
            entrada.Text = "";
            nombreArchivo = "*Sin Titulo";
            this.Text = nombreArchivo;
        }

        private void abrirArchivo(string fileName)
        {   //Abrir archivo y colocar el texto en entrada
            flagArchivo = true;
            pathArchivo = Path.GetDirectoryName(fileName);
            nombreArchivo = Path.GetFileName(fileName);
            this.Text = nombreArchivo;
            entrada.Text = archivo.leerArchivo(fileName);

            escribirLinea(" - Abriendo archivo: " + nombreArchivo);
        }

        

        private void guardar()
        {   //Guardar Archivo
            if (flagArchivo)
            {
                guardarArchivo(pathArchivo+"\\"+nombreArchivo);
            }
            else
            {
                guardarComo();
            }
        }

        private void guardarComo()
        {   //Guardar Archivo nuevo
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                flagArchivo = true;
                pathArchivo = Path.GetDirectoryName(saveFileDialog1.FileName);
                nombreArchivo = Path.GetFileName(saveFileDialog1.FileName);
                this.Text = nombreArchivo;
                guardarArchivo(saveFileDialog1.FileName);
            }
        }

        private void guardarArchivo(String path)
        {
            archivo.crearArchivo(path, entrada.Text);
            flagEditado = false;
        }

        private void salir()
        {
            if (entrada.Text.Length > 0 && flagEditado)
            {
                DialogResult result = MessageBox.Show("Desea guardar los cambios?", "Salir", MessageBoxButtons.YesNoCancel);

                switch (result)
                {
                    case DialogResult.Yes:
                        guardar();
                        this.Dispose();
                        break;
                    case DialogResult.No:
                        this.Dispose();
                        break;
                    case DialogResult.Cancel:

                        break;
                }             
            }
            else
            {
                this.Dispose();
            }
        }

        public void escribirLinea(String txt)
        {
            txt_consola += txt + "\n ";
            consola.Text = txt_consola;
        }

       

        //Analisis

        private void ejecutarAnalisis()
        {   //Ejecutar el análisis
            
            if(entrada.Text.Length > 0)
            {
                analisisLexico();
                pintarPalabras();

                analisisSintactico();

                if(listaErroresSintacticos.Count() == 0)
                {
                    ejecutarInstrucciones();
                }

                generarReportes();
            } else
            {
                escribirLinea(" -- No hay texto para analizar...");
            }
        }

        private void analisisLexico()
        {
            escribirLinea(" - Ejecutando análisis léxico...");

            Scanner sc = new Scanner(entrada.Text);
            listaTokens = sc.Scan();
            listaErroresLexicos = sc.getErrores();
            escribirLinea("\t* Tokens reconocidos: "+listaTokens.Count());
            escribirLinea("\t* Errores léxicos: " + listaErroresLexicos.Count());
        }

        private void analisisSintactico()
        {
            escribirLinea(" - Ejecutando análisis Sintáctico...");
            //Limpiar lista de comentarios
            List<Token> tempList = new List<Token>();
            foreach(Token t in listaTokens)
            {
                if(t.getToken() != TokenSQL.COMENTARIO_BLOQUE && t.getToken() != TokenSQL.COMENTARIO_LINEA)
                {
                    tempList.Add(t);
                }
            }
            Parser ps = new Parser(tempList);
            raiz = ps.Pars();
            listaErroresSintacticos = ps.getErroes();
            
            escribirLinea("\t* Errores sintácticos: " + listaErroresSintacticos.Count());
        }


        private void ejecutarInstrucciones()
        {
            ArbolAST ast = new ArbolAST(this.raiz, this.listaTablas);
            
            ast.graficarArbol();
            escribirLinea("\t* Árbol AST generado...");
            ast.ejecutarAcciones();

            escribirLinea("\t* Tablas: " + listaTablas.Count());
        }

        private void generarReportes()
        {
            escribirLinea(" - Generando los reportes...");

            //pathCarpeta = @"C:\Users\JOSED\source\repos\-OLC1-PY1_201503836\Reportes";

            String descripcion = "Reporte de todos los toquens reconocidos por el programa al ejecutar el Scanner()";
            Archivo a = new Archivo();
            HTML html = new HTML("Reporte Tokens", "Reporte de Tokens - SQL-es", descripcion);
            a.crearHTML(pathCarpeta, "reporteTokens", html.crear(listaTokens));

            foreach(Token t in listaErroresSintacticos)
            {
                listaErroresLexicos.Add(t);
            }

            html = new HTML("Reporte De Errores", "Reporte de Errores - SQL-es", "Errores encontrados...");
            a.crearHTML(pathCarpeta, "reporteErr", html.crear(listaErroresLexicos));
        }

        private void mostrarTablas()
        {
            escribirLinea("\t> tablas.html...");

            Archivo a = new Archivo();
            HTML html = new HTML();
            a.crearHTML(pathCarpeta, "tablas", html.crear(this.listaTablas));
            openFile(pathCarpeta + "\\tablas.html");
        }

        private void openFile(String ruta)
        {
            Process.Start(ruta);
        }

        private void pintarPalabras()
        {
            if(listaTokens.Count() > 0)
            {
                x = 0;
                foreach (Token t in listaTokens)
                {
                    x = entrada.GetFirstCharIndexFromLine(t.getFila() - 1);
                    if(t.getToken() == TokenSQL.CADENA || t.getToken() == TokenSQL.FECHA)
                    {
                        entrada.Select(x + t.getColumna() - 1, t.getLexema().Length +2);
                    }
                    else
                    {
                        entrada.Select(x + t.getColumna() - 1, t.getLexema().Length);
                    }
                    
                    entrada.SelectionColor = colorPiker(t.getToken());
                }

                entrada.Select(0, 0);
            }
           
        }

        private Color colorPiker(TokenSQL t)
        {
            switch (t)
            {
                case TokenSQL.PR_TABLA:
                    return Color.DarkOrchid;
                case TokenSQL.PR_INSERTAR:
                    return Color.DarkOrchid;
                case TokenSQL.PR_ELIMINAR:
                    return Color.DarkOrchid;
                case TokenSQL.PR_ACTUALIZAR:
                    return Color.DarkOrchid;
                case TokenSQL.FECHA:
                    return Color.Peru;
                case TokenSQL.ENTERO:
                    return Color.SteelBlue;
                case TokenSQL.FLOTANTE:
                    return Color.SteelBlue;
                case TokenSQL.CADENA:
                    return Color.Teal;
                case TokenSQL.COMENTARIO_BLOQUE:
                    return Color.Gray;
                case TokenSQL.COMENTARIO_LINEA:
                    return Color.Gray;
                case TokenSQL.ID:
                    return Color.Sienna;
                case TokenSQL.CL_IGUAL:
                    return Color.Firebrick;
                case TokenSQL.CL_DIFERENTE:
                    return Color.Firebrick;
                case TokenSQL.CL_MAYOR_IGUAL:
                    return Color.Firebrick;
                case TokenSQL.CL_MENOR_IGUAL:
                    return Color.Firebrick;
                case TokenSQL.CL_MAYOR:
                    return Color.Firebrick;
                case TokenSQL.CL_MENOR:
                    return Color.Firebrick;
                default:
                    return Color.White;
            }
        }
    }
}
