using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.controler.noise
{
    public class Noise
    {
        private bool init = false;

        private int[,] mat;

        private int[,] mat1;
        private int[,] mat2;

        private List<List<float[,]>> matCalc;

        private int e;

        public Noise()
        {
            mat1 = new int[4, 4]
            {
                {1, 0, 0, 0 },
                {0, 0, 1, 0 },
                {-3, 3, -2, -1 },
                {2, -2, 1, 1 }
            };

            mat2 = new int[4, 4]
            {
                {1, 0, -3, 2 },
                {0, 0, 3, -2 },
                {0, 1, -2, 1 },
                {0, 0, -1, 1 }
            };
        }

        public bool GetNoise(float x, float y, out float noise)
        {
            if (!init)
            {
                noise = 0;
                return false;
            }

            int xMin = (int)(x * e);
            int yMin = (int)(y * e);
            float rX = x * e - xMin;
            float rY = y * e - yMin;



            noise = GetNoise(xMin, yMin, rX, rY);
            return true;
        }

        public void InitGradientNoise(int e , float mean , float stdDev)
        {
            

            init = false;
            this.e = e;
            Random rand = new Random(14298);
            List<List<float[]>> list = new List<List<float[]>>();
            list.Capacity = e + 1;
            for (var i = 0; i <= e; ++i)
            {
                List<float[]> l = new List<float[]>();
                l.Capacity = e + 1;
                for (var j = 0; j <= e; ++j)
                {

                    float[] tab;
                    tab = new float[4] { 0, RandNormal(rand, mean, stdDev), RandNormal(rand, mean, stdDev), 0 };
                    l.Add(tab);

                }
                list.Add(l);
            }


            matCalc = new List<List<float[,]>>();
            matCalc.Capacity = e;

            for (var i = 0; i < e; ++i)
            {
                List<float[,]> l = new List<float[,]>();
                l.Capacity = e;
                for (var j = 0; j < e; ++j)
                {
                    float[,] tab = new float[4, 4] {
                       {list[i][j][0],list[i][j+1][0],list[i][j][2],list[i][j+1][2] } ,
                       {list[i+1][j][0],list[i+1][j+1][0],list[i+1][j][2], list[i+1][j+1][2] },
                       {list[i][j][1],list[i][j+1][1],list[i][j][3],list[i+1][j+1][3] },
                       {list[i+1][j][1],list[i+1][j+1][1],list[i+1][j][3],list[i][j+1][3]}
                    };
                    l.Add(ProcessMat(tab));
                }
                matCalc.Add(l);
            }

            init = true;

        }

        public void InitValueNoise(int e)
        {
            init = false;
            this.e = e; 
            Random rand = new Random(14298);
            List<List<float[]>> list = new List<List<float[]>>();
            list.Capacity = e + 1;
            for (var i = 0; i <= e; ++i)
            {
                List<float[]> l = new List<float[]>();
                l.Capacity = e + 1;
                for (var j = 0; j <= e; ++j)
                {
                    
                    float[] tab;
                        
                    float val = (float)(rand.NextDouble() * 2) - 1f;

                    tab = new float[4] { val, 0, 0, 0 };
                    l.Add(tab);

                }
                list.Add(l);
            }


            matCalc = new List<List<float[,]>>();
            matCalc.Capacity = e;

            for (var i = 0; i < e; ++i)
            {
                List<float[,]> l = new List<float[,]>();
                l.Capacity = e;
                for (var j = 0; j < e; ++j)
                {
                    float[,] tab = new float[4,4] {
                       {list[i][j][0],list[i][j+1][0],list[i][j][2],list[i][j+1][2] } ,
                       {list[i+1][j][0],list[i+1][j+1][0],list[i+1][j][2], list[i+1][j+1][2] },
                       {list[i][j][1],list[i][j+1][1],list[i][j][3],list[i+1][j+1][3] },
                       {list[i+1][j][1],list[i+1][j+1][1],list[i+1][j][3],list[i][j+1][3]}                       
                    };
                    l.Add(ProcessMat(tab));
                }
                matCalc.Add(l);
            }

            init = true;
        }

        private float RandNormal(Random rand , float mean, float stdDev)
        {
            float u1 =(float) rand.NextDouble(); //these are uniform(0,1) random doubles
            float u2 = (float)rand.NextDouble();
            float randStdNormal = (float)Math.Sqrt(-2.0 * Math.Log(u1)) * (float)Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            float randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }

        private float[,] ProcessMat(float[,] tab)
        {
            float[,] resu = new float[4, 4];
            float[,] temp = new float[4, 4];

            for (int i =0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    float sum = 0;

                    for (int k = 0; k < 4; ++k)
                    {
                        sum += mat1[i, k] * tab[k, j];

                    }
                    temp[i, j] = sum;
                }
            }

            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    float sum = 0;

                    for (int k = 0; k < 4; ++k)
                    {
                        sum += temp[i, k] * mat2[k, j];

                    }
                    resu[i, j] = sum;
                }
            }

            return resu;
        }

        private float GetNoise(int x , int y, float rX, float rY)
        {
            if (x < 0)
                x = 0;
            if (x >= e)
                x = e - 1;

            if (y < 0)
                y = 0;
            if (y >= e)
                y = e - 1;

            float sum = 0;

            float[,] tab = matCalc[x][y];
         
            for (int i = 0; i < 4;++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    sum += tab[i,j] * Pow(rX, i) * Pow(rY, j);
                }
            }
            
            return sum;

        }

        private float Pow(float x, int p)
        {
            float r = 1;
            for(int i = 0;i< p; ++i)
            {
                r *= x;
            }
            return r;
        }
    }
}
