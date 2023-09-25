using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trajectorypath_controler;
using WindowsFormsApplication1;

namespace Path_controler
{
    public class Pos//不公開會找不到要取哪個記憶體資料
    {
        int id = 0;//表示線段編號
        public double[] S;//儲存為陣列用
        public Matrix4x4 ori;

        public Pos()
        {
            S = new double[6];
            ori = new Matrix4x4(S);
        }
        public Pos(double[] Q)
        {
            S =(double[]) Q.Clone();
            ori = new Matrix4x4(Q);
        }

        public Pos(double x, double y, double z, double γ, double β, double α)
        {
            S = new double[6];
            ori = new Matrix4x4(x, y, z, α, β, γ);
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.γ = γ;
            this.β = β;
            this.α = α;
        }
        public double X { get => S[0]; set => S[0] = value; }
        public double Y { get => S[1]; set => S[1] = value; }
        public double Z { get => S[2]; set => S[2] = value; }
        public double γ { get => S[3]; set => S[3] = value; }
        public double β { get => S[4]; set => S[4] = value; }
        public double α { get => S[5]; set => S[5] = value; }

        public Pos Move(Pos q,double dx,double dy,double dz)
        {
            q.X += dx;
            q.Y += dy;
            q.Z += dz;
            return q;
        }
        public Pos Trans(Matrix4x4 p,double dx,double dy,double dz)
        {
            Pos q = new Pos();
            q.ori = p.Trans(p,dx,dy,dz); 
            return q;
        }

        public double[] Rots(Matrix4x4 point,Matrix4x4 rot)
        {
            double[] q = new double[6];
            point =  point * rot;
            q = point.GetXYZTransAndXYZFixedAngle(point);
          
            return q;
        }

        public static Pos operator *(Matrix4x4 wTp,Pos p)
        {
            Pos tmp = new Pos();
            Matrix4x4 pTs = new Matrix4x4(p.S);//軌跡坐標系的點
            Matrix4x4 wTs = wTp * pTs;
            tmp.ori = wTs;
            tmp.S = wTs.GetXYZTransAndXYZFixedAngle();

            return tmp;
        }
        //private Matrix4x4 Create_RotMat(double rx,double ry,double rz)
        //{
        //    Matrix4x4 MatZ = new Matrix4x4(0, 0, 0);
        //    MatZ.coord = MatZ.RotZ(MatZ.coord, rz);
        //    Matrix4x4 MatY = new Matrix4x4(0, 0, 0);
        //    MatY.coord = MatY.RotY(MatY.coord, ry);
        //    Matrix4x4 MatX = new Matrix4x4(0, 0, 0);
        //    MatX.coord = MatX.RotX(MatX.coord, rx);
        //    Matrix4x4 RotM = MatZ * MatY * MatX;
        //    return RotM;
        //}
    }

    public interface ILine
    {
        int Id { get; set;}//線段順序
        List<Pos> Q { get; set;}//儲存每個的直線的點
    }
    public class Line:ILine
    {
        int id = 0;//線段順序
        public List<Pos> q = new List<Pos>();//儲存每個的直線的點

        public int Id { get => id; set => id = value; }

        public List<Pos> Q { get => q; set => q = value; }

        public Line(double[] str)
        {
            id = 0;
            Q = new List<Pos>();
        }
        public Line(int id)
        {
            Id = id;
            Q = new List<Pos>();
        }
        public void AddLine(Pos point)
        {
            Q.Add(point);
        }
        public void AddLines(Pos[] point)
        {
            Q.AddRange(point);
        }
        public Line(Pos str, Pos fin)
        {
            id = 0;
            Q = new List<Pos>();
            Q.Add(str);
            Q.Add(fin);
        }
        public Line()
        {
        }
    }
    public class Arc : ILine
    {
        //初始化，給記憶體空間
        int id = 0;//線段順序
        public List<Pos> q = new List<Pos>();//儲存每個的直線的點

        public int Id { get => id; set => id = value; }

        public List<Pos> Q {get => q; set => q = value;}

