using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Win_Test
{
    //远程驱动命令类
    public class ServerCommand
    {
        //[DataMember]
        public string QUERY_SYSTEM { get; set; }
        //正在控制/查询的子系统的ID
        //[DataMember]
        public int DATA_TYPE { get; set; }
        //数据类型：1-流量，2-运动参数，3-三维料堆，4-安全信息，5-任务规划信息，6-远程驱动系统交互
        //[DataMember]
        public int QUERY_TYPE { get; set; }
        //命令类型：0-提供给本地服务器端以初始化的数据，1-PLC的一帧数据，2-命令模式
        //[DataMember]
        public string COMMAND_NAME { get; set; }
        //命令名：该名字即为“基本指令表.xlsx”中的“英文指令名”
        //[DataMember]
        public string DATA_STRING { get; set; }
        //数据字符串：用于存放向其他系统转发的内容（暂无实现）
        //[DataMember]
        public int DATA_INT { get; set; }
        //数据整型：0-False，1-True
        //[DataMember]
        public float DATA_FLOAT { get; set; }
        //数据浮点型：度数，深度
    }
}