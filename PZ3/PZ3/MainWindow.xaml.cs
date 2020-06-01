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

namespace PZ3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point start = new Point();
        private Point diffOffset = new Point();
        private int zoomMax = 12;
        private int zoomCurent = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void viewport1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleY = 1;
            if (e.Delta > 0 && zoomCurent < zoomMax)
            {
                scaleX = skaliranje.ScaleX + 0.1;
                scaleY = skaliranje.ScaleY + 0.1;
                zoomCurent++;
                skaliranje.ScaleX = scaleX;
                skaliranje.ScaleY = scaleY;
            }
            else if (e.Delta <= 0 && zoomCurent > -zoomMax)
            {
                if (zoomCurent > -5)
                {
                    scaleX = skaliranje.ScaleX - 0.1;
                    scaleY = skaliranje.ScaleY - 0.1;
                    zoomCurent--;
                    skaliranje.ScaleX = scaleX;
                    skaliranje.ScaleY = scaleY;
                }
            }
        }
    }
}
