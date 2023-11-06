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
using System;

public class Crate
{
	Random rand = new Random();
    //Random randPrice = new rand(50, 501);
    //Random randId = new Random(0, 1000000);
    public string Id { get; private set; }
    public double price { get; private set; }

    public Crate(int _Id)
	{
		Id = Convert.ToString(_Id);
        price = Math.Round(rand.NextDouble() * (500 - 50) + 50,2);
	}
}
