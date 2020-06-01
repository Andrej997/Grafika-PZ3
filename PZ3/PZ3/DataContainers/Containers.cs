using PZ2.Model;
using System.Collections.Generic;

namespace PZ2.DataContainers
{
    public class Containers
    {
        /// <summary>
        /// Donji levi ugao latitude
        /// </summary>
        public double DLLat { get => 45.2325; }
        /// <summary>
        /// Donji levi ugao longlitude
        /// </summary>
        public double DLLon { get => 19.793909; }

        /// <summary>
        /// Gornji desni ugao latitude
        /// </summary>
        public double GDLat { get => 45.277031; }
        /// <summary>
        /// Gornji desni ugao longlitude
        /// </summary>
        public double GDLon { get => 19.894459; }

        protected static HashSet<SubstationEntity> substationEntities = new HashSet<SubstationEntity>(); // 67
        public static HashSet<SubstationEntity> GetSubstations => substationEntities;

        protected static HashSet<NodeEntity> nodeEntities = new HashSet<NodeEntity>(); // 2043
        public static HashSet<NodeEntity> GetNodes => nodeEntities;

        protected static HashSet<SwitchEntity> switchEntities = new HashSet<SwitchEntity>(); // 2282
        public static HashSet<SwitchEntity> GetSwitches => switchEntities;

        protected static HashSet<LineEntity> lineEntities = new HashSet<LineEntity>(); // 2336
        public static HashSet<LineEntity> GetLines => lineEntities;

        protected static HashSet<Point> pointsFromLines = new HashSet<Point>(); // 8747
        public static HashSet<Point> GetPoints => pointsFromLines;
    }
}
