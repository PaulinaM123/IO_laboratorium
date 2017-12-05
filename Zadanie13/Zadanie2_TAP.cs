﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie13
{
    class Zadanie2_TAP
    {
        private bool zadanie2 = false;
        public bool Z2
        {
            get { return zadanie2; }
            set { zadanie2 = value; }
        }
        public void Zadanie2()
        {
            //ZADANIE 2. ODKOMENTUJ I POPRAW  
            /*
                Task.Run(
                    () ==
                    {
                       Z2 = true;
                    }
             */

            //opcja 1) 
            //var t = Task.Run(
            //      () =>
            //      {
            //         Z2 = true;
            //      });

            //t.Wait();
            //Task.WaitAll(t);

            //opcja 2)
            Task.Run(
                  () =>
                  {
                      Z2 = true;
                  }).Wait();

        }
        static void Main(string[] args)
        {
            Zadanie2_TAP z = new Zadanie2_TAP();
            Console.WriteLine("z.Z2 is " + z.Z2);

            z.zadanie2 = true;
            Console.WriteLine("z.Z2 is " + z.Z2);
        }

    }
}