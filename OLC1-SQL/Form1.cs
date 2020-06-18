using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_SQL
{
    public partial class Form1 : Form
    {
        //Variables globales
        int contadorPos, altura, numFila, col, row, temp;
        String txt_consola = "\tConsola: \n\t";


        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 10;
            timer1.Start();

            statusProgresBar.Value = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            numLineTxt.Refresh();
            labelRowColUpdate();


        //If statusProgressBar Then
        //    If tiempo1 = 250 Then
        //        Me.ProgressBar.Value() = 0
        //        Me.LabelStatus.Text() = "..."
        //        statusProgressBar = False
        //        tiempo1 = 0
        //    Else
        //        tiempo1 += 1
        //    End If
        //End If
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

                    Console.WriteLine(contadorPos);

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

        public void escribirLinea(String txt)
        {
            txt_consola += txt + "\n\t";
            consola.Text = txt_consola;
        }
    }
}
