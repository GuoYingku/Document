using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenYangRemoteSystem.Subclass
{
    public class SystemVariables
    {
        public DateTime TimeStamp { get; set; }

        public bool MaterialStackerRotationSpeedButton { get; set; }
        public bool MaterialFeederRotationSpeedButton { get; set; }
        public bool MaterialFeederPitchSpeedButton { get; set; }
        public int MaterialStackerRotationSpeed { get; set; }
        public int MaterialFeederRotationSpeed { get; set; }
        public int MaterialFeederPitchSpeed { get; set; }


        //ID1
        public bool ManualSampling { get; set; } // 手动取料
        public bool AutoSamplingStopped { get; set; } // 自动取料停止
        public bool ManualMaterialStacking { get; set; } // 手动堆料
        public bool Motor2 { get; set; } // 电机2
        public bool ScraperStarted { get; set; } // 刮板启动
        public bool Motor3 { get; set; } // 电机3
        public bool Motor4 { get; set; } // 电机4
        public bool Motor1 { get; set; } // 电机1
        public bool AutoMaterialStackingStopped { get; set; } // 自动堆料停止
        public bool ScraperStopped { get; set; } // 刮板停止
        public bool AutoMaterialStacking { get; set; } // 自动堆料
        public bool AutoSampling { get; set; } // 自动取料


        //441
        public bool PLCSignal { get; set; }
    }
}
