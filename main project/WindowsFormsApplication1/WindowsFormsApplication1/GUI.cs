using Path_controler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApplication1;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class GUI : Form
    {
        Kinematics kn;
        DataTable dataTable;
       
        public GUI()
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add("Index");
            dataTable.Columns.Add("X");
            dataTable.Columns.Add("Y");
            dataTable.Columns.Add("Z");
            dataTable.Columns.Add("γ");
            dataTable.Columns.Add("β");
            dataTable.Columns.Add("α");
            dataGridView1.DataSource = dataTable;//吃 dataTable 來源
        }

        public GUI(ref Kinematics kn)
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add("Index");
            dataTable.Columns.Add("X");
            dataTable.Columns.Add("Y");
            dataTable.Columns.Add("Z");
            dataTable.Columns.Add("γ");
            dataTable.Columns.Add("β");
            dataTable.Columns.Add("α");
            dataGridView1.DataSource = dataTable;
            this.kn = kn;
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        //private void btn_deg_Click(object sender, EventArgs e)
        //{
        //    string[] tmp = tb_deg.Text.Split(',');
        //    for (int i = 0; i < tmp.Length; i++)
        //    {
        //        Form1.tmp_deg[i] = double.Parse(tmp[i]);
        //    }
        //    if (Form1.thd == null)
        //    {
        //        Form1.thd = new Thread(Form1.Anima);
        //        Form1.thd.Start();
        //    }
        //    else
        //    {
        //        if (Form1.thd.ThreadState == System.Threading.ThreadState.Running)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            Form1.thd = new Thread(Form1.Anima);
        //            Form1.thd.Start();
        //        }
        //    }
        //}

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                tb_curr_deg.Invoke(new EventHandler(delegate {
                    tb_curr_deg.Text = string.Format("{0,6:F3},{1,6:F3},{2,6:F3},{3,6:F3},{4,6:F3},{5,6:F3}", Form1.message_curr_deg[0], Form1.message_curr_deg[1], Form1.message_curr_deg[2], Form1.message_curr_deg[3], Form1.message_curr_deg[4], Form1.message_curr_deg[5]);
                    
                }));

                tb_curr_end.Invoke(new EventHandler(delegate
                {

                    tb_curr_end.Text = Form1.message_curr_end.ToString();
                }));

                //tb_curr_q.Invoke(new EventHandler(delegate
                //{
                //    tb_curr_q.Text = string.Format("{0,6:F3},{1,6:F3},{2,6:F3},{3,6:F3},{4,6:F3},{5,6:F3}", Form1.message_curr_q[0], Form1.message_curr_q[1], Form1.message_curr_q[2], Form1.message_curr_q[3], Form1.message_curr_q[4], Form1.message_curr_q[5]);
                   
                //}));


                this.Invalidate();

                Thread.Sleep(10);
            }
        }

        //private void btn_pos0_Click(object sender, EventArgs e)
        //{
        //    string[] tmp = tb_pos0.Text.Split(',');
        //    double[] q = new double[tmp.Length];
        //    for (int i = 0; i < tmp.Length; i++)
        //    {
        //        q[i] = double.Parse(tmp[i]);
        //    }
        //    Matrix4x4 pos = new Matrix4x4(q[0], q[1], q[2], q[3], q[4], q[5]);//q =[x,y,z,rx,ry,rz]
        //    kn.pos[0] = pos;//覆蓋
        //}
        Matrix4x4 tag = null;
        //private void btn_q2mat_Click(object sender, EventArgs e)
        //{
        //    //string[] tmp = tb_q.Text.Split(',');
        //    double[] q = new double[Form1.message_curr_q.Length];//q = [ x, y, z, rx, ry, rz]
        //    for (int i = 0; i < Form1.message_curr_q.Length; i++)
        //    {
        //        //q[i] = double.Parse(tmp[i]);
        //        q[i] = Form1.message_curr_q[i];
        //    }
        //    //tag 世界坐標系下的末端
        //    tag = new Matrix4x4(q[0], q[1], q[2], q[3], q[4], q[5]);//q =[x,y,z,rx,ry,rz]

        //    Form1.Mat_shadow = tag;
        //    tb_end.Invoke(new EventHandler(delegate {
        //        tb_end.Text = tag.ToString();
        //    }));

        //}

        //private void btn_sendend_Click(object sender, EventArgs e)
        //{
        //    //string[] tmpstr = tb_end.Text.Split(new string[2] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        //    //double[,] test_end = new double[4, 4];
        //    //int c = 0;
        //    //for (int j = 0; j < 4; j++)
        //    //{
        //    //    for (int i = 0; i < 4; i++)
        //    //    {
        //    //        test_end[j, i] = double.Parse(tmpstr[c]);
        //    //        c += 1;
        //    //    }
        //    //}
        //    Matrix4x4 test_final = new Matrix4x4(tag);

        //    Form1.degs = kn.InverseKinematics(kn.pos[kn.pos.Count - 1]);
        //    double[] ans_deg = kn.InverseKinematics(test_final);

        //    for (int i = 0; i < ans_deg.Length; i++)
        //    {
        //        Form1.tmp_deg[i] = ans_deg[i];
        //    }
        //    if (Form1.thd == null)
        //    {
        //        Form1.thd = new Thread(Form1.Anima);
        //        Form1.thd.Start();
        //    }
        //    else
        //    {
        //        if (Form1.thd.ThreadState == System.Threading.ThreadState.Running)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            Form1.thd = new Thread(Form1.Anima);
        //            Form1.thd.Start();
        //        }
        //    }
        //}//設定末端點

        //private void btn_copy_Click(object sender, EventArgs e)複製目前末端點
        //{
        //    tb_q.Invoke(new EventHandler(delegate
        //    {
        //        tb_q.Text = string.Format("{0,6:F3},{1,6:F3},{2,6:F3},{3,6:F3},{4,6:F3},{5,6:F3}", Form1.message_curr_q[0], Form1.message_curr_q[1], Form1.message_curr_q[2], Form1.message_curr_q[3], Form1.message_curr_q[4], Form1.message_curr_q[5]);

        //    }));
        //}//

        //private void btn_MoveShadow_Click(object sender, EventArgs e)
        //{
        //    string[] tmp = tb_move.Text.Split(',');
        //    double[] q = new double[tmp.Length];
        //    for (int i = 0; i < tmp.Length; i++)
        //    {
        //        q[i] = double.Parse(tmp[i]);
        //    }
        //    Matrix4x4 move = new Matrix4x4(q[0], q[1], q[2], q[3], q[4], q[5]);
        //    Form1.Mat_shadow = Form1.Mat_shadow * move;
        //}
        Thread runthd = null;

        private void bt_clear_qlist_Click(object sender, EventArgs e)
        {
            Form1.path.lines.Clear();
            dataTable.Clear();
        }

        //private void bt_add_line_Click(object sender, EventArgs e)
        //{
        //    string[] tmp = tb_str.Text.Split(',');
        //    string[] tmp2 = tb_fin.Text.Split(',');
        //    double[] str = new double[tmp.Length];
        //    double[] fin = new double[tmp2.Length];
        //    for (int i = 0; i < tmp.Length; i++)
        //    {
        //        str[i] = double.Parse(tmp[i]);
        //        fin[i] = double.Parse(tmp2[i]);
        //    }
        //    Form1.path.Addline(new Line(new Pos(str), new Pos(fin)));
        //}

        //private void bt_add_arc_Click(object sender, EventArgs e)
        //{
        //    string[] tmp = tb_str.Text.Split(',');
        //    string[] tmp2 = tb_fin.Text.Split(',');
        //    string[] tmp3 = tb_via.Text.Split(',');
        //    double[] str = new double[tmp.Length];
        //    double[] via = new double[tmp3.Length];
        //    double[] fin = new double[tmp2.Length];
        //    for (int i = 0; i < tmp.Length; i++)
        //    {
        //        str[i] = double.Parse(tmp[i]);
        //        via[i] = double.Parse(tmp3[i]);
        //        fin[i] = double.Parse(tmp2[i]);
        //    }
        //    Form1.path.AddArc(new Arc(new Pos(str), new Pos(via), new Pos(fin)));
        //}

        private void bt_run_path_Click(object sender, EventArgs e)
        {
            try
            {
                runthd = new Thread(Form1.Anima_path);
                runthd.Start(kn);
            }
            catch (Exception)
            {
            }
        }

        private void Data()
        {
            for (int i = 0; i < Form1.path.lines.Count; i++)
            {
                for (int j = 0; j < Form1.path.lines[i].Q.Count; j++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i * Form1.path.lines[i].Q.Count + j;
                    for (int k = 0; k < Form1.path.lines[i].Q[j].S.Length; k++)
                    {
                        dataRow[k + 1] = Form1.path.lines[i].Q[j].S[k];
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            dataGridView1.Invoke(new EventHandler(delegate //因不同執行續要給予委派
            {
                dataGridView1.Refresh();
            }));     
        }

        private void bt_add_lines_Click(object sender, EventArgs e)
        {
            Matrix4x4 pTw = Form1.path.ori.Inverse(Form1.path.ori);
            string[] line = tb_lines.Lines;//Lines 存取多行字串
            string[][] tmp = new string[line.Length][];
            double[] str = new double[6];
            double[] via = new double[6];
            double[] fin = new double[6];
            if (line.Length == 2)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    tmp[i] = line[i].Split(',');
                }
                for (int i = 0; i < tmp[0].Length; i++)
                {
                    str[i] = double.Parse(tmp[0][i]);//wTstr
                    fin[i] = double.Parse(tmp[1][i]);
                }
                Form1.path.Addline(new Line(pTw * new Pos(str), pTw * new Pos(fin)));//將加入的點全部轉到軌跡坐標系下
            }
            else if(line.Length == 3)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    tmp[i] = line[i].Split(',');
                }
                for (int i = 0; i < tmp[0].Length; i++)
                {
                    str[i] = double.Parse(tmp[0][i]);
                    via[i] = double.Parse(tmp[1][i]);
                    fin[i] = double.Parse(tmp[2][i]);
                }
                Form1.path.AddArc(new Arc(pTw * new Pos(str), pTw * new Pos(via), pTw * new Pos(fin)));
            }
           // Form1.copy = Form1.copy.copy(Form1.path);//先將原先的路徑複製出來
            dataTable.Clear();
            try
            {
                runthd = new Thread(Data);
                runthd.Start();
            }
            catch (Exception)
            {
            }
        }

        private void bt_insert_Click(object sender, EventArgs e)
        {
            string id = tb_id.Text;
            int id_tmp = int.Parse(id);

            string[] line = tb_lines.Lines;//Lines 存取多行字串
            string[][] tmp = new string[line.Length][];
            double[] str = new double[6];
            double[] via = new double[6];
            double[] fin = new double[6];
            if (line.Length == 2)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    tmp[i] = line[i].Split(',');
                }
                for (int i = 0; i < tmp[0].Length; i++)
                {
                    str[i] = double.Parse(tmp[0][i]);
                    fin[i] = double.Parse(tmp[1][i]);
                }
                Line item = new Line(new Pos(str),new Pos(fin));
                Form1.path.Insert(id_tmp, item);
            }
            else if (line.Length == 3)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    tmp[i] = line[i].Split(',');
                }
                for (int i = 0; i < tmp[0].Length; i++)
                {
                    str[i] = double.Parse(tmp[0][i]);
                    via[i] = double.Parse(tmp[1][i]);
                    fin[i] = double.Parse(tmp[2][i]);
                }
                Arc item = new Arc(new Pos(str), new Pos(via), new Pos(fin));
                Form1.path.Insert(id_tmp, item);
            }
            dataTable.Clear();
            try
            {
                runthd = new Thread(Data);
                runthd.Start();
            }
            catch (Exception)
            {
            }
        }
        private void bt_delete_Click(object sender, EventArgs e)
        {
            string id = tb_id.Text;
            int tmp = int.Parse(id);
            Form1.path.Delete(tmp);
            dataTable.Clear();
            try
            {
                runthd = new Thread(Data);
                runthd.Start();
            }
            catch (Exception)
            {
            }
        }

        private void bt_trnandrot_Click(object sender, EventArgs e)//將算好的路徑分割點加入平移或旋轉
        {
            string[] tmp = tb_trans_rot.Text.Split(',');
            double[] qs = new double[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                qs[i] = double.Parse(tmp[i]);
            }
            Form1.path.Trans(qs[0], qs[1], qs[2]);
            Form1.path.Rot(qs[5], qs[4], qs[3]);
            dataTable.Clear();
            try
            {
                runthd = new Thread(Data);
                runthd.Start();
            }
            catch (Exception)
            {
            }
        }
        private void bt_copy_path_Click(object sender, EventArgs e)
        {
            Form1.path = Form1.copy.copy(Form1.copy);
            dataTable.Clear();
            try
            {
                runthd = new Thread(Data);
                runthd.Start();
            }
            catch (Exception)
            {
            }
        }
    }
}
