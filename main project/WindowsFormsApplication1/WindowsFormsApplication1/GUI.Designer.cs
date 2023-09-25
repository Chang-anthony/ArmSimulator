namespace WindowsFormsApplication1
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_curr_end = new System.Windows.Forms.TextBox();
            this.tb_curr_deg = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button2 = new System.Windows.Forms.Button();
            this.bt_clear_qlist = new System.Windows.Forms.Button();
            this.bt_run_path = new System.Windows.Forms.Button();
            this.tb_lines = new System.Windows.Forms.TextBox();
            this.bt_add_lines = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bt_delete = new System.Windows.Forms.Button();
            this.bt_insert = new System.Windows.Forms.Button();
            this.tb_id = new System.Windows.Forms.TextBox();
            this.bt_trnandrot = new System.Windows.Forms.Button();
            this.tb_trans_rot = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_curr_end
            // 
            this.tb_curr_end.Font = new System.Drawing.Font("新細明體", 16F);
            this.tb_curr_end.Location = new System.Drawing.Point(402, 45);
            this.tb_curr_end.Margin = new System.Windows.Forms.Padding(2);
            this.tb_curr_end.Multiline = true;
            this.tb_curr_end.Name = "tb_curr_end";
            this.tb_curr_end.Size = new System.Drawing.Size(369, 167);
            this.tb_curr_end.TabIndex = 4;
            this.tb_curr_end.Text = "0,0,0,0,0,0";
            this.tb_curr_end.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_curr_deg
            // 
            this.tb_curr_deg.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tb_curr_deg.Location = new System.Drawing.Point(402, 1);
            this.tb_curr_deg.Margin = new System.Windows.Forms.Padding(2);
            this.tb_curr_deg.Name = "tb_curr_deg";
            this.tb_curr_deg.Size = new System.Drawing.Size(369, 40);
            this.tb_curr_deg.TabIndex = 6;
            this.tb_curr_deg.Text = "0,0,0,0,0,0";
            this.tb_curr_deg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5005, 5069);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(604, 364);
            this.button2.TabIndex = 32;
            this.button2.Text = "Clear q";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // bt_clear_qlist
            // 
            this.bt_clear_qlist.Location = new System.Drawing.Point(334, 173);
            this.bt_clear_qlist.Margin = new System.Windows.Forms.Padding(2);
            this.bt_clear_qlist.Name = "bt_clear_qlist";
            this.bt_clear_qlist.Size = new System.Drawing.Size(64, 39);
            this.bt_clear_qlist.TabIndex = 33;
            this.bt_clear_qlist.Text = "Clear q";
            this.bt_clear_qlist.UseVisualStyleBackColor = true;
            this.bt_clear_qlist.Click += new System.EventHandler(this.bt_clear_qlist_Click);
            // 
            // bt_run_path
            // 
            this.bt_run_path.Location = new System.Drawing.Point(334, 87);
            this.bt_run_path.Margin = new System.Windows.Forms.Padding(2);
            this.bt_run_path.Name = "bt_run_path";
            this.bt_run_path.Size = new System.Drawing.Size(64, 39);
            this.bt_run_path.TabIndex = 39;
            this.bt_run_path.Text = "Run path";
            this.bt_run_path.UseVisualStyleBackColor = true;
            this.bt_run_path.Click += new System.EventHandler(this.bt_run_path_Click);
            // 
            // tb_lines
            // 
            this.tb_lines.Font = new System.Drawing.Font("新細明體", 16F);
            this.tb_lines.Location = new System.Drawing.Point(0, 44);
            this.tb_lines.Margin = new System.Windows.Forms.Padding(2);
            this.tb_lines.Multiline = true;
            this.tb_lines.Name = "tb_lines";
            this.tb_lines.Size = new System.Drawing.Size(330, 168);
            this.tb_lines.TabIndex = 40;
            this.tb_lines.Text = "0,0,0,0,0,0";
            this.tb_lines.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bt_add_lines
            // 
            this.bt_add_lines.Location = new System.Drawing.Point(334, 44);
            this.bt_add_lines.Margin = new System.Windows.Forms.Padding(2);
            this.bt_add_lines.Name = "bt_add_lines";
            this.bt_add_lines.Size = new System.Drawing.Size(64, 39);
            this.bt_add_lines.TabIndex = 41;
            this.bt_add_lines.Text = "add path";
            this.bt_add_lines.UseVisualStyleBackColor = true;
            this.bt_add_lines.Click += new System.EventHandler(this.bt_add_lines_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 217);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(772, 168);
            this.dataGridView1.TabIndex = 42;
            // 
            // bt_delete
            // 
            this.bt_delete.Location = new System.Drawing.Point(334, 130);
            this.bt_delete.Margin = new System.Windows.Forms.Padding(2);
            this.bt_delete.Name = "bt_delete";
            this.bt_delete.Size = new System.Drawing.Size(64, 39);
            this.bt_delete.TabIndex = 43;
            this.bt_delete.Text = "delete";
            this.bt_delete.UseVisualStyleBackColor = true;
            this.bt_delete.Click += new System.EventHandler(this.bt_delete_Click);
            // 
            // bt_insert
            // 
            this.bt_insert.Location = new System.Drawing.Point(0, 1);
            this.bt_insert.Margin = new System.Windows.Forms.Padding(2);
            this.bt_insert.Name = "bt_insert";
            this.bt_insert.Size = new System.Drawing.Size(64, 39);
            this.bt_insert.TabIndex = 44;
            this.bt_insert.Text = "insert";
            this.bt_insert.UseVisualStyleBackColor = true;
            this.bt_insert.Click += new System.EventHandler(this.bt_insert_Click);
            // 
            // tb_id
            // 
            this.tb_id.Font = new System.Drawing.Font("新細明體", 16F);
            this.tb_id.Location = new System.Drawing.Point(68, 0);
            this.tb_id.Margin = new System.Windows.Forms.Padding(2);
            this.tb_id.Name = "tb_id";
            this.tb_id.Size = new System.Drawing.Size(55, 39);
            this.tb_id.TabIndex = 45;
            this.tb_id.Text = "0";
            this.tb_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bt_trnandrot
            // 
            this.bt_trnandrot.Location = new System.Drawing.Point(0, 390);
            this.bt_trnandrot.Margin = new System.Windows.Forms.Padding(2);
            this.bt_trnandrot.Name = "bt_trnandrot";
            this.bt_trnandrot.Size = new System.Drawing.Size(64, 39);
            this.bt_trnandrot.TabIndex = 46;
            this.bt_trnandrot.Text = "rot trans";
            this.bt_trnandrot.UseVisualStyleBackColor = true;
            this.bt_trnandrot.Click += new System.EventHandler(this.bt_trnandrot_Click);
            // 
            // tb_trans_rot
            // 
            this.tb_trans_rot.Font = new System.Drawing.Font("新細明體", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tb_trans_rot.Location = new System.Drawing.Point(68, 390);
            this.tb_trans_rot.Margin = new System.Windows.Forms.Padding(2);
            this.tb_trans_rot.Name = "tb_trans_rot";
            this.tb_trans_rot.Size = new System.Drawing.Size(330, 40);
            this.tb_trans_rot.TabIndex = 47;
            this.tb_trans_rot.Text = "0,0,0,0,0,0";
            this.tb_trans_rot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 496);
            this.Controls.Add(this.tb_trans_rot);
            this.Controls.Add(this.bt_trnandrot);
            this.Controls.Add(this.tb_id);
            this.Controls.Add(this.bt_insert);
            this.Controls.Add(this.bt_delete);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.bt_add_lines);
            this.Controls.Add(this.tb_lines);
            this.Controls.Add(this.bt_run_path);
            this.Controls.Add(this.bt_clear_qlist);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tb_curr_deg);
            this.Controls.Add(this.tb_curr_end);
            this.Font = new System.Drawing.Font("新細明體", 9F);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GUI";
            this.Text = " ";
            this.Load += new System.EventHandler(this.GUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_curr_end;
        private System.Windows.Forms.TextBox tb_curr_deg;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bt_clear_qlist;
        private System.Windows.Forms.Button bt_run_path;
        private System.Windows.Forms.TextBox tb_lines;
        private System.Windows.Forms.Button bt_add_lines;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bt_delete;
        private System.Windows.Forms.Button bt_insert;
        private System.Windows.Forms.TextBox tb_id;
        private System.Windows.Forms.Button bt_trnandrot;
        private System.Windows.Forms.TextBox tb_trans_rot;
    }
}