namespace ETL_role
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLireExcel = new System.Windows.Forms.Button();
            this.dataGridViewBilan = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBilan)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLireExcel
            // 
            this.btnLireExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLireExcel.Location = new System.Drawing.Point(968, 110);
            this.btnLireExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLireExcel.Name = "btnLireExcel";
            this.btnLireExcel.Size = new System.Drawing.Size(183, 68);
            this.btnLireExcel.TabIndex = 0;
            this.btnLireExcel.Text = "Charger";
            this.btnLireExcel.UseVisualStyleBackColor = true;
            this.btnLireExcel.Click += new System.EventHandler(this.btnLireExcel_Click);
            // 
            // dataGridViewBilan
            // 
            this.dataGridViewBilan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBilan.Location = new System.Drawing.Point(12, 254);
            this.dataGridViewBilan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewBilan.Name = "dataGridViewBilan";
            this.dataGridViewBilan.RowHeadersWidth = 51;
            this.dataGridViewBilan.RowTemplate.Height = 24;
            this.dataGridViewBilan.Size = new System.Drawing.Size(1313, 321);
            this.dataGridViewBilan.TabIndex = 1;
            this.dataGridViewBilan.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(440, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Gestion Comerciale";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(41, 135);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(326, 29);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Nombre de valeurs nulles :";
            this.lblMessage.Click += new System.EventHandler(this.lblMessage_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Bilan";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 597);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewBilan);
            this.Controls.Add(this.btnLireExcel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "ETL";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBilan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLireExcel;
        private System.Windows.Forms.DataGridView dataGridViewBilan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label2;
    }
}

