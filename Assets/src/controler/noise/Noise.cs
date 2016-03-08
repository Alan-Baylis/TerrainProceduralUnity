using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.controler.noise
{
    public class Noise
    {
        public enum Type { GRADIENT, VALUE};
        private static Noise instance;

        private bool init = false;

        private int[,] mat;

        private int[,] mat1;
        private int[,] mat2;

        private List<List<float[]>> matCalc;

        private int e;

        private Noise()
        {
            mat = new int[16, 16] { 
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
                {-3,3,0,0,-2,-1,0,0,0,0,0,0,0,0,0,0 },
                {2,-2,0,0,1,1,0,0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0 },
                {0,0,0,0,0,0,0,0,3,3,0,0,-2,-1,0,0 },
                {0,0,0,0,0,0,0,0,2,-2,0,0,1,1,0,0 },
                {-3,0,3,0,0,0,0,0,-2,0,-1,0,0,0,0,0 },
                {0,0,0,0,-3,0,3,0,0,0,0,0,-2,0,-1,0 },
                {9,-9,-9,9,6,3,-6,-3,6,-6,3,-3,4,2,2,1 },
                {-6,6,6,-6,-3,-3,3,3,-4,4,-2,2,-2,-2,-1,-1 },
                {2,0,-2,0,0,0,0,0,1,0,1,0,0,0,0,0 },
                {0,0,0,0,2,0,-2,0,0,0,0,0,1,0,1,0 },
                {-6,6,6,-6,-4,-2,4,2,-3,3,-3,3,-2,-1,-2,-1 },
                {4,-4,-4,4,2,2,-2,-2,2,-2,2,-2,1,1,1,1 }
            };

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
        public static Noise GetInstance()
        {
            if (instance == null)
                instance = new Noise();

            return instance;
        }

        public void Init(int e, Type type)
        {
            init = false;
            this.e = e; 
            Random rand = new Random(14298);
            List<List<float[]>> list = new List<List<float[]>>();
            //list.Capacity = e + 1;
            for (var i = 0; i <= e; ++i)
            {
                List<float[]> l = new List<float[]>();
                //l.Capacity = e + 1;
                for (var j = 0; j <= e; ++j)
                {
                    
                    float[] tab;
                    switch (type)
                    {
                        
                        case Type.GRADIENT:
                            UnityEngine.Vector2 v = new UnityEngine.Vector2((float)(rand.NextDouble() * 2) - 1f, (float)(rand.NextDouble() * 2) - 1f);
                            //v.Normalize();
                            tab = new float[4] { 0, v.x, v.y, 0 };
                            l.Add(tab);
                            break;

                        case Type.VALUE:
                            float val = (float)(rand.NextDouble() * 2) - 1f;

                            tab = new float[4] { val, 0, 0, 0 };
                            l.Add(tab);
                            break;
                    }

            }
                list.Add(l);
            }


            matCalc = new List<List<float[]>>();
            //matCalc.Capacity = e;

            for (var i = 0; i < e; ++i)
            {
                List<float[]> l = new List<float[]>();
                //l.Capacity = e;
                for (var j = 0; j < e; ++j)
                {
                    float[] tab = new float[16] {
                        list[i][j][0],
                        list[i+1][j][0],
                        list[i][j+1][0],
                        list[i+1][j+1][0],
                        list[i][j][1],
                        list[i+1][j][1],
                        list[i][j+1][1],
                        list[i+1][j+1][1],
                        list[i][j][2],
                        list[i+1][j][2],
                        list[i][j+1][2],
                        list[i+1][j+1][2],
                        list[i][j][3],
                        list[i+1][j][3],
                        list[i][j+1][3],
                        list[i+1][j+1][3]
                    };
                    l.Add(ProcessMat(tab));
                }
                matCalc.Add(l);
            }

            init = true;
        }

        private float[] ProcessMat(float[] tab)
        {
            List<float> resu = new List<float>();
            resu.Capacity = 16;



            for (int i = 0; i < 16; ++i)
            {
                float sum = 0;
                for (int j = 0; j < 16; ++j)
                {
                    sum += mat[i, j] * tab[j];
                }
                resu.Add(sum);
            }

            return resu.ToArray();
        }

        public bool GetNoise(float x , float y , out float noise)
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


           
            noise =  GetNoise(xMin, yMin, rX, rY);
            return true;
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

            float[] tab = matCalc[x][y];
         
            for (int i = 0; i < 4;++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    sum += tab[i + j * 4] * Pow(rX, i) * Pow(rY, j);
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