        public Arc()
        {
            id = 0;
            Q = new List<Pos>();
        }
        public Arc(int id)
        {
            Id = id;
            Q = new List<Pos>();
        }
        public void AddArc(Pos point)
        {
            Q.Add(point);
        }
        public void AddArcs(Pos[] point)
        {
            Q.AddRange(point);
        }
        public Arc(Pos str,Pos via, Pos fin)
        {
            id = 0;
            Q = new List<Pos>();
            Q.Add(str);
            Q.Add(via);
            Q.Add(fin);
        }
    }
    public class Path
    {
        public List<ILine> lines;//儲存每段路徑
        public Matrix4x4 ori;//path 的原點
        public void Trans(double dx, double dy, double dz)
        {
            ori = ori.Trans(ori,dx, dy, dz);
        }

        public void Rot(double rx,double ry,double rz)
        {
            Matrix4x4 rot = new Matrix4x4(0, 0, 0);
            rot = rot.Create_RotXYZ(rx, ry, rz);
            ori = ori * rot;
        }
        public Path()
        {
             lines = new List<ILine>();
             ori = new Matrix4x4(0,0,0);//path 的參考座標
        }

        public Path copy(Path path)
        {
            Path copy = path;
            return copy;
        }

        public List<Pos> Convert(List<Pos> pTs) 
        {
            List<Pos> wTs = new List<Pos>();
            for (int i = 0; i < pTs.Count; i++)
            {
                wTs.Add(ori * pTs[i]);//ori 世界坐標系下的參考原點, pTs 軌跡坐標系下的
            }
            return wTs;
        }

        public void Addline(Line line)//加入直線路徑
        {
            line.Id = lines.Count;//依照順序加入
            lines.Add(line);
        }
        public void AddArc(Arc arc)//加入曲線路徑
        {
            arc.Id = lines.Count;//依照順序加入
            lines.Add(arc);
        }
        public void Delete(int id)//刪除所選線段路徑
        {
            if (lines.Count < id)//根本沒有存所選id這個
            {
                return;
            }
            lines.RemoveAt(id);
        }
        public void Insert(int id,ILine line)//加入所選線段路徑
        {
            lines.Insert(id, line);
        }

        public List<Pos> Create_Path_Point(int id,int hz)//算出對應id 線段的 所有時間分割點姿態
        {
            List < Pos > point= new List<Pos>();//path 坐標系下的 pos

            if (lines[id].Q.Count == 2)//表此id線段為直線
            {
                Trajectory[] q = new Trajectory[6];//總共有6個姿態
                Pos[] coord1 = new Pos[hz + 1];//最後會算出分割點數加一的姿態數量
                for (int i = 0; i < coord1.Length; i++)
                {
                    coord1[i] = new Pos();//給予新記憶體空間
                }
                point.AddRange(coord1);//直接將coord1的範圍加入
                for (int i = 0; i < q.Length; i++)
                {
                    q[i] = new Trajectory(lines[id].Q[0].S[i], lines[id].Q[1].S[i], 1);
                    double[] tmp = q[i].CreateSt(hz);
                    for (int j = 0; j < coord1.Length; j++)
                    {
                        coord1[j].S[i] = tmp[j];//將個別的時間分割點算出再分別丟入 point的狀態[x, y, z, γ,β,α]
                        //假設 現在hz 為100 ，所以分割點算出會有101點
                        //coord1[j] 大小就為101
                        //而每個coord[j] 裡面又存了 point狀態[x, y, z, γ,β,α]
                        //所以最後會成為 coord1[hz+1].point[6]的狀態
                        //算出來就為在這時段裡所有的分割點姿態
                    }
                }      
            }
            else if (lines[id].Q.Count == 3)//表此線段為曲線
            {
                Trajectory[] q = new Trajectory[6];//總共有6個姿態
                Pos[] coord1 = new Pos[hz];//最後會算出分割點數加一的姿態數量
                for (int i = 0; i < coord1.Length; i++)
                {
                    coord1[i] = new Pos();//給予新記憶體空間
                }
                point.AddRange(coord1);//直接將coord1的範圍加入
                for (int i = 0; i < 6; i++)
                {
                    q[i] = new Trajectory(lines[id].Q[0].S[i], lines[id].Q[1].S[i],lines[id].Q[2].S[i], 2);
                    double[] tmp = q[i].CreateViaSt(hz);
                    for (int j = 0; j < coord1.Length; j++)
                    {
                        coord1[j].S[i] = tmp[j];
                    }
                }
            }
            point = Convert(point);
            return point;
        }
    }
}
