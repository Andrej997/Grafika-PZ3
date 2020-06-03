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
        private int zoomOutMax = 10;
        private int zoomCurent = 1;
        private int zoomInMax = 8;

        public Point3DCollection Positions { get; private set; }
        public Int32Collection Indicies { get; private set; }

        // lista svih modela
        private HashSet<GeometryModel3D> models = new HashSet<GeometryModel3D>();
        // model koji smo pogodili
        private GeometryModel3D hitgeo;

        AxisAngleRotation3D ax3d;

        public MainWindow()
        {
            InitializeComponent();

            ax3d = new AxisAngleRotation3D(new Vector3D(0, 0, 0), 0 /* inicijalno da ne bude pomerena slika*/);
            RotateTransform3D myRotateTransform = new RotateTransform3D(ax3d);
            MyModel.Transform = myRotateTransform;

            #region Load all data
            Controller.XMLLoader.LoadXml();
            #endregion

            #region Draw all data on map
            DrawNodeEntity(DataContainers.Containers.GetNodes);
            DrawSwitchEntity(DataContainers.Containers.GetSwitches);
            DrawSubstationEntity(DataContainers.Containers.GetSubstations);
            DrawLineEntities();
            #endregion
        }

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
                toolTip.Content = $"Node\n" +
                                $"Id: {item.Id}\n" +
                                $"Name: {item.Name}";

                int connections = Controller.CheckInData.AdditionalTask8NO(item);

                System.Windows.Point point = Controller.CheckInData.FindPointOnMap(latitude, longitude);

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
                toolTip.Content = $"Switch\n" +
                                    $"Id: {item.Id}\n" +
                                    $"Name: {item.Name}" +
                                    $"Status: {item.Status}";

                int connections = Controller.CheckInData.AdditionalTask8SW(item);

                System.Windows.Point point = Controller.CheckInData.FindPointOnMap(latitude, longitude);

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
                toolTip.Content = $"Substation\n" +
                                    $"Id: {item.Id}\n" +
                                    $"Name: {item.Name}";

                int connections = Controller.CheckInData.AdditionalTask8SU(item);

                System.Windows.Point point = Controller.CheckInData.FindPointOnMap(latitude, longitude);

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
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), 0, point.Y),
                    new Point3D(-point.X, 0, point.Y + DataContainers.Containers.cubeSize),
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), 0, point.Y + DataContainers.Containers.cubeSize),
                    new Point3D(-point.X, DataContainers.Containers.cubeSize, point.Y),
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), DataContainers.Containers.cubeSize, point.Y),
                    new Point3D(-(point.X ), DataContainers.Containers.cubeSize,  point.Y + DataContainers.Containers.cubeSize),
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), DataContainers.Containers.cubeSize,  point.Y + DataContainers.Containers.cubeSize),
                };
            else if (lvl == 1)
                Positions = new Point3DCollection()
                {
                    new Point3D(-point.X , DataContainers.Containers.up, point.Y),
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), DataContainers.Containers.up, point.Y),
                    new Point3D(-point.X, DataContainers.Containers.up, point.Y + DataContainers.Containers.cubeSize),
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), DataContainers.Containers.up, point.Y + DataContainers.Containers.cubeSize),
                    new Point3D(-point.X, DataContainers.Containers.cubeSize + DataContainers.Containers.up, point.Y),
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), DataContainers.Containers.cubeSize + DataContainers.Containers.up, point.Y),
                    new Point3D(-(point.X ), DataContainers.Containers.cubeSize + DataContainers.Containers.up,  point.Y + DataContainers.Containers.cubeSize),
                    new Point3D(-(point.X + DataContainers.Containers.cubeSize), DataContainers.Containers.cubeSize + DataContainers.Containers.up,  point.Y + DataContainers.Containers.cubeSize),
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

                    System.Windows.Point point = Controller.CheckInData.FindPointOnMap(latitude, longitude);
                    points.Add(point);
                }

                ToolTip toolTip = new System.Windows.Controls.ToolTip();
                toolTip.Content = $"Line\n" +
                                    $"Id: {line.Id}\n" +
                                    $"Name: {line.Name}";

                DrawLine(points, toolTip, line);
            }
        }

        private void DrawLine(HashSet<System.Windows.Point> points, ToolTip tt, LineEntity line)
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
                    new Point3D(-(points.ElementAt(i).X + DataContainers.Containers.lineSize), 0, points.ElementAt(i).Y),
                    new Point3D(-points.ElementAt(i).X, 0, points.ElementAt(i).Y + DataContainers.Containers.lineSize),
                    new Point3D(-(points.ElementAt(i).X + DataContainers.Containers.lineSize), 0, points.ElementAt(i).Y + DataContainers.Containers.lineSize),
                    new Point3D(-points.ElementAt(i + 1).X, DataContainers.Containers.lineSize, points.ElementAt(i + 1).Y),
                    new Point3D(-(points.ElementAt(i + 1).X + DataContainers.Containers.lineSize), DataContainers.Containers.lineSize, points.ElementAt(i + 1).Y),
                    new Point3D(-points.ElementAt(i + 1).X, DataContainers.Containers.lineSize, points.ElementAt(i + 1).Y + DataContainers.Containers.lineSize),
                    new Point3D(-(points.ElementAt(i + 1).X + DataContainers.Containers.lineSize), DataContainers.Containers.lineSize, points.ElementAt(i + 1).Y + DataContainers.Containers.lineSize),
                };

                GeometryModel3D firstObj = new GeometryModel3D();
                firstObj.Material = new DiffuseMaterial(Brushes.Black);
                firstObj.Geometry = new MeshGeometry3D() { Positions = Positions, TriangleIndices = Indicies };
                firstObj.SetValue(IsToolTip, tt);
                firstObj.SetValue(IsLine, line);

                Map.Children.Add(firstObj);
                models.Add(firstObj);
            }

        }
        #endregion

        #region ToolTip
        public static readonly DependencyProperty IsToolTip = DependencyProperty.RegisterAttached("ToolTip", typeof(ToolTip), typeof(GeometryModel3D));
        public static void SetIsToolTip(GeometryModel3D element, ToolTip value) => element.SetValue(IsToolTip, value);
        public static ToolTip GetIsToolTip(GeometryModel3D element) => (ToolTip)element.GetValue(IsToolTip);
        #endregion

        #region Line
        public static readonly DependencyProperty IsLine = DependencyProperty.RegisterAttached("Line", typeof(LineEntity), typeof(GeometryModel3D));
        public static void SetIsLine(GeometryModel3D element, LineEntity value) => element.SetValue(IsLine, value);
        public static LineEntity GetIsLine(GeometryModel3D element) => (LineEntity)element.GetValue(IsLine);
        #endregion

        #region Mouse Actions
        private void viewport1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Point p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleZ = 1;
            if (e.Delta > 0 && zoomCurent < zoomOutMax)
            {
                scaleX = skaliranje.ScaleX + 0.1;
                scaleZ = skaliranje.ScaleZ + 0.1;
                zoomCurent++;
                skaliranje.ScaleX = scaleX;
                skaliranje.ScaleZ = scaleZ;
            }
            else if (e.Delta <= 0 && zoomCurent > -zoomOutMax)
            {
                if (zoomCurent > -zoomInMax)
                {
                    scaleX = skaliranje.ScaleX - 0.1;
                    scaleZ = skaliranje.ScaleZ - 0.1;
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
                // moze da legne (ide samo do 90 stepen)
                if (ax3d.Angle < 90)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d + 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX > 0 && offsetY > 0) // 4
            {
                // moze da legne (ide samo do 90 stepen)
                if (ax3d.Angle < 90)
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
                // moze da legne "na stomak"
                if (ax3d.Angle > -90)
                {
                    ax3d.Axis = new Vector3D(1, 0, 0);
                    ax3d.Angle = lastAx3d - 0.5;
                    lastAx3d = ax3d.Angle;
                }
            }
            if (Math.Abs(offsetX) < Math.Abs(offsetY) && offsetX < 0 && offsetY < 0) // 8
            {
                // moze da legne "na stomak"
                if (ax3d.Angle > -90)
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
            //if (e.MiddleButton == MouseButtonState.Pressed)
            //{
            //    viewport1.CaptureMouse();
            //    start = e.GetPosition(this);
            //    diffOffset.X = translacija.OffsetX;
            //    diffOffset.Y = translacija.OffsetY;
            //}

            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
                viewport1.CaptureMouse();
                start = e.GetPosition(this);
                diffOffset.X = translacija.OffsetX;
                diffOffset.Y = translacija.OffsetY;
            //}

            #region HitTest
            System.Windows.Point mouseposition = e.GetPosition(viewport1);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);

            PointHitTestParameters pointparams =
                     new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams =
                     new RayHitTestParameters(testpoint3D, testdirection);

            //test for a result in the Viewport3D     
            hitgeo = null;
            VisualTreeHelper.HitTest(viewport1, null, HTResult, pointparams);
            #endregion
        }

        private HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;
            ToolTip toolTip = new ToolTip();
            LineEntity line = new LineEntity();

            if (rayResult != null)
            {
                bool gasit = false;
                foreach (var model in models)
                {
                    if (model == rayResult.ModelHit)
                    {
                        hitgeo = (GeometryModel3D)rayResult.ModelHit;
                        gasit = true;
                        toolTip = GetIsToolTip(model);
                        MessageBox.Show(toolTip.Content.ToString());

                        line = GetIsLine(model);
                        if (line != null)
                        {
                            NodeEntity node1;
                            NodeEntity node2;
                            for (int i = 0; i < DataContainers.Containers.GetNodes.Count; i++)
                            {
                                if (DataContainers.Containers.GetNodes.ElementAt(i).Id == line.FirstEnd)
                                    node1 = DataContainers.Containers.GetNodes.ElementAt(i);
                                if (DataContainers.Containers.GetNodes.ElementAt(i).Id == line.SecondEnd)
                                    node2 = DataContainers.Containers.GetNodes.ElementAt(i);
                            }
                            SwitchEntity switch1;
                            SwitchEntity switch2;
                            for (int i = 0; i < DataContainers.Containers.GetSwitches.Count; i++)
                            {
                                if (DataContainers.Containers.GetSwitches.ElementAt(i).Id == line.FirstEnd)
                                    switch1 = DataContainers.Containers.GetSwitches.ElementAt(i);
                                if (DataContainers.Containers.GetSwitches.ElementAt(i).Id == line.SecondEnd)
                                    switch2 = DataContainers.Containers.GetSwitches.ElementAt(i);
                            }
                            SubstationEntity substation1;
                            SubstationEntity substation2;
                            for (int i = 0; i < DataContainers.Containers.GetSubstations.Count; i++)
                            {
                                if (DataContainers.Containers.GetSubstations.ElementAt(i).Id == line.FirstEnd)
                                    substation1 = DataContainers.Containers.GetSubstations.ElementAt(i);
                                if (DataContainers.Containers.GetSubstations.ElementAt(i).Id == line.SecondEnd)
                                    substation2 = DataContainers.Containers.GetSubstations.ElementAt(i);
                            }
                        }
                    }
                }
                if (!gasit)
                {
                    hitgeo = null;
                }
            }
            
            return HitTestResultBehavior.Stop;
        }

        /// <summary>
        /// Pustanje dugmeta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewport1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //if (e.MiddleButton == MouseButtonState.Released)
            //{
            //    viewport1.ReleaseMouseCapture();
            //}
            //if (e.LeftButton == MouseButtonState.Released)
            //{
                viewport1.ReleaseMouseCapture();
            //}
        }
        #endregion
    }
}
