﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace harcocska
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
		public CJatekos jatekos = null;
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		public UserControl1()
        {
            InitializeComponent();
        }
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		public UserControl1(CJatekos j)
		{
			InitializeComponent();
			jatekos = j;
			nevLabel.Content = jatekos.nev;
			fafeltoltes();
			//RecursiveFaBejaras((TreeViewItem)treeView1.Items[0], "barakk2");

		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		public void fafeltoltes()
		{
			treeView1.Items.Clear();


			TreeViewItem root = null;

			foreach (TreeNode<CFejlsztesiElem> node in jatekos.f.Root)
			{

				string indent = CreateIndent(node.Level);
				Console.WriteLine(indent + (node.Data.name ?? "null"));

				TreeViewItem tvi = new TreeViewItem();
				tvi.Header = node.Data.name;




				if (node.Level == 0)
				{
					treeView1.Items.Add(tvi);
					root = tvi;
				}
				else
				{
					TreeViewItem temp = RecursiveFaBejaras(root, node.Parent.Data.name);
					temp.Items.Add(tvi);
				}





			}
			foreach (object item in treeView1.Items)
			{
				TreeViewItem treeItem = (TreeViewItem)item;

				if (treeItem != null)
				{
					ExpandAllNodes(treeItem);
					treeItem.IsExpanded = true;
				}
			}
		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		private void ExpandAllNodes(TreeViewItem rootItem)
		{
			foreach (object item in rootItem.Items)
			{
				TreeViewItem treeItem = (TreeViewItem)item;

				if (treeItem != null)
				{
					ExpandAllNodes(treeItem);
					treeItem.IsExpanded = true;
				}
			}
		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		private TreeViewItem RecursiveFaBejaras(TreeViewItem treeViewItem, string keresett)
		{
			TreeViewItem ret = null;

			if ((string)treeViewItem.Header == keresett)
			{
				ret = treeViewItem;
				return ret;
			}
			// Print the node.  
			//System.Diagnostics.Debug.WriteLine(treeNode.Text);
			//MessageBox.Show(treeNode.Text);
			// Print each node recursively.  
			foreach (TreeViewItem tn in treeViewItem.Items)
			{
				if ((string)tn.Header == keresett)
				{
					ret = tn;
					break;
				}
				else
				{
					ret = RecursiveFaBejaras(tn, keresett);
				}
			}
			return ret;
		}
		//mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
		private static String CreateIndent(int depth)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < depth; i++)
			{
				sb.Append(' ');
			}
			return sb.ToString();
		}
	}

	
}
