using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace HanoiTower
{
    class EllapsedTime
    {
        Tower target, sourse, helper;
        int height, delay;
        bool detailChecking;
        public EllapsedTime(Tower target, Tower sourse, Tower helper, int height, int delay, int diskNumber, bool detailChecking)
        {
            this.delay = delay;
            this.detailChecking = detailChecking;
            this.height = height;
            this.target = target;
            this.sourse = sourse;
            this.helper = helper;
        }
        void Count()
        {
            int smoothnes = 15;
            int RunTimes = 25;
            int step = 1;
            int slidingAvarage = 3;
            int procent = 1;
            List<long> timersResults = new(RunTimes);
            Console.CursorVisible = false;
            Console.WriteLine("Процесс выполнения завершён на   %.");
            for (int i = step; i <= (RunTimes + slidingAvarage) * step; i += step)
            {
                long[] results = new long[smoothnes];
                for (int j = 0; j < smoothnes; j++)
                    results[j] = GetAllapsedTime(i);
                //results[j] = GetAllapsedTime(GetMatrix(i, 123321), GetMatrix(i, 321123));


                RemoveWrongValues(results, i, smoothnes);
                //RemoveWrongValues(results, GetMatrix(i, 123321), GetMatrix(i, 321123), smoothnes);
                long smoothResult = results[0];
                for (int j = 1; j < smoothnes; j++)
                    smoothResult = (smoothResult + results[j]) / 2;

                if ((i / (float)((RunTimes + slidingAvarage) * step)) * 100 > procent)
                {
                    Console.SetCursorPosition(31, 0);
                    Console.Write(procent);
                    procent++;
                }
                timersResults.Add(smoothResult);
            }
            WriteToCSV(SlidingAvarageFilter(slidingAvarage, timersResults).ToArray());
        }
        private long GetAllapsedTime(int array)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            //Program.IteerativeMethod(sourse, helper, target, array, delay, detailChecking);
            Program.RecursivMethod(target, sourse, helper, height, delay, array, detailChecking);
            stopwatch.Stop();
            return stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
            // / (TimeSpan.TicksPerMillisecond / 1000)
        }
        private List<decimal> SlidingAvarageFilter(int slidingAvarage, List<long> values)
        {
            List<decimal> result = new(values.Count);
            for (int i = slidingAvarage; i < values.Count; i++)
            {
                long avarage = 0L;
                for (int j = slidingAvarage; j > 0; j--)
                    avarage += values[i - j];

                result.Add((decimal)avarage / (decimal)slidingAvarage);
            }
            return result;
        }
        private void RemoveWrongValues(long[] values, int array, int smoothnes)
        {
            for (int j = 1; j < smoothnes; j++)
            {
                if (values[j - 1] != 0L)
                {
                    while (values[j] / values[j - 1] > 2f)
                    {
                        values[j] = GetAllapsedTime(array);
                        if (values[j - 1] == 0L)
                            break;
                    }
                }
                if (values[j] != 0L)
                {
                    while (values[j - 1] / values[j] > 2f)
                    {
                        values[j - 1] = GetAllapsedTime(array);
                        if (values[j] == 0L)
                            break;
                    }
                }
            }
        }
        private void WriteToCSV(decimal[] timersResults)
        {
            string[] values = new string[timersResults.Length];
            for (int i = 0; i < timersResults.Length; i++)
                values[i] = Convert.ToString(timersResults[i]);

            File.WriteAllLines("Result.csv", values);
        }
    }
}
