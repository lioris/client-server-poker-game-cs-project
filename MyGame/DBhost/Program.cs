﻿using System;
using System.ServiceModel;

namespace DBhost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("wait while DB host is starting......");
            ServiceHost host = new ServiceHost(typeof(DBservice.DBservice));
            host.Open();

            Console.WriteLine("Data Base Host is ON");
            Console.ReadLine();

            host.Close();
            Console.WriteLine("Data Base Host is OFF");

        }
    }
}
