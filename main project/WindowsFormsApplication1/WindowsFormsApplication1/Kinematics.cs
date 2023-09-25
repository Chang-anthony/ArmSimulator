using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class DH
    {
        double r, b, δ;
        public double α, a, d, θ;
        public DH(double α, double a, double d, double θ, double r, double b, double δ)
        {
            //DH α a d θ
            //RotX(α) TransX(a) TransZ(d) RotZ(θ)
            this.α = α;
            this.a = a;
            this.d = d;
            this.θ = θ;

            //配合實際馬達安裝角
            this.r = r;
            this.b = b;
            this.δ = δ;

        }
    }
    public class Kinematics
    {
        int scale = 30;
        public List<DH> dh = new List<DH>();
        public List<Matrix4x4> nTn1 = new List<Matrix4x4>();//n => n+1

        public List<Matrix4x4> pos = new List<Matrix4x4>();
        public Matrix4x4[][] Skeleton;

        public Matrix4x4[][] Cylinder;
        
       
        /// <summary>
        /// 骨架
        /// </summary>
        public void CreateSkeleton(Matrix4x4[] pos)
        {
            Skeleton = new Matrix4x4[pos.Length-1][];
            Skeleton[0] = new Matrix4x4[2];
            Skeleton[0][0] = new Matrix4x4(pos[0].coord[0, 3], pos[0].coord[1, 3],0);
            Skeleton[0][1] = new Matrix4x4(pos[0]);
            for (int i = 1; i < (Skeleton.GetLength(0)); i++)
            {
                Skeleton[i] = new Matrix4x4[2];

                Skeleton[i][0] = new Matrix4x4(pos[i]);
                Skeleton[i][1] = new Matrix4x4(pos[i + 1]);
            }
        }
        /// <summary>
        /// 圓柱體
        /// </summary>
        public void CreateCylinder(Matrix4x4[] pos)
        {
            if (Cylinder == null)
            {
                Cylinder = new Matrix4x4[pos.Length][];
                for (int i = 0; i < pos.Length; i++)
                {
                    Cylinder[i] = new Matrix4x4[scale * 2];
                    for (int k = 0; k < Cylinder[i].Length; k++)
                    {
                        Cylinder[i][k] = new Matrix4x4(0,0,0);
                    }
                }
            }
            double r = 1;
            
            
            Matrix4x4 ori = new Matrix4x4(pos[0].coord[0, 3], pos[0].coord[1, 3], 0);
           
            Point3D zshift = new Point3D(pos[0].coord[0, 3] - ori.coord[0, 3], pos[0].coord[1, 3] - ori.coord[1, 3], pos[0].coord[2, 3] - ori.coord[2, 3]);

            Matrix4x4[] rs = new Matrix4x4[scale];
            Matrix4x4[] rs_ = new Matrix4x4[scale];

            for (int k = 0; k < scale; k++)
            {
                Matrix4x4 rz = new Matrix4x4(ori.RotZ(ori.coord, k * 360.0 / scale));
                Matrix4x4 tx = new Matrix4x4(ori.Trans(ori.coord, r, 0, 0));
                rs[k] = rz * tx;
                rs_[k] =new Matrix4x4( rs[k].Trans(rs[k].coord, zshift.x, zshift.y, zshift.z));
            }

            for (int k = 0; k < scale; k++)
            {
                Cylinder[0][k].Copy( rs[k]);
            }

            for (int k = scale; k < scale*2; k++)
            {
                Cylinder[0][k].Copy( rs_[k - scale]);
            }

            //(Cylinder.GetLength(0))
            for (int i = 1; i < (Cylinder.GetLength(0)); i++)
            {
                //if (i==4 || i==5)
                //{
                //    continue;
                //}
                //Cylinder[i] = new Matrix4x4[scale*2];
                

                rs = new Matrix4x4[scale];
                rs_ = new Matrix4x4[scale];

                for (int k = 0; k < scale; k++)
                {
                    //pos[i] * nTn1[i] = pos[i+1]
                    if (i==3)
                    {
                        Matrix4x4 rz = new Matrix4x4(pos[i].RotY(pos[i].coord, k * 360.0 / scale));
                        Matrix4x4 rz_inv = rz.GetRotationMat().GetInverse();
                        Matrix4x4 rztx = new Matrix4x4(rz.Trans(rz.coord, r, 0, 0));
                        Matrix4x4 ry_inv = new Matrix4x4(pos[i].RotY(new Matrix4x4(0, 0, 0).coord, -k * 360.0 / scale));
                        rs[k] = rztx * ry_inv;
                        rs_[k] = rs[k] * nTn1[i];
                    }
                    else
                    {
                        double _R = r;
                        Matrix4x4 rz;
                        Matrix4x4 rz_inv;
                        Matrix4x4 rztx;
                        if (i==4 )
                        {
                            _R = r *(i- 2);
                            rz = new Matrix4x4(pos[i].RotZ(pos[i].coord, k * 360.0 / scale));
                            rz_inv = rz.GetRotationMat().GetInverse();
                            rztx = new Matrix4x4(rz.Trans(rz.coord,  _R, 0, 0));
                            rs[k] = rztx * rz_inv;
                            rs_[k] = rs[k] * nTn1[i];
                        }
                        else if (i==5)
                        {
                            _R = r * (i - 2);
                            rz = new Matrix4x4(pos[i].RotZ(pos[i].coord, k * 360.0 / scale));
                            rz_inv = rz.GetRotationMat().GetInverse();
                            rztx = new Matrix4x4(rz.Trans(rz.coord, _R, 0, 0));
                            rs[k] = rztx * rz_inv;
                            rs_[k] = rs[k] * nTn1[i];
                        }
                        else if (i == 6)
                        {
                            _R = r * (i - 2);
                            rz = new Matrix4x4(pos[i].RotZ(pos[i].coord, k * 360.0 / scale));
                            rz_inv = rz.GetRotationMat().GetInverse();
                            rztx = new Matrix4x4(rz.Trans(rz.coord, _R, 0, 0));
                            rs[k] = rztx * rz_inv;
                            rs_[k] = rs[k] * nTn1[i-1];
                        }
                        else if (i == 2)
                        {
                            // Pi * iTi+1 = Pi+1
                            
                            rz = new Matrix4x4(pos[i].RotX(pos[i].coord, k * 360.0 / scale));
                            rz_inv = rz.GetRotationMat().GetInverse();
                            rztx = new Matrix4x4(rz.Trans(rz.coord, 0, 0, _R));
                            rs[k] = rztx;
                            rs_[k] = rs[k] * nTn1[i];
                        }
                        else
                        {
                            rz = new Matrix4x4(pos[i].RotX(pos[i].coord, k * 360.0 / scale));
                            rz_inv = rz.GetRotationMat().GetInverse();
                            rztx = new Matrix4x4(rz.Trans(rz.coord, 0, 0, _R));
                            rs[k] = rztx * rz_inv;
                            rs_[k] = rs[k] * nTn1[i];
                        }
                        
                    }
                    
                }

                for (int k = 0; k < scale; k++)
                {
                    Cylinder[i][k].Copy( rs[k]);
                }

                for (int k = scale; k < scale * 2; k++)
                {
                    Cylinder[i][k].Copy( rs_[k - scale]);
                }
            }
        }
        /// <summary>
        /// 立方體
        /// </summary>
        public void CreateCube()
        {

        }
        public void DrawCylinder(Graphics g, Camera cam)
        {
            if (Cylinder == null)
            {
                return;
            }
            for (int i = 0; i < Cylinder.GetLength(0); i++)
            {
                if (Cylinder[i]==null)
                {
                    continue;
                }
                Matrix4x4[] ps = new Matrix4x4[scale*2];
                double[] lens = new double[scale * 2];
                for (int k = 0; k < scale*2; k++)
                {
                    ps[k] = cam.Convert(Cylinder[i][k], out lens[k]);
                }

                PointF[] p_s = new PointF[scale * 2];
                for (int k = 0; k < scale * 2; k++)
                {
                    p_s[k] = ps[k].Coord2D(ps[k].coord);
                }

                Pen p = new Pen(Color.FromArgb(108, 50, 50, 50), 5);
                
                if (i == 4)
                {
                    p = new Pen(Color.FromArgb(128, 10, 150, 10), 5);
                }
                if (i == 5)
                {
                    p = new Pen(Color.FromArgb(128, 10, 10, 150), 5);
                }
                if (i == 6)
                {
                    p = new Pen(Color.FromArgb(128, 150, 10, 0), 5);
                }
                //底邊的圓
                double near = 2;
                for (int k = 0; k < scale-1; k++)
                {
                    if (lens[k] > near && lens[k + 1] > near)
                    {
                        g.DrawLine(p, p_s[k], p_s[k + 1]);
                    }
                    
                }
                if (lens[0] > near && lens[scale - 1] > near)
                {
                    g.DrawLine(p, p_s[0], p_s[scale - 1]);
                }
                

                //頂邊的圓
                for (int k = scale; k < scale*2-1; k++)
                {
                    if (lens[k] > near && lens[k + 1] > near)
                    {
                        g.DrawLine(p, p_s[k], p_s[k + 1]);
                    }
                }
                if (lens[scale] > near && lens[scale * 2 - 1] > near)
                {
                    g.DrawLine(p, p_s[scale], p_s[scale * 2 - 1]);
                }
                

                //邊
                for (int k = 0; k < scale; k++)
                {
                    if (lens[k] > near && lens[k + scale] > near)
                    {
                        g.DrawLine(p, p_s[k], p_s[k + scale]);
                    }
                }



            }

        }

        public void DrawSkeleton(Graphics g, Camera cam)
        {
            if (Skeleton==null)
            {
                return;
            }
            for (int i = 0; i < Skeleton.GetLength(0); i++)
			{
                Matrix4x4 p0 = cam.Convert(Skeleton[i][0]);
                Matrix4x4 p1 = cam.Convert(Skeleton[i][1]);

                PointF P_0 = p0.Coord2D(p0.coord);
                PointF P_1 = p1.Coord2D(p1.coord);

                Pen p =new Pen(Color.FromArgb(128,100,100,100),5);
                g.DrawLine(p, P_0, P_1);
			}
            
        }

        public Kinematics()
        {
            pos.Add(new Matrix4x4(0, 0, 0));
        }
        public Kinematics(double x, double y, double z)
        {
            pos.Add(new Matrix4x4(x, y, z));
        }
        public void Add(double α, double a, double d, double θ)
        {
            dh.Add(new DH(α, a, d, θ, 0, 0, 0));
            nTn1.Add(new Matrix4x4(0, 0, 0));
            pos.Add(new Matrix4x4(0, 0, 0));
        }

        /// <summary>
        /// pos 世界座標系 
        /// ori 手臂連桿的坐標系
        /// </summary>
        /// <param name="Deg"></param>
        public void ForwardKinematics(double[] Deg)
        {
            //更新角度
            for (int i = 0; i < dh.Count; i++)
            {
                dh[i].θ = Deg[i];
            }
            //Create_RTTR 
            for (int i = 0; i < nTn1.Count; i++)
            {
                nTn1[i] = Create_RTTR(dh[i].α, dh[i].a, dh[i].d, dh[i].θ);//RTTR 
                //nTn1[i] = Create_RTRT(dh[i].α, dh[i].a, dh[i].d, dh[i].θ);//RTRT
            }

            //連乘
            Matrix4x4 ori = new Matrix4x4(pos[0]);
            for (int i = 0; i < nTn1.Count; i++)
            {
                ori =ori*nTn1[i];//[i+1] = [i] * [ i T (i+1) ]
                pos[i + 1].Copy(ori);//pos[i+1] = [i+1]
            }
            
        }
        public Matrix4x4[] GetPos()
        {
            Matrix4x4 ori = new Matrix4x4(pos[0]);
            Matrix4x4[] pos_out = new Matrix4x4[pos.Count];
            pos_out[0] = new Matrix4x4(ori);
            for (int i = 0; i < nTn1.Count; i++)
            {
                ori = ori * nTn1[i];//[i+1] = [i] * [ i T (i+1) ]
                pos_out[i+1] = new Matrix4x4(ori);//pos[i+1] = [i+1]
            }
            CreateSkeleton(pos_out);
            CreateCylinder(pos_out);
            return pos_out;
        }

        /// <summary>
        /// ori 連桿的坐標系
        /// </summary>
        /// <param name="Deg"></param>
        /// <param name="ori"></param>
        public void ForwardKinematics(double[] Deg, out Matrix4x4 ori)
        {
            //更新角度
            for (int i = 0; i < dh.Count; i++)
            {
                dh[i].θ = Deg[i];
            }
            //Create_RTTR 
            for (int i = 0; i < nTn1.Count; i++)
            {
                nTn1[i] = Create_RTTR(dh[i].α, dh[i].a, dh[i].d, dh[i].θ);//RTTR 
                //nTn1[i] = Create_RTRT(dh[i].α, dh[i].a, dh[i].d, dh[i].θ);//RTRT
            }

            //連乘
            ori = new Matrix4x4(0, 0, 0);
            for (int i = 0; i < nTn1.Count; i++)
            {
                ori = ori * nTn1[i];//[i+1] = [i] * [ i T (i+1) ]
                //pos[i + 1].Copy(ori);//pos[i+1] = [i+1]
            }
            //CreateSkeleton();
        }
        public void ForwardKinematics(double[] Deg, ref Matrix4x4[] pos)
        {
            //更新角度
            double[] degs = new double[dh.Count]; 
            for (int i = 0; i < dh.Count; i++)
            {
                degs[i] = Deg[i];
            }
            //Create_RTTR 
            Matrix4x4[] nTn1_tmp = new Matrix4x4[nTn1.Count];
            for (int i = 0; i < nTn1.Count; i++)
            {
                nTn1_tmp[i] = Create_RTTR(dh[i].α, dh[i].a, dh[i].d, degs[i]);//RTTR 
                //nTn1[i] = Create_RTRT(dh[i].α, dh[i].a, dh[i].d, dh[i].θ);//RTRT
            }

            //連乘
            Matrix4x4 ori = new Matrix4x4(pos[0]);
            for (int i = 0; i < nTn1.Count; i++)
            {
                ori = ori * nTn1_tmp[i];//[i+1] = [i] * [ i T (i+1) ]
                pos[i + 1].Copy(ori);//pos[i+1] = [i+1]
            }
            //CreateSkeleton();
        }

        public double[] GetDiff(Matrix4x4 pos, double[] degs,int j)
        {
            //當前資訊
            double[] curr_degs = new double[6] { dh[0].θ, dh[1].θ, dh[2].θ, dh[3].θ, dh[4].θ, dh[5].θ };

            Matrix4x4 curr_end;
            ForwardKinematics(curr_degs, out curr_end);
            //變動後資訊
            double ddeg = 0.01;//角度變動量
            double[] next_degs = new double[6] { (curr_degs[0] + ddeg), curr_degs[1], curr_degs[2], curr_degs[3], curr_degs[4], curr_degs[5] };
            
            Matrix4x4 next_end;
            ForwardKinematics(next_degs, out next_end);
            //數值偏微分
            //∂Px/∂θ_0 = (Px(ndeg) - Px(cdeg)) / (ndeg - cdeg)
            //ddeg = (ndeg - cdeg)
            double P_0 = (next_end.coord[j, 3] - curr_end.coord[j, 3]) / ddeg;

            next_degs = new double[6] { curr_degs[0], (curr_degs[1] + ddeg), curr_degs[2], curr_degs[3], curr_degs[4], curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);

            double P_1 = (next_end.coord[j, 3] - curr_end.coord[j, 3]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1] , (curr_degs[2]+ ddeg), curr_degs[3], curr_degs[4], curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);

            double P_2 = (next_end.coord[j, 3] - curr_end.coord[j, 3]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1], curr_degs[2], (curr_degs[3] + ddeg), curr_degs[4], curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);

            double P_3 = (next_end.coord[j, 3] - curr_end.coord[j, 3]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1], curr_degs[2], curr_degs[3], (curr_degs[4] + ddeg), curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);

            double P_4 = (next_end.coord[j, 3] - curr_end.coord[j, 3]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1], curr_degs[2], curr_degs[3], curr_degs[4], (curr_degs[5] + ddeg) };
            ForwardKinematics(next_degs, out next_end);

            double P_5 = (next_end.coord[j, 3] - curr_end.coord[j, 3]) / ddeg;


            dh[0].θ = curr_degs[0];
            dh[1].θ = curr_degs[1];
            dh[2].θ = curr_degs[2];
            dh[3].θ = curr_degs[3];
            dh[4].θ = curr_degs[4];
            dh[5].θ = curr_degs[5];
            return new double[6] { P_0, P_1, P_2, P_3, P_4, P_5 };
        }

        public double[] GetDiffAngle(Matrix4x4 pos, double[] degs, int j)
        {
            //當前資訊
            double[] curr_degs = new double[6] { dh[0].θ, dh[1].θ, dh[2].θ, dh[3].θ, dh[4].θ, dh[5].θ };
            ForwardKinematics(curr_degs);
            double[] curr_angle = GetXYZFixedAngle(pos);

            //變動後資訊
            double ddeg = 0.01;//角度變動量
            double[] next_degs = new double[6] { (curr_degs[0] + ddeg), curr_degs[1], curr_degs[2], curr_degs[3], curr_degs[4], curr_degs[5] };
            Matrix4x4 next_end;
            ForwardKinematics(next_degs, out next_end);
            double[] next_angle = GetXYZFixedAngle(next_end);

            //數值偏微分
            //∂Px/∂θ_0 = (Px(ndeg) - Px(cdeg)) / (ndeg - cdeg)
            //ddeg = (ndeg - cdeg)
            double P_0 = diffAngle(next_angle[j], curr_angle[j]) / ddeg;

            next_degs = new double[6] { curr_degs[0], (curr_degs[1] + ddeg), curr_degs[2], curr_degs[3], curr_degs[4], curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);
            next_angle = GetXYZFixedAngle(next_end);

            double P_1 = diffAngle(next_angle[j], curr_angle[j]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1], (curr_degs[2] + ddeg), curr_degs[3], curr_degs[4], curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);
            next_angle = GetXYZFixedAngle(next_end);

            double P_2 = diffAngle(next_angle[j], curr_angle[j]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1], curr_degs[2], (curr_degs[3] + ddeg), curr_degs[4], curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);
            next_angle = GetXYZFixedAngle(next_end);

            double P_3 = diffAngle(next_angle[j], curr_angle[j]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1], curr_degs[2], curr_degs[3], (curr_degs[4] + ddeg), curr_degs[5] };
            ForwardKinematics(next_degs, out next_end);
            next_angle = GetXYZFixedAngle(next_end);

            double P_4 = diffAngle(next_angle[j], curr_angle[j]) / ddeg;

            next_degs = new double[6] { curr_degs[0], curr_degs[1], curr_degs[2], curr_degs[3], curr_degs[4], (curr_degs[5] + ddeg) };
            ForwardKinematics(next_degs, out next_end);
            next_angle = GetXYZFixedAngle(next_end);

            double P_5 = diffAngle(next_angle[j] , curr_angle[j]) / ddeg;



            dh[0].θ = curr_degs[0];
            dh[1].θ = curr_degs[1];
            dh[2].θ = curr_degs[2];
            dh[3].θ = curr_degs[3];
            dh[4].θ = curr_degs[4];
            dh[5].θ = curr_degs[5];
            return new double[6] { P_0, P_1, P_2, P_3, P_4, P_5 };
        }

        public double[] GetIKdegs(double[] Point, object sender)//計算所有分割點姿態角度用
        {
            Kinematics kn = (Kinematics)sender;
            double[] deg_out = new double[6];
            Matrix4x4 P_out_pos = new Matrix4x4(Point[0], Point[1], Point[2]);
            Matrix4x4 P_out_rot = P_out_pos.Create_RotXYZ(Point[5], Point[4], Point[3]);//倒著因為輸入為 [x,y,z,γ,β,α]
            Matrix4x4 P_out_fin = P_out_pos * P_out_rot;

            deg_out = kn.InverseKinematics(P_out_fin);

            return deg_out;
        }
        public double diffAngle(double next_angle, double curr_angle)
        {
            double ddeg = next_angle - curr_angle;
            if (ddeg >180)
            {
                ddeg = ddeg - 360;
            }
            else if (ddeg < -180)
            {
                ddeg = ddeg + 360;
            }
            return ddeg*Math.PI/180.0;
        }

        public double[] GetXYZFixedAngle(Matrix4x4 pos)
        { 
            double r11 =pos.coord[0,0];//Xx
            double r21 =pos.coord[1,0];//Xy
            double r31 =pos.coord[2,0];//Xz

            double r32 = pos.coord[2, 1];//Yz
            double r33 = pos.coord[2, 2];//Zz

            double β = Math.Atan2(-r31, Math.Sqrt(r11 * r11 + r21 * r21));
            double α = 0;
            double γ = 0;
            if (Math.Abs(β) !=90d)
            {
                α = Math.Atan2(r21 / Math.Cos(β), r11 / Math.Cos(β));
                γ = Math.Atan2(r32 / Math.Cos(β), r33 / Math.Cos(β));
            }
            else
            {
                if ( β==90d)
                {
                    double r12 = pos.coord[0, 1];//Yx
                    double r22 = pos.coord[1, 1];//Yy
                    α = 0;
                    γ = Math.Atan2(r12, r22);
                }
                else if (β == -90d)
                {
                    double r12 = pos.coord[0, 1];//Yx
                    double r22 = pos.coord[1, 1];//Yy
                    α = 0;
                    γ = -Math.Atan2(r12, r22);
                }
            }

            return new double[] { γ * 180.0 / Math.PI, β * 180.0 / Math.PI, α * 180.0 / Math.PI };//換回角度
        }

        public double[] GetXYZTransAndXYZFixedAngle(Matrix4x4 pos)
        {
            double r11 = pos.coord[0, 0];//Xx
            double r21 = pos.coord[1, 0];//Xy
            double r31 = pos.coord[2, 0];//Xz

            double r32 = pos.coord[2, 1];//Yz
            double r33 = pos.coord[2, 2];//Zz

            double β = Math.Atan2(-r31, Math.Sqrt(r11 * r11 + r21 * r21));
            double α = 0;
            double γ = 0;
            if (Math.Abs(β) != 90d)
            {
                α = Math.Atan2(r21 / Math.Cos(β), r11 / Math.Cos(β));
                γ = Math.Atan2(r32 / Math.Cos(β), r33 / Math.Cos(β));
            }
            else
            {
                if (β == 90d)
                {
                    double r12 = pos.coord[0, 1];//Yx
                    double r22 = pos.coord[1, 1];//Yy
                    α = 0;
                    γ = Math.Atan2(r12, r22);
                }
                else if (β == -90d)
                {
                    double r12 = pos.coord[0, 1];//Yx
                    double r22 = pos.coord[1, 1];//Yy
                    α = 0;
                    γ = -Math.Atan2(r12, r22);
                }
            }

            return new double[] { pos.coord[0, 3], pos.coord[1, 3], pos.coord[2, 3], γ * 180.0 / Math.PI, β * 180.0 / Math.PI, α * 180.0 / Math.PI };//換回角度
        }

        public double[,] Jacobin_Matrix(Matrix4x4 pos, double[] degs)//當前的
        { 
            Matrix4x4 dpos=new Matrix4x4(0,0,0);//變動的

            // ∂Px/∂θ_0 ∂Px/∂θ_1 ∂Px/∂θ_2 ∂Px/∂θ_3 ∂Px/∂θ_4 ∂Px/∂θ_5 
            // ∂Py/∂θ_0 ∂Py/∂θ_1 ∂Py/∂θ_2 ∂Py/∂θ_3 ∂Py/∂θ_4 ∂Py/∂θ_5 
            // ∂Pz/∂θ_0 ∂Pz/∂θ_1 ∂Pz/∂θ_2 ∂Pz/∂θ_3 ∂Pz/∂θ_4 ∂Pz/∂θ_5

            // ∂Pα/∂θ_0 ∂Pα/∂θ_1 ∂Pα/∂θ_2 ∂Pα/∂θ_3 ∂Pα/∂θ_4 ∂Pα/∂θ_5
            // ∂Pβ/∂θ_0 ∂Pβ/∂θ_1 ∂Pβ/∂θ_2 ∂Pβ/∂θ_3 ∂Pβ/∂θ_4 ∂Pβ/∂θ_5
            // ∂Pγ/∂θ_0 ∂Pγ/∂θ_1 ∂Pγ/∂θ_2 ∂Pγ/∂θ_3 ∂Pγ/∂θ_4 ∂Pγ/∂θ_5

            // 6 * 6
            double[] Pxn = GetDiff(pos, degs, 0);//1*6
            double[] Pyn = GetDiff(pos, degs, 1);//1*6
            double[] Pzn = GetDiff(pos, degs, 2);//1*6

            double[] Pα = GetDiffAngle(pos, degs, 0);//1*6
            double[] Pβ = GetDiffAngle(pos, degs, 1);//1*6
            double[] Pγ = GetDiffAngle(pos, degs, 2);//1*6


            double[,] J = new double[6, 6];

            for (int i = 0; i < 6; i++)
            {
                J[0, i] = Pxn[i];
                J[1, i] = Pyn[i];
                J[2, i] = Pzn[i];
                J[3, i] = Pα[i];
                J[4, i] = Pβ[i];
                J[5, i] = Pγ[i];
            }
            return J;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag">世界座標系的座標</param>
        /// <returns></returns>
        public double[] InverseKinematics(Matrix4x4 tag)//要求的末端點
        { 
            //世界座標系下
            //pos[0] 手臂的原點
            //tag 手臂的末端點
            // pos[0] * 0T6 = tag

            // (pos[0]^-1 * pos[0]) * 0T6 =pos[0]^-1 * tag
            // I * 0T6 =pos[0]^-1 * tag

            //0T6 = pos[0]^-1 * tag

            //手臂的坐標系下
            //0T6 末端點
            //已知 當前末端End(關節角度已知 dh[i].θ ) 與需求末端 Tag  
            //求出 需求末端 的 關節角度

            //Using_f = true : 方程式解
            //Using_f = false : 數值解
            bool Using_f = true;
            if (Using_f)
            {
                #region 方程式解
                //1.方程式解 (困難) p.117   4-7 puma 560 IK

                //沒有數值 只有方程式
                //Matrix4x4 _1T6 = new Matrix4x4(0, 0, 0);
                //for (int i = 1; i < nTn1.Count; i++)
                //{
                //    _1T6 = _1T6 * nTn1[i];//[i+1] = [i] * [ i T (i+1) ]
                //}

                //0T6 = pos[0](-1) * tag
                Matrix4x4 _0T6 = pos[0].GetInverse() * tag;

                double Px = _0T6.coord[0, 3];//要求的末端點
                double Py = _0T6.coord[1, 3];//要求的末端點
                double d3 = dh[2].d;//(2,4)
                double θ1 = Math.Atan2(Py, Px) - Math.Atan2(d3, Math.Sqrt(Px * Px + Py * Py - d3 * d3));
                //double _θ1 = Math.Atan2(Py, Px) - Math.Atan2(d3, -Math.Sqrt(Px * Px + Py * Py - d3 * d3));

                double Pz = _0T6.coord[2, 3];//要求的末端點
                double dh_a2 = dh[2].a;
                double dh_a3 = dh[3].a;
                double dh_d3 = dh[2].d;
                double dh_d4 = dh[3].d;
                double K = (Px * Px + Py * Py + Pz * Pz - dh_a2 * dh_a2 - dh_a3 * dh_a3 - dh_d3 * dh_d3 - dh_d4 * dh_d4) / (2 * dh_a2);


                double θ3 = Math.Atan2(dh_a3, dh_d4) - Math.Atan2(K, Math.Sqrt(dh_a3 * dh_a3 + dh_d4 * dh_d4 - K * K));

                //Matrix4x4 _3T6 = new Matrix4x4(0, 0, 0);
                //for (int i = 3; i < nTn1.Count; i++)
                //{
                //    _3T6 = _3T6 * nTn1[i];//[i+1] = [i] * [ i T (i+1) ]
                //}

                double u1 = (-dh_a3 - dh_a2 * Math.Cos(θ3)) * Pz;
                double u2 = -(Math.Cos(θ1) * Px + Math.Sin(θ1) * Py) * (dh_d4 - dh_a2 * Math.Sin(θ3));

                double v1 = (-dh_d4 + dh_a2 * Math.Sin(θ3)) * Pz;
                double v2 = -(dh_a3 + dh_a2 * Math.Cos(θ3)) * (Math.Cos(θ1) * Px + Math.Sin(θ1) * Py);


                double l = Pz * Pz + Math.Pow(Math.Cos(θ1) * Px + Math.Sin(θ1) * Py, 2);

                double s23 = (u1 + u2) / l;
                double c23 = (v1 - v2) / l;


                double θ23 = Math.Atan2(s23, c23);

                double θ2 = θ23 - θ3;

                //θ4
                double w1 = -_0T6.coord[0, 2] * Math.Sin(θ1) + _0T6.coord[1, 2] * Math.Cos(θ1);

                double x1 = -_0T6.coord[0, 2] * Math.Cos(θ1) * Math.Cos(θ23) - _0T6.coord[1, 2] * Math.Sin(θ1) * Math.Cos(θ23) + _0T6.coord[2, 2] * Math.Sin(θ23);



                double θ4 = Math.Atan2(w1, x1);//θ5 ==0 奇異點 !!

                double s5 = -(_0T6.coord[0, 2] * (Math.Cos(θ1) * Math.Cos(θ23) * Math.Cos(θ4) + Math.Sin(θ1) * Math.Sin(θ4)) + _0T6.coord[1, 2] * (Math.Sin(θ1) * Math.Cos(θ23) * Math.Cos(θ4) - Math.Cos(θ1) * Math.Sin(θ4)) - _0T6.coord[2, 2] * (Math.Sin(θ23) * Math.Cos(θ4)));

                double c5 = _0T6.coord[0, 2] * (-Math.Cos(θ1) * Math.Sin(θ23)) + _0T6.coord[1, 2] * (-Math.Sin(θ1) * Math.Sin(θ23)) + _0T6.coord[2, 2] * (-Math.Cos(θ23));

                double θ5 = Math.Atan2(s5, c5);



                double s6 = -_0T6.coord[0, 0] * (Math.Cos(θ1) * Math.Cos(θ23) * Math.Sin(θ4) - Math.Sin(θ1) * Math.Cos(θ4)) - _0T6.coord[1, 0] * (Math.Sin(θ1) * Math.Cos(θ23) * Math.Sin(θ4) + Math.Cos(θ1) * Math.Cos(θ4)) + _0T6.coord[2, 0] * (Math.Sin(θ23) * Math.Sin(θ4));


                double r11_ = _0T6.coord[0, 0] * ((Math.Cos(θ1) * Math.Cos(θ23) * Math.Cos(θ4) + Math.Sin(θ1) * Math.Sin(θ4)) * Math.Cos(θ5) - Math.Cos(θ1) * Math.Sin(θ23) * Math.Sin(θ5));

                double r21_ = _0T6.coord[1, 0] * ((Math.Sin(θ1) * Math.Cos(θ23) * Math.Cos(θ4) - Math.Cos(θ1) * Math.Sin(θ4)) * Math.Cos(θ5) - Math.Sin(θ1) * Math.Sin(θ23) * Math.Sin(θ5));

                double r31_ = -_0T6.coord[2, 0] * (Math.Sin(θ23) * Math.Cos(θ4) * Math.Cos(θ5) + Math.Cos(θ23) * Math.Sin(θ5));

                double c6 = r11_ + r21_ + r31_;

                double θ6 = Math.Atan2(s6, c6);


                double θ4_ = θ4;
                double θ6_ = θ6;

                if (Math.Abs(θ5) < 0.0001)
                {
                    θ4 = θ4_ + θ6_;

                    if (θ4 >= 180d)
                    {
                        θ4 = θ4 - 360d;
                    }
                    else if (θ4 <= -180d)
                    {
                        θ4 = θ4 + 360d;
                    }

                    θ6 = 0;
                }

                θ1 = θ1 * 180.0 / Math.PI;
                θ2 = θ2 * 180.0 / Math.PI;
                θ3 = θ3 * 180.0 / Math.PI;
                θ4 = θ4 * 180.0 / Math.PI;
                θ5 = θ5 * 180.0 / Math.PI;
                θ6 = θ6 * 180.0 / Math.PI;

                double[] degs_ = new double[6] { θ1, θ2, θ3, θ4, θ5, θ6 };

                return degs_;
                #endregion
            }
            else
            {
                #region Jacobin Matrix 數值解
                //2.Jacobin Matrix 數值解  ( 第二難 )奇異點 問題
                // v =J*ω
                // J(-1)v = ω
                // θn+1 = θn + ω * dt
                //Matrix4x4 P1 = new Matrix4x4(0, 0, 0);
                //Matrix4x4 P2 = new Matrix4x4(0, 0, 0);
                //當前的末端點

                // V  L =    轉軸當前的位置 到 末端點的位置 之間的差   就是L向量
                // L1----L6

                //tag 要求的末端點
                //end 當前的末端點
                //J 求出當前末端點(end)的Jacobians Matrix
                //dh degs => 當前的角度 
                //nTn1 => 當前的連桿轉換關係
                int limmit = 20;
                Matrix4x4[] pos_tmp = new Matrix4x4[pos.Count];
                for (int i = 0; i < pos.Count; i++)
                {
                    pos_tmp[i] = new Matrix4x4(pos[i]);
                }
                double[] degs = new double[6] { dh[0].θ, dh[1].θ, dh[2].θ, dh[3].θ, dh[4].θ, dh[5].θ };
            Jacobin:

                Matrix4x4 end = new Matrix4x4(pos_tmp[pos_tmp.Length-1]);
                //for (int i = 0; i < nTn1.Count; i++)
                //{
                //    end = end * nTn1[i];//[i+1] = [i] * [ i T (i+1) ]
                //}

                

                int ind_end = pos_tmp.Length - 1;

                //L1 θ1的位置 PX PY PZ  末端的位置 PX PY PZ
                //Point3D L1 = new Point3D(pos_tmp[ind_end].coord[0, 3] - pos_tmp[0].coord[0, 3],
                //    pos_tmp[ind_end].coord[1, 3] - pos_tmp[0].coord[1, 3],
                //    pos_tmp[ind_end].coord[2, 3] - pos_tmp[0].coord[2, 3]);
                //Z1 θ1的轉軸向量
                //Point3D Z1 = new Point3D(pos_tmp[0].coord[0, 2],pos_tmp[0].coord[1,2],pos_tmp[0].coord[2,2]);
                //Point3D partial_V1 = Z1.cross(Z1, L1);

                Point3D[] L = new Point3D[6];
                Point3D[] Z = new Point3D[6];
                Point3D[] partial_V = new Point3D[6];
                Point3D[] partial_ω = new Point3D[6];

                //XYZfixedAngle
                //Point3D end_x = new Point3D(1, 0, 0);//世界座標係下的x軸
                //Point3D end_y = new Point3D(0, 1, 0);//世界座標係下的y軸
                //Point3D end_z = new Point3D(0, 0, 1);//世界座標係下的Z軸

                Point3D end_x = new Point3D(end.coord[0, 0], end.coord[1, 0], end.coord[2, 0]);//末端座標下的x軸
                Point3D end_y = new Point3D(end.coord[0, 1], end.coord[1, 1], end.coord[2, 1]);//末端座標下的y軸
                Point3D end_z = new Point3D(end.coord[0, 2], end.coord[1, 2], end.coord[2, 2]);//末端座標下的Z軸

                for (int i = 1; i < (6+1); i++)
                {
                    Point3D Li = new Point3D(end.coord[0, 3] - pos_tmp[i].coord[0, 3],
                    end.coord[1, 3] - pos_tmp[i].coord[1, 3],
                    end.coord[2, 3] - pos_tmp[i].coord[2, 3]);
                    //Z1 θ1的轉軸向量
                    Point3D Zi = new Point3D(pos_tmp[i].coord[0, 2], pos_tmp[i].coord[1, 2], pos_tmp[i].coord[2, 2]);
                    Point3D partial_Vi = Zi.cross(Zi, Li);

                    Point3D partial_ωi = new Point3D(Zi.dot(Zi, end_x), Zi.dot(Zi, end_y), Zi.dot(Zi, end_z));

                    L[i-1] = Li;
                    Z[i-1] = Zi;
                    partial_V[i-1] = partial_Vi;
                    partial_ω[i-1] = partial_ωi;
                }

                double[,] J = new double[6, 6];//[j,i]  [row,colum]

                for (int i = 0; i < 6; i++)
                {
                    J[0, i] = partial_V[i].x;
                    J[1, i] = partial_V[i].y;
                    J[2, i] = partial_V[i].z;
                    J[3, i] = partial_ω[i].x;
                    J[4, i] = partial_ω[i].y;
                    J[5, i] = partial_ω[i].z;
                }
                //double[,] J = Jacobin_Matrix(end, degs);
                ///逆矩陣 begin
                double[][] matrix = new double[][] 
                {
                    new double[6] , 
                    new double[6] ,
                    new double[6] ,
                    new double[6] ,
                    new double[6] ,
                    new double[6] 
                };
                for (int j = 0; j < 6; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        matrix[j][i] = J[j, i];
                    }
                }
                bool check;
                double[][] Invmatrix = Tool.InverseMatrix(matrix, out check);
                if (check ==false)
                {
                    limmit = -1;
                }

                double det = Tool.Determinant(matrix);
                double det_inv = Tool.Determinant(Invmatrix);

                double detxinv = det * det_inv;

                double[,] invJ = new double[6, 6];


                for (int j = 0; j < 6; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        invJ[j, i] = Invmatrix[j][i];
                    }
                }

                //StringBuilder sb = new StringBuilder();
                //StringBuilder sbi = new StringBuilder();

                //for (int j = 0; j < 6; j++)
                //{
                //    for (int i = 0; i < 6; i++)
                //    {
                //        sb.Append(J[j, i]);
                //        sbi.Append(invJ[j, i]);
                //        if (i!=5)
                //        {
                //            sb.Append(" ");
                //            sbi.Append(" ");
                //        }
                        
                //    }
                //    sb.Append("\n");
                //    sbi.Append("\n");
                //}
                //Console.WriteLine(sb.ToString());

                //Console.WriteLine(sbi.ToString());
                //測試 J*J-1 = I
                double[,] Test_I = new double[6, 6];
                for (int j = 0; j < 6; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Test_I[j, i] = 0;
                        for (int k = 0; k < 6; k++)
                        {
                            Test_I[j, i] += invJ[j, k] * J[k, i];//要記得+=
                        }
                    }
                }
                ///逆矩陣 end
                ///
                // end * eTg = tag   : eTg 4x4
                //eTg = end^-1 * tag
                //  GetXYZFixedAngle(eTg)
                Matrix4x4 eTg = end.GetInverse() * tag;
                double[] dv = new double[3] { eTg.coord[0, 3], eTg.coord[1, 3], eTg.coord[2, 3] };

                double[] dω = GetXYZFixedAngle(eTg);
                dω[0] = dω[0] * Math.PI / 180.0;
                dω[1] = dω[1] * Math.PI / 180.0;
                dω[2] = dω[2] * Math.PI / 180.0;

                for (int i = 0; i < 3; i++)
                {
                    if (dω[i] > 180)
                    {
                        dω[i] = dω[i] - 360;
                    }
                    else if (dω[i] < -180)
                    {
                        dω[i] = dω[i] + 360;
                    }
                }

                double[] tagω = GetXYZFixedAngle(tag);
                double[] endω = GetXYZFixedAngle(end);
                double[] V = new double[6] {    tag.coord[0, 3] - end.coord[0, 3],
                                                tag.coord[1, 3] - end.coord[1, 3],
                                                tag.coord[2, 3] - end.coord[2, 3],
                                                dω[0],
                                                dω[1],
                                                dω[2] };

                // J(-1)v = ω  求 ω
                double[] VP = new double[3] { V[0], V[1], V[2] };
                double[] Vω = new double[3] { V[3], V[4], V[5] };

                double[] V_End = new double[6] { VP[0], VP[1], VP[2], Vω[0], Vω[1], Vω[2] };//V

                double[] ω = new double[6];

                for (int j = 0; j < 6; j++)
                {
                    ω[j] = 0;
                    for (int k = 0; k < 6; k++)
                    {
                        ω[j] += invJ[j, k] * V_End[k];
                    }
                }
                //更新 角度 θn+1 = θn + ω * dt
                double dt = 1;
                double[] new_degs = new double[6] { degs[0] + ω[0]*(180.0/Math.PI) * dt,
                                                    degs[1] + ω[1]*(180.0/Math.PI) * dt,
                                                    degs[2] + ω[2]*(180.0/Math.PI) * dt,
                                                    degs[3] + ω[3]*(180.0/Math.PI) * dt,
                                                    degs[4] + ω[4]*(180.0/Math.PI) * dt,
                                                    degs[5] + ω[5]*(180.0/Math.PI) * dt };
                for (int i = 0; i < 6; i++)
                {
                    //把範圍限定在 +-360 之間
                    if (new_degs[i]>=360)
                    {
                        new_degs[i] -= 360;
                    }
                    else if (new_degs[i]<= -360)
                    {
                        new_degs[i] += 360;
                    }

                    //把範圍限定在 +-180 之間
                    if (new_degs[i] >= 180)
                    {
                        new_degs[i] = new_degs[i] - 360;
                    }
                    else if (new_degs[i] <= -180)
                    {
                        new_degs[i] = new_degs[i] + 360;
                    }
                }

                ForwardKinematics(new_degs,ref pos_tmp);
                for (int i = 0; i < 6; i++)
                {
                    degs[i] = new_degs[i];
                }

                double lost = 0;
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        lost +=Math.Pow (tag.coord[j, i] - pos_tmp[pos_tmp.Length - 1].coord[j, i],2);
                    }
                }
                Console.WriteLine("lost : " + lost);
                if (lost <= 0.000001)
                {
                    return new_degs;
                }
                else
                {
                    limmit -= 1;
                    if (limmit<0)
                    {
                        return new_degs;
                    }
                    goto Jacobin;
                }

                // lost = tag - end
                // if lost <0.01;
                // 結束
                return new_degs;
                #endregion
            }  
            //3.剃度下降法 (PSO NN ....) 重點 : 損失函數 (最簡單的)  問題 (必須要有電腦 運算複雜度高)
        }

        private Matrix4x4 Create_RotX(double rx)
        {
            Matrix4x4 RotX = new Matrix4x4(0, 0, 0);
            RotX.coord = RotX.RotX(RotX.coord, rx);
            return RotX;
        }
        private Matrix4x4 Create_RotY(double ry)
        {
            Matrix4x4 RotY = new Matrix4x4(0, 0, 0);
            RotY.coord = RotY.RotY(RotY.coord, ry);
            return RotY;
        }
        private Matrix4x4 Create_RotZ(double rz)
        {
            Matrix4x4 RotZ = new Matrix4x4(0, 0, 0);
            RotZ.coord = RotZ.RotZ(RotZ.coord, rz);
            return RotZ;
        }
        private Matrix4x4 Create_TransX(double dx)
        {
            Matrix4x4 TransX = new Matrix4x4(dx, 0, 0);
            return TransX;
        }
        private Matrix4x4 Create_TransY(double dy)
        {
            Matrix4x4 TransX = new Matrix4x4(0, dy, 0);
            return TransX;
        }
        private Matrix4x4 Create_TransZ(double dz)
        {
            Matrix4x4 TransZ = new Matrix4x4(0, 0, dz);
            return TransZ;
        }
        /// <summary>
        /// //RotX(α) * TransX(a) * TransZ(d) * RotZ(θ)
        /// </summary>
        /// <param name="dz"></param>
        /// <returns></returns>
        public Matrix4x4 Create_RTTR(double α, double a, double d, double θ)
        {
            Matrix4x4 RotX = Create_RotX(α);
            Matrix4x4 TransX = Create_TransX(a);
            Matrix4x4 TransZ = Create_TransZ(d);
            Matrix4x4 RotZ = Create_RotZ(θ);//ROTZ

            Matrix4x4 RTTR = ((RotX * TransX) * TransZ) * RotZ;

            return RTTR;
        }
        public Matrix4x4 Create_RTRT(double α, double a, double d, double θ)
        {
            Matrix4x4 RotX = Create_RotX(α);
            Matrix4x4 TransX = Create_TransX(a);
            Matrix4x4 TransZ = Create_TransZ(d);
            Matrix4x4 RotZ = Create_RotZ(θ);//ROTZ

            Matrix4x4 RTRT = ((RotX * TransX) * RotZ) * TransZ;

            return RTRT;
        }

    }
}
