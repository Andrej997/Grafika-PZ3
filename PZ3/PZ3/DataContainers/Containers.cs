using PZ3.Model;
using System.Collections.Generic;

namespace PZ3.DataContainers
{
    public class Containers
    {
        /// <summary>
        /// Donji levi ugao latitude (min X)
        /// </summary>
        public static double DLLat { get => 45.2325; }
        /// <summary>
        /// Donji levi ugao longlitude (max X)
        /// </summary>
        public static double DLLon { get => 19.793909; }

        /// <summary>
        /// Gornji desni ugao latitude (min Z)
        /// </summary>
        public static double GDLat { get => 45.277031; }
        /// <summary>
        /// Gornji desni ugao longlitude (max Z)
        /// </summary>
        public static double GDLon { get => 19.894459; }

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

        public static HashSet<System.Windows.Point> pointsOfAllEntities = new HashSet<System.Windows.Point>();
    }
}
