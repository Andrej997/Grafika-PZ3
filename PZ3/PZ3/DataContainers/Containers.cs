using PZ2.Model;
using System.Collections.Generic;

namespace PZ2.DataContainers
{
    public class Containers
    {
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
