using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace trajectorypath_controler
{
    public class Trajectory
    {
        double a10;
        double a11;
        double a12;
        double a13;

        double a20;
        double a21;
        double a22;
        double a23;


        double a0;
        double a1;
        double a2;
        double a3;


        double tf;
        double tf1;
        double tf2;
        double θg;//末端
        double θv;//中繼
        double θ0;
        private double θf;

        public double Getddeg(double tmp_deg, double degs)
        {
            double ddeg = tmp_deg - degs;//角度差

            if (ddeg >= 180d)
            {
                ddeg = ddeg - 360d;
            }
            else if (ddeg <= -180d)
            {
                ddeg = ddeg + 360d;
            }
            return ddeg;
        }
        public Trajectory()
        { 
        }
        public Trajectory(double str_deg, double via_deg, double fin_deg, double tf)
        {

            double ddeg_vdegs = new double();//末端到中繼角度差
            double ddeg_fdegs = new double();//末端到中繼角度差
            ddeg_vdegs = Getddeg(via_deg, str_deg);//中繼點與起點的角度差

            this.θ0 = str_deg;
            this.θv = θ0 + ddeg_vdegs;
            ddeg_fdegs = Getddeg(fin_deg, θv);//末端點與中繼點的角度差
            this.θg = θv + ddeg_fdegs;
            this.tf1 = tf / 2.0;
            this.tf2 = tf / 2.0;

            a10 = θ0;
            a11 = 0;
            a12 = (12.0 * this.θv - 3.0 * this.θg - 9.0 * θ0) / (4.0 * Math.Pow(tf1, 2));
            a13 = (-8.0 * this.θv + 3.0 * this.θg + 5.0 * θ0) / (4.0 * Math.Pow(tf1, 3));

            a20 = θv;
            a21 = (3.0 * this.θg - 3.0 * this.θ0) / (4.0 * tf2);
            a22 = (-12.0 * this.θv + 6.0 * this.θg + 6.0 * θ0) / (4.0 * Math.Pow(tf2, 2));
            a23 = (8.0 * this.θv - 5.0 * this.θg - 3.0 * θ0) / (4.0 * Math.Pow(tf2, 3));

        }

        public Trajectory(double θ0, double θf, double tf)
        {
            double ddeg = Getddeg(θf, θ0);

            this.θ0 = θ0;
            this.θf = θ0 + ddeg;
            this.tf = tf;
            a0 = θ0;
            a1 = 0;
            a2 = 3.0 * (this.θf - θ0) / Math.Pow(tf, 2);
            a3 = -2.0 * (this.θf - θ0) / Math.Pow(tf, 3);
        }

        double Createθt(double t)
        {
            // θt = ??
            double θt = a0 + a1 * t + a2 * t * t + a3 * Math.Pow(t, 3);
            return θt;
        }

        double Createθt_oritovia(double t)
        {
            //double a0, double a1, double a2, double a3,
            double x = 0;
            x = a10 + a11 * t + a12 * (Math.Pow(t, 2)) + a13 * Math.Pow(t, 3);
            return x;
        }

        double Createθt_viatofin(double t)
        {
            //double a0, double a1, double a2, double a3,
            double x = 0;
            x = a20 + a21 * t + a22 * (Math.Pow(t, 2)) + a23 * Math.Pow(t, 3);
            return x;
        }

        public double[] CreateSt(int step)
        {
            double[] pts = new double[step + 1];
            for (int i = 0; i <= step; i++)
            {
                pts[i] = Createθt((double)(i) * tf / (double)(step));
            }
            return pts;
        }

        public double[] CreateViaSt(int step)
        {
            if (step%2==1)
            {
                step = step + 1;
            }
            double[] pts = new double[step];
            for (int i = 0; i < step/2; i++)
            {
                pts[i] = Createθt_oritovia((double)(i) * tf1 / (double)(step/2));
            }
            for (int i = 0; i < step / 2; i++)
            {
                pts[i+step/2] = Createθt_viatofin((double)(i) * tf2 / (double)(step/2));
            }
            return pts;
        }
    }
}
