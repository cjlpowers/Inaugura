using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

using System.Xml;

namespace MediaLibrary
{
	public partial class Window1
	{
        private XmlNode mMediaLibraryXml;

		public Window1()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("MediaLibrary.xml");

            this.mMediaLibraryXml = xmlDoc.DocumentElement;
            this.ShowList();
        }

        private void ShowList()
        {
            ShowList(this.mMediaLibraryXml);
        }

        private void ShowList(XmlNode libraryXml)
        {
            this.mTreeView.ItemsSource = libraryXml.SelectNodes("/library/media");
        }

        private void ShowList(XmlNode[] mediaList)
        {
            this.mTreeView.ItemsSource = mediaList;
        }

        private void Search(string searchText)
        {
            searchText = searchText.ToLower();
            System.Collections.Generic.List<XmlNode> list = new System.Collections.Generic.List<XmlNode>();
            XmlNodeList nodes = this.mMediaLibraryXml.SelectNodes("/library/media");

            foreach (XmlNode node in nodes)
            {
                bool found = false;
                if (node.Attributes["name"] != null && node.Attributes["name"].Value.ToLower().Contains(searchText))
                {
                    found = true;
                    continue;
                }
                
                XmlNodeList items = node.SelectNodes("item");
                foreach (XmlNode item in items)
                {
                    if (item.Attributes["name"] != null && item.Attributes["name"].Value.ToLower().Contains(searchText))
                        found = true;
                    else if (item.Attributes["type"] != null && item.Attributes["type"].Value.ToLower().Contains(searchText))
                        found = true;
                    else if (item.Attributes["tags"] != null && item.Attributes["tags"].Value.ToLower().Contains(searchText))
                        found = true;

                    if (found)
                        break;
                }

                if (found)
                    list.Add(node);
            }

            this.ShowList(list.ToArray());
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.mTxtSearch.Text))
                this.ShowList();
            else
                this.Search(this.mTxtSearch.Text);
                
        }
	}
}