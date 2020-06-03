using PZ3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Controller
{
    public class CheckInData
    {
        #region Dodatni zadatak 8
        public static int AdditionalTask8NO(NodeEntity item)
        {
            int counter = 0;
            foreach (var line in DataContainers.Containers.GetLines)
            {
                if (line.FirstEnd == item.Id || line.SecondEnd == item.Id)
                {
                    ++counter;
                }
            }
            return counter;
        }

        public static int AdditionalTask8SW(SwitchEntity item)
        {
            int counter = 0;
            foreach (var line in DataContainers.Containers.GetLines)
            {
                if (line.FirstEnd == item.Id || line.SecondEnd == item.Id)
                {
                    ++counter;
                }
            }
            return counter;
        }

        public static int AdditionalTask8SU(SubstationEntity item)
        {
            int counter = 0;
            foreach (var line in DataContainers.Containers.GetLines)
            {
                if (line.FirstEnd == item.Id || line.SecondEnd == item.Id)
                {
                    ++counter;
                }
            }
            return counter;
        }
        #endregion

        #region Does it match?
        public static int DaLiPOstoji(System.Windows.Point point)
        {
            foreach (var pointEntity in DataContainers.Containers.pointsOfAllEntities)
            {
                var absoluteX = Math.Abs(point.X - pointEntity.X);
                var absoluteY = Math.Abs(point.Y - pointEntity.Y);
                if (absoluteX < 0.0000010 || absoluteY < 0.0000010)
                {
                    return 1;
                }
            }

            DataContainers.Containers.pointsOfAllEntities.Add(point);
            return 0;
        }
        #endregion

        #region Find location on map
        public static System.Windows.Point FindPointOnMap(double X, double Y) => new System.Windows.Point(
                (Y - DataContainers.Containers.DLLon) / (DataContainers.Containers.GDLon - DataContainers.Containers.DLLon) * (DataContainers.Containers.mapSize - DataContainers.Containers.cubeSize),
                (X - DataContainers.Containers.DLLat) / (DataContainers.Containers.GDLat - DataContainers.Containers.DLLat) * (DataContainers.Containers.mapSize - DataContainers.Containers.cubeSize)
            );

        #endregion
    }
}
