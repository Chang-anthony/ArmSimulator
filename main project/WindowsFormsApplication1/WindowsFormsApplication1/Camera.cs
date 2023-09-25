using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Point3D
    {
        public double x, y, z;
        public Point3D()
        { 
        
        }
        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// cross 差積 (外積)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public Point3D cross(Point3D a,Point3D b)
        {
            //i,j,k
            //x,y,z
            //u,v,w
            //
            Point3D c = new Point3D();
            c.x =    a.y * b.z - a.z * b.y;
            c.y = -( a.x * b.z - a.z * b.x);
            c.z =    a.x * b.y - a.y * b.x;
            return c;
        }
        /// <summary>
        /// dot 點積 (內積)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public double dot(Point3D a, Point3D b)
        {
            return a.x*b.x+a.y*b.y+a.z*b.z;
        }
        /// <summary>
        /// 單位向量 長度1
        /// </summary>
        /// <returns></returns>
        public Point3D norm()
        {
            double len = Math.Sqrt(x * x + y * y + z * z);
            Point3D n = new Point3D(x / len, y / len, z / len);
            return n;
        }
    }
    public class Camera
    {
        public Matrix4x4 pos;
        public Camera(double x,double y,double z)
        {
            pos = new Matrix4x4(x,y,z);
        }

        public void PTZ(double dx,double dy)
        {
            Matrix4x4 rotx = new Matrix4x4(0, 0, 0);
            rotx.coord = rotx.RotX(rotx.coord, dy);

            Matrix4x4 roty = new Matrix4x4(0, 0, 0);
            roty.coord = roty.RotY(roty.coord, dx);

            Matrix4x4 tmp =pos.GetRotationMat();//將旋轉跟位移分開

            Matrix4x4 end = (roty * (rotx * tmp));//前乘

            pos.SetRotationMat(end);//將旋轉完的矩陣送回
        }
        public void PTZ(Matrix4x4 pos,double dx, double dy)
        {
            Matrix4x4 rotx = new Matrix4x4(0, 0, 0);
            rotx.coord = rotx.RotX(rotx.coord, dy);

            Matrix4x4 rotz = new Matrix4x4(0, 0, 0);
            rotz.coord = rotz.RotZ(rotz.coord, dx);

            Matrix4x4 tmp = pos.GetRotationMat();//將旋轉跟位移分開(防止前乘將位移變動)

            //前乘世界座標系的Z軸(固定角 fixed)  後乘攝影機座標系的X軸(變動角 Euler)
            Matrix4x4 end = (rotz * ( tmp *rotx));

            this.pos.SetRotationMat(end);//將旋轉完的矩陣送回
            //this.pos = pos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LookAt">你看著的座標</param>
        /// <param name="Top">世界的Z軸</param>
        public void GetMat(Matrix4x4 LookAt,Point3D Top)
        { 
            double dx =LookAt.coord[0,3]-pos.coord[0,3];
            double dy =LookAt.coord[1,3]-pos.coord[1,3];
            double dz =LookAt.coord[2,3]-pos.coord[2,3];

            Point3D look = new Point3D(dx, dy, dz).norm();//攝影機的Z軸

            Point3D right = look.cross(look, Top).norm();//攝影機的X軸
            Point3D top = right.cross(right, look).norm();//攝影機的 -Y軸

            pos.coord[0, 0] = right.x;//X軸方向
            pos.coord[1, 0] = right.y;
            pos.coord[2, 0] = right.z;

            pos.coord[0, 1] = -top.x;//Y軸方向
            pos.coord[1, 1] = -top.y;
            pos.coord[2, 1] = -top.z;

            pos.coord[0, 2] = look.x;//Z軸方向
            pos.coord[1, 2] = look.y;
            pos.coord[2, 2] = look.z;

        }

        public Matrix4x4 Inverse()
        { 
            return new Matrix4x4( pos.inverse());
        }

        /// <summary>
        /// wTp 世界座標系中的座標點  ==> cTp 攝影機座標系的座標點
        /// cTw 在攝影機座標系中的世界座標
        /// wTc 在世界座標系中的攝影機座標
        /// </summary>
        /// <param name="p"></param>
        /// <returns>攝影機座標系的座標點</returns>
        public Matrix4x4 Convert(Matrix4x4 p)
        {
            Matrix4x4 cTw = Inverse();
            Matrix4x4 cTp = cTw * p;
            return cTp;
        }

        public Matrix4x4 Convert(Matrix4x4 p,out double Len)
        {
            Matrix4x4 cTw = Inverse();
            Matrix4x4 cTp = cTw * p;
            Len = GetLenZ(cTp);//得到對攝影機的距離 ZAxis
            return cTp;
        }
        public Matrix4x4[] Convert(Matrix4x4[] p)
        {
            Matrix4x4 cTw = Inverse();
            Matrix4x4[] cTp = new Matrix4x4[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                cTp[i] =  cTw * p[i];
            }
            return cTp;
        }
        public Matrix4x4[] Convert(Matrix4x4[] p, out double[] Len)
        {
            Matrix4x4 cTw = Inverse();
            Matrix4x4[] cTp = new Matrix4x4[p.Length];
            Len = new double[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                cTp[i] = cTw * p[i];
                Len[i] = GetLenZ(cTp[i]);
            }
            return cTp;
        }
        public double GetLen(Matrix4x4 p)
        {
            //(x*x+y*y+z*z)^0.5
            return Math.Sqrt(p.coord[0, 3] * p.coord[0, 3] + p.coord[1, 3] * p.coord[1, 3] + p.coord[2, 3] * p.coord[2, 3]);
        }
        public double GetLenZ(Matrix4x4 p)
        {
            //PZ
            return p.coord[2, 3];
        }
        public Matrix4x4[,] Convert(Matrix4x4[,] p, out double[,] lens)// p 世界坐標系下的座標
        {
            Matrix4x4 cTw = Inverse();
            Matrix4x4[,] cTp = new Matrix4x4[p.GetLength(0), p.GetLength(1)];

            lens = new double[p.GetLength(0), p.GetLength(1)];

            for (int j = 0; j < p.GetLength(0); j++)
            {
                for (int i = 0; i < p.GetLength(1); i++)
                {
                    cTp[j, i] = cTw * p[j, i];
                    lens[j, i] = GetLenZ(cTp[j, i]);//得到對攝影機的距離 ZAxis
                }
            }
            return cTp;
        }

    }
}
