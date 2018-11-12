using System;
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
using harcocska;

namespace Windows
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class UserControl1 : UserControl
	{
		public CJatekos jatekos = null;

		public UserControl1()
		{
			InitializeComponent();
		}
		public UserControl1(CJatekos j)
		{
			InitializeComponent();
			jatekos = j;
			nevLabel.Content = jatekos.nev;
			fafeltoltes();
			//RecursiveFaBejaras((TreeViewItem)treeView1.Items[0], "barakk2");
		}
		public void fafeltoltes()
		{
			treeView1.Items.Clear();


			TreeViewItem root = null;

			foreach (TreeNode<CFejlesztesiElem> node in jatekos.f.Root)
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
				treeIcon(treeItem);
			}
		}
		public BitmapImage CreateImage(string path)
		{
			BitmapImage myBitmapImage = new BitmapImage();
			myBitmapImage.BeginInit();
			myBitmapImage.UriSource = new Uri(path);
			myBitmapImage.EndInit();
			return myBitmapImage;
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
				treeIcon(treeItem);
			}
		}

		private void treeIcon(TreeViewItem treeItem)
		{
			TreeNode<CFejlesztesiElem> found = jatekos.f.Root.FindTreeNode(node => node.Data != null && node.Data.name == (string)treeItem.Header);
			//if (found!=null)
			//{
			string s = treeItem.Header.ToString();
			StackPanel stack = new StackPanel();
			stack.Orientation = Orientation.Horizontal;
			treeItem.Header = stack;
			ImageSource iconSource;
			if (found.Data.feloldott)
				iconSource = CreateImage(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "fejlesztes_engedett.png"));
			else
				iconSource = CreateImage(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "fejlesztes_tiltott.png"));

			Image icon = new Image();
			icon.Width = 10;
			icon.VerticalAlignment = VerticalAlignment.Center;
			icon.Margin = new Thickness(0, 0, 4, 0);
			icon.Source = iconSource;
			stack.Children.Add(icon);

			//Add the HeaderText After Adding the icon
			TextBlock textBlock = new TextBlock();
			textBlock.Text = s;
			textBlock.VerticalAlignment = VerticalAlignment.Center;
			stack.Children.Add(textBlock);
			//}
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
