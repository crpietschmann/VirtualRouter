/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtualRouter.Wlan.WinAPI;
using VirtualRouter.Wlan;

namespace VirtualRouterHost
{
    [DataContract]
    public class ConnectedPeer
    {
        public ConnectedPeer()
        {
        }

        public ConnectedPeer(WlanStation peer)
            : this()
        {
            this.MacAddress = peer.MacAddress;
        }

        [DataMember]
        public string MacAddress { get; set; }
    }
}
