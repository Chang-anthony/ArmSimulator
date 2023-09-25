using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Path_controler;
//using static WindowsFormsApplication1.Trajectory_via;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //[System.ComponentModel.Browsable(false)]
        //public event System.Windows.Forms.MouseEventHandler MouseWheel;
        //靜態的 638 472 
        static double W = 640;
        static double H = 480;
        static double r = Math.Sqrt(Math.Pow((W / 2d), 2) + Math.Pow((H / 2d), 2));//對邊

        static double FOV = 90;//Deg

        static double γ = FOV / 2d;//夾角(deg)

        static double f = r / Math.Tan(γ * Math.PI / 180d);//求鄰邊(帶入徑度)

        //static double α = Math.Atan((W / 2d) / f) * 180d / Math.PI;
        //static double HFOV = α*2d;//Deg

        //static double β = Math.Atan((H / 2d) / f) * 180d / Math.PI;
        //static double VFOV = β*2d;//Deg
        ////static double r = FOV / 2d;

        //double[,] ori = new double[4, 4];
        //double[,] coord = new double[4, 4];//齊次變換矩陣 初始值 => 0

        Matrix4x4 Mat_1 = new Matrix4x4(0,0,0);

        Matrix4x4 Mat_2 = new Matrix4x4(10, 15, 60);

        List<Matrix4x4> Mats = new List<Matrix4x4>();

        public static Matrix4x4 Mat_shadow = new Matrix4x4(0, 0, 0);
        public static double[] q_point = new double[6];

        Kinematics kn = new Kinematics(0,0,40);
        public static double[] degs = new double[6];//儲存關節角度位置
        public static double[] tmp_qstr = new double[6];// 儲存當下姿態用

        public static double[] message_curr_deg = new double[6];
        public static Matrix4x4 message_curr_end = new Matrix4x4(0,0,0);
        public static double[] message_curr_q = new double[6];
       
        public static Path path = new Path();
        public static Path copy = new Path();
        public static List<Pos> coord = new List<Pos>(); //動態路徑規劃

        GUI gui;
        public static Camera camera;
        Ground gnd;
        Graphics g;//畫家g 外包的
        Bitmap b;//紙 
        double a2 = 20;
        double d3 = 0;
        double a3 = 0;
        double d4 = 20;
        public Form1()//建構子  動態的
        {
            InitializeComponent();//案F12

            //p0
            kn.Add(0, 0, 0, 0);
            //p1
            kn.Add(-90, 0, 0, 0);
            //p2
            kn.Add(0, a2, d3, 0);
            //p3
            kn.Add(-90, a3, d4, 0);
            //p4
            kn.Add(90, 0, 0, 0);
            //p5
            kn.Add(-90, 0, 0, 0);
            //p6
            kn.ForwardKinematics(new double[]{0,0,0,0,0,0});

            gui = new GUI(ref kn);
            gui.Show();


            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.form1_MouseWheel);

            this.Width = 640;
            this.Height = 480;
            this.DoubleBuffered = true;


            camera = new Camera(10, 80, 80);
            camera.GetMat(new Matrix4x4(0, 0, 0), new Point3D(0, 0, 1));

            //4*4*4 = 64
            for (int i = 0; i < 20; i+=10)
            {
                for (int j = 0; j < 20; j+=10)
                {
                    for (int k = 0; k < 20; k+=10)
                    {
                        Mats.Add(new Matrix4x4(-i, -j, 0 + k));
                    }
                }
            }

            gnd = new Ground(26, 20);

            b = new Bitmap(this.Width, this.Height);//創造紙 寬度:
            g = Graphics.FromImage(b);//  FromImage   g畫家從哪裡拿到紙張?   ans : b
            //g.DrawLine(new Pen(Color.Red), new Point(10, 10), new Point(100, 100));

        }
        private void form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Update the drawing based upon the mouse wheel scrolling.

            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            int numberOfPixelsToMove = numberOfTextLinesToMove;

            if (numberOfPixelsToMove != 0)
            {
                if (numberOfPixelsToMove > 0)
                {
                    camera.pos.coord = camera.pos.Trans(camera.pos.coord, 0, 0, 5);//Z軸朝前  正值 向後移動
                    //camera.GetMat(new Matrix4x4(0, 0, 0), new Point3D(0, 0, 1));//更新
                }
                else if (numberOfPixelsToMove < 0)
                {
                    camera.pos.coord = camera.pos.Trans(camera.pos.coord, 0, 0, -5);//Z軸朝前  負值 向前移動
                    //camera.GetMat(new Matrix4x4(0, 0, 0), new Point3D(0, 0, 1));//更新
                }
            }
        }

        //public double[,] RotX(double[,] src, double Deg)
        //{
        //    double[,] dst = new double[4, 4];
        //    double[,] matZ = new double[4, 4];
        //    double rad = Deg * Math.PI / 180d;
        //    matZ[0, 0] = Math.Cos(rad); matZ[0, 1] = -Math.Sin(rad);
        //    matZ[1, 0] = Math.Sin(rad); matZ[1, 1] = Math.Cos(rad);
        //    matZ[2, 2] = 1;
        //    matZ[3, 3] = 1;

        //    for (int j = 0; j < 4; j++)//rows j
        //    {
        //        for (int i = 0; i < 4; i++)//colums i
        //        {
        //            dst[j, i] = 0;
        //            for (int k = 0; k < 4; k++)//平移 K
        //            {
        //                dst[j, i] += src[j, k] * matZ[k, i];
        //            }
        //        }
        //    }
        //    return dst;
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            ////畫家本人 e
            g.Clear(Color.White);

            //旋轉

            kn.ForwardKinematics(degs);
            Console.WriteLine(degs[1]);

            double[] ans_deg = kn.InverseKinematics(kn.pos[kn.pos.Count - 1]);
            //tb_ik.Invoke(new EventHandler(delegate
            //{
            //    tb_ik.Text = string.Format("{0,6:F3},{1,6:F3},{2,6:F3},{3,6:F3},{4,6:F3},{5,6:F3}", 
            //        ans_deg[0],ans_deg[1], ans_deg[2], ans_deg[3], ans_deg[4], ans_deg[5]);
            //    tb_ik.Refresh();//不會同步更新  要手動更新
            //}));

            message_curr_deg = ans_deg;
            // string.Format("{0,6:F3},{1,6:F3},{2,6:F3},{3,6:F3},{4,6:F3},{5,6:F3}", 
            //ans_deg[0],ans_deg[1], ans_deg[2], ans_deg[3], ans_deg[4], ans_deg[5]);

            //deg0 += 0.1;

            Mat_1.coord = Mat_1.RotZ(Mat_1.coord, 1);

            for (int i = 0; i < Mats.Count; i++)
            {
                Mats[i].coord = Mats[i].RotZ(Mats[i].coord, -1);
            }

            //投影
            double[,] Lens;
            Matrix4x4[,] c_gnd = camera.Convert(gnd.mat, out Lens);
            gnd.draw(g, c_gnd, Lens);


            //劃出手臂關節座標系
            //Console.WriteLine("pos : " + kn.pos[kn.pos.Count - 1].coord[0, 3] +" , "
            //    + kn.pos[kn.pos.Count - 1].coord[1, 3] + " , "
            //    + kn.pos[kn.pos.Count - 1].coord[2, 3]);

            Matrix4x4 end = kn.pos[kn.pos.Count - 1];

            message_curr_end.Copy(end);

            double[] q = end.GetXYZTransAndXYZFixedAngle();

            message_curr_q = q;
            // string.Format("{0,6:F3},{1,6:F3},{2,6:F3},{3,6:F3},{4,6:F3},{5,6:F3}",
            //q[0], q[1], q[2], q[3], q[4], q[5]);

            //tb_end.Invoke(new EventHandler(delegate {
            //    tb_end.Text = end.ToString();
            //    tb_end.Refresh();//不會同步更新  要手動更新
            //}));
            double cs_len;
            Matrix4x4 c_shadow = camera.Convert(Mat_shadow, out cs_len);
            if (cs_len > 10)
            {
                c_shadow.draw(g, 45, 5);
            }

            //kn.ForwardKinematics(degs);
            double[] kn_len;

            Matrix4x4[] Pos_out = kn.GetPos();
            Matrix4x4[] c_kn = camera.Convert(Pos_out, out kn_len);

            kn.DrawSkeleton(g, camera);
            kn.DrawCylinder(g, camera);
            //Console.WriteLine("c_pos : " + c_kn[kn.pos.Count - 1].coord[0, 3] + " , "
            //    + c_kn[kn.pos.Count - 1].coord[1, 3] + " , "
            //    + c_kn[kn.pos.Count - 1].coord[2, 3]);
            for (int i = 0; i < c_kn.Length; i++)
            {
                //if (kn_len[i]>10)
                //{
                //    c_kn[i].draw(g);
                //}

                if (i != 4 && i != 5)
                {
                    if (kn_len[i] > 3)
                    {
                        c_kn[i].draw(g);
                    }
                }

            }


            //Matrix4x4 c_Mat_1 = camera.Convert(Mat_1);
            //c_Mat_1.draw(g);

            //Matrix4x4[] c_Mats = camera.Convert(Mats.ToArray());
            //for (int i = 0; i < c_Mats.Length; i++)
            //{
            //    c_Mats[i].draw(g);
            //}

            //Mat_1.coord = Mat_1.RotZ(Mat_1.coord, 1);
            //Mat_1.draw(g);


            //Mat_2.coord = Mat_2.RotZ(Mat_2.coord, -1);
            //Mat_2.draw(g);



            ////coord = RotX(coord, 0.1);//每次(10ms)旋轉Z軸0.01度
            ////coord = RotX(coord, 0.1);

            //PointF P = Coord2D(ori);
            //PointF X = Xaxis2D(ori);
            //PointF Y = Yaxis2D(ori);
            //PointF Z = Zaxis2D(ori);

            //g.DrawLine(new Pen(Color.Red, 3), P, X);//X

            //g.DrawLine(new Pen(Color.Green, 3), P, Y);//Y

            //g.DrawLine(new Pen(Color.Blue, 3), P, Z);//Z

            //P = Coord2D(coord);
            //X = Xaxis2D(coord);
            //Y = Yaxis2D(coord);
            //Z = Zaxis2D(coord);

            //g.DrawLine(new Pen(Color.Red,3), P, X);//X

            //g.DrawLine(new Pen(Color.Green,3), P, Y);//Y

            //g.DrawLine(new Pen(Color.Blue,3), P, Z);//Z

            ////g.DrawLine(new Pen(Color.Red, 3), new PointF(0, 0), new PointF(this.Width, this.Height));//X




            e.Graphics.DrawImage(b, 0, 0);//放的是紙張   不是放畫家g
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //ori = RotZ(ori, -45);
            //coord = RotZ(coord, -45);

            //coord = move(coord, 10, 0, 0);//dx = 10
            //coord = RotZ(coord, 45);//rz 45 deg

            //先旋轉 再位移
            //Rot    trans

            //先位移 再旋轉
            //trans  Rot 

            //不考慮旋轉只位移(原始的單位向量方向) Move 純加法
            

            while (true)
            {
                this.Invalidate();
                Thread.Sleep(10);//休息 10 ms (毫秒)
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //如果想要提示   ALT + → 
            if (e.KeyCode==Keys.Up)
	        {
                camera.pos.coord = camera.pos.Trans(camera.pos.coord, 0, -5, 0);//Y軸朝下  負值 向上移動
                //camera.GetMat(new Matrix4x4(0, 0, 0), new Point3D(0, 0, 1));//更新
	        }
            else if (e.KeyCode == Keys.Down)
            {
                camera.pos.coord = camera.pos.Trans(camera.pos.coord, 0, 5, 0);//Y軸朝下  正值 向下移動
                //camera.GetMat(new Matrix4x4(0, 0, 0), new Point3D(0, 0, 1));//更新
            }
            else if (e.KeyCode == Keys.Left)
            {
                camera.pos.coord = camera.pos.Trans(camera.pos.coord, -5, 0, 0);//X軸  負值 向左移動
                //camera.GetMat(new Matrix4x4(0, 0, 0), new Point3D(0, 0, 1));//更新
            }
            else if (e.KeyCode == Keys.Right)
            {
                camera.pos.coord = camera.pos.Trans(camera.pos.coord, 5, 0, 0);//X軸  正值 向右移動
                //camera.GetMat(new Matrix4x4(0, 0, 0), new Point3D(0, 0, 1));//更新
            }
        }
        PointF m_down = new PointF(0, 0);
        PointF m_up = new PointF(0, 0);
        PointF m_move = new PointF(0, 0);
        PointF ds = new PointF(0, 0);//位移量
        bool IsDown = false;
        Matrix4x4 camera_tmp=new Matrix4x4(0,0,0);
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsDown==false)
            {
                camera_tmp.Copy(camera.pos);//只有在按下瞬間更新
                m_down = e.Location;
            }
            IsDown = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (IsDown == true)
            {
                m_up = e.Location;
            }
            IsDown = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            m_move = e.Location;
            if (IsDown)
            {
                ds.X =( m_move.X - m_down.X)/10F;
                ds.Y =( m_move.Y - m_down.Y)/10F;
                Console.WriteLine(ds);
                camera.PTZ(camera_tmp,ds.X, ds.Y);
            }
            
        }
        public static double[] tmp_deg = new double[6];//終點角度
        public static double[] via_deg = new double[6];//中繼點角度
        //所有姿態用 
        public static double[] tmp_qfin = new double[6];
        public static double[] tmp_qvia = new double[6];
        public static Thread thd = null;
        private void btn_set_Click(object sender, EventArgs e)
        {
            //string[] tmp = tb_deg.Text.Split(',');
            //for (int i = 0; i < tmp.Length; i++)
            //{
            //    tmp_deg[i] = double.Parse(tmp[i]);
            //}
            //if (thd==null)
            //{
            //    thd = new Thread(Anima);
            //    thd.Start();
            //}
            //else
            //{
            //    if (thd.ThreadState==ThreadState.Running)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        thd = new Thread(Anima);
            //        thd.Start();
            //    }
            //}

        }

        /// <summary>
        /// 作業 善用第7章公式
        /// 不要經過奇異點 
        /// </summary>
        /// 

        //public static void Anima()//Kinematics kn)
        //{
        //    //真．回家作業(8/31 前)
        //    //末端點空間的軌跡規劃
        //    //如果先有末端點軌跡  再產生關節軌跡  要怎麼做??
        //    // q = [x,y,z,γ,β,α]

        //    // p1 => q1 起點
        //    // p2 => q2 終點

        //    //關節空間的軌跡
        //    //Trajectory[] tary = new Trajectory[6];
        //    #region 不使用了
        //    //output degs
        //    //desired tmp_deg

        //    //double[] ddeg = new double[6];//角度差
        //    ////tmp_deg[i] = θf
        //    ////degs[i] = θ0

        //    ////ddeg[i] = θf - θ0
        //    ////線性分割
        //    //for (int i = 0; i < 6; i++)
        //    //{
        //    //    ddeg[i] = tmp_deg[i] - degs[i];
        //    //    //ddeg -270   tmp_deg(目標) =0   degs(現在) =270

        //    //    if (ddeg[i] >= 180d)
        //    //    {
        //    //        ddeg[i] = ddeg[i] - 360d;
        //    //    }
        //    //    else if (ddeg[i] <= -180d)
        //    //    {
        //    //        ddeg[i] = ddeg[i] + 360d;
        //    //    }
        //    //    //ddeg +90   degs(現在) =270   tmp_deg(目標) =360
        //    //    //finnal(目標) = degs(現在) + ddeg
        //    //    /** 初始化 new 建構子()
        //    //     * //肯定不會有迴圈 for
        //    //     * 
        //    //    // θ0

        //    //    // θf
        //    //    **/

        //    //}
        //    #endregion
        //    double tf = 1;
        //    double hz = 100;
        //    for (int i = 0; i < 6; i++)
        //    {
        //        // θ0  θf  tf
        //        //tary[i] = new Trajectory(degs[i], tmp_deg[i],tf);
        //    }
        //    //T = 100 *10ms = 1s
        //    //考驗  :  公式裡的時間(物理時間) 程式碼裡的時間(切割時間)  要統一
        //    //LOOP θ(t)   t [0,1]   dt = 0.01
        //    for (double t = 1; t <= 100; t++)
        //    {
        //        for (int i = 0; i < 6; i++)
        //        {
        //            /**
        //            // (ddeg[i] / 100d) = 角速度 
        //            //tary.Createθt(t)
        //             * */
        //            //degs[i] = (ddeg[i] / 100d) + degs[i];
        //            //degs[i] = tary[i].Createθt(t / hz);
        //        }
        //        Thread.Sleep(10);//10ms
        //    }
        //}

        /// <summary>
        /// 208 範例7.2   7.12 7.13  7.15
        /// </summary>
        //public static void Anima_via()
        //{
        //    //關節空間的軌跡(7/20)
        //    Console.WriteLine(degs[0]);//現在
        //    Console.WriteLine(tmp_deg[0]);//目標
        //    Console.WriteLine(via_deg[0]);//中繼


        //    //Trajectory_via
        //    Trajectory[] tary = new Trajectory[6];

        //    double tf = 1;
        //    for (int i = 0; i < 6; i++)
        //    {
        //        // θ0  θv  θg  tf
        //        tary[i] = new Trajectory(degs[i], tmp_deg[i], tf);
        //    }
        //    //T = 100 *10ms = 1s
        //    //考驗  :  公式裡的時間(物理時間) 程式碼裡的時間(切割時間)  要統一
        //    //LOOP θ(t)   t [0,1]   dt = 0.01
        //    for (double t = 1; t <= 100; t++)
        //    {
        //        for (int i = 0; i < 6; i++)
        //        {
        //            /**
        //            // (ddeg[i] / 100d) = 角速度 
        //            //tary.Createθt(t)

        //             * */
        //            //degs[i] = (ddeg[i] / 100d) + degs[i];
        //            degs[i] = tary[i].Createθt(t * 0.01);
        //        }
        //        Thread.Sleep(10);//10ms
        //    }
        //}

        //public static void Anima_via()
        //{
        //    double tf_all = 2;
        //    double hz = 100;
        //    //Trajectory_via[] tray_via = new Trajectory_via[6];
        //    for (int i = 0; i < 6; i++)
        //    {
        //        //tray_via[i] = new Trajectory_via(degs[i], via_deg[i], tmp_deg[i], tf_all);
        //    }
        //    for (double t = 0; t <= 100; t ++) //總時間為1秒 切割100份
        //    {
        //        for (int i = 0; i < 6; i++)
        //        {
        //            //degs[i] = tray_via[i].Createθt_oritovia(t / hz);
        //        }
        //        Console.WriteLine();
        //        Thread.Sleep(10);//10ms
        //    }
        //    for (double t = 0; t <= 100; t++) //總時間為1秒 切割100份
        //    {
        //        for (int i = 0; i < 6; i++)
        //        {
        //            //degs[i] = tray_via[i].Createθt_viatofin(t / hz);
        //        }
        //        Console.WriteLine();
        //        Thread.Sleep(10);//10ms
        //    }
        //}

        public static void Anima_path(object sender)
        {
            Kinematics kn = (Kinematics)sender;
            coord.Clear();//清除所有算過路徑
            for (int i = 0; i < path.lines.Count; i++)
            {
                coord.AddRange(path.Create_Path_Point(i, 100));//算出所有的path 每個線段 分割點路徑
            }
            for (int i = 0; i < coord.Count; i++)
            {
                degs = kn.GetIKdegs(coord[i].S,kn);//S 存的就是每段速度變化
                Thread.Sleep(10);//10ms
            }
        }
        //public static void Anima_end(object sender)//Kinematics kn)//sender 不管丟進是甚麼類別的記憶體 ，之後要在自己設定
        //{
        //    Kinematics kn = (Kinematics)sender;
        //    #region 真回家作業可做參考(打錯的)(一樣變成用關節角輸出)
        //    //真．回家作業(8/31 前)
        //    //末端點空間的軌跡規劃
        //    //如果先有末端點軌跡  再產生關節軌跡  要怎麼做??
        //    // q = [x,y,z,γ,β,α]

        //    // p1 => q1 起點
        //    // p2 => q2 終點

        //    //double[] P1 = new double[3] { tmp_q1[0], tmp_q1[1], tmp_q1[2] };//位置
        //    //double[] θ1 = new double[3] { tmp_q1[3], tmp_q1[4], tmp_q1[5] };//角度
        //    //double[] P2 = new double[3] { tmp_q2[0], tmp_q2[1], tmp_q2[2] };//位置
        //    //double[] θ2 = new double[3] { tmp_q2[3], tmp_q2[4], tmp_q2[5] };//角度

        //    //Matrix4x4 P1_pos = new Matrix4x4(P1[0], P1[1], P1[2]);
        //    //Matrix4x4 P1_rot = P1_pos.Create_RotXYZ(θ1[2], θ1[1], θ1[0]);
        //    //Matrix4x4 P1_fin = P1_pos * P1_rot;

        //    //Matrix4x4 P2_pos = new Matrix4x4(P2[0], P2[1], P2[2]);
        //    //Matrix4x4 P2_rot = P1_pos.Create_RotXYZ(θ2[2], θ2[1], θ2[0]);
        //    //Matrix4x4 P2_fin = P2_pos * P2_rot;

        //    ////這樣會成為一個4X4矩陣 有表示位置與方向的矩陣

        //    //double[] θ0 = kn.InverseKinematics(P1_fin);

        //    //double[] θf = kn.InverseKinematics(P2_fin);

        //    // q1,q2 => q(t)
        //    // q(t) => θ(t)
        //    //double[] dq = new double[6]; //[dx, dy, dz, dγ, dβ, dα]//再利用這個去做分割
        //    //for (int i = 0; i < 6; i++)
        //    //{
        //    //    dq[i] = tmp_q2[i] - tmp_q1[i];
        //    //}
        //    #endregion

        //    //關節空間的軌跡
        //    //End[] P_all = new End[6];
        //    List<double[]> P_out = new List<double[]>();
        //    List<double[]> deg_out = new List<double[]>();

        //    double tf = 1;
        //    double hz = 100;
        //    for (int i = 0; i < 6; i++)
        //    {
        //        // θ0  θf  tf
        //        //P_all[i] = new End(tmp_qstr[i], tmp_qfin[i], tf);//分別算出[x, y, z, γ, β, α] 軌跡參數
        //    }
        //    //T = 100 *10ms = 1s
        //    //考驗  :  公式裡的時間(物理時間) 程式碼裡的時間(切割時間)  要統一
        //    //LOOP θ(t)   t [0,1]   dt = 0.01
        //    for (double t = 0; t <= 100; t++)
        //    {
        //        double[] TMP = new double[6];
        //        for (int i = 0; i < 6; i++)
        //        {
        //            /**
        //            // (ddeg[i] / 100d) = 角速度 
        //            //tary.Createθt(t)
        //             * */
        //            //TMP[i] = P_all[i].Createθt(t / hz);//將求出的每個時間分割點的軌跡紀錄
        //        }
        //        P_out.Add(TMP);
        //    }
        //    for (double t = 1; t <= 100; t++)
        //    {
        //        //degs = P_all[0].GetIKdegs(P_out[(int)(t-1)], kn);//P_fin[0] 只是讓我能用 GetIKdegs 的方法而已
        //        //Matrix4x4 P2_pos = new Matrix4x4(P_out[(int)(t - 1)][0], P_out[(int)(t - 1)][1], P_out[(int)(t - 1)][2]);
        //        //Matrix4x4 P2_rot = P2_pos.Create_RotXYZ(P_out[(int)(t - 1)][5], P_out[(int)(t - 1)][4], P_out[(int)(t - 1)][3]);
        //        //Matrix4x4 P2_fin = P2_pos * P2_rot;
        //        //degs = kn.InverseKinematics(P2_fin);
        //        deg_out.Add(degs);//IK 後丟回 輸出角)
        //        Thread.Sleep(10);//10ms
        //    }
        //}
        //public static void Anima_via_end(object sender)
        //{
        //    Kinematics kn = (Kinematics)sender;
        //    double tf_all = 2;
        //    double hz = 100;

        //    //End_via[] P_via = new End_via[6];//有中繼點的
        //    //End_via[] P_fin = new End_via[6];//有中繼點的
        //    List<double[]> P_strtovia = new List<double[]>();
        //    List<double[]> P_viatofin = new List<double[]>();
        //    List<double[]> deg_out = new List<double[]>();
        //    List<double[]> P_out = new List<double[]>();

        //    for (int i = 0; i < 6; i++)
        //    {
        //       // P_via[i] = new End_via(tmp_qstr[i], tmp_qvia[i], tmp_qfin[i], tf_all);
        //    }
        //    for (double t = 0; t <= 100; t++)//總時間為1秒 切割100份 存到中繼點的
        //    {
        //        double[] tmp = new double[6];//儲存每個時段分割點用的
        //            for (int i = 0; i < 6; i++)
        //            {
        //                //tmp[i] = P_via[i].Createθt_oritovia(t / hz);
        //            }
        //            P_strtovia.Add(tmp);
        //            P_out.Add(tmp);//順序堆疊方式加入，索引越高越後加入
        //    }
        //    for (double t = 0; t <= 100; t++)//總時間為1秒 切割100份 存到中繼點的
        //    {
        //        double[] tmp = new double[6];//儲存每個時段分割點用的
        //        for (int i = 0; i < 6; i++)
        //        {
        //            //tmp[i] = P_via[i].Createθt_viatofin(t / hz);
        //        }
        //        P_viatofin.Add(tmp);
        //        P_out.Add(tmp);
                
        //    }

        //    for (double t = 1; t <= 200; t++)//直接全跑
        //    {
        //        //degs = P_via[0].GetIKdegs_via(P_out[(int)(t-1)], kn);//P_fin[0] 只是讓我能用 GetIKdegs 的方法而已
        //        //Matrix4x4 P2_pos = new Matrix4x4(P_out[(int)(t-1)][0], P_out[(int)(t - 1)][1], P_out[(int)(t - 1)][2]);
        //        //Matrix4x4 P2_rot = P2_pos.Create_RotXYZ(P_out[(int)(t - 1)][5], P_out[(int)(t - 1)][4], P_out[(int)(t - 1)][3]);
        //        //Matrix4x4 P2_fin = P2_pos * P2_rot;
        //        //degs = kn.InverseKinematics(P2_fin);
        //        deg_out.Add(degs);//IK 後丟回 輸出角)
        //        Thread.Sleep(10);//10ms 
        //    }
        //}

        private void btn_testik_Click(object sender, EventArgs e)
        {
            //tb_q = [ Px Py Pz α β γ ]
            //string[] tmp = tb_q.Text.Split(',');
            //double[] P = new double[3] { double.Parse(tmp[0]), double.Parse(tmp[1]), double.Parse(tmp[2]) };//位置
            //double[] q = new double[3] { double.Parse(tmp[3]), double.Parse(tmp[4]), double.Parse(tmp[5]) };//角度 
            //Matrix4x4 test = new Matrix4x4(P[0], P[1], P[2]);
            //Matrix4x4 testrot = test.Create_RotXYZ(q[2], q[1], q[0]);
            //Matrix4x4 testendmat = test * testrot;

            //tb_endtest.Invoke(new EventHandler(delegate {
            //    tb_endtest.Text = testendmat.ToString();
            //    tb_endtest.Refresh();
            //}));

            //string[] tmpstr = testendmat.ToString().Split(new string[2] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //double[,] test_end = new double[4, 4];
            //int c = 0;
            //for (int j = 0; j < 4; j++)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        test_end[j, i] = double.Parse(tmpstr[c]);
            //        c += 1;
            //    }
            //}
            //Matrix4x4 test_final = new Matrix4x4(test_end);
            //Console.WriteLine(test_final.ToString());

            //double[] ans_deg = kn.InverseKinematics(test_final);
            //tb_ikangle.Invoke(new EventHandler(delegate
            //{
            //    tb_ikangle.Text = string.Format("{0,6:F3},{1,6:F3},{2,6:F3},{3,6:F3},{4,6:F3},{5,6:F3}",
            //        ans_deg[0], ans_deg[1], ans_deg[2], ans_deg[3], ans_deg[4], ans_deg[5]);
            //    tb_ikangle.Refresh();//不會同步更新  要手動更新
            //}));
        }

        private void tb_q_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //W = this.Width;
            //H = this.Height;
            //r = Math.Sqrt(Math.Pow((W / 2d), 2) + Math.Pow((H / 2d), 2));//對邊

            //FOV = 90;//Deg

            //γ = FOV / 2d;//夾角(deg)

            //f = r / Math.Tan(γ * Math.PI / 180d);//求鄰邊(帶入徑度)
        }
    }
}
