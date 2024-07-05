using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ShenYangRemoteSystem.Subclass
{
    public class ControlHelper
    {
        public ControlHelper(UserControl userControl) 
        {
            uc = userControl;
        
        }

        /// <summary>
        /// 要使用本库的用户控件类对象
        /// </summary>
        public UserControl uc;

        /// <summary>
        /// 全局变量类对象——以Form1为例
        /// </summary>
        public SystemVariables variables = Form1.systemVariables;



        #region 通用Image显示方法（True == 红色）
        /// <summary>
        /// 通用Image显示方法（当变量为True时显示红色图片）
        /// </summary>
        /// <param name="uc">用户控件类对象</param>
        /// <param name="name">控件名</param>
        /// <param name="variableName">变量名</param>
        public void UpdateRedByVariable(string name, string variableName)
        {
            Control[] controls = uc.Controls.Find(name, true);

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                bool? variableValue = GetVariableBool(variableName);

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
        #endregion

        #region 通用Image显示方法（True == 绿色）
        /// <summary>
        /// 通用Image显示方法（当变量为True时显示绿色图片）
        /// </summary>
        /// <param name="uc">用户控件类对象</param>
        /// <param name="name">控件名</param>
        /// <param name="variableName">变量名</param>
        public void UpdateGreenByVariable(string name, string variableName)
        {
            Control[] controls = uc.Controls.Find(name, true);

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                bool? variableValue = GetVariableBool(variableName);

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
        #endregion

        #region 通用整型更新方法
        /// <summary>
        /// 通用长整型更新方法
        /// </summary>
        /// <param name="uc">用户控件类对象</param>
        /// <param name="name">控件名</param>
        /// <param name="variableName">变量名</param>
        public void UpdateIntByVariable(string name, string variableName)
        {
            if (uc.InvokeRequired)
            {
                uc.BeginInvoke((MethodInvoker)(() => UpdateIntByVariable(name, variableName)));
                return;
            }

            Control[] controls = uc.Controls.Find(name, true); // 查找名为 name 的控件

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                object variableValueObject = GetVariableObject(variableName);

                if (variableValueObject != null)
                {
                    int variableValue = (int)variableValueObject;

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

        #region 通用长整型更新方法
        /// <summary>
        /// 通用长整型更新方法
        /// </summary>
        /// <param name="uc">用户控件类对象</param>
        /// <param name="name">控件名</param>
        /// <param name="variableName">变量名</param>
        public void UpdateLongByVariable(string name, string variableName)
        {
            if (uc.InvokeRequired)
            {
                uc.BeginInvoke((MethodInvoker)(() => UpdateLongByVariable(name, variableName)));
                return;
            }

            Control[] controls = uc.Controls.Find(name, true); // 查找名为 name 的控件

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                object variableValueObject = GetVariableObject(variableName);

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

        #region 通用浮点型更新方法
        /// <summary>
        /// 通用浮点型更新方法
        /// </summary>
        /// <param name="uc">用户控件类对象</param>
        /// <param name="name">控件名</param>
        /// <param name="variableName">变量名</param>
        public void UpdateFloatByVariable(string name, string variableName)
        {
            if (uc.InvokeRequired)
            {
                uc.BeginInvoke((MethodInvoker)(() => UpdateFloatByVariable(name, variableName)));
                return;
            }

            Control[] controls = uc.Controls.Find(name, true); // 查找名为 name 的控件

            if (controls.Length > 0)
            {
                Control targetControl = controls[0];

                object variableValueObject = GetVariableObject(variableName);

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

        #region 通用类型返回方法
        public object GetVariableObject(string variableName)
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

        #region 通用布尔值返回方法
        private bool? GetVariableBool(string variableName)
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
