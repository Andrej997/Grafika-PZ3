using PZ3.Model;
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
using System.Windows.Media.Animation;
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
        private System.Windows.Point start = new System.Windows.Point();
        private System.Windows.Point diffOffset = new System.Windows.Point();
        private int zoomMax = 12;
        private int zoomCurent = 1;

        private double cubeSize = 0.006;
        private double up = 0.012;
        private double lineSize = 0.002;
        private double mapSize = 1;
        public Point3DCollection Positions { get; private set; }
        public Int32Collection Indicies { get; private set; }

        private List<GeometryModel3D> models = new List<GeometryModel3D>();

        AxisAngleRotation3D ax3d;

        
        
        public MainWindow()
        {
            InitializeComponent();

            ax3d = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0 /* inicijalno da ne bude pomerena slika*/);
            //ax3d.Angle = -134;
            RotateTransform3D myRotateTransform = new RotateTransform3D(ax3d);
            MyModel.Transform = myRotateTransform;

            Controller.XMLLoader.LoadXml();

            DrawNodeEntity(DataContainers.Containers.GetNodes);

            DrawSwitchEntity(DataContainers.Containers.GetSwitches);

            DrawSubstationEntity(DataContainers.Containers.GetSubstations);

            DrawLineEntities();
        }

        private System.Windows.Point FindPointOnMap(double X, double Y)  => new System.Windows.Point(
            (Y - DataContainers.Containers.DLLon) / (DataContainers.Containers.GDLon - DataContainers.Containers.DLLon) * (mapSize - cubeSize),
            (X - DataContainers.Containers.DLLat) / (DataContainers.Containers.GDLat - DataContainers.Containers.DLLat) * (mapSize - cubeSize));

        #region Drawing Nodes
        private void DrawNodeEntity(HashSet<NodeEntity> powerEntities)
        {
            foreach (var item in powerEntities)
            {
                double latitude;
                double longitude;

                Controller.XMLLoader.ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                // ako pw izlazi van "okvira"
                //if (latitude < DataContainers.Containers.DLLat || longitude < DataContainers.Containers.DLLon) continue;
                //else if (latitude > DataContainers.Containers.GDLat || longitude > DataContainers.Containers.GDLon) continue;
                if (latitude < 45.2325 || latitude > 45.277031 || longitude < 19.793909 || longitude > 19.894459)
                    continue;
                ToolTip toolTip = new ToolTip();
                toolTip.Content = $"id: {item.Id}\n" +
                                $"name: {item.Name}";

                int connections = Controller.CheckInData.AdditionalTask8NO(item);

                System.Windows.Point point = FindPointOnMap(latitude, longitude);

                int lvl = Controller.CheckInData.DaLiPOstoji(point);
                
                DrawPowerEntitiy(point, toolTip, lvl, 0, connections);
            }
        }

        private void DrawSwitchEntity(HashSet<SwitchEntity> powerEntities)
        {
            foreach (var item in powerEntities)
            {
                double latitude;
                double longitude;

                Controller.XMLLoader.ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                // ako pw izlazi van "okvira"
                //if (latitude < DataContainers.Containers.DLLat || longitude < DataContainers.Containers.DLLon) continue;
                //else if (latitude > DataContainers.Containers.GDLat || longitude > DataContainers.Containers.GDLon) continue;
                if (latitude < 45.2325 || latitude > 45.277031 || longitude < 19.793909 || longitude > 19.894459)
                    continue;
                ToolTip toolTip = new ToolTip();
                toolTip.Content = $"id: {item.Id}\n" +
                                $"name: {item.Name}\n" +
                                $"status: {item.Status}";

                int connections = Controller.CheckInData.AdditionalTask8SW(item);

                System.Windows.Point point = FindPointOnMap(latitude, longitude);

                int lvl = Controller.CheckInData.DaLiPOstoji(point);

                DrawPowerEntitiy(point, toolTip, lvl, 1, connections);
            }
        }

        private void DrawSubstationEntity(HashSet<SubstationEntity> powerEntities)
        {
            foreach (var item in powerEntities)
            {
                double latitude;
                double longitude;

                Controller.XMLLoader.ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                // ako pw izlazi van "okvira"
                //if (latitude < DataContainers.Containers.DLLat || longitude < DataContainers.Containers.DLLon) continue;
                //else if (latitude > DataContainers.Containers.GDLat || longitude > DataContainers.Containers.GDLon) continue;
                if (latitude < 45.2325 || latitude > 45.277031 || longitude < 19.793909 || longitude > 19.894459)
                    continue;

                ToolTip toolTip = new ToolTip();
                toolTip.Content = $"id: {item.Id}\n" +
                                $"name: {item.Name}";

                int connections = Controller.CheckInData.AdditionalTask8SU(item);

                System.Windows.Point point = FindPointOnMap(latitude, longitude);

                int lvl = Controller.CheckInData.DaLiPOstoji(point);

                DrawPowerEntitiy(point, toolTip, lvl, 2, connections);
            }
        }

        private void DrawPowerEntitiy(System.Windows.Point point, ToolTip toolTip, int lvl, int color, int connections)
        {
            GeometryModel3D firstObj = new GeometryModel3D();

            if (color == 0)
            {
                if (connections >= 0 && connections < 3)
                    firstObj.Material = new DiffuseMaterial(Brushes.LightPink);
                if (connections >= 3 && connections < 5)
                    firstObj.Material = new DiffuseMaterial(Brushes.Pink);
                if (connections >= 5)
                    firstObj.Material = new DiffuseMaterial(Brushes.DarkViolet);
            }
            else if (color == 1)
            {
                if (connections >= 0 && connections < 3)
                    firstObj.Material = new DiffuseMaterial(Brushes.LightGreen);
                if (connections >= 3 && connections < 5)
                    firstObj.Material = new DiffuseMaterial(Brushes.Green);
                if (connections >= 5)
                    firstObj.Material = new DiffuseMaterial(Brushes.DarkGreen);
            }
            else if (color == 2)
            {
                if (connections >= 0 && connections < 3)
                    firstObj.Material = new DiffuseMaterial(Brushes.OrangeRed);
                if (connections >= 3 && connections < 5)
                    firstObj.Material = new DiffuseMaterial(Brushes.Red);
                if (connections >= 5)
                    firstObj.Material = new DiffuseMaterial(Brushes.DarkRed);
            }

            if (lvl == 0)
                Positions = new Point3DCollection()
                {
                    new Point3D(-point.X , 0, point.Y),
                    new Point3D(-(point.X + cubeSize), 0, point.Y),
                    new Point3D(-point.X, 0, point.Y + cubeSize),
                    new Point3D(-(point.X + cubeSize), 0, point.Y + cubeSize),
                    new Point3D(-point.X, cubeSize, point.Y),
                    new Point3D(-(point.X + cubeSize), cubeSize, point.Y),
                    new Point3D(-(point.X ), cubeSize,  point.Y + cubeSize),
                    new Point3D(-(point.X + cubeSize), cubeSize,  point.Y + cubeSize),
                };
            else if (lvl == 1)
                Positions = new Point3DCollection()
                {
                    new Point3D(-point.X , up, point.Y),
                    new Point3D(-(point.X + cubeSize), up, point.Y),
                    new Point3D(-point.X, up, point.Y + cubeSize),
                    new Point3D(-(point.X + cubeSize), up, point.Y + cubeSize),
                    new Point3D(-point.X, cubeSize + up, point.Y),
                    new Point3D(-(point.X + cubeSize), cubeSize + up, point.Y),
                    new Point3D(-(point.X ), cubeSize + up,  point.Y + cubeSize),
                    new Point3D(-(point.X + cubeSize), cubeSize + up,  point.Y + cubeSize),
                };

            Indicies = new Int32Collection()
            {
                2,3,1,  2,1,0,  7,1,3,  7,5,1,  6,5,7,  6,4,5,  6,2,4,  2,0,4,  2,7,3,  2,6,7,  0,1,5,  0,5,4
            };

            firstObj.Geometry = new MeshGeometry3D() { Positions = Positions, TriangleIndices = Indicies };
            firstObj.SetValue(IsToolTip, toolTip);

            Map.Children.Add(firstObj);
            models.Add(firstObj);
        }
        #endregion

        #region Drawing Lines
        private void DrawLineEntities()
        {
            foreach (var line in DataContainers.Containers.GetLines)
            {
                double latitude = 0;
                double longitude = 0;
                HashSet<System.Windows.Point> points = new HashSet<System.Windows.Point>();

                foreach (var vertex in line.Vertices)
                {
                    Controller.XMLLoader.ToLatLon(vertex.X, vertex.Y, 34, out latitude, out longitude);
                    if (latitude < 45.2325 || latitude > 45.277031 || longitude < 19.793909 || longitude > 19.894459)
                        continue;

                    System.Windows.Point point = FindPointOnMap(latitude, longitude);
                    points.Add(point);
                }

                ToolTip tt = new System.Windows.Controls.ToolTip();
                string str = String.Format("LINE - ID: {0}, Name: {1}, Type: {2}", line.Id, line.Name, line.LineType);
                tt.Content = str;

                DrawLine(points, tt);
            }
        }

        private void DrawLine(HashSet<System.Windows.Point> points, ToolTip tt)
        {
            Indicies = new Int32Collection()
            {
                0,1,2,  1,3,2,  0,4,2,  4,6,2,  5,1,7,  1,3,7,  6,7,2,  7,3,2,  4,5,0,  5,1,0,  4,5,6,  5,7,6
            };

            for (int i = 0; i < points.Count - 1; i++)
            {
                Positions = new Point3DCollection()
                {
                    new Point3D(-points.ElementAt(i).X, 0, points.ElementAt(i).Y),
                    new Point3D(-(points.ElementAt(i).X + lineSize), 0, points.ElementAt(i).Y),
                    new Point3D(-points.ElementAt(i).X, 0, points.ElementAt(i).Y + lineSize),
                    new Point3D(-(points.ElementAt(i).X + lineSize), 0, points.ElementAt(i).Y + lineSize),
                    new Point3D(-points.ElementAt(i + 1).X, lineSize, points.ElementAt(i + 1).Y),
                    new Point3D(-(points.ElementAt(i + 1).X + lineSize), lineSize, points.ElementAt(i + 1).Y),
                    new Point3D(-points.ElementAt(i + 1).X, lineSize, points.ElementAt(i + 1).Y + lineSize),
                    new Point3D(-(points.ElementAt(i + 1).X + lineSize), lineSize, points.ElementAt(i + 1).Y + lineSize),
                };

                GeometryModel3D firstObj = new GeometryModel3D();
                firstObj.Material = new DiffuseMaterial(Brushes.Black);
                firstObj.Geometry = new MeshGeometry3D() { Positions = Positions, TriangleIndices = Indicies };
                firstObj.SetValue(IsToolTip, tt);

                Map.Children.Add(firstObj);
                models.Add(firstObj);
            }

        }
        #endregion

        public static readonly DependencyProperty IsToolTip = DependencyProperty.RegisterAttached("ToolTip", typeof(ToolTip), typeof(GeometryModel3D));

        #region Mouse Actions
        private void viewport1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Point p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleZ = 1;
            if (e.Delta > 0 && zoomCurent < zoomMax)
            {
                scaleX = skaliranje.ScaleX - 0.1;
                scaleZ = skaliranje.ScaleZ - 0.1;
                zoomCurent++;
                skaliranje.ScaleX = scaleX;
                skaliranje.ScaleZ = scaleZ;
            }
            else if (e.Delta <= 0 && zoomCurent > -zoomMax)
            {
                if (zoomCurent > -5)
                {
                    scaleX = skaliranje.ScaleX + 0.1;
                    scaleZ = skaliranje.ScaleZ + 0.1;
                    zoomCurent--;
                    skaliranje.ScaleX = scaleX;
                    skaliranje.ScaleZ = scaleZ;
                }
            }
        }

        private void viewport1_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(slider1.Value); 
            if (viewport1.IsMouseCaptured)
            {
                System.Windows.Point end = e.GetPosition(this);
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
                    //Console.WriteLine("offsetX = " + end.X + " - " + start.X + " = " + offsetX);
                    double w = this.Width;
                    double h = this.Height;
                    double translateX = (offsetX * 100) / w;
                    double translateY = -(offsetY * 100) / h;
                    translacija.OffsetX = diffOffset.X + (translateX / (100 * skaliranje.ScaleX));
                    translacija.OffsetZ = diffOffset.Y + (translateY / (-100 * skaliranje.ScaleX));
                }
            }
        }
        private double lastAx3d = 0;
        private void SetAx3d(double offsetX, double offsetY)
        {
            Console.WriteLine("ax3d.Angle = " + ax3d.Angle);
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX < 0 && offsetY < 0) // 1
            {
                //if (ax3d.Angle > -60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d - 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX < 0 && offsetY > 0) // 2
            {
                //if (ax3d.Angle > -60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d - 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX < 0 && offsetY > 0) // 3
            {
                //if (ax3d.Angle < 40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d + 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX > 0 && offsetY > 0) // 4
            {
                //if (ax3d.Angle < 40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d + 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX > 0 && offsetY > 0) // 5
            {
                //if (ax3d.Angle < 60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d + 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) > Math.Abs(offsetY) && offsetX > 0 && offsetY < 0) // 6
            {
                //if (ax3d.Angle < 60)
                {
                    ax3d.Axis = new Vector3D(0, 1, 0);
                    ax3d.Angle = lastAx3d + 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX > 0 && offsetY < 0) // 7
            {
                //if (ax3d.Angle > -40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d - 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX < 0 && offsetY < 0) // 8
            {
                //if (ax3d.Angle > -40)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d - 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
        }

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
        #endregion
    }
}
