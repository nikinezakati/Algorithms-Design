using System;
using System.Collections.Generic;
using TestCommon;

namespace E2
{
    public class Q0Chart : Processor
    {
        public Q0Chart(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            E2Processors.Q0Processor(inStr, Solve);

        public string[] Solve(int professorsCount,
                              int classCount,
                              int timeCount,
                              long[,,] professorsCanTeach,
                              long[,,] classCanBeOccupied)
        {
            clauses = new List<string>();
            data = new long[professorsCount, classCount, timeCount];
            var cnt = 0;
            for (int i = 0; i < professorsCount; i++)
            {
                for (int j = 0; j < classCount; j++)
                {
                    for (int k = 0; k < timeCount; k++)
                    {
                        data[i, j, k] = ++cnt;
                    }
                }
            }
            TeacherTimes(professorsCount, classCount, timeCount, professorsCanTeach);
            ClassTimes(professorsCount, classCount, timeCount, classCanBeOccupied);
            for (int i = 0; i < professorsCount; i++)
            {
                for (int k = 0; k < timeCount; k++)
                {
                    AtMostOneClassInOneTime(i, k, classCount);
                }
            }
            for (int j = 0; j < classCount; j++)
            {
                for (int k = 0; k < timeCount; k++)
                {
                    AtMostOneTeacherInOneTime(j, k, professorsCount);
                }
            }
            for (int i = 0; i < professorsCount; i++)
            {
                TeacherAtLeastOneTime(i, classCount, timeCount);
            }
            for (int j = 0; j < classCount; j++)
            {
                ClassAtLeastOneTime(j, professorsCount, timeCount);
            }
            clauses.Insert(0, $"{clauses.Count} {professorsCount * classCount * timeCount}");
            return clauses.ToArray();
        }

        private void ClassAtLeastOneTime(int j, int professorsCount, int timeCount)
        {
            var stream = "";
            for (int i = 0; i < professorsCount; i++)
            {
                for (int k = 0; k < timeCount; k++)
                {
                    stream += $"{data[i, j, k]} ";
                }
            }
            stream += "0";
            clauses.Add(stream);
        }

        private void TeacherAtLeastOneTime(int i, int classCount, int timeCount)
        {
            var stream = "";
            for (int j = 0; j < classCount; j++)
            {
                for (int k = 0; k < timeCount; k++)
                {
                    stream += $"{data[i, j, k]} ";
                }
            }
            stream += "0";
            clauses.Add(stream);
        }

        private void AtMostOneTeacherInOneTime(int j, int k, int professorsCount)
        {
            var stream = "";
            for (int i = 0; i < professorsCount; i++)
            {
                for (int m = i + 1; m < professorsCount; m++)
                {
                    if (i != m)
                    {
                        stream += $"{-data[i, j, k]} {-data[m, j, k]} 0";
                        clauses.Add(stream);
                        stream = "";
                    }
                }
            }
        }

        private void AtMostOneClassInOneTime(int i, int k, int classCount)
        {
            // ~(xi1k ^ xi2k) => ( ~xi1k V ~xi2k)
            var stream = "";
            for (int j = 0; j < classCount; j++)
            {
                for (int m = j + 1; m < classCount; m++)
                {
                    if (j != m)
                    {
                        stream += $"{-data[i, j, k]} {-data[i, m, k]} 0";
                        clauses.Add(stream);
                        stream = "";
                    }
                }
            }
        }

        private void ClassTimes(int professorsCount, int classCount, int timeCount, long[,,] classCanBeOccupied)
        {
            var stream = "";
            for (int i = 0; i < professorsCount; i++)
            {
                for (int j = 0; j < classCount; j++)
                {
                    for (int k = 0; k < timeCount; k++)
                    {
                        if (classCanBeOccupied[i, j, k] == -1)
                            stream += $"{-data[i, j, k]} 0";

                        clauses.Add(stream);
                        stream = "";
                    }
                }
            }
        }

        private void TeacherTimes(int professorsCount, int classCount, int timeCount, long[,,] professorsCanTeach)
        {
            var stream = "";
            for (int i = 0; i < professorsCount; i++)
            {
                for (int j = 0; j < classCount; j++)
                {
                    for (int k = 0; k < timeCount; k++)
                    {
                        if (professorsCanTeach[i, j, k] == -1)
                            stream += $"{-data[i, j, k]} 0";
                        clauses.Add(stream);
                        stream = "";
                    }
                }
            }

        }
        public long[,,] data;
        public List<string> clauses = new List<string>();
        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

    }

}