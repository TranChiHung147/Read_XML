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
using System.Xml;

namespace ReadFileXML
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        void openFile(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = false;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files(*.*)|*.*";
            dlg.Multiselect = false;
            if (dlg.ShowDialog() != true) { return; }

            try
            {
                xmlDoc.Load(dlg.FileName);
            }
            catch (XmlException)
            {
                MessageBox.Show("The XML file is invalid");
                return;
            }

            XmlNode root = (XmlNode)xmlDoc.DocumentElement;
            Node_ noderoot = new Node_(root, 1);
            for (int i = 2; i < ui_root.Children.Count; i++)
            {
                ui_root.Children.RemoveAt(i);
            }
            ui_root.Children.Add(noderoot.Item());
        }

    }

    public class Node_
    {
        public XmlNode Node { get; set; }
        public int Level { get; set; } = 0;
        public StackPanel panelNode = new StackPanel();
        public Node_(XmlNode node, int level)
        {
            this.Node = node;
            this.Level = level;
        }

        public StackPanel Item()
        {   
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            TextBlock textBlock = new TextBlock();
            textBlock.Width = 30 * Level;
            panel.Children.Add(textBlock);

            Button button = new Button();
            button.Height = 15;
            button.Width = 15;
            button.Content = "+";
            button.FontSize = 10;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Background = Brushes.White;
            button.Foreground = Brushes.Red;
            button.Click += Extension_click;
            panel.Children.Add(button);

            Label label = new Label();
            label.Content = Node.Attributes["text"].Value.ToString();
            label.Height = 30;
            label.FontSize = 15;
            panel.Children.Add(label);
            panelNode.Children.Add(panel);
     
            return panelNode;
        }

        void Extension_click(object sender, RoutedEventArgs e)
        {
            StackPanel p = (StackPanel)panelNode.Children[0];
            Button b = (Button)p.Children[1];
            if (b.Content == "+")
            {
                b.Content = "-";
                foreach (XmlNode n in Node.ChildNodes)
                {
                    Node_ temp = new Node_(n, Level + 1);
                    StackPanel panel = temp.Item();
                    panelNode.Children.Add(panel);
                }
            }
            else
            {
                b.Content = "+";
                for (int i = panelNode.Children.Count-1; i > 0 ; i--)
                {
                    panelNode.Children.RemoveAt(i);
                }
            }
            
        }
    }
}
