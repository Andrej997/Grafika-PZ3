using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Point = PZ2.Model.Point;

using PZ2.DataContainers;
using System.Windows.Shapes;

namespace PZ2.Controller
{
    /// <summary>
    /// nasledio sam je, jer sam stavio da 
    /// liste budu protekted kako ne bi moglo
    /// da se pristupi iz neke druge klase.
    /// </summary>
    public class XMLLoader : Containers 
    {
        private static XmlDocument xmlDoc;

        /// <summary>
        /// tacka najbliza koordinatnom pocetku
        /// </summary>
        private static double closestX;
        private static double closestY;

        /// <summary>
        /// tacka najdalja koordinatnom pocetku
        /// </summary>
        private static double farthestX;
        private static double farthestY;

        /// <summary>
        /// Automaticly load Geographic.xml
        /// </summary>
        public static void LoadXml()
        {
            #region Path to .xml
            xmlDoc = new XmlDocument();
            xmlDoc.Load("../../GeographicData/Geographic.xml");
            XmlNodeList nodeList;
            #endregion

            #region Substations
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            LoadSubstationEntities(nodeList);
            #endregion 

            #region Nodes
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            LoadNodeEntities(nodeList);
            #endregion

            #region Switches
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            LoadSwitcheEntities(nodeList);
            #endregion

            #region Lines
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            LoadLineEntities(nodeList);
            #endregion
        }

        #region Entity Loaders
        private static void LoadSubstationEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                SubstationEntity sub = new SubstationEntity();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                substationEntities.Add(sub);
            }
        }
        private static void LoadNodeEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                NodeEntity n = new NodeEntity();
                n.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                n.Name = node.SelectSingleNode("Name").InnerText;
                n.X = double.Parse(node.SelectSingleNode("X").InnerText);
                n.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                nodeEntities.Add(n);
            }
        }
        private static void LoadSwitcheEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                SwitchEntity s = new SwitchEntity();
                s.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                s.Name = node.SelectSingleNode("Name").InnerText;
                s.Status = node.SelectSingleNode("Status").InnerText;
                s.X = double.Parse(node.SelectSingleNode("X").InnerText);
                s.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                switchEntities.Add(s);
            }
        }
        private static void LoadLineEntities(XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                LineEntity l = new LineEntity();
                l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                l.Name = node.SelectSingleNode("Name").InnerText;
                if (node.SelectSingleNode("IsUnderground").InnerText.Equals("true"))
                {
                    l.IsUnderground = true;
                }
                else
                {
                    l.IsUnderground = false;
                }
                l.R = float.Parse(node.SelectSingleNode("R").InnerText);
                l.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                l.LineType = node.SelectSingleNode("LineType").InnerText;
                l.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);
                l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);

                lineEntities.Add(l);

                foreach (XmlNode pointNode in node.ChildNodes[9].ChildNodes) // 9 posto je Vertices 9. node u jednom line objektu
                {
                    Point p = new Point();

                    p.X = double.Parse(pointNode.SelectSingleNode("X").InnerText);
                    p.Y = double.Parse(pointNode.SelectSingleNode("Y").InnerText);

                    pointsFromLines.Add(p);
                }
            }
        }
        #endregion

        #region Get closest and farthest point from (0,0)
        private static void GetClosestPoint(out double closestX, out double closestY)
        {
            closestX = 1000;
            closestY = 1000;
            double latitude;
            double longitude;
            foreach (var item in substationEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude < closestX)
                    closestX = latitude;
                if (longitude < closestY)
                    closestY = longitude;
            }
            foreach (var item in nodeEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude < closestX)
                    closestX = latitude;
                if (longitude < closestY)
                    closestY = longitude;
            }
            foreach (var item in switchEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude < closestX)
                    closestX = latitude;
                if (longitude < closestY)
                    closestY = longitude;
            }
        }
        private static void GetFarthestPoint(out double farthestX, out double farthestY)
        {
            farthestX = 0;
            farthestY = 0;
            double latitude;
            double longitude;
            foreach (var item in substationEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude > farthestX)
                    farthestX = latitude;
                if (longitude > farthestY)
                    farthestY = longitude;
            }
            foreach (var item in nodeEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude > farthestX)
                    farthestX = latitude;
                if (longitude > farthestY)
                    farthestY = longitude;
            }
            foreach (var item in switchEntities)
            {
                ToLatLon(item.X, item.Y, 34, out latitude, out longitude);
                if (latitude > farthestX)
                    farthestX = latitude;
                if (longitude > farthestY)
                    farthestY = longitude;
            }
        }
        #endregion
        
        #region Converter
        //From UTM to Latitude and longitude in decimal
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
        #endregion
    }
}
