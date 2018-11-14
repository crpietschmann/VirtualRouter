/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VirtualRouterClient
{
    public class TileView : ViewBase
    {
        private DataTemplate itemTemplate;
        public DataTemplate ItemTemplate
        {
            get { return itemTemplate; }
            set { itemTemplate = value; }
        }

        private Brush selectedBackground = Brushes.Transparent;
        public Brush SelectedBackground
        {
            get { return selectedBackground; }
            set { selectedBackground = value; }
        }

        private Brush selectedBorderBrush = Brushes.Black;
        public Brush SelectedBorderBrush
        {
            get { return selectedBorderBrush; }
            set { selectedBorderBrush = value; }
        }

        protected override object DefaultStyleKey
        {
            get { return new ComponentResourceKey(GetType(), "TileView"); }
        }

        protected override object ItemContainerDefaultStyleKey
        {
            get { return new ComponentResourceKey(GetType(), "TileViewItem"); }
        }
    }
}
