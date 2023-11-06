﻿///////////////////////////////////////////////////////////////////////////////
//
// Author: Ethan Horton, hortonew@etsu.edu
//         Elijah Combs, combsey@etsu.edu
//         Nicholas Crump, crumpna@etsu.edu
//         Zackary Jordan, jordanzd@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 3
// Description: 
//              
//
//
///////////////////////////////////////////////////////////////////////////////
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FazbearEntertainements_Project3
{
    internal class Warehouse
    {
        List<Dock> dockList { get; set; }
        public Queue<Truck> Entrance { get; set; }

        public Warehouse() 
        {
            dockList = new List<Dock>();
            Entrance = new Queue<Truck>();
        }

        public void Run()
        {
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            //-----Options-----
            int increments = 48;
            int numOfTrucksCanArrive = 3;
            //-----------------

            //--Report Values--
            int numDocks = 0;
            double revenue = 0.00;
            int longestLine = 0;
            int numTrucksProcessed = 0;
            int numCratesUnloaded = 0;
            double totalCratesUnloadedValue = 0.00;
            double averageCrateValue = 0.00;
            double averageTruckValue = 0.00;
            List<DockInfo> dockInfoList = new List<DockInfo>();
            //-----------------

            Random rand = new Random();

            while (true)
            {
                try
                {
                    Console.WriteLine("How many docks are there?");
                    numDocks = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }

            WriteHeaders(dateTime, numDocks);

            List<int> deckIDList = new List<int>();
            bool deckIDCheck = true;
            int randomDeckID = 0;

            for (int i = 0; i < numDocks; i++)
            {
                deckIDCheck = true;
                while (deckIDCheck == true)
                {
                    deckIDCheck = false;
                    randomDeckID = rand.Next(0, 1000000);
                    foreach (int item in deckIDList)
                    {
                        if (item == randomDeckID)
                        {
                            deckIDCheck = true;
                        }
                    }
                }
                deckIDList.Add(randomDeckID);
                Dock dock = new Dock(randomDeckID.ToString());
                DockInfo dockInfo = new DockInfo();
                dockList.Add(dock);
                dockInfoList.Add(dockInfo);
            }

            int arrivalFlip1 = 0;
            int arrivalFlip2 = 0;
            int amountofTrucksArrived = 0;

            if (numOfTrucksCanArrive < 1)
            {
                numOfTrucksCanArrive = 1;
            }

            int subNumOfTrucksCanArrive = Convert.ToInt32(Math.Floor(Convert.ToDecimal(numOfTrucksCanArrive / 2)));
            if (subNumOfTrucksCanArrive < 1)
            {
                subNumOfTrucksCanArrive = 1;
            }

            int addNumOfTrucksCanArrive = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(numOfTrucksCanArrive / 2)));

            for (int i = 1; i <= increments; i++)
            {


                if (i > 24 && i < 34)
                {
                    //x2 more chance to have trucks
                    arrivalFlip1 = rand.Next(0, 2);
                    arrivalFlip2 = rand.Next(0, 2);
                    if (arrivalFlip1 == 0 || arrivalFlip2 == 0)
                    {
                        amountofTrucksArrived = rand.Next(1, numOfTrucksCanArrive + addNumOfTrucksCanArrive);
                    }
                    else
                    {
                        amountofTrucksArrived = 0;
                    }
                }
                else if (i < 12 || i > 44)
                {
                    //x2 less chance to have trucks
                    arrivalFlip1 = rand.Next(0, 2);
                    arrivalFlip2 = rand.Next(0, 2);
                    if (arrivalFlip1 == 0 && arrivalFlip2 == 0)
                    {
                        amountofTrucksArrived = rand.Next(1, subNumOfTrucksCanArrive + 1);
                    }
                    else
                    {
                        amountofTrucksArrived = 0;
                    }
                }
                else
                {
                    //normal chance to have trucks
                    arrivalFlip1 = rand.Next(0, 2);
                    if (arrivalFlip1 == 0)
                    {
                        amountofTrucksArrived = rand.Next(1, numOfTrucksCanArrive + 1);
                    }
                    else
                    {
                        amountofTrucksArrived = 0;
                    }
                }
                for (int t = 0; t < amountofTrucksArrived; t++)
                {
                    List<int> crateIDList = new List<int>();
                    bool crateIDCheck = true;
                    int randomcrateID = 0;
                    Truck truck = new Truck(randomName(), randomCompany());
                    for(int j = 0; j < rand.Next(1,16); j++)
                    {
                        crateIDCheck = true;
                        while (crateIDCheck == true)
                        {
                            crateIDCheck = false;
                            randomcrateID = rand.Next(0, 1000000);
                            foreach (int item in deckIDList)
                            {
                                if (item == randomcrateID)
                                {
                                    crateIDCheck = true;
                                }
                            }
                        }
                        crateIDList.Add(randomcrateID);

                        Crate crate = new Crate(randomcrateID);
                        truck.Load(crate);
                    }
                    Entrance.Enqueue(truck);
                }

                int shortestDockLengthIndex = 0;
                for (int d = 1; d < numDocks; d++)
                {
                    if (dockList[d - 1].Line.Count > dockList[d].Line.Count)
                    {
                        shortestDockLengthIndex = d;
                    }
                }
                if (Entrance.Count > 0)
                {
                    dockList[shortestDockLengthIndex].JoinLine(Entrance.Dequeue());
                }
                

                for (int d = 0; d < numDocks; d++)
                {
                    if (dockList[d].Line.Count > 0)
                    {
                        string dockState = "";
                        if (dockList[d].Line.Peek().Trailer.Count-1 > 0)
                        {
                            //A crate was unloaded, but the truck still has more crates to unload
                            dockState = "Truck Still Unloading";
                        }
                        else if (dockList[d].Line.Peek().Trailer.Count-1 == 0 && dockList[d].Line.Count-1 > 0)
                        {
                            //A crate was unloaded, and the truck has no more crates to unload, and another truck is already in the Dock
                            dockState = "Dock Still Occupied";
                        }
                        else
                        {
                            //A crate was unloaded, and the truck has no more crates to unload, but another truck is NOT already in the Dock
                            dockState = "Dock is Empty";
                        }
                        WriteShipmentLog(dateTime, i, d+1, dockList[d].Line.Peek().Driver, dockList[d].Line.Peek().DeliveryCompany, dockList[d].Line.Peek().Trailer.Peek().Id, dockList[d].Line.Peek().Trailer.Peek().price, dockState);
                        revenue = revenue + dockList[d].Line.Peek().Trailer.Peek().price;
                        totalCratesUnloadedValue = totalCratesUnloadedValue + dockList[d].Line.Peek().Trailer.Peek().price;
                        numCratesUnloaded = numCratesUnloaded + 1;
                        dockInfoList[d].IncreaseTotalCost(dockList[d].Line.Peek().Trailer.Peek().price);
                        dockInfoList[d].IncreaseUseTime();
                        dockList[d].Line.Peek().Unload();                        
                        if (dockList[d].Line.Peek().Trailer.Count == 0)
                        {
                            dockList[d].SendOff();
                            numTrucksProcessed = numTrucksProcessed + 1;
                        }
                    }
                }

                for (int d = 0; d < numDocks; d++)
                {
                    if (dockList[d].Line.Count > 0)
                    {
                        revenue = revenue - 100.00;
                        dockInfoList[d].decreaseTotalCost(100.00);
                    }
                }

                for (int d = 0; d < numDocks; d++)
                {
                    if (longestLine < dockList[d].Line.Count)
                    {
                        longestLine = dockList[d].Line.Count;
                    }
                }
                for (int d = 0; d < numDocks; d++)
                {
                    dockInfoList[d].SetNotInUseTime(increments);
                    dockInfoList[d].SetAvgTimeInUse(increments);
                }
            }
            averageCrateValue = Math.Round(totalCratesUnloadedValue / numCratesUnloaded,2);
            averageTruckValue = Math.Round(totalCratesUnloadedValue / numTrucksProcessed,2);
            
            WriteReport(dateTime, numDocks, Math.Round(revenue,2), longestLine, numTrucksProcessed, numCratesUnloaded, Math.Round(totalCratesUnloadedValue,2), averageCrateValue, averageTruckValue, dockInfoList);
        }
        public void WriteShipmentLog(string dateTime, int increment, int dockNum, string name, string company, string crateID, double cratePrice, string dockState)
        {
            try
            {
                string filepath = $@"..\..\..\data\output\logs\shipmentLog_" + dateTime + ".csv";
                StreamWriter rwr = new StreamWriter(filepath, true);
                rwr.WriteLine($@"{increment* 30} Minutes,{dockNum},{name},{company},{crateID},{cratePrice}$,{dockState}");
                rwr.Close();
            }
            catch
            {
                throw new FileLoadException();
            }

        }

        public void WriteReport(string dateTime, int numDocks, double revenue, int longestLine, int numTrucksProcessed, int numCratesUnloaded, double totalCratesUnloadedValue, double averageCrateValue, double averageTruckValue,List<DockInfo> dockInfoList)
        {
            try
            {
                string filepath = $@"..\..\..\data\output\reports\shipmentReport_" + dateTime + ".csv";
                StreamWriter rwr = new StreamWriter(filepath, true);
                string data = $@"{numDocks},{revenue}$,{longestLine},{numTrucksProcessed},{numCratesUnloaded},{totalCratesUnloadedValue}$,{averageCrateValue}$,{averageTruckValue}$";
                for (int d = 0; d < numDocks; d++)
                {
                    data = data + $",{d+1},{dockInfoList[d].inUseTime * 30} Minutes,{dockInfoList[d].notInUseTime * 30} Minutes,{dockInfoList[d].avgTimeInUse}%,{Math.Round(dockInfoList[d].totalCost,2)}$";
                }
                rwr.WriteLine(data);
                rwr.Close();
            }
            catch
            {
                throw new FileLoadException();
            }

        }

        public void WriteHeaders(string dateTime, int numDocks)
        {
            try
            {
                string filepath = $@"..\..\..\data\output\logs\shipmentLog_" + dateTime + ".csv";
                StreamWriter rwr = new StreamWriter(filepath, true);
                rwr.WriteLine($@"Time Increment,Dock #,Truck Driver's Name,Delivery Company,Crate ID,Crate Value,Dock Status");
                rwr.Close();

                filepath = $@"..\..\..\data\output\reports\shipmentReport_" + dateTime + ".csv";
                rwr = new StreamWriter(filepath, true);
                string data = $@"# of Docks,Revenue,Longest Dock Line,# Trucks Processed,# Crates Unloaded,Total Value of Unloaded Crates,Average Value of Crate,Average Value of Truck";
                for (int d = 0; d < numDocks; d++)
                {
                    data = data + $",Dock #,Time Dock Was In Use,Time Dock Was Not In Use,Percent of Time Dock Was In Use,Cost of Dock Operation";
                }
                rwr.WriteLine(data);
                rwr.Close();
            }
            catch
            {
                throw new FileLoadException();
            }
        }

        public string randomName()
        {
            Random random = new Random();
            int randName = random.Next(0, Enum.GetNames(typeof(Names)).Length);
            int randType = random.Next(1, 42);

            char[] nameArr = Convert.ToString((Names)Enum.Parse(typeof(Names), Convert.ToString(randName))).ToCharArray();
            string newName = "";
            foreach (char item in nameArr)
            {
                if (item != '_')
                {
                    newName += item;
                }
                else
                {
                    newName += " ";
                }
            }

            return newName;
        }

        public string randomCompany()
        {
            Random random = new Random();
            int randCompany = random.Next(0, Enum.GetNames(typeof(Companies)).Length);
            int randType = random.Next(1, 44);

            char[] nameArr = Convert.ToString((Companies)Enum.Parse(typeof(Companies), Convert.ToString(randCompany))).ToCharArray();
            string newCompany = "";
            foreach (char item in nameArr)
            {
                if (item != '_')
                {
                    newCompany += item;
                }
                else
                {
                    newCompany += " ";
                }
            }

            return newCompany;
        }
    }
}
