using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WebView2Version
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetLatin(object sender, RoutedEventArgs e)
        {
            getVersion(false);
        }

        private void GetJapanese(object sender, RoutedEventArgs e)
        {
            getVersion(true);
        }

        private void getVersion(bool getForeign)
        {
            string result = string.Empty;

            try
            {
                var currentDir = Directory.GetCurrentDirectory();
                String dllPath;
                if (getForeign)
                {
                    dllPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\Contents\お客様"));
                }
                else
                {
                    dllPath = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\Contents\latin"));
                }

                var dllFile = Path.Combine(dllPath, @"WebView2Loader.dll");
                var dllExists = File.Exists(dllFile);

                if (!dllExists)
                {
                    VersionString.Text = $"<Missing file {dllFile}>";
                    return;
                }

                CoreWebView2Environment.SetLoaderDllFolderPath(dllPath);

                result = CoreWebView2Environment.GetAvailableBrowserVersionString();
            }
            catch (InvalidOperationException ex)
            {
                result = "Note: CoreWebView2Environment.SetLoaderDllFolderPath() can only be called once. You have to rerun this app to try the other path.\n\nException caught:\n" + ex.ToString();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            VersionString.Text = result;
            Console.WriteLine($"CoreWebView2Environment.GetAvailableBrowserVersionString() returned {result}");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
