using System;
using System.Collections.Generic;

namespace HanoiTower
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            var delay = 1;
            var diskNumber = 28;
            bool CheckEvryStep = false;
            Tower tower1 = new(diskNumber, 1);
            Tower tower2 = new(0, 2);
            Tower tower3 = new(0, 3);
            DrowTowers(diskNumber, delay, tower1, tower2, tower3, CheckEvryStep);
            //IteerativeMethod(tower1, tower2, tower3, diskNumber, delay, CheckEvryStep);
            RecursivMethod(tower3, tower1, tower2, diskNumber, delay, diskNumber, CheckEvryStep);
            DrowTowers(diskNumber, delay, tower1, tower2, tower3, CheckEvryStep);
            Console.ReadKey();
        }
        private static void DrowTowers(int diskNumber, int delay, Tower tower1, Tower tower2, Tower tower3, bool detailChecking)
        {
            for (int i = 1 + diskNumber; i > 1; i--)
            {
                Console.SetCursorPosition(2, i);
                Console.Write(i - 1);
            }
            List<Tower> towers = new(3);
            towers.Add(tower1);
            towers.Add(tower2);
            towers.Add(tower3);
            for (int i = 0; i < 3; i++)
            {
                int startLeft = 7 + i * 10;
                var towerDisks = towers[i].GetDislStack.ToArray();
                for (int j = 0; j < diskNumber - towers[i].GetDislStack.Count; j++)
                {
                    Console.SetCursorPosition(startLeft, 2 + j);
                    Console.Write("        ");
                }
                for (int j = 0; j < towers[i].GetDislStack.Count; j++)
                {
                    Console.SetCursorPosition(startLeft, 2 +  j + diskNumber - towers[i].GetDislStack.Count);
                    Console.Write("Disk " + towerDisks[j]);
                }
            }
            if (detailChecking)           
                Console.ReadKey();

            else
                System.Threading.Thread.Sleep(delay);
        }
        public static void IteerativeMethod(Tower tower1, Tower tower2, Tower tower3, int diskNum, int delay, bool detailChecking)
        {
            List<Tower> towers = new(3);
            towers.Add(tower1);
            towers.Add(tower2);
            towers.Add(tower3);
            for (int i = 0; i < Math.Pow(2, diskNum) - 1; i++)
            {
                Tower.SetPriorites(tower1, tower2, tower3);
                Stack<int> towerStack = new(3);
                for (int j = 3; j >= 1; j--)
                {
                    foreach (var tower in towers)
                    {
                        if (tower.Priority == j)
                            towerStack.Push(tower.TowerNumber);
                    }
                }
                int positionMove = 1;
                if (diskNum % 2 == 1)
                    positionMove = -1;

                for (int j = 0; j < 3; j++)
                {
                    int towerTo = 0;
                    int towerFrom = towerStack.Pop();
                    if (towers[towerFrom - 1].DiskOnTop % 2 == 1)
                        towerTo = towerFrom + positionMove;

                    else
                        towerTo = towerFrom - positionMove;

                    if (towerTo == 4)
                        towerTo = 1;

                    else if (towerTo == 0)
                        towerTo = 3;

                    if (Tower.IsMoveAvailable(towers[towerFrom - 1], towers[towerTo - 1]))
                    {
                        towers[towerTo - 1].PutDiskOnTop(towers[towerFrom - 1].GetDiskFromTop);
                        break;
                    }                    
                }
                DrowTowers(diskNum, delay, tower1, tower2, tower3, detailChecking);
            }
        }
        public static void RecursivMethod(Tower target, Tower sourse, Tower helper, int height, int delay, int diskNumber, bool detailChecking)
        {           
            if (height == 1)
            {
                target.PutDiskOnTop(sourse.GetDiskFromTop);
                drowResults();
            }                          

            else
            {
                RecursivMethod(helper, sourse, target, height - 1, delay, diskNumber, detailChecking);
                target.PutDiskOnTop(sourse.GetDiskFromTop);
                drowResults();
                RecursivMethod(target, helper, sourse, height - 1, delay, diskNumber, detailChecking);
            }
            
            void drowResults()
            {
                List<Tower> towers = new(3);
                towers.Add(sourse);
                towers.Add(target);
                towers.Add(helper);
                Stack<Tower> towersStack = new(3);
                for (int i = 3; i >= 1; i--)
                {
                    foreach (var tower in towers)
                    {
                        if (tower.TowerNumber == i)
                        {
                            towersStack.Push(tower);
                            break;
                        }
                    }
                }
                DrowTowers(diskNumber, delay, towersStack.Pop(), towersStack.Pop(), towersStack.Pop(), detailChecking);
            }
        }
    }
    public class Tower
    {
        private readonly Stack<int> diskStack = new(28);
        public int Priority { get; private set; }
        public readonly int TowerNumber = 0;
        public Tower(int disckNumber, int towerNum)
        {
            TowerNumber = towerNum;
            for (int i = disckNumber; i >= 1; i--)
                diskStack.Push(i);
        }
        public int DiskOnTop 
        { 
            get 
            {
                if (diskStack.Count == 0)
                    return 29;

                return diskStack.Peek();
            } 
        }
        public int GetDiskFromTop
        {
            get
            {
                if (diskStack.Count == 0)
                    throw new Exception("Попытка взять диск с пустого шеста");

                return diskStack.Pop();
            }
        }
        public void PutDiskOnTop(int disckNum)
        {
            diskStack.Push(disckNum);
        }
        public Stack<int> GetDislStack => diskStack;

        public static bool IsMoveAvailable(Tower towerFrom, Tower towerTo) => towerFrom.DiskOnTop < towerTo.DiskOnTop;

        public static void SetPriorites(Tower tower1, Tower tower2, Tower tower3)
        {
            List<Tower> towers = new(3);
            towers.Add(tower1);
            towers.Add(tower2);
            towers.Add(tower3);
            int biggestDisk = tower1.DiskOnTop;
            int towerNum = 0;
            for (int i = 1; i < towers.Count; i++)
            {
                if (towers[i].DiskOnTop > biggestDisk)
                {
                    biggestDisk = towers[i].DiskOnTop;
                    towerNum = i;
                }
            }
            towers[towerNum].Priority = 1;
            towers.RemoveAt(towerNum);
            if (towers[0].DiskOnTop > towers[1].DiskOnTop)
            {
                towers[0].Priority = 2;
                towers[1].Priority = 3;
            }
            else
            {
                towers[1].Priority = 2;
                towers[0].Priority = 3;
            }
        }
    }
}
