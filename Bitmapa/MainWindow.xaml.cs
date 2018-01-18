using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bitmapa
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
        private async Task Asynchronicznie_wczytaj_Async()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files ( *png, *jpg, *bmp) | *.png; *.jpg; *.bmp";
            openDialog.InitialDirectory = "C:\\Users\\PM\\Desktop";
            string lokalizacja = openDialog.FileName.ToString();

            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
            {
                await Task.Run(() =>
                {
                    lokalizacja = openDialog.FileName;

                    Thread.Sleep(5000);

                    BitmapImage image1 = new BitmapImage(new Uri(lokalizacja));
                    Async.Source = image1;
                });
            }

        }

        private void Synchronicznie_wczytaj()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files ( *png, *jpg, *bmp) | *.png; *.jpg; *.bmp";
            openDialog.InitialDirectory = "C:\\Users\\PM\\Desktop";
            string lokalizacja = openDialog.FileName.ToString();

            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
            {
                lokalizacja = openDialog.FileName;

                Thread.Sleep(5000);

                BitmapImage image = new BitmapImage(new Uri(lokalizacja));
                Sync.Source = image;
            }

        }

        private void Synchronicznie_Click(object sender, RoutedEventArgs e)
        {
            Synchronicznie_wczytaj();
        }



        private void Asynchronicznie_Click(object sender, RoutedEventArgs e)
        {
            Asynchronicznie_wczytaj_Async();
        }
    }
}

/****************WNIOSKI****************
 *W przypadku, gdy obraz wczytywany jest synchronicznie, interfejs urzytkownika jest nieaktywny 
 * do momentu skończenia wykonywania operacji wczytywania i wyświetlania obrazu.
 * Problem ten rozwiązany jest poprzez asynchroniczne wczytywanie i wyświetlanie obrazu.
 * Aby efekt był widoczny, pomocniczo zostało dodane opóźnienie.
 */
