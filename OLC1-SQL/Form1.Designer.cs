namespace OLC1_SQL
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.entrada = new System.Windows.Forms.RichTextBox();
            this.numLineTxt = new System.Windows.Forms.PictureBox();
            this.consola = new System.Windows.Forms.RichTextBox();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label_FilaColumna = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgresBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(779, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.label_FilaColumna,
            this.statusProgresBar,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(0, 365);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(779, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel1.Text = "SQL - español";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.entrada);
            this.splitContainer1.Panel1.Controls.Add(this.numLineTxt);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.consola);
            this.splitContainer1.Size = new System.Drawing.Size(779, 341);
            this.splitContainer1.SplitterDistance = 528;
            this.splitContainer1.TabIndex = 2;
            // 
            // entrada
            // 
            this.entrada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.entrada.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entrada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entrada.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entrada.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.entrada.Location = new System.Drawing.Point(50, 0);
            this.entrada.Name = "entrada";
            this.entrada.Size = new System.Drawing.Size(478, 341);
            this.entrada.TabIndex = 1;
            this.entrada.Text = "";
            // 
            // numLineTxt
            // 
            this.numLineTxt.Dock = System.Windows.Forms.DockStyle.Left;
            this.numLineTxt.Location = new System.Drawing.Point(0, 0);
            this.numLineTxt.MaximumSize = new System.Drawing.Size(50, 0);
            this.numLineTxt.MinimumSize = new System.Drawing.Size(50, 0);
            this.numLineTxt.Name = "numLineTxt";
            this.numLineTxt.Size = new System.Drawing.Size(50, 341);
            this.numLineTxt.TabIndex = 0;
            this.numLineTxt.TabStop = false;
            this.numLineTxt.Paint += new System.Windows.Forms.PaintEventHandler(this.numLineTxt_Paint);
            // 
            // consola
            // 
            this.consola.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.consola.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.consola.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consola.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consola.ForeColor = System.Drawing.SystemColors.Control;
            this.consola.Location = new System.Drawing.Point(0, 0);
            this.consola.Name = "consola";
            this.consola.ReadOnly = true;
            this.consola.Size = new System.Drawing.Size(247, 341);
            this.consola.TabIndex = 0;
            this.consola.Text = "";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // label_FilaColumna
            // 
            this.label_FilaColumna.ForeColor = System.Drawing.Color.White;
            this.label_FilaColumna.Name = "label_FilaColumna";
            this.label_FilaColumna.Size = new System.Drawing.Size(98, 17);
            this.label_FilaColumna.Text = "Fila 1, Columna 0";
            // 
            // statusProgresBar
            // 
            this.statusProgresBar.Name = "statusProgresBar";
            this.statusProgresBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(63, 17);
            this.toolStripStatusLabel4.Text = "Progreso...";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 387);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLineTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.RichTextBox entrada;
        private System.Windows.Forms.PictureBox numLineTxt;
        private System.Windows.Forms.RichTextBox consola;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel label_FilaColumna;
        private System.Windows.Forms.ToolStripProgressBar statusProgresBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.Timer timer1;
    }
}

