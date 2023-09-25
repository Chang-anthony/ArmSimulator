using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Matrix4x4
    {
        static double W = 640;
        static double H = 480;
        static double r = Math.Sqrt(Math.Pow((W / 2d), 2) + Math.Pow((H / 2d), 2));//對邊

        static double FOV = 90;//Deg

        static double γ = FOV / 2d;//夾角(deg)

        static double f = r / Math.Tan(γ * Math.PI / 180d);//求鄰邊(帶入徑度)

        public double[,] coord = new double[4, 4];
        public Matrix4x4(double x, double y, double z)
        {
            coord[0, 0] = 1;
            coord[1, 1] = 1;
            coord[2, 2] = 1;
            coord[3, 3] = 1;

            coord[0, 3] = x;
            coord[1, 3] = y;
            coord[2, 3] = z;

        }
        public Matrix4x4(double x, double y, double z, double rx, double ry, double rz)
        {
            coord[0, 0] = 1;
            coord[1, 1] = 1;
            coord[2, 2] = 1;
            coord[3, 3] = 1;

            coord[0, 3] = x;
            coord[1, 3] = y;
            coord[2, 3] = z;

            Matrix4x4 RotXYZ = Create_RotXYZ( rx,  ry,  rz);

            this.SetRotationMat(RotXYZ);

        }
        public Matrix4x4(double[] q)
        {
            coord[0, 0] = 1;
            coord[1, 1] = 1;
            coord[2, 2] = 1;
            coord[3, 3] = 1;

            coord[0, 3] = q[0];
            coord[1, 3] = q[1];
            coord[2, 3] = q[2];

            Matrix4x4 RotXYZ = Create_RotXYZ(q[5], q[4], q[4]);
            this.SetRotationMat(RotXYZ);
        }
        public Matrix4x4(double[,] src)
        {
            coord = src;
        }
        public Matrix4x4(Matrix4x4 src)
        {
            Copy(src);
        }
        /// <summary>
        /// 將矩陣 轉換回 q
        /// q = [ x, y, z, rz, ry, rx]
        /// </summary>
        /// <param name="pos">Matrix4x4</param>
        /// <returns>X Y Z RX RY RZ</returns>
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

            return new double[] { pos.coord[0, 3], pos.coord[1, 3], pos.coord[2, 3], α * 180.0 / Math.PI, β * 180.0 / Math.PI, γ * 180.0 / Math.PI };//換回角度
        }
        public double[] GetXYZTransAndXYZFixedAngle()//取出矩陣的 PX PY PZ 與 固定角
        {
            double r11 = coord[0, 0];//Xx
            double r21 = coord[1, 0];//Xy
            double r31 = coord[2, 0];//Xz

            double r32 = coord[2, 1];//Yz
            double r33 = coord[2, 2];//Zz

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
                    double r12 = coord[0, 1];//Yx
                    double r22 = coord[1, 1];//Yy
                    α = 0;
                    γ = Math.Atan2(r12, r22);
                }
                else if (β == -90d)
                {
                    double r12 = coord[0, 1];//Yx
                    double r22 = coord[1, 1];//Yy
                    α = 0;
                    γ = -Math.Atan2(r12, r22);
                }
            }

            return new double[] { coord[0, 3], coord[1, 3], coord[2, 3], γ * 180.0 / Math.PI, β * 180.0 / Math.PI, α * 180.0 / Math.PI };//換回角度
        }

        public double[] Getqpoint()
        {
            double[] q = GetXYZTransAndXYZFixedAngle();

            return q;
        }
        public string str_qpiont(double[] p)
        {
            double[] q = p;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < q.Length; i++)
            {
                sb.Append(string.Format("{0,6:F3}", q[i]));
                if (i < q.Length - 1)
                {
                    sb.Append(" , ");
                }
            }
            sb.Append("\r\n");
            return sb.ToString();
        }
        public string str_q()//將取出的list點 輸出在table
        {
            double[] q = GetXYZTransAndXYZFixedAngle();
            StringBuilder sb = new StringBuilder();
           
            for (int i = 0; i < q.Length; i++)
            {
                sb.Append(string.Format("{0,6:F3}", q[i]));
                if (i < q.Length-1)
                {
                    sb.Append(" , ");
                }
            }
            sb.Append("\r\n");
            return sb.ToString();
        }

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < 4; j++)
			{
                for (int i = 0; i < 4; i++)
                {
                    sb.Append(string.Format("{0,6:F3}",coord[j, i]));
                    if (i<3)
                    {
                        sb.Append(" , ");
                    }
                    
                }
                sb.Append("\r\n");
			}

            return sb.ToString();
        }
        public void Copy(Matrix4x4 src)//複製矩陣的值
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    coord[j, i] = src.coord[j, i];
                }
            }
        }
        public Matrix4x4 GetRotationMat()
        {
            Matrix4x4 Rotation = new Matrix4x4(0, 0, 0);
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    Rotation.coord[j, i] = coord[j, i];
                }
            }
            return Rotation;
        }
        public void SetRotationMat(Matrix4x4 Rotation)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    coord[j, i] = Rotation.coord[j, i];
                }
            }
        }

        public void draw(Graphics g)
        {
            PointF P = Coord2D(coord);
            PointF X = Xaxis2D(coord);
            PointF Y = Yaxis2D(coord);
            PointF Z = Zaxis2D(coord);

            g.DrawLine(new Pen(Color.FromArgb(128,255,0,0), 3), P, X);//X

            g.DrawLine(new Pen(Color.FromArgb(128, 0, 255, 0), 3), P, Y);//Y

            g.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255), 3), P, Z);//Z
        }
        public void draw(Graphics g,int A,int d)
        {
            PointF P = Coord2D(coord);
            PointF X = Xaxis2D(coord);
            PointF Y = Yaxis2D(coord);
            PointF Z = Zaxis2D(coord);

            g.DrawLine(new Pen(Color.FromArgb(A, 255, 0, 0), d), P, X);//X

            g.DrawLine(new Pen(Color.FromArgb(A, 0, 255, 0), d), P, Y);//Y

            g.DrawLine(new Pen(Color.FromArgb(A, 0, 0, 255), d), P, Z);//Z
        }

        public double[,] inverse()
        {
            double[,] inv = new double[4, 4];

            //轉至
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    inv[j, i] = coord[i, j];
                }
            }

            for (int j = 0; j < 3; j++)
            {
                inv[j, 3] = 0;
                for (int k = 0; k < 3; k++)
                {
                    //T 是第3欄   K是捲動量 j=0:x j=1:y j=2:z
                    inv[j, 3] +=inv[j, k] * coord[k, 3];
                }
                inv[j, 3] = -inv[j, 3];//取負號   -(R)T x T
            }
            inv[3, 3] = 1;//輔助因子 要設為1
            return inv;
        }
        public Matrix4x4 GetInverse()
        {
            double[,] inv = new double[4, 4];

            //轉至
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    inv[j, i] = coord[i, j];
                }
            }

            for (int j = 0; j < 3; j++)
            {
                inv[j, 3] = 0;
                for (int k = 0; k < 3; k++)
                {
                    //T 是第3欄   K是捲動量 j=0:x j=1:y j=2:z
                    inv[j, 3] += inv[j, k] * coord[k, 3];
                }
                inv[j, 3] = -inv[j, 3];//取負號   -(R)T x T
            }
            inv[3, 3] = 1;//輔助因子 要設為1
            return new Matrix4x4(inv);
        }

        public double[,] move(double[,] src, double dx, double dy, double dz)
        {
            src[0, 3] += dx;
            src[1, 3] += dy;
            src[2, 3] += dz;
            return src;
        }
        public double[,] Trans(double[,] src, double dx, double dy, double dz)
        {
            double[,] dst = new double[4, 4];
            double[,] trans = new double[4, 4];

            trans[0, 0] = 1;
            trans[1, 1] = 1;
            trans[2, 2] = 1;
            trans[3, 3] = 1;

            trans[0, 3] = dx;
            trans[1, 3] = dy;
            trans[2, 3] = dz;

            for (int j = 0; j < 4; j++)//rows j
            {
                for (int i = 0; i < 4; i++)//colums i
                {
                    dst[j, i] = 0;
                    for (int k = 0; k < 4; k++)//平移 K
                    {
                        dst[j, i] += src[j, k] * trans[k, i];
                    }
                }
            }
            return dst;
        }

        public Matrix4x4 Trans(Matrix4x4 src, double dx, double dy, double dz)
        {
            Matrix4x4 dst = new Matrix4x4(0, 0, 0);
            Matrix4x4 trans = new Matrix4x4(0, 0, 0);

            trans.coord[0, 0] = 1;
            trans.coord[1, 1] = 1;
            trans.coord[2, 2] = 1;
            trans.coord[3, 3] = 1;

            trans.coord[0, 3] = dx;
            trans.coord[1, 3] = dy;
            trans.coord[2, 3] = dz;

            for (int j = 0; j < 4; j++)//rows j
            {
                for (int i = 0; i < 4; i++)//colums i
                {
                    dst.coord[j, i] = 0;
                    for (int k = 0; k < 4; k++)//平移 K
                    {
                        dst.coord[j, i] += src.coord[j, k] * trans.coord[k, i];
                    }
                }
            }
            return dst;
        }
        public Matrix4x4 Create_RotXYZ(double rx, double ry, double rz)// rz ry rx
        {
            Matrix4x4 MatZ = new Matrix4x4(0, 0, 0);
            MatZ.coord = MatZ.RotZ(MatZ.coord, rz);
            Matrix4x4 MatY = new Matrix4x4(0, 0, 0);
            MatY.coord = MatY.RotY(MatY.coord, ry);
            Matrix4x4 MatX = new Matrix4x4(0, 0, 0);
            MatX.coord = MatX.RotX(MatX.coord, rx);
            Matrix4x4 MatXYZ = MatZ * MatY * MatX;
            return MatXYZ;
        }

        public Matrix4x4 Inverse(Matrix4x4 pos)
        {
            return new Matrix4x4(pos.inverse());
        }
        public Matrix4x4 Convert(Matrix4x4 p,Matrix4x4 pos)
        {
            Matrix4x4 cTw = Inverse(pos);
            Matrix4x4 cTp = cTw * p;
            return cTp;
        }

        public static Matrix4x4 operator *(Matrix4x4 A ,Matrix4x4 B)
        {
            double[,] dst = new double[4, 4];
            for (int j = 0; j < 4; j++)//rows j
            {
                for (int i = 0; i < 4; i++)//colums i
                {
                    dst[j, i] = 0;
                    for (int k = 0; k < 4; k++)//平移 K
                    {
                        dst[j, i] += A.coord[j, k] * B.coord[k, i];
                    }
                }
            }
            return new Matrix4x4(dst);
        }
        public static Matrix4x4 operator -(Matrix4x4 A, Matrix4x4 B)
        {
            double[,] dst = new double[4, 4];
            for (int j = 0; j < 4; j++)//rows j
            {
                for (int i = 0; i < 4; i++)//colums i
                {
                    dst[j, i] = A.coord[j, i] - B.coord[j, i];
                }
            }
            return new Matrix4x4(dst);
        }

        public double[,] RotZ(double[,] src, double Deg)
        {
            double[,] dst = new double[4, 4];
            double[,] matZ = new double[4, 4];
            double rad = Deg * Math.PI / 180d;
            matZ[0, 0] = Math.Cos(rad); matZ[0, 1] = -Math.Sin(rad);
            matZ[1, 0] = Math.Sin(rad); matZ[1, 1] = Math.Cos(rad);
            matZ[2, 2] = 1;
            matZ[3, 3] = 1;

            for (int j = 0; j < 4; j++)//rows j
            {
                for (int i = 0; i < 4; i++)//colums i
                {
                    dst[j, i] = 0;
                    for (int k = 0; k < 4; k++)//平移 K
                    {
                        dst[j, i] += src[j, k] * matZ[k, i];
                    }
                }
            }
            return dst;
        }
        public double[,] RotY(double[,] src, double Deg)
        {
            double[,] dst = new double[4, 4];
            double[,] matY = new double[4, 4];
            double rad = Deg * Math.PI / 180d;
            //[ROW,COLUMN]  [J,I]
            matY[0, 0] = Math.Cos(rad); matY[0, 2] = Math.Sin(rad);
            matY[2, 0] = -Math.Sin(rad); matY[2, 2] = Math.Cos(rad);
            matY[1, 1] = 1;
            matY[3, 3] = 1;

            for (int j = 0; j < 4; j++)//rows j
            {
                for (int i = 0; i < 4; i++)//colums i
                {
                    dst[j, i] = 0;
                    for (int k = 0; k < 4; k++)//平移 K
                    {
                        dst[j, i] += src[j, k] * matY[k, i];
                    }
                }
            }
            return dst;
        }
        public double[,] RotX(double[,] src, double Deg)
        {
            double[,] dst = new double[4, 4];
            double[,] matX = new double[4, 4];
            double rad = Deg * Math.PI / 180d;
            //[ROW,COLUMN]  [J,I]
            matX[1, 1] = Math.Cos(rad); matX[1, 2] = -Math.Sin(rad);
            matX[2, 1] = Math.Sin(rad); matX[2, 2] = Math.Cos(rad);
            matX[0, 0] = 1;
            matX[3, 3] = 1;

            for (int j = 0; j < 4; j++)//rows j
            {
                for (int i = 0; i < 4; i++)//colums i
                {
                    dst[j, i] = 0;
                    for (int k = 0; k < 4; k++)//平移 K
                    {
                        dst[j, i] += src[j, k] * matX[k, i];
                    }
                }
            }
            return dst;
        }

        public PointF Coord2D(double[,] src)
        {
            PointF p;
            //u,v 2D
            //x y z 3D  z = F
            //f : F = u : X = v : Y
            double F = src[2, 3];

            double u = f * src[0, 3] / F;
            double v = f * src[1, 3] / F;
            p = new PointF((float)(u + W / 2d), (float)(v + H / 2d));

            return p;
        }
        public PointF Xaxis2D(double[,] src)
        {
            PointF axisX;

            double scale = 10;
            //u,v 2D
            //x y z 3D  z = F
            //f : F = u : X = v : Y
            double F = src[2, 3] + src[2, 0] * scale;// Pz + Xz *倍率

            double u = f * (src[0, 3] + src[0, 0] * scale) / F;
            double v = f * (src[1, 3] + src[1, 0] * scale) / F;
            axisX = new PointF((float)(u + W / 2d), (float)(v + H / 2d));

            return axisX;
        }
        public PointF Yaxis2D(double[,] src)
        {
            PointF axisY;

            double scale = 10;
            //u,v 2D
            //x y z 3D  z = F
            //f : F = u : X = v : Y
            double F = src[2, 3] + src[2, 1] * scale;// Pz + Yz *倍率

            double u = f * (src[0, 3] + src[0, 1] * scale) / F;
            double v = f * (src[1, 3] + src[1, 1] * scale) / F;
            axisY = new PointF((float)(u + W / 2d), (float)(v + H / 2d));

            return axisY;
        }
        public PointF Zaxis2D(double[,] src)
        {
            PointF axisZ;

            double scale = 10;
            //u,v 2D
            //x y z 3D  z = F
            //f : F = u : X = v : Y
            double Zx = src[0, 2];
            double Zy = src[1, 2];
            double Zz = src[2, 2];

            double Px = src[0, 3];
            double Py = src[1, 3];
            double Pz = src[2, 3];

            double F = Pz + Zz * scale;// Pz + Zz *倍率

            double u = f * (Px + Zx * scale) / F;
            double v = f * (Py + Zy * scale) / F;
            axisZ = new PointF((float)(u + W / 2d), (float)(v + H / 2d));

            return axisZ;
        }
    }
}
