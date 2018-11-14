/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace VirtualRouterHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(AssemblyTitle + " " + AssemblyVersion);
            Console.WriteLine(AssemblyCopyright);
            Console.WriteLine();

            if (args.Length > 0 && args[0] == "/?")
            {
                Console.WriteLine("Usage: VirtualRouterHostConsole [SSID] [Passkey]");
                return;
            }


            var virtualRouterHost = new VirtualRouterHost.VirtualRouterHost();
            var serviceHost = new ServiceHost(virtualRouterHost);

            if(args.Length == 2)
            {
                var strSSID = args[0];
                var strPassKey = args[1];

                virtualRouterHost.SetConnectionSettings(strSSID, 100);
                virtualRouterHost.SetPassword(strPassKey);

                Console.WriteLine("SSID: " + strSSID);
                Console.WriteLine("Passkey: " + strPassKey);
                Console.WriteLine();
            }           


            var conns = virtualRouterHost.GetSharableConnections();
            var connToShare = conns.FirstOrDefault();
            if (!virtualRouterHost.Start(connToShare))
            {
                Console.WriteLine("ERROR: Virtual Router could not be started. Supported hardware may not have been found.");
                Console.WriteLine();
            }


            Console.WriteLine("Starting Service...");

            if (serviceHost.State != CommunicationState.Opened)
            {
                serviceHost.Open();
            }

            Console.WriteLine();

            Console.WriteLine("Virtual Router Service Running... [Press Enter To Stop]");

            Console.ReadLine();

            serviceHost.Close();


            virtualRouterHost.Stop();

            Console.WriteLine("Virtual Router Service Stopped.");
        }



        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
    }
}
