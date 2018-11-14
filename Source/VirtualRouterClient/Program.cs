/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/

using System;

namespace VirtualRouterClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SingleInstanceManager manager = new SingleInstanceManager();
            manager.Run(args);
        }
    }
}
