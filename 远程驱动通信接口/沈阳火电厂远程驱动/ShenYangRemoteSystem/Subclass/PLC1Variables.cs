using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenYangRemoteSystem.Subclass
{
    public class PLC1Variables
    {
        public DateTime TimeStamp { get; set; }

        public bool MaterialStackerRotationSpeedButton { get; set; }
        public bool MaterialFeederRotationSpeedButton { get; set; }
        public bool MaterialFeederPitchSpeedButton { get; set; }
        public int MaterialStackerRotationSpeed { get; set; }
        public int MaterialFeederRotationSpeed { get; set; }
        public int MaterialFeederPitchSpeed { get; set; }
    }
}
