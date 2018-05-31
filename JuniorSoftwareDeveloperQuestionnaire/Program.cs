using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace JuniorSoftwareDeveloperQuestionnaire
{
    public struct PATH_PYRAMID
    {
        public int ROW_INDEX;
        public int COL_INDEX;

        public PATH_PYRAMID(int ar1, int ar2)
        {
            ROW_INDEX = ar1;
            COL_INDEX = ar2;
        }
    }

    public struct PATH_AND_SUM
    {
        public List<PATH_PYRAMID> PATH;
        public int SUM;
        public PATH_AND_SUM(List<PATH_PYRAMID> ar1, int ar2)
        {
            PATH = ar1;
            SUM = ar2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                @"input.txt");
            string str = "";
            List<int[]> pyramid = new List<int[]>();
            if (File.Exists(filepath))
            {   //read file
                StreamReader sr = new StreamReader(filepath);
                while ((str = sr.ReadLine()) != null)
                {
                    string[] cols = str.Split(' ');
                    //Console.WriteLine("\n cols: {0}",cols.Length);
                    List<int> line = new List<int>();
                    for (int i = 0; i < cols.Length; i++)
                    {
                        try
                        {
                            //Console.Write(cols[i] + " ");
                            line.Add(Convert.ToInt32(cols[i]));
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("\n Format Exception: {0}",cols[i]);
                            Console.ReadKey();
                            Environment.Exit(-1);
                        }
                    }
                    //Console.WriteLine();
                    int[] intArray = new int[line.Count];
                    for (int i = 0; i < line.Count; i++)
                    {
                        intArray[i] = line.ElementAt(i);
                    }
                    pyramid.Add(intArray);
                }
                sr.Close();
                Console.WriteLine("\n!File read success!\nContent:");
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("File not found");
            }
            displayPyramid(pyramid);
            //Console.ReadKey();
            // Finding path over non-prime nums goes below
            List<PATH_AND_SUM> pathAndSum = new List<PATH_AND_SUM>();
            List<PATH_PYRAMID> path = new List<PATH_PYRAMID>();
            iterateOverNonPrimes(pyramid, pathAndSum, path, 0, 0, 0);
            displayPathAndSum(pyramid, pathAndSum);
            displayBestPathAndSum(pyramid, pathAndSum);
            Console.ReadKey();

        }

        public static void iterateOverNonPrimes(List<int[]> pyramid, List<PATH_AND_SUM> pathAndSum,
                                    List<PATH_PYRAMID> path, int rowInx, int colInx, int sum)
        {
            bool rightEnd = false;
            bool leftEnd = false;
            sum += pyramid.ElementAt(rowInx)[colInx];
            path.Add(new PATH_PYRAMID(rowInx,colInx));
            if (rowInx + 1 < pyramid.Count)
            {
                if (!checkForPrime(pyramid.ElementAt(rowInx + 1)[colInx]))
                {
                    //Console.WriteLine("Non prime: {0}", pyramid.ElementAt(rowInx + 1)[colInx]);
                    iterateOverNonPrimes(pyramid, pathAndSum, path, rowInx + 1, colInx, sum);
                }
                else
                {
                    leftEnd = true;
                    //Console.WriteLine("prime: {0}", pyramid.ElementAt(rowInx + 1)[colInx]);
                }

                if (!checkForPrime(pyramid.ElementAt(rowInx + 1)[colInx + 1]))
                {
                    //Console.WriteLine("Non prime: {0}", pyramid.ElementAt(rowInx + 1)[colInx+1]);
                    iterateOverNonPrimes(pyramid, pathAndSum, path, rowInx + 1, colInx + 1, sum);
                }
                else
                {
                    rightEnd = true;
                    //Console.WriteLine("prime: {0}", pyramid.ElementAt(rowInx + 1)[colInx + 1]);
                }

                if (leftEnd && rightEnd)
                {
                    pathAndSum.Add(new PATH_AND_SUM(new List<PATH_PYRAMID>(path), sum));
                }
            }
            else
            {
                pathAndSum.Add(new PATH_AND_SUM(new List<PATH_PYRAMID>(path), sum));

            }
            path.RemoveAt(path.Count-1);
        }

        public static void displayPyramid(List<int[]> list)
        {
            Console.WriteLine();
            for (int i = 0; i < list.Count; i++)
            {
                for(int j=0; j<list.ElementAt(i).Length; j++)
                {
                    Console.Write(list.ElementAt(i)[j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static Boolean checkForPrime(int x)
        {
            if (x == 2 || x == 3)
            {
                return true;
            }
            else
            {
                for (int i = 2; i <= x / 2; i++)
                {
                    if (x % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static void displayPathAndSum(List<int[]> pyramid, List<PATH_AND_SUM> list)
        {
            int row;
            int col;
            Console.Write("Path and sum");
            Console.WriteLine("\t [Total: " + list.Count +"]");
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.ElementAt(i).PATH.Count; j++)
                {
                    row = list.ElementAt(i).PATH.ElementAt(j).ROW_INDEX;
                    col = list.ElementAt(i).PATH.ElementAt(j).COL_INDEX;
                    Console.Write(pyramid.ElementAt(row)[col] + " > ");
                }
                Console.WriteLine("sum: " + list.ElementAt(i).SUM);
            }
            Console.WriteLine();
        }

        public static void displayBestPathAndSum(List<int[]> pyramid, List<PATH_AND_SUM> list)
        {
            int maxSum;
            int maxDepth;
            int maxInx = 0;
            int row;
            int col;
            Console.WriteLine("Best Path:");
            maxSum = list.ElementAt(0).SUM;
            maxDepth = list.ElementAt(0).PATH.Count;
            for (int i = 1; i < list.Count; i++)
            {
                if (list.ElementAt(i).SUM == maxSum && list.ElementAt(i).PATH.Count > list.ElementAt(maxInx).PATH.Count)
                {   
                    maxInx = i;
                    maxDepth = list.ElementAt(i).PATH.Count;
                }
                else if (list.ElementAt(i).PATH.Count == maxDepth && list.ElementAt(i).SUM > maxSum)
                {
                    maxInx = i;
                    maxSum = list.ElementAt(i).SUM;
                }
                else if (list.ElementAt(i).PATH.Count > maxDepth)
                {
                    maxInx = i;
                    maxSum = list.ElementAt(i).SUM;
                    maxDepth = list.ElementAt(i).PATH.Count;
                }
            }
            for (int i = 0; i < list.ElementAt(maxInx).PATH.Count; i++)
            {
                row = list.ElementAt(maxInx).PATH.ElementAt(i).ROW_INDEX;
                col = list.ElementAt(maxInx).PATH.ElementAt(i).COL_INDEX;
                Console.Write(pyramid.ElementAt(row)[col] + " > ");
            }
            Console.WriteLine("sum: " + list.ElementAt(maxInx).SUM + "\n");

            for (int i = 0; i < pyramid.Count; i++)
            {
                for (int j = 0; j < pyramid.ElementAt(i).Length; j++)
                {
                    for (int k = 0; k < list.ElementAt(maxInx).PATH.Count; k++)
                    {
                        PATH_PYRAMID temp = list.ElementAt(maxInx).PATH.ElementAt(k);
                        if (temp.ROW_INDEX == i && temp.COL_INDEX == j)
                        {
                            Console.Write("*");
                        }
                    }
                        Console.Write(pyramid.ElementAt(i)[j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    } //class end
}
