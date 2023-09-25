using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Ground
    {
        int w = 0;
        int h = 0;
        int l = 10;//10 pixel  
        public Matrix4x4[,] mat;
        public Ground(int w,int h)
        {
            this.w = w;
            this.h = h;

            mat = new Matrix4x4[w, h];// 數量

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    mat[i, j] = new Matrix4x4((i-w/2)*l,(j-h/2)*l,0);
                }
            }
        }
        public void draw(Graphics g)
        {
            mat[w / 2, h / 2].draw(g);
            SolidBrush sb = new SolidBrush(Color.FromArgb(128, 128, 128, 128));//淺色
            SolidBrush sb_b = new SolidBrush(Color.FromArgb(128, 64, 64, 64));
            for (int i = 0; i < (w-1); i++)//w-1 最後要記得減一
            {
                for (int j = 0; j < (h - 1); j++)//h-1 最後要記得減一
                {
                    PointF p0 = mat[i, j].Coord2D(mat[i, j].coord);
                    PointF p1 = mat[i, j+1].Coord2D(mat[i, j+1].coord);
                    PointF p2 = mat[i+1, j+1].Coord2D(mat[i+1, j+1].coord);
                    PointF p3 = mat[i+1, j].Coord2D(mat[i+1, j].coord);

                    PointF[] ps=new PointF[4]{p0,p1,p2,p3};//P0 左上角點  P3 右上角點
                                                           //P1 左下角點  P2 右下角點


                    int i_Iseven = (i % 2 == 0) ? 0 : 1;//i是不是奇數(even)  奇數為1  偶數為0
                    int j_Iseven = (j % 2 == 0) ? 0 : 1;//j是不是奇數(even)  奇數為1  偶數為0
                    if ((i_Iseven ^ j_Iseven)==1)// I XOR J 
                    {
                        //淺色
                        g.FillPolygon(sb, ps);
                    }
                    else
                    {
                        //深色
                        g.FillPolygon(sb_b, ps);
                    }
                }
            }
            

        }
        public void draw(Graphics g,Matrix4x4[,] mat,double[,] Lens)
        {
            mat[w / 2, h / 2].draw(g);
            SolidBrush sb = new SolidBrush(Color.FromArgb(128, 128, 128, 128));//淺色
            SolidBrush sb_b = new SolidBrush(Color.FromArgb(128, 64, 64, 64));
            for (int i = 0; i < (w - 1); i++)//w-1 最後要記得減一
            {
                for (int j = 0; j < (h - 1); j++)//h-1 最後要記得減一
                {
                    if (Lens[i, j] < 10 || Lens[i, j+1] < 10 || Lens[i+1, j+1] < 10 || Lens[i+1, j] < 10)
                    {
                        //其中一點太近 就跳下個
                        continue; //忽略之後的程式碼  走下一個迴圈的內容
                    }


                    PointF p0 = mat[i, j].Coord2D(mat[i, j].coord);
                    PointF p1 = mat[i, j + 1].Coord2D(mat[i, j + 1].coord);
                    PointF p2 = mat[i + 1, j + 1].Coord2D(mat[i + 1, j + 1].coord);
                    PointF p3 = mat[i + 1, j].Coord2D(mat[i + 1, j].coord);

                    PointF[] ps = new PointF[4] { p0, p1, p2, p3 };//P0 左上角點  P3 右上角點
                    //P1 左下角點  P2 右下角點


                    int i_Iseven = (i % 2 == 0) ? 0 : 1;//i是不是奇數(even)  奇數為1  偶數為0
                    int j_Iseven = (j % 2 == 0) ? 0 : 1;//j是不是奇數(even)  奇數為1  偶數為0
                    if ((i_Iseven ^ j_Iseven) == 1)// I XOR J 
                    {
                        //淺色
                        g.FillPolygon(sb, ps);
                    }
                    else
                    {
                        //深色
                        g.FillPolygon(sb_b, ps);
                    }
                }
            }


        }
    }
}
