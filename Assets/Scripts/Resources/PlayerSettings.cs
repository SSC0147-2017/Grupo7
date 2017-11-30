using System;
using Spaceships.Util;

namespace Spaceships.Resources {
    [Serializable]
    public class PlayerSettings {
        public string Name;
        public int Controller;
        public Team Team;
    }
}