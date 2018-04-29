// AUTHOR: Aylet Lopez
// FILENAME: main.cs
// DATE: 4/29/2018
// REVISION HISTORY: Rev NEW
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptWordDriver;

//Description: The Main will take the input, and call to the driver, that will then 
//call to the encoding class.  This allows the driver to be independent from the 
//main routine. 
//Dependencies: Depends on Driver and EncryptWord Class


namespace MainFile
{
    class main
    {
        static void Main(string[] args)
        {
            Driver drive = new Driver();
            Console.WriteLine(); 
        }
    }
}
