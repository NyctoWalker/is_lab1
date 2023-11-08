using System;
using System.Web;
using System.Windows;
/*using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;*/

namespace is_lab5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppVM VM = new();
            DataContext = VM;
            ((AppVM)DataContext).WebBrowser = wb;
        }
    }
}
