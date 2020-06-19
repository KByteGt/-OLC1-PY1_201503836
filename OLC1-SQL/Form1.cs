using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_SQL
{
    public partial class Form1 : Form
    {
        //Variables globales
        int contadorPos, altura, numFila, col, row, temp;
        String txt_consola = " Consola: \n\n ";
        bool flagArchivo = false;
        bool flagCarpeta = false;
        bool flagEditado = false;
        String pathArchivo = "";
        String pathCarpeta = "";
        String nombreArchivo = "Sin_titulo";

        Archivo archivo = new Archivo();
        SaveFileDialog saveFileDialog1;


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

        public Form1()
        {
            ///////////////////////////////////////////////
            InitializeComponent();
            timer1.Interval = 10;
            timer1.Start();

            statusProgresBar.Value = 0;
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
                Filter = "SQL JDLG (*.jdlg)|*.jdlg",
                Title = "Guardar como archivo SQL-es"
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

            if (entrada.Modified)
            {
                flagEditado = true;
            }
          
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

        
    }
}
