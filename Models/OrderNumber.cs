using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class OrderNumber
    {
        // Just a static variable simply to display the basket number on the menu tab
        public static int orderValue = 0;

        public static int tNum = 0;
        public static string tStatus = "";

        public static bool paid = false; 
    }
}