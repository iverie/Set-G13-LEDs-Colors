using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace SetLEDColor_G13
{
    class Program
    {
        [DllImport("LogitechLedEnginesWrapper ", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLedInit();

        [DllImport("LogitechLedEnginesWrapper ", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLedSaveCurrentLighting();

        [DllImport("LogitechLedEnginesWrapper ", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiLedSetLighting(int redPercentage, int greenPercentage, int bluePercentage);

        [DllImport("LogitechLedEnginesWrapper ", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LogiLedShutdown();

        static void Main(string[] args)
        {
            /* Kills previous process if it exists, so that there are not two instances accessing the driver */
            Process[] pname = Process.GetProcessesByName(AppDomain.CurrentDomain.FriendlyName.Remove(AppDomain.CurrentDomain.FriendlyName.Length - 4));
            if (pname.Length > 1)
            {
                pname.Where(p => p.Id != Process.GetCurrentProcess().Id).First().Kill();
            }

            int r, g, b;
            if (args.Length < 3)
            {
                Console.WriteLine("Three inputs needed (r%, g%, b%)");
                r = 10;
                g = 10;
                b = 10;
            }
            else
            {
                r = (int)Int32.Parse(args[0]);
                g = (int)Int32.Parse(args[1]);
                b = (int)Int32.Parse(args[2]);
            }

            LogiLedInit();
            LogiLedSaveCurrentLighting();
            LogiLedSetLighting(r, g, b);


            /* The app needs to be kept alive since the lighting is restored upon exit */
            while (true)
            {
                Thread.Sleep(60000);
                LogiLedSetLighting(r, g, b);
            }
        }
    }
}
