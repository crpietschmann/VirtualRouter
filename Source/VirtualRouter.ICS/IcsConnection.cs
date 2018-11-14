/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using NETCONLib;

namespace IcsMgr
{
    public class IcsConnection
    {
        private INetSharingManager _NSManager;

        public IcsConnection(INetSharingManager pIcsMgr, INetConnection pNetConnection)
        {
            this.INetConnection = pNetConnection;
            this._NSManager = pIcsMgr;
        }

        public INetConnection INetConnection { get; private set; }

        private INetSharingConfiguration _config = null;
        public INetSharingConfiguration config
        {
            get
            {
                if (this._config == null)
                {
                    this._config = this._NSManager.get_INetSharingConfigurationForINetConnection(this.INetConnection);
                }
                return this._config;
            }
        }

        private INetConnectionProps _props = null;
        public INetConnectionProps props
        {
            get
            {
                if (this._props == null)
                {
                    this._props = this._NSManager.get_NetConnectionProps(this.INetConnection);
                }
                return this._props;
            }
        }

        public bool IsSupported
        {
            get
            {
                var props = this.props;

                return ((
                        props.MediaType == tagNETCON_MEDIATYPE.NCM_LAN
                        || props.MediaType == tagNETCON_MEDIATYPE.NCM_DIRECT
                        || props.MediaType == tagNETCON_MEDIATYPE.NCM_ISDN
                        || props.MediaType == tagNETCON_MEDIATYPE.NCM_PHONE
                        || props.MediaType == tagNETCON_MEDIATYPE.NCM_PPPOE
                        || props.MediaType == tagNETCON_MEDIATYPE.NCM_TUNNEL
                        || props.MediaType == tagNETCON_MEDIATYPE.NCM_BRIDGE
                    ) && (
                        props.Status != tagNETCON_STATUS.NCS_DISCONNECTED
                    ));
            }
        }

        public bool SharingEnabled
        {
            get
            {
                return this.config.SharingEnabled;
            }
        }

        public bool IsPublic
        {
            get
            {
                return (this.config.SharingConnectionType == tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PUBLIC);
            }
        }

        public bool IsPrivate
        {
            get
            {
                return (this.config.SharingConnectionType == tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PRIVATE);
            }
        }

        public bool IsConnected
        {
            get
            {
                return (this.props.Status == tagNETCON_STATUS.NCS_CONNECTED);
            }
        }

        public string DeviceName
        {
            get
            {
                return this.props.DeviceName;
            }
        }

        public string Name
        {
            get
            {
                return this.props.Name;
            }
        }

        public Guid Guid
        {
            get
            {
                return new Guid(this.props.Guid);
            }
        }

        public void DisableSharing()
        {
            var config = this.config;
            if (config.SharingEnabled)
            {
                config.DisableSharing();
            }
        }

        public void EnableAsPublic()
        {
            var config = this.config;

            config.DisableSharing();
            
            config.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PUBLIC);
        }

        public void EnableAsPrivate()
        {
            var config = this.config;

            config.DisableSharing();

            config.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PRIVATE);
        }

        public bool IsMatch(Guid guid)
        {
            return ((new Guid(this.props.Guid)).ToString().ToLowerInvariant() == guid.ToString().ToLowerInvariant());
        }
    }
}
