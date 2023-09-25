using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Tool
    {
        //static double W = 640;
        //static double H = 480;
        //static double r = Math.Sqrt(Math.Pow((W / 2d), 2) + Math.Pow((H / 2d), 2));//對邊

        //static double FOV = 90;//Deg

        //static double γ = FOV / 2d;//夾角(deg)

        //static double f = r / Math.Tan(γ * Math.PI / 180d);//求鄰邊(帶入徑度)
        /// <summary>
        /// 求矩陣的逆矩陣
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[][] InverseMatrix(double[][] matrix,out bool check)
        {
            check = true;
            //matrix必須為非空
            if (matrix == null || matrix.Length == 0)
            {
                check = false;
                return new double[][] { };
            }
            //matrix 必須為方陣
            int len = matrix.Length;
            for (int counter = 0; counter < matrix.Length; counter++)
            {
                if (matrix[counter].Length != len)
                {
                    check = false;
                    Console.WriteLine("matrix 必須為方陣");
                    //throw new Exception("matrix 必須為方陣");
                }
            }
            //計算矩陣行列式的值
            double dDeterminant = Determinant(matrix);
            if (Math.Abs(dDeterminant) <= 1E-6)
            {
                Console.WriteLine("矩陣不可逆");
                check = false;
                //throw new Exception("矩陣不可逆");
            }
            //製作一個伴隨矩陣大小的矩陣
            double[][] result = AdjointMatrix(matrix);
            //矩陣的每項除以矩陣行列式的值，即為所求
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    result[i][j] = result[i][j] / dDeterminant;
                }
            }
            return result;
        }
        /// <summary>
        /// 遞迴計算行列式的值
        /// </summary>
        /// <param name="matrix">矩陣</param>
        /// <returns></returns>
        public static double Determinant(double[][] matrix)
        {
            //二階及以下行列式直接計算
            if (matrix.Length == 0) return 0;
            else if (matrix.Length == 1) return matrix[0][0];
            else if (matrix.Length == 2)
            {
                return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
            }
            //對第一行使用“加邊法”遞迴計算行列式的值
            double dSum = 0, dSign = 1;
            for (int i = 0; i < matrix.Length; i++)
            {
                double[][] matrixTemp = new double[matrix.Length - 1][];
                for (int count = 0; count < matrix.Length - 1; count++)
                {
                    matrixTemp[count] = new double[matrix.Length - 1];
                }
                for (int j = 0; j < matrixTemp.Length; j++)
                {
                    for (int k = 0; k < matrixTemp.Length; k++)
                    {
                        matrixTemp[j][k] = matrix[j + 1][k >= i ? k + 1 : k];
                    }
                }
                dSum += (matrix[0][i] * dSign * Determinant(matrixTemp));
                dSign = dSign * -1;
            }
            return dSum;
        }
        /// <summary>
        /// 計算方陣的伴隨矩陣
        /// </summary>
        /// <param name="matrix">方陣</param>
        /// <returns></returns>
        public static double[][] AdjointMatrix(double[][] matrix)
        {
            //製作一個伴隨矩陣大小的矩陣
            double[][] result = new double[matrix.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new double[matrix[i].Length];
            }
            //生成伴隨矩陣
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result.Length; j++)
                {
                    //儲存代數餘子式的矩陣（行、列數都比原矩陣少1）
                    double[][] temp = new double[result.Length - 1][];
                    for (int k = 0; k < result.Length - 1; k++)
                    {
                        temp[k] = new double[result[k].Length - 1];
                    }
                    //生成代數餘子式
                    for (int x = 0; x < temp.Length; x++)
                    {
                        for (int y = 0; y < temp.Length; y++)
                        {
                            temp[x][y] = matrix[x < i ? x : x + 1][y < j ? y : y + 1];
                        }
                    }
                    //Console.WriteLine("代數餘子式:");
                    //PrintMatrix(temp);
                    result[j][i] = ((i + j) % 2 == 0 ? 1 : -1) * Determinant(temp);
                }
            }
            //Console.WriteLine("伴隨矩陣：");
            //PrintMatrix(result);
            return result;
        }
        /// <summary>
        /// 列印矩陣
        /// </summary>
        /// <param name="matrix">待列印矩陣</param>
        private static void PrintMatrix(double[][] matrix, string title = "")
        {
            //1.標題值為空則不顯示標題
            if (!String.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine(title);
            }
            //2.列印矩陣
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j] + "\t");
                    //注意不能寫為：Console.Write(matrix[i][j] + '\t');
                }
                Console.WriteLine();
            }
            //3.空行
            Console.WriteLine();
        }
    }
}
