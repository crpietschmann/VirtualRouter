/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Runtime.Serialization;
using IcsMgr;

namespace VirtualRouterHost
{
    [DataContract]
    public class SharableConnection
    {
        public SharableConnection() { }

        public SharableConnection(IcsConnection conn)
        {
            this.Name = conn.Name;
            this.DeviceName = conn.DeviceName;
            this.Guid = conn.Guid;
        }
        
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public Guid Guid { get; set; }
    }
}