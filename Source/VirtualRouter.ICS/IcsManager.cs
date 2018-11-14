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
using NETCONLib;

namespace IcsMgr
{
    public class IcsManager
    {
        protected INetSharingManager _NSManager;

        public IcsManager()
        {
            this.Init();
        }

        public void Init()
        {
            this._NSManager = new NetSharingManagerClass();
        }

        public void EnableIcs(Guid publicGuid, Guid privateGuid)
        {
            if (!this.SharingInstalled)
            {
                throw new Exception("Internet Connection Sharing NOT Installed");
            }

            var connections = this.Connections;

            IcsConnection publicConn = (from c in connections
                              where c.IsMatch(publicGuid)
                              select c).First();

            IcsConnection privateConn = (from c in connections
                                         where c.IsMatch(privateGuid)
                                        select c).First();

            this.DisableIcsOnAll();

            publicConn.EnableAsPublic();
            privateConn.EnableAsPrivate();
        }

        public void DisableIcsOnAll()
        {
            foreach (var conn in this.Connections)
            {
                if (conn.IsSupported)
                {
                    conn.DisableSharing();
                }
            }
        }

        public List<IcsConnection> Connections
        {
            get
            {
                var list = new List<IcsConnection>();

                foreach (INetConnection conn in this._NSManager.EnumEveryConnection)
                {
                    list.Add(new IcsConnection(this._NSManager, conn));
                }

                return list;
            }
        }

        public bool SharingInstalled
        {
            get
            {
                return this._NSManager.SharingInstalled;
            }
        }
    }
}
