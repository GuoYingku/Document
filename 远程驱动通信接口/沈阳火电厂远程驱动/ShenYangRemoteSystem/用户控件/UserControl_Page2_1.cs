using ShenYangRemoteSystem.Subclass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShenYangRemoteSystem.用户控件
{
    public partial class UserControl_Page2_1 : UserControl
    {
        public UserControl_Page2_1()
        {
            InitializeComponent();
        }

        private void UserControl_Page2_1_Load(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(Uc2_1LifeProcess));

            thread1.Start();

            connectedThreads.Add(thread1); // 将新连接的线程添加到列表中

            // 如果线程数超过一个，则依据FIFO关闭线程
            if (connectedThreads.Count > 1)
            {
                Thread earliestThread = connectedThreads[0];
                connectedThreads.RemoveAt(0);
                try
                {
                    earliestThread.Abort();
                }
                catch { }
            }

            controlHelper = new ControlHelper(this);// 初始化控件帮助类对象
        }


        public ControlHelper controlHelper;


        List<Thread> connectedThreads = new List<Thread>(); // 用于存储已连接的线程


        //图片更新进程
        private void Uc2_1LifeProcess()
        {
            while (true)
            {
                SystemVariables systemVariables = Form1.systemVariables;//此处没有使用局部变量表示的全局变量类对象，而是使用了ControlHelper类中实例化的全局变量类对象，这里先观察以下可不可行，如果可行的话以后都用这种方案。

                Thread.Sleep(1000);

                
                //系统状态
                controlHelper.UpdateRedByVariable("pictureBox88", "MaterialStackerRotationSpeedButton");

                controlHelper.UpdateIntByVariable("label96", "MaterialStackerRotationSpeed");
            }
        }

        #region 指示灯显示方法



        #region PLC1变量用
        //控件图片更新方法（Ture == 红色）
        public void UpdateRedByVariableForPLC1(string name, string variableName, PLC1Variables variables)
        {
            Control[] controls = this.Controls.Find(name, true);

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                bool? variableValue = GetVariableBool(variableName, variables);

                if (variableValue == true)
                {
                    targetControl.BackgroundImage = Properties.Resources.RedSquare;
                }
                else if (variableValue == false)
                {
                    targetControl.BackgroundImage = Properties.Resources.GreenSquare;
                }
            }
        }
        
        //控件图片更新方法（Ture == 绿色）
        public void UpdateGreenByVariableForPLC1(string name, string variableName, PLC1Variables variables)
        {
            Control[] controls = this.Controls.Find(name, true);

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                bool? variableValue = GetVariableBool(variableName, variables);

                if (variableValue == true)
                {
                    targetControl.BackgroundImage = Properties.Resources.GreenSquare;
                }
                else if (variableValue == false)
                {
                    targetControl.BackgroundImage = Properties.Resources.RedSquare;
                }
            }
        }

        //变量长整型更新方法
        public void UpdateLongByVariableForPLC1(string name, string variableName, PLC1Variables variables)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() => UpdateLongByVariableForPLC1(name, variableName, variables)));
                return;
            }

            Control[] controls = this.Controls.Find(name, true); // 查找名为 name 的控件

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                object variableValueObject = GetVariableObject(variableName, variables);

                if (variableValueObject != null)
                {
                    long variableValue = (long)variableValueObject;

                    if (targetControl.InvokeRequired)
                    {
                        targetControl.BeginInvoke((MethodInvoker)(() => targetControl.Text = variableValue.ToString()));
                    }
                    else
                    {
                        targetControl.Text = variableValue.ToString(); // 将控件的值替换为指定属性值
                    }
                }
            }
        }

        #endregion



        #region PLC2变量用
        //控件图片更新方法（Ture == 红色）
        public void UpdateRedByVariableForPLC2(string name, string variableName, PLC2Variables variables)
        {
            Control[] controls = this.Controls.Find(name, true);

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                bool? variableValue = GetVariableBool2(variableName, variables);

                if (variableValue == true)
                {
                    targetControl.BackgroundImage = Properties.Resources.RedSquare;
                }
                else if (variableValue == false)
                {
                    targetControl.BackgroundImage = Properties.Resources.GreenSquare;
                }
            }
        }

        //变量浮点型更新方法
        public void UpdateFloatByVariableForPLC2(string name, string variableName, PLC2Variables variables)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() => UpdateFloatByVariableForPLC2(name, variableName, variables)));
                return;
            }

            Control[] controls = this.Controls.Find(name, true); // 查找名为 name 的控件

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                object variableValueObject = GetVariableObject2(variableName, variables);

                if (variableValueObject != null)
                {
                    float variableValue = (float)variableValueObject;

                    if (targetControl.InvokeRequired)
                    {
                        targetControl.BeginInvoke((MethodInvoker)(() => targetControl.Text = variableValue.ToString("0.00")));
                    }
                    else
                    {
                        targetControl.Text = variableValue.ToString("0.00"); // 将控件的值替换为指定属性值
                    }
                }
            }
        }

        #endregion



        #endregion

        #region 通用类型检测方法（PLC1）
        //类型检测方法
        private object GetVariableObject(string variableName, PLC1Variables variables)
        {
            if (string.IsNullOrEmpty(variableName) || variables == null)
            {
                return null;
            }

            // 获取属性信息
            var property = variables.GetType().GetProperty(variableName);

            if (property == null)
            {
                return null;
            }

            // 获取属性值
            var variableValue = property.GetValue(variables);

            // 返回与属性相同数据类型的值
            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Boolean:
                    return (bool)variableValue;
                case TypeCode.Byte:
                    return (byte)variableValue;
                case TypeCode.Char:
                    return (char)variableValue;
                case TypeCode.DateTime:
                    return (DateTime)variableValue;
                case TypeCode.Decimal:
                    return (decimal)variableValue;
                case TypeCode.Double:
                    return (double)variableValue;
                case TypeCode.Int16:
                    return (short)variableValue;
                case TypeCode.Int32:
                    return (int)variableValue;
                case TypeCode.Int64:
                    return (long)variableValue;
                case TypeCode.SByte:
                    return (sbyte)variableValue;
                case TypeCode.Single:
                    return (float)variableValue;
                case TypeCode.String:
                    return (string)variableValue;
                case TypeCode.UInt16:
                    return (ushort)variableValue;
                case TypeCode.UInt32:
                    return (uint)variableValue;
                case TypeCode.UInt64:
                    return (ulong)variableValue;
                default:
                    return null;
            }
        }
        #endregion

        #region 布尔类型检测方法（PLC1）
        //布尔类型检测方法
        private bool? GetVariableBool(string variableName, PLC1Variables variables)
        {
            if (string.IsNullOrEmpty(variableName) || variables == null)
            {
                return null;
            }

            var property = variables.GetType().GetProperty(variableName);

            if (property == null)
            {
                return null;
            }

            bool variableValue = (bool)property.GetValue(variables);

            return variableValue;
        }

        #endregion

        #region 通用类型检测方法（PLC2）
        private object GetVariableObject2(string variableName, PLC2Variables variables)
        {
            if (string.IsNullOrEmpty(variableName) || variables == null)
            {
                return null;
            }

            // 获取属性信息
            var property = variables.GetType().GetProperty(variableName);

            if (property == null)
            {
                return null;
            }

            // 获取属性值
            var variableValue = property.GetValue(variables);

            // 返回与属性相同数据类型的值
            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Boolean:
                    return (bool)variableValue;
                case TypeCode.Byte:
                    return (byte)variableValue;
                case TypeCode.Char:
                    return (char)variableValue;
                case TypeCode.DateTime:
                    return (DateTime)variableValue;
                case TypeCode.Decimal:
                    return (decimal)variableValue;
                case TypeCode.Double:
                    return (double)variableValue;
                case TypeCode.Int16:
                    return (short)variableValue;
                case TypeCode.Int32:
                    return (int)variableValue;
                case TypeCode.Int64:
                    return (long)variableValue;
                case TypeCode.SByte:
                    return (sbyte)variableValue;
                case TypeCode.Single:
                    return (float)variableValue;
                case TypeCode.String:
                    return (string)variableValue;
                case TypeCode.UInt16:
                    return (ushort)variableValue;
                case TypeCode.UInt32:
                    return (uint)variableValue;
                case TypeCode.UInt64:
                    return (ulong)variableValue;
                default:
                    return null;
            }
        }
        #endregion

        #region 布尔类型检测方法（PLC2）
        //布尔类型检测方法
        private bool? GetVariableBool2(string variableName, PLC2Variables variables)
        {
            if (string.IsNullOrEmpty(variableName) || variables == null)
            {
                return null;
            }

            var property = variables.GetType().GetProperty(variableName);

            if (property == null)
            {
                return null;
            }

            bool variableValue = (bool)property.GetValue(variables);

            return variableValue;
        }
        #endregion
    }
}
