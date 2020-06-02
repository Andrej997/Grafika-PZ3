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
using System.Windows.Media.Media3D;
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
        private Point3D point3DOriginal = new Point3D();
        private Point3D point3D = new Point3D();

        AxisAngleRotation3D ax3d;

        public MainWindow()
        {
            InitializeComponent();
            ax3d = new AxisAngleRotation3D(new Vector3D(0, 0, 0), 0 /* inicijalno da ne bude pomerena slika*/);
            RotateTransform3D myRotateTransform = new RotateTransform3D(ax3d);
            MyModel.Transform = myRotateTransform;

            point3DOriginal = camera.Position;
            point3D = camera.Position;
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

        private void viewport1_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(slider1.Value); 
            if (viewport1.IsMouseCaptured)
            {
                Point end = e.GetPosition(this);
                double offsetX = end.X - start.X;
                double offsetY = end.Y - start.Y;
                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    Console.WriteLine("offsetX = " + end.X + " - " + start.X + " = " + offsetX);
                    Console.WriteLine("offsetY = " + end.Y + " - " + start.Y + " = " + offsetY);
                    this.SetAx3d(offsetX, offsetY);
                }
                else if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Console.WriteLine("offsetX = " + end.X + " - " + start.X + " = " + offsetX);
                    double w = this.Width;
                    double h = this.Height;
                    double translateX = (offsetX * 100) / w;
                    double translateY = -(offsetY * 100) / h;
                    translacija.OffsetX = diffOffset.X + (translateX / (100 * skaliranje.ScaleX));
                    translacija.OffsetY = diffOffset.Y + (translateY / (100 * skaliranje.ScaleX));
                }
            }
        }
        private double lastAx3d = 0;
        private void SetAx3d(double offsetX, double offsetY)
        {
            camera.Position = point3DOriginal;
            Console.WriteLine("ax3d.Angle = " + ax3d.Angle);
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX < 0 && offsetY < 0) // 1
            {
                if (ax3d.Angle > -60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d - 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX < 0 && offsetY > 0) // 2
            {
                if (ax3d.Angle > -60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d - 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX < 0 && offsetY > 0) // 3
            {
                if (ax3d.Angle < 40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d + 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX > 0 && offsetY > 0) // 4
            {
                if (ax3d.Angle < 40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d + 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX > 0 && offsetY > 0) // 5
            {
                if (ax3d.Angle < 60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d + 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX > 0 && offsetY < 0) // 6
            {
                if (ax3d.Angle < 60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d + 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX > 0 && offsetY < 0) // 7
            {
                if (ax3d.Angle > -40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d - 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX < 0 && offsetY < 0) // 8
            {
                if (ax3d.Angle > -40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d - 0.1;
                    lastAx3d = ax3d.Angle;
                }
            }
        }

        private HashSet<Point> Vector()
        {
            return null;
        }

        private Point GetCenter() =>
            new Point(viewport1.ActualWidth / 2, viewport1.ActualHeight / 2);

        /// <summary>
        /// Pritisak na dugme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewport1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                viewport1.CaptureMouse();
                start = e.GetPosition(this);
                diffOffset.X = translacija.OffsetX;
                diffOffset.Y = translacija.OffsetY;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                viewport1.CaptureMouse();
                start = e.GetPosition(this);
                diffOffset.X = translacija.OffsetX;
                diffOffset.Y = translacija.OffsetY;
            }
        }

        /// <summary>
        /// Pustanje dugmeta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewport1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released)
            {
                viewport1.ReleaseMouseCapture();
            }
            if (e.LeftButton == MouseButtonState.Released)
            {
                viewport1.ReleaseMouseCapture();
            }
        }

    }
}
