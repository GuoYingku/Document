using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ShenYangRemoteSystem.用户控件;
using ShenYangRemoteSystem.Subclass;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Configuration;
using S7.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Diagnostics;
using System.Collections.Concurrent;
using Org.BouncyCastle.Ocsp;
using MySqlX.XDevAPI.Common;
//using Google.Protobuf.WellKnownTypes;

namespace ShenYangRemoteSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region 程序初始化

            systemCommand.QUERY_SYSTEM = "RC"; //本系统ID
            this.FormClosed += Form1_FormClosed; //主窗体退出事件
            //this.WindowState = FormWindowState.Minimized; //初始化窗体显示状态---最小化
            //this.ShowInTaskbar = false; //禁用（隐藏）任务栏图标

            try
            {
                websocketIp = File.ReadAllText("ipconfig.txt");
            }
            catch{ }


            Thread thread1 = new Thread(new ThreadStart(ConnectToDatabase));
            Thread thread2 = new Thread(new ThreadStart(Process_PLC_Main));
            Thread thread3 = new Thread(new ThreadStart(Process_D1PLC1_DataGet));
            Thread thread4 = new Thread(new ThreadStart(Process_D1PLC2_DataGet));
            Thread thread5 = new Thread(new ThreadStart(Process_D2PLC1_DataGet));
            Thread thread6 = new Thread(new ThreadStart(Process_D2PLC2_DataGet));
            Thread thread7 = new Thread(new ThreadStart(Process_SocketListening));


            //thread1.Start();
            thread2.Start();
            //thread3.Start();
            //thread4.Start();
            thread5.Start();
            thread6.Start();
            thread7.Start();


            UpdateClock(null, null); //初始化时间
            Page1Btn_Click(null, null); //初始化主界面


            #region UI处理业务
            // 将所有按钮添加到列表中
            buttons = new List<Button> { Page1Btn, Page2Btn, Page3Btn, Page5Btn };

            // 为每个按钮添加单击事件处理程序
            foreach (Button button in buttons)
            {
                button.Click += ChangeColor;
                button.Tag = button.ForeColor;
            }

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
            {
                Interval = 500 //Refresh rate 0.5s
            };
            timer.Tick += new EventHandler(UpdateClock);
            timer.Start();

            Page1Btn.ForeColor = Color.Yellow;
            Page1Btn.BackColor = Color.FromArgb(16, 78, 139);
            #endregion


            #region 键值对d1PLC1
            //1
            d1PLC1addresses.Add("LargeCarElectricCurrent", 40444);// Word
            d1PLC1addresses.Add("RotaryElectricCurrent", 40454);// Word
            d1PLC1addresses.Add("SuspensionBeltElectricCurrent", 40464);// Word
            d1PLC1addresses.Add("BucketWheelElectricCurrent", 40474);// Word
            d1PLC1addresses.Add("LargeCarTravelDistance", 40400);// Word
            d1PLC1addresses.Add("RotaryAngle", 40418);// Word
            d1PLC1addresses.Add("VariableAmplitudeAngle", 40428);// Word
            d1PLC1addresses.Add("RiseCount", 40500);// Word
            d1PLC1addresses.Add("RotaryCount", 40504);// Word
            d1PLC1addresses.Add("DiversionPlateAngle", 40406);// Word
            d1PLC1addresses.Add("TwoMachineDistance", 40484);// Word
            d1PLC1addresses.Add("DriverRoomAngle", 40498);// Word


            d1PLC1addresses.Add("VacuumCircuitBreakerClosed", 00100);
            d1PLC1addresses.Add("LowVoltageControlPowerClosed", 00102);
            d1PLC1addresses.Add("LowVoltagePowerClosed", 00103);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationLowOilLevel", 00105);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationOilBlockage", 00106);
            d1PLC1addresses.Add("AllowBucketWheelMaterialLoading", 00112);
            d1PLC1addresses.Add("AllowBucketWheelMaterialUnloading", 00113);
            d1PLC1addresses.Add("LargeCarMainCircuitBreaker", 00121);
            d1PLC1addresses.Add("LargeCarMotorCircuitBreaker", 00122);
            d1PLC1addresses.Add("LargeCarBrakeCircuitBreaker", 00123);
            d1PLC1addresses.Add("LargeCarFrequencyConverterContact", 00125);
            d1PLC1addresses.Add("LargeCarBrakeContact", 00126);
            d1PLC1addresses.Add("LargeCarFrequencyConverterFault", 00128);
            d1PLC1addresses.Add("LargeCarBrakeResistorOverheatSwitch", 00129);
            d1PLC1addresses.Add("LargeCarForwardLimit", 00130);
            d1PLC1addresses.Add("LargeCarReverseLimit", 00131);
            d1PLC1addresses.Add("LargeCarForwardExtremeLimit", 00132);
            d1PLC1addresses.Add("LargeCarReverseExtremeLimit", 00133);
            d1PLC1addresses.Add("CableReelMainCircuitBreaker", 00140);
            d1PLC1addresses.Add("CableReelMotorOverload", 00141);
            d1PLC1addresses.Add("PowerReelContact", 00142);
            d1PLC1addresses.Add("ReelOverTensionLimit1", 00200);
            d1PLC1addresses.Add("ReelOverLooseLimit1", 00201);
            d1PLC1addresses.Add("VibrationMotorMainCircuitBreaker", 00186);
            d1PLC1addresses.Add("RotaryBrakeOverload", 00163);
            d1PLC1addresses.Add("RotaryMainCircuitBreaker", 00162);
            d1PLC1addresses.Add("ClampMotorOverload", 00149);
            d1PLC1addresses.Add("LeftAnchorLiftLimit", 00150);
            d1PLC1addresses.Add("RightAnchorLiftLimit", 00151);
            d1PLC1addresses.Add("LeftClampRelaxLimit", 00152);
            d1PLC1addresses.Add("RightClampRelaxLimit", 00153);
            d1PLC1addresses.Add("BucketWheelMotorMainCircuitBreaker", 00171);
            d1PLC1addresses.Add("RotaryFanContact", 00167);
            d1PLC1addresses.Add("RotaryBrakeContact", 00166);
            d1PLC1addresses.Add("RotaryFrequencyConverterContact", 00165);
            d1PLC1addresses.Add("SystemInterlockSwitch", 00270);
            d1PLC1addresses.Add("VariableAmplitudeMainCircuitBreaker", 00180);
            d1PLC1addresses.Add("VariableAmplitudeMotorOverload", 00181);
            d1PLC1addresses.Add("VariableAmplitudeMotorContact", 00182);
            d1PLC1addresses.Add("VariableAmplitudeHeaterContact", 00183);
            d1PLC1addresses.Add("VariableAmplitudeFanContact", 00184);
            d1PLC1addresses.Add("SuspensionBeltMainCircuitBreaker", 00188);
            d1PLC1addresses.Add("SuspensionBeltMotorOverload", 00189);
            d1PLC1addresses.Add("SuspensionBeltMaterialLoadingRunningContact", 00190);
            d1PLC1addresses.Add("SuspensionBeltMaterialUnloadingRunningContact", 00191);
            d1PLC1addresses.Add("SuspensionBeltBrakeContact", 00192);
            d1PLC1addresses.Add("CentralMaterialDustDetectionSwitch", 00175);
            d1PLC1addresses.Add("DiversionBaffleMainCircuitBreaker", 00220);
            d1PLC1addresses.Add("VibrationMotorOverload", 00187);
            d1PLC1addresses.Add("ClampMainCircuitBreaker", 00148);
            d1PLC1addresses.Add("BucketWheelMotorOverload", 00172);
            d1PLC1addresses.Add("BucketWheelLubricationPumpContact", 00174);
            d1PLC1addresses.Add("RotaryLeftTurnLimit", 00304);
            d1PLC1addresses.Add("RotaryRightTurnLimit", 00305);
            d1PLC1addresses.Add("RotaryLeftTurnExtremeLimit", 00306);
            d1PLC1addresses.Add("RotaryRightTurnExtremeLimit", 00307);
            d1PLC1addresses.Add("RotaryLeftTurnForbiddenZoneLimit", 00308);
            d1PLC1addresses.Add("RotaryRightTurnForbiddenZoneLimit", 00309);
            d1PLC1addresses.Add("RotaryZeroPositionLimit", 00312);
            d1PLC1addresses.Add("BucketWheelOverTorqueSwitch", 00355);
            d1PLC1addresses.Add("BucketWheelForcedLubricationFlowSwitch", 00354);
            d1PLC1addresses.Add("VariableAmplitudeUpperLimit", 00360);
            d1PLC1addresses.Add("VariableAmplitudeLowerLimit", 00361);
            d1PLC1addresses.Add("VariableAmplitudeUpperExtremeLimit", 00362);
            d1PLC1addresses.Add("VariableAmplitudeLowerExtremeLimit", 00363);
            d1PLC1addresses.Add("VariableAmplitudeLowerForbiddenZoneLimit", 00364);
            d1PLC1addresses.Add("CabinFrontBalanceLimit", 00365);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterStartup", 00326);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterStop", 00327);
            d1PLC1addresses.Add("VariableAmplitudeFanStop", 00329);
            d1PLC1addresses.Add("VariableAmplitudeFanStartup", 00328);
            d1PLC1addresses.Add("VariableAmplitudeOilLevelLowSignal", 00332);
            d1PLC1addresses.Add("VariableAmplitudePumpStationOverheatAlarm", 00330);
            d1PLC1addresses.Add("VariableAmplitudeOilLevelVeryLowSignal", 00331);
            d1PLC1addresses.Add("RotaryCentralizedLubricationLowOilLevelFault", 00349);
            d1PLC1addresses.Add("LargeCarFrequencyConverterPowerOn", 00409);
            d1PLC1addresses.Add("LargeCarBrakeOpen", 00410);
            d1PLC1addresses.Add("LargeCarFrequencyConverterFaultReset", 00414);
            d1PLC1addresses.Add("LargeCarReverseCommand", 00413);
            d1PLC1addresses.Add("LargeCarHighLowSpeedSelection", 00415);
            d1PLC1addresses.Add("BucketWheelMaterialLoadingRunning", 00403);

            d1PLC1addresses.Add("BucketWheelFault", 00406);

            d1PLC1addresses.Add("SuspensionBeltFirstLevelDeviationSwitch", 00340);
            d1PLC1addresses.Add("SuspensionBeltSecondLevelDeviationSwitch", 00341);
            d1PLC1addresses.Add("SuspensionBeltEmergencyStopSwitch", 00342);
            d1PLC1addresses.Add("SuspensionBeltSpeedDetectionSwitch", 00343);
            d1PLC1addresses.Add("SuspensionBeltMaterialFlowDetectionSwitch", 00344);
            d1PLC1addresses.Add("SuspensionBeltLongitudinalTearSwitch", 00345);
            d1PLC1addresses.Add("RotaryCentralizedLubricationOilBlockageFault", 00350);
            d1PLC1addresses.Add("LargeCarForwardCommand", 00412);
            d1PLC1addresses.Add("VariableAmplitudeOilPumpMotorRunning", 00440);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterRunning", 00441);
            d1PLC1addresses.Add("VariableAmplitudeFanRunning", 00442);
            d1PLC1addresses.Add("LeftClampPumpRunning", 00423);
            d1PLC1addresses.Add("RightClampPumpRunning", 00424);
            d1PLC1addresses.Add("LeftClampElectromagneticValveOpen", 00425);
            d1PLC1addresses.Add("RightClampElectromagneticValveOpen", 00426);
            d1PLC1addresses.Add("RotaryFrequencyConverterPowerOn", 00427);
            d1PLC1addresses.Add("RotaryBrakeOpen", 00428);
            d1PLC1addresses.Add("RotaryLeftTurnCommand", 00429);
            d1PLC1addresses.Add("RotaryRightTurnCommand", 00430);
            d1PLC1addresses.Add("RotaryFrequencyConverterFaultReset", 00431);
            d1PLC1addresses.Add("RotarySpeedGivenSelection", 00432);
            d1PLC1addresses.Add("RotaryFanRunning", 00433);
            d1PLC1addresses.Add("VariableAmplitudeLowerElectromagneticValveOpen", 00445);



            d1PLC1addresses.Add("SingleAction", 0053);
            d1PLC1addresses.Add("LinkAction", 0054);
            d1PLC1addresses.Add("Automatic", 0055);
            d1PLC1addresses.Add("LargeCarFault", 001000);
            d1PLC1addresses.Add("LargeCarForwardLimiting", 001001);
            d1PLC1addresses.Add("LargeCarReverseLimiting", 001002);
            d1PLC1addresses.Add("AnchorClamp", 001003);
            d1PLC1addresses.Add("LargeCarForward", 001004);
            d1PLC1addresses.Add("LargeCarReverse", 001005);
            d1PLC1addresses.Add("RotaryFault", 001020);
            d1PLC1addresses.Add("RotaryLeftTurnLimiting", 001021);
            d1PLC1addresses.Add("RotaryRightTurnLimiting", 001022);
            d1PLC1addresses.Add("RotaryLeftTurn", 001024);
            d1PLC1addresses.Add("RotaryRightTurn", 001025);
            d1PLC1addresses.Add("VariableAmplitudeFault", 001040);
            d1PLC1addresses.Add("VariableAmplitudeUpperLimiting", 001041);
            d1PLC1addresses.Add("VariableAmplitudeLowerLimiting", 001042);
            d1PLC1addresses.Add("VariableAmplitudeUpper", 001044);
            d1PLC1addresses.Add("VariableAmplitudeLower", 001045);
            d1PLC1addresses.Add("SuspensionBeltFault", 001060);
            d1PLC1addresses.Add("SuspensionBeltManualLoading", 001063);
            d1PLC1addresses.Add("SuspensionBeltManualUnloading", 001064);
            d1PLC1addresses.Add("SuspensionBeltLinkLoading", 001065);
            d1PLC1addresses.Add("SuspensionBeltLinkUnloading", 001066);
            d1PLC1addresses.Add("BucketWheelFaulting", 001080);
            d1PLC1addresses.Add("BucketWheelSingleStartup", 001082);
            d1PLC1addresses.Add("BucketWheelLinkStartup", 001083);
            d1PLC1addresses.Add("ClampFault", 001100);
            d1PLC1addresses.Add("ClampRelax", 001103);
            d1PLC1addresses.Add("CentralBaffleFault", 001140);
            d1PLC1addresses.Add("TailCarBeltFault", 001160);
            d1PLC1addresses.Add("MaterialLevelMeter", 001503);
            d1PLC1addresses.Add("ManualIntervention", 002020);
            d1PLC1addresses.Add("InterventionRelease", 002021);
            d1PLC1addresses.Add("SuspensionBeltLoadingButton", 002000);
            d1PLC1addresses.Add("SuspensionBeltStopButton", 002001);
            d1PLC1addresses.Add("SuspensionBeltUnloadingButton", 002002);
            d1PLC1addresses.Add("BucketWheelStartupButton", 002003);
            d1PLC1addresses.Add("BucketWheelStopButton", 002004);



            d1PLC1addresses.Add("LeftAnchorNotLifted", 001700);
            d1PLC1addresses.Add("RightAnchorNotLifted", 001701);
            d1PLC1addresses.Add("ClampNotRelaxed", 001702);
            d1PLC1addresses.Add("LargeCarBrakeNotOpen", 001704);
            d1PLC1addresses.Add("LargeCarFrequencyConverterNotPowered", 001706);
            d1PLC1addresses.Add("LargeCarBrakeContactAuxiliaryFault", 001707);
            d1PLC1addresses.Add("LargeCarFrequencyConverterContactAuxiliaryFault", 001708);
            d1PLC1addresses.Add("RotaryFrequencyConverterNotPowered", 001720);
            d1PLC1addresses.Add("RotaryFrequencyConverterContactAuxiliaryFault", 001722);
            d1PLC1addresses.Add("RotaryBrakeContactAuxiliaryFault", 001723);
            d1PLC1addresses.Add("VariableAmplitudeOilPumpMotorNotRunning", 001730);
            d1PLC1addresses.Add("SuspensionBeltBrakeContactAuxiliaryFault", 001741);
            d1PLC1addresses.Add("SuspensionBeltLoadingContactAuxiliaryFault", 001742);
            d1PLC1addresses.Add("SuspensionBeltUnloadingContactAuxiliaryFault", 001743);
            d1PLC1addresses.Add("SuspensionBeltFirstLevelDeviation", 001750);
            d1PLC1addresses.Add("BucketWheelLubricationPumpContactAuxiliaryFault", 001751);
            d1PLC1addresses.Add("WindproofSystemCableLimit1", 00145);
            d1PLC1addresses.Add("RotaryFrequencyConverterFault", 00168);
            d1PLC1addresses.Add("RotaryFanOverload", 00164);
            d1PLC1addresses.Add("RotaryBrakeResistorOverheatSwitch", 00170);
            d1PLC1addresses.Add("DiversionBaffleMotorOverload", 00221);
            d1PLC1addresses.Add("TailCarFirstLevelDeviationSwitch", 00224);
            d1PLC1addresses.Add("TailCarSecondLevelDeviationSwitch", 00225);
            d1PLC1addresses.Add("TailCarEmergencyStopSwitch", 00226);
            d1PLC1addresses.Add("RotaryLeftTurnForbiddenLimit", 00310);
            d1PLC1addresses.Add("RotaryRightTurnForbiddenLimit", 00311);
            d1PLC1addresses.Add("BucketWheelMaterialUnloadingRunning", 00404);
            d1PLC1addresses.Add("VariableAmplitudeUpperElectromagneticValveOpen", 00444);
            d1PLC1addresses.Add("SuspensionBeltLoadingRunning", 00449);
            d1PLC1addresses.Add("SuspensionBeltUnloadingRunning", 00450);
            d1PLC1addresses.Add("SuspensionBeltBrakeOpen", 00451);
            d1PLC1addresses.Add("BucketWheelMotorRunning", 00465);
            d1PLC1addresses.Add("BucketWheelLubricationPumpRunning", 00466);
            d1PLC1addresses.Add("DiversionBaffleDownRunning", 00460);
            d1PLC1addresses.Add("DiversionBaffleUpRunning", 00461);
            d1PLC1addresses.Add("VibrationMotorRunning", 00454);
            d1PLC1addresses.Add("VariableAmplitudeBoostValveOpen", 00446);
            d1PLC1addresses.Add("BaffleDownLimit", 00322);
            d1PLC1addresses.Add("BaffleUpLimit", 00323);
            d1PLC1addresses.Add("RotaryOverTorque", 00380);
            d1PLC1addresses.Add("VariableAmplitudeOilPumpMotorContactFault", 001732);
            d1PLC1addresses.Add("BucketWheelMotorContactAuxiliaryFault", 001752);
            d1PLC1addresses.Add("TailCarOilPumpMotorContactAuxiliaryFault", 001753);
            d1PLC1addresses.Add("VibrationMotorFault", 001200);
            d1PLC1addresses.Add("ReelEmptySwitch", 00202);
            d1PLC1addresses.Add("WindproofSystemCableNotOpen", 001703);
            d1PLC1addresses.Add("LargeCarLimitAction", 001754);

            //201
            d1PLC1addresses.Add("RotaryLimitAction", 1755);
            d1PLC1addresses.Add("VariableAmplitudeLimitAction", 1756);
            d1PLC1addresses.Add("ForbiddenZoneLimitAction", 1757);
            d1PLC1addresses.Add("RotaryCrashSwitchAction", 1758);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationLowOilLevelAlarm", 1760);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationOilBlockageAlarm", 1761);
            d1PLC1addresses.Add("RotaryCentralizedLubricationLowOilLevelAlarm", 1762);
            d1PLC1addresses.Add("RotaryCentralizedLubricationOilBlockageAlarm", 1763);
            d1PLC1addresses.Add("StrongWindPreAlarm", 1764);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationLowOilLevelAlarm", 1765);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationOilBlockageAlarm", 1767);
            d1PLC1addresses.Add("ManualGuideSlotLiftButton", 2015);
            d1PLC1addresses.Add("ManualBucketWheelSlotStopButton", 2016);
            d1PLC1addresses.Add("ManualBucketWheelSlotDownButton", 2017);
            d1PLC1addresses.Add("CentralBaffleManualLiftButton", 2005);
            d1PLC1addresses.Add("CentralBaffleManualStopButton", 2006);
            d1PLC1addresses.Add("CentralBaffleManualDownButton", 2007);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterManualStartupButton", 2011);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterManualStopButton", 2012);
            d1PLC1addresses.Add("VariableAmplitudeFanManualStartupButton", 2013);
            d1PLC1addresses.Add("VariableAmplitudeFanManualStopButton", 2014);

            //222  Bit
            d1PLC1addresses.Add("ElectricRoomEmergencyStopButtonAction", 4005200);// Bit
            d1PLC1addresses.Add("CabinEmergencyStopButtonAction", 4005201);// Bit
            d1PLC1addresses.Add("EmergencyStopRelayNot", 4005202);// Bit
            d1PLC1addresses.Add("TransformerOverheatAlarm", 4005203);// Bit
            d1PLC1addresses.Add("ElectricRoomPLCModulePowerFault", 4005204);// Bit
            d1PLC1addresses.Add("CabinPLCModulePowerFault", 4005205);// Bit
            d1PLC1addresses.Add("ElectricRoomFireAlarm", 4005206);// Bit
            d1PLC1addresses.Add("CabinFireAlarm", 4005207);// Bit
            d1PLC1addresses.Add("SuspensionBeltEmergencyStop", 4005208);// Bit
            d1PLC1addresses.Add("TailCarBeltEmergencyStopSwitch", 4005209);// Bit
            d1PLC1addresses.Add("LargeCarMainCircuitBreakerFault", 4010000);// Bit
            d1PLC1addresses.Add("LargeCarMotorCircuitBreakerFault", 4010001);// Bit
            d1PLC1addresses.Add("LargeCarBrakeCircuitBreakerFault", 4010002);// Bit
            d1PLC1addresses.Add("Spare1", 4010003);// Bit
            d1PLC1addresses.Add("CarFrequencyConverterFault", 4010004);// Bit
            d1PLC1addresses.Add("LargeCarBrakeResistorOverheatJump", 4010005);// Bit
            d1PLC1addresses.Add("CableReelMainCircuitBreakerFault", 4010006);// Bit
            d1PLC1addresses.Add("CableReelMotorOverloading", 4010007);// Bit
            d1PLC1addresses.Add("PowerReelCableOverLooseAlarm", 4010008);// Bit
            d1PLC1addresses.Add("PowerReelCableOverTightAlarm", 4010009);// Bit
            d1PLC1addresses.Add("PowerReelFullDiskAlarm", 4010010);// Bit
            d1PLC1addresses.Add("PowerReelEmptyDiskAlarm", 4010011);// Bit
            d1PLC1addresses.Add("LargeCarOperationHandleFault", 4010012);// Bit
            d1PLC1addresses.Add("RotaryMainCircuitBreakerFault", 4012000);// Bit
            d1PLC1addresses.Add("RotaryBrakeOverloadAlarm", 4012001);// Bit
            d1PLC1addresses.Add("RotaryFanOverloadAlarm", 4012002);// Bit
            d1PLC1addresses.Add("RotaryFrequencyConverterFaulting", 4012003);// Bit
            d1PLC1addresses.Add("RotaryBrakeResistorOverheatSwitching", 4012004);// Bit
            d1PLC1addresses.Add("RotaryOverTorqueSwitch", 4012005);// Bit
            d1PLC1addresses.Add("ReversalHandleFault", 4012006);// Bit
            d1PLC1addresses.Add("LinkedBucketWheelNotRunning", 4012007);// Bit
            d1PLC1addresses.Add("VariableFrequencyMainCircuitBreakerFault", 4014000);// Bit
            d1PLC1addresses.Add("VariableFrequencyMotorOverload", 4014001);// Bit
            d1PLC1addresses.Add("VariableFrequencyPumpClogged", 4014005);// Bit
            d1PLC1addresses.Add("VariableFrequencyPumpStationHighTemperatureAlarm", 4014006);// Bit
            d1PLC1addresses.Add("VariableFrequencyOilTankLowLevelAlarm", 4014007);// Bit
            d1PLC1addresses.Add("Spare2", 4014008);// Bit
            d1PLC1addresses.Add("VariableFrequencyHandleFault", 4014009);// Bit
            d1PLC1addresses.Add("SuspendedBeltCircuitBreakerFault", 4016000);// Bit
            d1PLC1addresses.Add("SuspendedBeltMotorOverload", 4016001);// Bit
            d1PLC1addresses.Add("SuspendedBeltSecondLevelDeviationSwitch", 4016002);// Bit
            d1PLC1addresses.Add("SuspendedBeltEmergencyStop", 4016003);// Bit
            d1PLC1addresses.Add("SuspendedBeltSlip", 4016004);// Bit
            d1PLC1addresses.Add("SuspendedBeltLongitudinalTearSwitch", 4016005);// Bit
            d1PLC1addresses.Add("CentralHopperCloggedDetectionSwitch", 4016006);// Bit
            d1PLC1addresses.Add("StackingSwitchFault", 4016007);// Bit
            d1PLC1addresses.Add("CentralControlRoomNoStackingCommand", 4016008);// Bit
            d1PLC1addresses.Add("BucketWheelMotorMainCircuitBreakerFault", 4018000);// Bit
            d1PLC1addresses.Add("BucketWheelMotorOverloading", 4018001);// Bit
            d1PLC1addresses.Add("BucketWheelOverTorqueSwitching", 4018002);// Bit
            d1PLC1addresses.Add("BucketWheelTemperatureUpperLimitAlarm", 4018003);// Bit
            d1PLC1addresses.Add("ClampingDeviceMainCircuitBreakerFault", 4020000);// Bit
            d1PLC1addresses.Add("ClampingDeviceMotorOverload", 4020001);// Bit
            d1PLC1addresses.Add("LeftClampingDeviceTimeout", 4020002);// Bit
            d1PLC1addresses.Add("RightClampingDeviceTimeout", 4020003);// Bit
            d1PLC1addresses.Add("StrongWindAlarm", 4020004);// Bit
            d1PLC1addresses.Add("DryFogSystemLowAirPressure", 4022000);// Bit
            d1PLC1addresses.Add("DryFogSystemLowWaterPressure", 4022001);// Bit
            d1PLC1addresses.Add("DryFogSystemFilterClogged", 4022002);// Bit
            d1PLC1addresses.Add("DryFogSystemWaterTankLowLevel", 4022003);// Bit
            d1PLC1addresses.Add("DiversionPlateCircuitBreakerFault", 4024000);// Bit
            d1PLC1addresses.Add("DiversionPlateMotorOverload", 4024001);// Bit
            d1PLC1addresses.Add("DiversionPlateTimeout", 4024002);// Bit
            d1PLC1addresses.Add("CentralControlRoomNoStackingOrDiversionCommand", 4024003);// Bit
            d1PLC1addresses.Add("BucketWheelFeederCircuitBreakerFault", 4025000);// Bit
            d1PLC1addresses.Add("BucketWheelFeederMotorOverload", 4025001);// Bit
            d1PLC1addresses.Add("BucketWheelFeederTimeout", 4025002);// Bit
            d1PLC1addresses.Add("CentralControlRoomNoStackingUnloadingCommand", 4025003);// Bit
            d1PLC1addresses.Add("TailCarBeltFirstLevelDeviation", 4026000);// Bit
            d1PLC1addresses.Add("TailCarBeltSecondLevelDeviation", 4026001);// Bit
            d1PLC1addresses.Add("TailCarBeltLongitudinalTear", 4026002);// Bit
            d1PLC1addresses.Add("Spare3", 4026003);// Bit
            d1PLC1addresses.Add("VibrationMotorCircuitBreakerFault", 4028000);// Bit
            d1PLC1addresses.Add("VibrationMotorOverloading", 4028001);// Bit

            //296
            d1PLC1addresses.Add("DriverRoomEmergencyStopButton", 260);
            d1PLC1addresses.Add("ElectricalRoomEmergencyStopButton", 109);
            d1PLC1addresses.Add("EmergencyStopRelay", 110);
            d1PLC1addresses.Add("TwoMachineCollisionAlarm", 144);
            d1PLC1addresses.Add("RollerFullDiskSwitch", 203);
            d1PLC1addresses.Add("RollerMiddleSwitch", 204);
            d1PLC1addresses.Add("BucketWheelMotorContactor", 173);
            d1PLC1addresses.Add("VariableFrequencyOilBlockageSignal", 333);
            d1PLC1addresses.Add("VariableFrequencyOverpressureStop", 335);
            d1PLC1addresses.Add("VariableFrequencyPumpStationOverpressureAlarm", 334);
            d1PLC1addresses.Add("PowerRollerRunning", 420);
            d1PLC1addresses.Add("DriverRoomLevelingContactor", 185);
            d1PLC1addresses.Add("DryFogSystemIsLowAirPressure", 208);
            d1PLC1addresses.Add("DryFogSystemIsLowWaterPressure", 209);
            d1PLC1addresses.Add("WaterTankLowLevelSwitch", 211);
            d1PLC1addresses.Add("DriverRoomRiseValve", 447);
            d1PLC1addresses.Add("DriverRoomDescentValve", 448);
            d1PLC1addresses.Add("PowerCableRollerNotRunning", 1705);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingTemperatureUpperLimitAlarm", 1768);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingTemperatureLowerLimitAlarm", 1769);
            d1PLC1addresses.Add("AllowBucketWheelDiversion", 114);
            d1PLC1addresses.Add("WindproofSystemCableLimit2", 146);
            d1PLC1addresses.Add("WindproofSystemCableLimit3", 147);
            d1PLC1addresses.Add("RollerOverTightLimit2", 206);
            d1PLC1addresses.Add("RollerOverLooseLimit2", 207);
            d1PLC1addresses.Add("DryFogSystemFilterIsClogged", 210);
            d1PLC1addresses.Add("DryFogSystemAutoRun", 212);
            d1PLC1addresses.Add("DryFogSystemManualRun", 213);
            d1PLC1addresses.Add("DryFogSystemSprayStatus", 214);
            d1PLC1addresses.Add("DryFogSystemHeatRun", 215);
            d1PLC1addresses.Add("BucketWheelSlotMainCircuitBreaker", 222);
            d1PLC1addresses.Add("BucketWheelSlotMotorOverload", 223);
            d1PLC1addresses.Add("TailCarBeltLongitudinalTearing", 227);
            d1PLC1addresses.Add("ReversalBrakeRelease", 313);
            d1PLC1addresses.Add("BucketWheelSlotLiftLimit", 320);
            d1PLC1addresses.Add("BucketWheelSlotLowerLimit", 321);
            d1PLC1addresses.Add("DiversionPlateLimit", 324);
            d1PLC1addresses.Add("SuspendedBeltBrakeRelease", 346);
            d1PLC1addresses.Add("BrokenBeltCaptureAlarm", 347);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationLowOilLevel", 351);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationClogged", 352);
            d1PLC1addresses.Add("DriverRoomRearBalanceLimit", 366);
            d1PLC1addresses.Add("BucketWheelDiversionRunning", 405);
            d1PLC1addresses.Add("DriverRoomLevelingPumpRunning", 443);
            d1PLC1addresses.Add("BucketWheelSlotLift", 452);
            d1PLC1addresses.Add("BucketWheelSlotLower", 453);
            d1PLC1addresses.Add("RemoteEmergencyStop", 107);

            d1PLC1addresses.Add("DryFogDustSuppressionStackingRunning", 480);
            d1PLC1addresses.Add("DryFogDustSuppressionReclaimingRunning", 481);
            d1PLC1addresses.Add("DryFogDustSuppressionDiversionRunning", 482);
            d1PLC1addresses.Add("DryFogDustSuppressionRemoteStartRunning", 483);
            d1PLC1addresses.Add("DryFogDustSuppressionRemoteStopRunning", 484);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingUpperLimitAlarm", 250);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingLowerLimitAlarm", 251);

            //351  Bit
            d1PLC1addresses.Add("UnmannedEmergencyStop", 4005210);// Bit
            d1PLC1addresses.Add("RemoteEmergencyStoping", 4005211);// Bit
            d1PLC1addresses.Add("LargeVehicleMotor1OvertemperatureAlarm", 4010200);// Bit
            d1PLC1addresses.Add("LargeVehicleMotor2OvertemperatureAlarm", 4010201);// Bit 
            d1PLC1addresses.Add("LargeVehicleMotor3OvertemperatureAlarm", 4010202);// Bit
            d1PLC1addresses.Add("LargeVehicleMotor4OvertemperatureAlarm", 4010203);// Bit
            d1PLC1addresses.Add("LargeVehicleMotor5OvertemperatureAlarm", 4010204);// Bit
            d1PLC1addresses.Add("LargeVehicleMotor6OvertemperatureAlarm", 4010205);// Bit
            d1PLC1addresses.Add("WalkingReducerBearingTemperatureUpperLimitAlarm", 4010206);// Bit
            d1PLC1addresses.Add("WalkingReducerBearingTemperatureLowerLimitAlarm", 4010207);// Bit
            d1PLC1addresses.Add("WalkingReducerOilTemperatureUpperLimitAlarm", 4010208);// Bit
            d1PLC1addresses.Add("WalkingReducerOilTemperatureLowerLimitAlarm", 4010209);// Bit
            d1PLC1addresses.Add("ReversalTemperatureUpperLimitAlarm", 4012008);// Bit
            d1PLC1addresses.Add("ReversalTemperatureLowerLimitAlarm", 4012009);// Bit
            d1PLC1addresses.Add("BrokenBeltCaptureAlarming", 4016009);// Bit
            d1PLC1addresses.Add("SuspendedBeltTemperatureUpperLimitAlarm", 4016010);// Bit
            d1PLC1addresses.Add("SuspendedBeltTemperatureLowerLimitAlarm", 4016011);// Bit
            d1PLC1addresses.Add("SuspendedBeltRollerBearingTemperatureUpperLimitAlarm", 4016012);// Bit
            d1PLC1addresses.Add("SuspendedBeltRollerBearingTemperatureLowerLimitAlarm", 4016013);// Bit
            d1PLC1addresses.Add("BucketWheelTemperatureLowerLimitAlarm", 4018004);// Bit

            //371
            d1PLC1addresses.Add("CableRollerContactorAuxiliaryContactFault", 1709);
            d1PLC1addresses.Add("DriverRoomBalancePumpMotorNotRunning", 1731);
            d1PLC1addresses.Add("DriverRoomBalancePumpMotorAuxiliaryContactFault", 1733);
            d1PLC1addresses.Add("Remote", 295);


            d1PLC1addresses.Add("DriverRoomRiseButton", 2018);
            d1PLC1addresses.Add("DriverRoomDescentButton", 2022);

            #endregion

            #region 键值对d1PLC2
            //1
            //d1PLC2addresses.Add("XBZQ_FZ_VALUE", 4000049); //int
            //d1PLC2addresses.Add("XBZZ_FZ_VALUE", 4000050);
            //d1PLC2addresses.Add("XBZH_FZ_VALUE", 4000051);
            //d1PLC2addresses.Add("XBYQ_FZ_VALUE", 4000052);
            d1PLC2addresses.Add("XBZQ_FZ_VALUE", 400000); //float
            d1PLC2addresses.Add("XBZZ_FZ_VALUE", 400002);
            d1PLC2addresses.Add("XBZH_FZ_VALUE", 400004);
            d1PLC2addresses.Add("XBYQ_FZ_VALUE", 400006);
            d1PLC2addresses.Add("XBYZ_FZ_VALUE", 400008);
            d1PLC2addresses.Add("XBYH_FZ_VALUE", 400010);
            d1PLC2addresses.Add("QJY_VALUE", 400012);
            d1PLC2addresses.Add("DCZQ_FZ_VALUE", 400016);
            d1PLC2addresses.Add("DCYQ_FZ_VALUE", 400018);
            d1PLC2addresses.Add("DCZH_FZ_VALUE", 400020);
            d1PLC2addresses.Add("DCYH_FZ_VALUE", 400022);
            d1PLC2addresses.Add("XBTB_LWJ_VALUE", 400024);
            d1PLC2addresses.Add("ENCODE_DC_VALUE", 400032);
            d1PLC2addresses.Add("Encode_slew_VALUE", 400034);

            d1PLC2addresses.Add("Take_BySection", 00003); //bool
            d1PLC2addresses.Add("Take_Run_Rdy", 00004);
            d1PLC2addresses.Add("Take_Runing", 00005);
            d1PLC2addresses.Add("Take_Runing_Fault", 00006);
            d1PLC2addresses.Add("Take_Para_Set_ERR", 00007);
            d1PLC2addresses.Add("Take_Right_Arrive", 00008);
            d1PLC2addresses.Add("Take_Left_Arrive", 00009);
            d1PLC2addresses.Add("Take_SlewDirect", 00010);
            d1PLC2addresses.Add("Take_DCDirect", 00011);
            d1PLC2addresses.Add("Take_Right_CMD", 00012);
            d1PLC2addresses.Add("Take_Left_CMD", 00013);
            d1PLC2addresses.Add("Take_DCFWD_CMD", 00014);
            d1PLC2addresses.Add("Take_DCREV_CMD", 00015);
            d1PLC2addresses.Add("Take_Device_Enable", 00016);
            d1PLC2addresses.Add("Change_Direct", 00017);
            d1PLC2addresses.Add("Forbid_ChangeDirect", 00018);
            d1PLC2addresses.Add("Get_R_CurrentAngle", 00019);
            d1PLC2addresses.Add("Get_L_CurrentAngle", 00020);
            d1PLC2addresses.Add("Take_FWDStepSize_INC", 00021);
            d1PLC2addresses.Add("Take_FWDStepSize_DES", 00022);
            d1PLC2addresses.Add("Take_LeftBorder_INC", 00023);
            d1PLC2addresses.Add("Take_LeftBorder_DES", 00024);
            d1PLC2addresses.Add("Take_RightBorder_INC", 00025);
            d1PLC2addresses.Add("Take_RightBorder_DES", 00026);
            d1PLC2addresses.Add("ChangeDirectTimer_R", 00027);
            d1PLC2addresses.Add("Slew_Speed_Enable", 00028);
            d1PLC2addresses.Add("Take_Current_Lock", 00032);
            d1PLC2addresses.Add("Take_Current_H", 00033);
            d1PLC2addresses.Add("Take_Current_HH", 00034);
            d1PLC2addresses.Add("Take_Current_Norm", 00035);
            d1PLC2addresses.Add("Take_Current_Norm_PE", 00036);
            d1PLC2addresses.Add("Take_Forbid_CHDirect", 00037);
            d1PLC2addresses.Add("Take_Releas_CHDirect", 00038);
            d1PLC2addresses.Add("Take_ChT_MO", 00039);
            d1PLC2addresses.Add("Take_CHT_Enable", 00040);
            d1PLC2addresses.Add("Take_CHT_Start", 00041);
            d1PLC2addresses.Add("Take_CHT_Stop", 00042);
            d1PLC2addresses.Add("Take_ChT_Restrat", 00043);
            d1PLC2addresses.Add("Take_ChT_PerStart", 00044);
            d1PLC2addresses.Add("Take_CHT_Finsh", 00045);
            d1PLC2addresses.Add("Take_CHT_ERR", 00046);
            d1PLC2addresses.Add("Take_CHT_Onse", 00047);
            d1PLC2addresses.Add("Take_ChT_Left_CMD1", 00048);
            d1PLC2addresses.Add("Take_ChT_Right_CMD1", 00049);
            d1PLC2addresses.Add("Take_ChT_DcREV_CMD", 00050);
            d1PLC2addresses.Add("Take_ChT_LuffD_CMD1", 00051);
            d1PLC2addresses.Add("Take_ChT_LuffD_CMD2", 00052);
            d1PLC2addresses.Add("Take_CHT_Right_Reach", 00055);
            d1PLC2addresses.Add("Take_CHT_Left_Reach", 00056);
            d1PLC2addresses.Add("Take_CHT_Slew_Finish", 00057);
            d1PLC2addresses.Add("Take_CHT_Runing", 00058);
            d1PLC2addresses.Add("Take_Outside_INC", 00059);
            d1PLC2addresses.Add("Take_Inside_INC", 00064);
            d1PLC2addresses.Add("BeltBucket_OnZero", 00069);
            d1PLC2addresses.Add("Take_VVVF_Aear", 00070);
            d1PLC2addresses.Add("Take_TSOL_Enable", 00083);
            d1PLC2addresses.Add("Take_TSOL_Flag", 00084);
            d1PLC2addresses.Add("Take_TSOL_PE", 00085);
            d1PLC2addresses.Add("Take_TSOL_Reset_PE1", 00096);
            d1PLC2addresses.Add("Take_TSOL_Reset_PE2", 00097);
            d1PLC2addresses.Add("Take_LowSpeed", 00098);
            d1PLC2addresses.Add("Take_Record_Flag1", 00102);
            d1PLC2addresses.Add("Take_Record_Flag2", 00103);
            d1PLC2addresses.Add("Take_Record_Flag3", 00104);
            d1PLC2addresses.Add("Take_Run", 00107);
            d1PLC2addresses.Add("Take_Record", 00108);
            d1PLC2addresses.Add("Take_DCREV_CMD_FE", 00109);

            d1PLC2addresses.Add("Take_Step", 40041); //short
            d1PLC2addresses.Add("Take_Pause_TM", 40042);
            d1PLC2addresses.Add("Take_ChangeDirect_TM", 40043);
            d1PLC2addresses.Add("Take_ChT_CW", 40046);
            d1PLC2addresses.Add("Take_ChT_TM", 40047);
            d1PLC2addresses.Add("Take_CHT_Finish_Delaytime", 40048);

            d1PLC2addresses.Add("Take_DC_NextPos", 400050); //float
            d1PLC2addresses.Add("Take_DC_StepSize", 400052);
            d1PLC2addresses.Add("Take_Start_Point", 400054);
            d1PLC2addresses.Add("Take_End_Point", 400056);
            d1PLC2addresses.Add("Take_LeftBorder", 400058);
            d1PLC2addresses.Add("Take_RightBorder", 400060);
            d1PLC2addresses.Add("Take_OffSet1", 400062);
            d1PLC2addresses.Add("Take_OffSet2", 400064);
            d1PLC2addresses.Add("Take_TSOL_CU", 400066);
            d1PLC2addresses.Add("Take_NormCurrent", 400068);
            d1PLC2addresses.Add("Take_RightBorder_SP", 400070);
            d1PLC2addresses.Add("Take_LeftBorder_SP", 400072);
            d1PLC2addresses.Add("Take_MaxFlue_SP", 400090);
            d1PLC2addresses.Add("Take_MaxCurrent_SP", 400092);
            d1PLC2addresses.Add("Take_MinCurrent_SP", 400094);
            d1PLC2addresses.Add("Take_DCPosStrat_SP", 400096);
            d1PLC2addresses.Add("Take_DCPosEnd_SP", 400098);
            d1PLC2addresses.Add("MAC_Right_Border", 400100);
            d1PLC2addresses.Add("MAC_Left_Border", 400102);
            d1PLC2addresses.Add("Take_ChT_HTLuff", 400112);
            d1PLC2addresses.Add("Take_ChT_HTSlew_SP", 400114);
            d1PLC2addresses.Add("Take_ChT_TargetLuff", 400116);
            d1PLC2addresses.Add("DC_Pos", 400118);
            d1PLC2addresses.Add("SLEW_Angle", 400120);
            d1PLC2addresses.Add("Luff_Angle", 400122);
            d1PLC2addresses.Add("Coal_L_High", 400124);
            d1PLC2addresses.Add("Coal_R_High", 400126);
            d1PLC2addresses.Add("Bucket_Current", 400128);
            d1PLC2addresses.Add("BoomBelt_Current", 400130);
            d1PLC2addresses.Add("Slew_Current", 400136);
            d1PLC2addresses.Add("Travel_Current", 400138);
            d1PLC2addresses.Add("Luff_Current", 400140);
            d1PLC2addresses.Add("TailLuff_Current", 400142);
            d1PLC2addresses.Add("Belt_Flue", 400144);
            d1PLC2addresses.Add("Bucket_Pos", 400146);
            d1PLC2addresses.Add("DC_FixSize", 400148);
            d1PLC2addresses.Add("DC_FixSize_NEXT", 400150);
            d1PLC2addresses.Add("Luff_FixSize", 400152);
            d1PLC2addresses.Add("Luff_FixSize_NEXT", 400154);

            //127
            d1PLC2addresses.Add("Control_SEL_Local", 00120);
            d1PLC2addresses.Add("Control_SEL_CCR", 00121);
            d1PLC2addresses.Add("SEL_Take_Mode", 00122);
            d1PLC2addresses.Add("SEL_Stack_Mode", 00123);
            d1PLC2addresses.Add("SEL_Pass_Mode", 00124);
            d1PLC2addresses.Add("Test_Mode", 00125);
            d1PLC2addresses.Add("OperDesk_OnZero", 00126);
            d1PLC2addresses.Add("AutoBorder_Enable", 00127);
            d1PLC2addresses.Add("Working_Start", 00132);
            d1PLC2addresses.Add("Working_Pause", 00133);
            d1PLC2addresses.Add("Stop_Runing", 00134);
            d1PLC2addresses.Add("System_Emergence", 00135);
            d1PLC2addresses.Add("HMI_ErrReset", 00136);
            d1PLC2addresses.Add("DC_FWD_Limit", 00137);
            d1PLC2addresses.Add("DC_FWD_LLimit", 00138);
            d1PLC2addresses.Add("DC_FWD_SoftLimit", 00139);
            d1PLC2addresses.Add("DcFWD_LimitStatus", 00140);
            d1PLC2addresses.Add("DC_REV_Limit", 00141);
            d1PLC2addresses.Add("DC_REV_LLimit", 00142);
            d1PLC2addresses.Add("DC_REV_SoftLimit", 00143);
            d1PLC2addresses.Add("DcREV_LimitStatus", 00144);
            d1PLC2addresses.Add("Slew_R_Limit", 00145);
            d1PLC2addresses.Add("Slew_R_LLimit", 00146);
            d1PLC2addresses.Add("Slew_R_SoftLimit", 00147);
            d1PLC2addresses.Add("Slew_R_LimitStatus", 00148);
            d1PLC2addresses.Add("Slew_L_Limit", 00149);
            d1PLC2addresses.Add("Slew_L_LLimit", 00150);
            d1PLC2addresses.Add("Slew_L_SoftLimit", 00151);
            d1PLC2addresses.Add("Slew_L_LimitStatus", 00152);
            d1PLC2addresses.Add("Luff_Up_Limit", 00153);
            d1PLC2addresses.Add("Luff_Up_LLimit", 00154);
            d1PLC2addresses.Add("Luff_Up_SoftLimit", 00155);
            d1PLC2addresses.Add("LuffUp_LimitStatus", 00156);
            d1PLC2addresses.Add("Luff_Down_Limit", 00157);
            d1PLC2addresses.Add("Luff_Down_LLimit", 00158);
            d1PLC2addresses.Add("Luff_Down_SoftLimit", 00159);
            d1PLC2addresses.Add("LuffDown_LimitStatus", 00160);
            d1PLC2addresses.Add("OverBelt_R_Limit", 00161);
            d1PLC2addresses.Add("OverBelt_L_Limit", 00162);
            d1PLC2addresses.Add("OverBelt_D_Limit", 00163);
            d1PLC2addresses.Add("OverBelt_R_SoftLimit", 00164);
            d1PLC2addresses.Add("OverBelt_L_SoftLimit", 00165);
            d1PLC2addresses.Add("OverBelt_D_SoftLimit", 00166);
            d1PLC2addresses.Add("ErrReset", 00167);
            d1PLC2addresses.Add("XBTB_Baffle_OnTake", 00170);
            d1PLC2addresses.Add("XBTB_Baffle_OnStack", 00171);
            d1PLC2addresses.Add("ZXLD_Baffle_OnTake", 00172);
            d1PLC2addresses.Add("ZXLD_Baffle_OnStack", 00173);
            d1PLC2addresses.Add("ZXLD_Skrit_OnTake", 00180);
            d1PLC2addresses.Add("ZXLD_Skrit_OnStack", 00181);
            d1PLC2addresses.Add("BOOL_YL8", 00182);
            d1PLC2addresses.Add("PSOn_Light", 00184);
            d1PLC2addresses.Add("PSOff_Light", 00185);
            d1PLC2addresses.Add("CPSOn_Light", 00186);
            d1PLC2addresses.Add("CPSOff_Light", 00187);
            d1PLC2addresses.Add("Ground_Belt_Waiting", 00188);
            d1PLC2addresses.Add("Ground_Belt_Runing", 00189);
            d1PLC2addresses.Add("CantBeltTake_Runing", 00190);
            d1PLC2addresses.Add("CantBeltStack_Runing", 00191);
            d1PLC2addresses.Add("Cable_PS_Runing", 00194);
            d1PLC2addresses.Add("Cable_CPS_Runing", 00195);
            d1PLC2addresses.Add("Luff_OilBump_Runing", 00197);
            d1PLC2addresses.Add("Bucket_Runing", 00198);
            d1PLC2addresses.Add("DC_FWD_Runing", 00199);
            d1PLC2addresses.Add("DC_REV_Runing", 00200);
            d1PLC2addresses.Add("SLEW_R_Runing", 00201);
            d1PLC2addresses.Add("SLEW_L_Runing", 00202);
            d1PLC2addresses.Add("Luff_Up_Runing", 00203);
            d1PLC2addresses.Add("Luff_Down_Runing", 00204);
            d1PLC2addresses.Add("Tail_LuffU_Runing", 00205);
            d1PLC2addresses.Add("Tail_LuffD_Runing", 00206);
            d1PLC2addresses.Add("Lighting", 00207);
            d1PLC2addresses.Add("CCR_Take_Enable", 00208);
            d1PLC2addresses.Add("CCR_Stack_Enable", 00209);
            d1PLC2addresses.Add("Runing_RightField", 00212);
            d1PLC2addresses.Add("Runing_LeftField", 00213);
            d1PLC2addresses.Add("DC_Encoder_ERR", 00215);
            d1PLC2addresses.Add("Slew_Encoder_ERR", 00216);
            d1PLC2addresses.Add("Para_Intail_SB", 00218);
            d1PLC2addresses.Add("Alarming", 00219);
            d1PLC2addresses.Add("DC_Enable", 00220);
            d1PLC2addresses.Add("Slew_Enable", 00221);
            d1PLC2addresses.Add("Luff_Enable", 00222);
            d1PLC2addresses.Add("DC_FWD_Enable", 00224);
            d1PLC2addresses.Add("DC_REV_Enable", 00225);
            d1PLC2addresses.Add("Slew_R_Enable", 00226);
            d1PLC2addresses.Add("Slew_L_Enable", 00227);
            d1PLC2addresses.Add("LuffU_Enable", 00228);
            d1PLC2addresses.Add("LuffD_Enable", 00229);
            d1PLC2addresses.Add("Bucket_Enable", 00230);
            d1PLC2addresses.Add("Belt_Take_Enable", 00233);
            d1PLC2addresses.Add("Belt_Stack_Enable", 00234);
            d1PLC2addresses.Add("Rail_Relax_SB", 00238);
            d1PLC2addresses.Add("Rail_Clamp_SB", 00239);
            d1PLC2addresses.Add("PS_MO_SB", 00240);
            d1PLC2addresses.Add("PS_MC_SB", 00241);
            d1PLC2addresses.Add("CPS_MO_SB", 00242);
            d1PLC2addresses.Add("CPS_MC_SB", 00243);
            d1PLC2addresses.Add("BeltTake_MO_SB", 00244);
            d1PLC2addresses.Add("BeltStack_MO_SB", 00245);
            d1PLC2addresses.Add("Belt_MC_SB", 00246);
            d1PLC2addresses.Add("BeltTake_MO", 00247);
            d1PLC2addresses.Add("BeltStack_MO", 00248);
            d1PLC2addresses.Add("Bucket_MO_SB", 00255);
            d1PLC2addresses.Add("Bucket_MC_SB", 00256);
            d1PLC2addresses.Add("Bucket_MO", 00257);
            d1PLC2addresses.Add("Light_MO_SB", 00258);
            d1PLC2addresses.Add("Light_MC_SB", 00259);
            d1PLC2addresses.Add("Luff_OilBump_MO_SB", 00260);
            d1PLC2addresses.Add("Luff_OilBump_MC_SB", 00261);
            d1PLC2addresses.Add("Travel_MC_SB", 00270);
            d1PLC2addresses.Add("Emergency_Stop", 00375);
            d1PLC2addresses.Add("Travel_FWD_AO", 00376);
            d1PLC2addresses.Add("Travel_REV_AO", 00377);
            d1PLC2addresses.Add("Slew_R_AO", 00378);
            d1PLC2addresses.Add("Slew_L_AO", 00379);
            d1PLC2addresses.Add("LuffU_AO", 00380);
            d1PLC2addresses.Add("LuffD_AO", 00381);
            d1PLC2addresses.Add("Bucket_AO", 00384);
            d1PLC2addresses.Add("Bucket_AC", 00385);
            d1PLC2addresses.Add("Belt_Take_AO", 00386);
            d1PLC2addresses.Add("Belt_Take_AC", 00387);
            d1PLC2addresses.Add("Belt_Stack_AO", 00388);
            d1PLC2addresses.Add("Belt_Stack_AC", 00389);
            d1PLC2addresses.Add("XBTB_Baffle_Take_MO", 00400);
            d1PLC2addresses.Add("XBTB_Baffle_Stack_MO", 00401);
            d1PLC2addresses.Add("XBTB_Baffle_Err", 00402);
            d1PLC2addresses.Add("ZXLD_Baffle_Take_MO", 00403);
            d1PLC2addresses.Add("ZXLD_Baffle_Stack_MO", 00404);
            d1PLC2addresses.Add("ZXLD_Baffle_Err", 00405);
            d1PLC2addresses.Add("ZXLD_Skrit_Take_MO", 00406);
            d1PLC2addresses.Add("ZXLD_Skrit_Stack_MO", 00407);
            d1PLC2addresses.Add("ZXLD_Skrit_Err", 00408);
            d1PLC2addresses.Add("DC_R_Anchor", 00418);
            d1PLC2addresses.Add("DC_L_Anchor", 00419);
            d1PLC2addresses.Add("DC_R_Rail_Clamp", 00420);
            d1PLC2addresses.Add("DC_L_Rail_Clamp", 00421);
            d1PLC2addresses.Add("DC_R_Rail_Relax", 00422);
            d1PLC2addresses.Add("DC_L_Rail_Relax", 00423);
            d1PLC2addresses.Add("YL_Bit8", 00424);
            d1PLC2addresses.Add("YL_Bit5", 00425);
            d1PLC2addresses.Add("YL_Bit9", 00426);
            d1PLC2addresses.Add("YL_Bit7", 00427);
            d1PLC2addresses.Add("YL_Bit12", 00428);
            d1PLC2addresses.Add("YL_Bit10", 00429);
            d1PLC2addresses.Add("YL_Bit15", 00433);
            d1PLC2addresses.Add("DC_FWD_FixS_SB", 00438);
            d1PLC2addresses.Add("DC_FWD_FixS_Run", 00439);
            d1PLC2addresses.Add("DC_FWD_FixS_CMD", 00440);
            d1PLC2addresses.Add("DC_REV_FixS_SB", 00441);
            d1PLC2addresses.Add("DC_REV_FixS_Run", 00442);
            d1PLC2addresses.Add("DC_REV_FixS_CMD", 00443);
            d1PLC2addresses.Add("LuffU_FixS_SB", 00444);
            d1PLC2addresses.Add("LuffD_FixS_SB", 00445);
            d1PLC2addresses.Add("LuffU_FixS_Run", 00446);
            d1PLC2addresses.Add("LuffD_FixS_Run", 00447);
            d1PLC2addresses.Add("LuffU_FixS_CMD", 00448);
            d1PLC2addresses.Add("LuffD_FixS_CMD", 00449);
            d1PLC2addresses.Add("Skrit_Take_Start_SB", 00450);
            d1PLC2addresses.Add("Skrit_Take_Changing", 00451);
            d1PLC2addresses.Add("Skrit_Take_ChFinish", 00452);
            d1PLC2addresses.Add("Skrit_Take_Stop_SB", 00453);
            d1PLC2addresses.Add("Skrit_Stack_Start_SB", 00454);
            d1PLC2addresses.Add("Skrit_Stack_Changing", 00455);
            d1PLC2addresses.Add("Skrit_Stack_ChFinish", 00456);
            d1PLC2addresses.Add("Skrit_Stack_Stop_SB", 00457);
            d1PLC2addresses.Add("Slew_SAS_L_Alarm", 00458);
            d1PLC2addresses.Add("Slew_SAS_R_Alarm", 00459);
            d1PLC2addresses.Add("DC_SAS_F_Alarm", 00460);
            d1PLC2addresses.Add("DC_SAS_B_Alarm", 00461);
            d1PLC2addresses.Add("Slew_SAS_RR_Alarm", 00462);
            d1PLC2addresses.Add("Slew_SAS_LR_Alarm", 00463);
            d1PLC2addresses.Add("Slew_SAS_RU_Alarm", 00464);
            d1PLC2addresses.Add("Slew_SAS_LU_Alarm", 00465);

            //301
            d1PLC2addresses.Add("DC_SAS_RF_Alrm", 00466);//bool
            d1PLC2addresses.Add("DC_SAS_RB_Alrm", 00467);
            d1PLC2addresses.Add("DC_SAS_LF_Alrm", 00468);
            d1PLC2addresses.Add("DC_SAS_LB_Alrm", 00469);
            d1PLC2addresses.Add("FWD_Limit_Waring", 00470);
            d1PLC2addresses.Add("REV_Limit_Waring", 00471);
            d1PLC2addresses.Add("Slew_R_Limit_Waring", 00472);
            d1PLC2addresses.Add("Slew_L_Limit_Waring", 00473);
            d1PLC2addresses.Add("Luff_U_Limit_Waring", 00474);
            d1PLC2addresses.Add("Luff_D_Limit_Waring", 00475);
            d1PLC2addresses.Add("DC_SAS_Bypass", 00476);
            d1PLC2addresses.Add("Boom_SAS_Bypass", 00477);
            d1PLC2addresses.Add("DCPos_Bypass", 00478);
            d1PLC2addresses.Add("SlewAngle_Bypass", 00479);
            d1PLC2addresses.Add("LuffAngle_Bypass", 00480);
            d1PLC2addresses.Add("DC_Encoder_Bypass", 00481);
            d1PLC2addresses.Add("Slew_Encoder_Bypass", 00482);
            d1PLC2addresses.Add("OverBelt_R_Bypass", 00483);
            d1PLC2addresses.Add("OverBelt_L_Bypass", 00484);
            d1PLC2addresses.Add("OverBelt_D_Bypass", 00485);
            d1PLC2addresses.Add("DC_SAS_RF_Bypass", 00486);
            d1PLC2addresses.Add("DC_SAS_LF_Bypass", 00487);
            d1PLC2addresses.Add("DC_SAS_RB_Bypass", 00488);
            d1PLC2addresses.Add("DC_SAS_LB_Bypass", 00489);
            d1PLC2addresses.Add("Boom_SAS_RR_Bypass", 00490);
            d1PLC2addresses.Add("Boom_SAS_LR_Bypass", 00491);
            d1PLC2addresses.Add("Boom_SAS_RU_Bypass", 00492);
            d1PLC2addresses.Add("Boom_SAS_LU_Bypass", 00493);

            d1PLC2addresses.Add("XBTB_Baffle_CW", 40156); //short
            d1PLC2addresses.Add("XBTB_Baffle_TTSet", 40157);
            d1PLC2addresses.Add("XBTB_Baffle_STSet", 40158);
            d1PLC2addresses.Add("XBTB_Baffle_ACSet", 40159);
            d1PLC2addresses.Add("Working_Status", 40177);
            d1PLC2addresses.Add("Working_Start_TM", 40188);
            d1PLC2addresses.Add("Stop_Runing_TM", 40189);
            d1PLC2addresses.Add("PS_MO_SB_TM", 40190);
            d1PLC2addresses.Add("PS_MC_SB_TM", 40191);
            d1PLC2addresses.Add("CPS_MO_SB_TM", 40192);
            d1PLC2addresses.Add("CPS_MC_SB_TM", 40193);
            d1PLC2addresses.Add("Light_MO_SB_TM", 40194);
            d1PLC2addresses.Add("Light_MC_SB_TM", 40195);
            d1PLC2addresses.Add("LuffOilBump_MO_SB_TM", 40196);
            d1PLC2addresses.Add("LuffOilBump_MC_SB_TM", 40197);
            d1PLC2addresses.Add("Rail_Relax_SB_TM", 40200);
            d1PLC2addresses.Add("Rail_Clamp_SB_TM", 40201);
            d1PLC2addresses.Add("DC_Encoder_Value", 4000202); //int
            d1PLC2addresses.Add("Slew_Encoder_Value", 4000204); //int
            d1PLC2addresses.Add("StackSlew_CW", 40206); //short
            d1PLC2addresses.Add("StackPiont_CW", 40207);
            d1PLC2addresses.Add("SlewStack_TM1", 40208);
            d1PLC2addresses.Add("SlewStack_TM2", 40209);
            d1PLC2addresses.Add("Stack_Pause_TM", 40214);
            d1PLC2addresses.Add("StackWS_CW", 40215);
            d1PLC2addresses.Add("StackWS_Tier", 40216);
            d1PLC2addresses.Add("StackWS_Tier_SP", 40217);
            d1PLC2addresses.Add("StackWS_TM1", 40218);
            d1PLC2addresses.Add("StackWS_TM2", 40219);
            d1PLC2addresses.Add("SlewStack_TM3", 40220);

            d1PLC2addresses.Add("Stack_DcRevSize", 400224); //float
            d1PLC2addresses.Add("Stack_NextDCPos", 400226);
            d1PLC2addresses.Add("Stack_HighSet", 400228);
            d1PLC2addresses.Add("Stack_Start_Slew", 400230);
            d1PLC2addresses.Add("Stack_End_Slew", 400232);
            d1PLC2addresses.Add("Stack_RightBorder", 400234);
            d1PLC2addresses.Add("Stack_LeftBorder", 400236);
            d1PLC2addresses.Add("StackPiont_NextLuff", 400238);
            d1PLC2addresses.Add("StackPiont_LuffSize", 400240);
            d1PLC2addresses.Add("StackPiont_LuffMax", 400242);
            d1PLC2addresses.Add("Stack_Range", 400244);
            d1PLC2addresses.Add("Stack_Range_Middule", 400246);
            d1PLC2addresses.Add("Stack_OffSet", 400248);
            d1PLC2addresses.Add("Stack_OffSet_Min", 400250);
            d1PLC2addresses.Add("Stack_OffSet_Max", 400252);
            d1PLC2addresses.Add("Stack_M_OffSet", 400254);
            d1PLC2addresses.Add("StackPiont_FS_Next", 400256);
            d1PLC2addresses.Add("Stack_Start_Pos", 400258);
            d1PLC2addresses.Add("Stack_End_Pos", 400260);
            d1PLC2addresses.Add("StackW_DC_End", 400262);
            d1PLC2addresses.Add("StackWS_NextSlew", 400264);
            d1PLC2addresses.Add("StackWS_NextLuff", 400266);
            d1PLC2addresses.Add("StackWS_Slew_Start", 400268);
            d1PLC2addresses.Add("StackWS_Slew_End", 400270);
            d1PLC2addresses.Add("StackWS_Luff_End", 400272);
            d1PLC2addresses.Add("StackWS_Start_S", 400286);
            d1PLC2addresses.Add("StackWS_End_S", 400288);
            d1PLC2addresses.Add("StackWS_Luff_S", 400290);
            d1PLC2addresses.Add("StackWS_S_Offset", 400292);
            d1PLC2addresses.Add("StackWS_S_AllOffset", 400294);
            d1PLC2addresses.Add("StackWS_StartSOA_ABS", 400296);
            d1PLC2addresses.Add("Stack_RightBorder_SP", 400298);
            d1PLC2addresses.Add("Stack_LeftBorder_SP", 400300);

            d1PLC2addresses.Add("SlewStack_SEL", 00494); //bool
            d1PLC2addresses.Add("PointStack_SEL", 00495);
            d1PLC2addresses.Add("Stack_Runing", 00496);
            d1PLC2addresses.Add("Stack_Runing_Rdy", 00497);
            d1PLC2addresses.Add("Stack_Runing_Fault", 00498);
            d1PLC2addresses.Add("StackSlew_Direction", 00499);
            d1PLC2addresses.Add("StackSlew_Left_CMD", 00500);
            d1PLC2addresses.Add("StackSlew_Right_CMD", 00501);
            d1PLC2addresses.Add("StackSlew_DcREV_CMD", 00502);
            d1PLC2addresses.Add("StackSlew_H_Arrive", 00503);
            d1PLC2addresses.Add("StackSlew_L_Arrive", 00504);
            d1PLC2addresses.Add("StackSlew_R_Arrive", 00505);
            d1PLC2addresses.Add("StackPiont_Left_CMD", 00510);
            d1PLC2addresses.Add("StackPiont_Right_CMD", 00511);
            d1PLC2addresses.Add("StackPiont_LuffU_CMD", 00512);
            d1PLC2addresses.Add("StackPiont_DcRev_CMD", 00513);
            d1PLC2addresses.Add("StackPiont_H_Arrive", 00514);
            d1PLC2addresses.Add("StackPiont_D_Arrive", 00515);
            d1PLC2addresses.Add("StackEndPos_Arrive", 00516);
            d1PLC2addresses.Add("StackPiont_FS_Mode", 00517);
            d1PLC2addresses.Add("StackPiont_FS_Arrive", 00518);
            d1PLC2addresses.Add("StackPiont_FS_DWF", 00519);
            d1PLC2addresses.Add("StackPiont_FS_DW", 00520);
            d1PLC2addresses.Add("Stack_ParaSet_ERR", 00521);
            d1PLC2addresses.Add("StackRightBorder_INC", 00522);
            d1PLC2addresses.Add("StackRightBorder_DES", 00523);
            d1PLC2addresses.Add("StackLeftBorder_INC", 00524);
            d1PLC2addresses.Add("StackLeftBorder_DES", 00525);
            d1PLC2addresses.Add("Stack_DcRevSize_INC", 00526);
            d1PLC2addresses.Add("Stack_DcRevSize_DES", 00527);
            d1PLC2addresses.Add("StackPiont_Direct", 00530);
            d1PLC2addresses.Add("StackPiont_FS_Run", 00531);
            d1PLC2addresses.Add("Stack_DC_Direct", 00534);
            d1PLC2addresses.Add("Stack_DcFWD_Arrive", 00537);
            d1PLC2addresses.Add("Stack_DcREV_Arrive", 00538);
            d1PLC2addresses.Add("StackWS_SEL", 00550);
            d1PLC2addresses.Add("StackWS_Luff_Arrive", 00551);
            d1PLC2addresses.Add("StackWS_SEL_PE", 00552);
            d1PLC2addresses.Add("Stack_Record_Flag1", 00564);
            d1PLC2addresses.Add("Stack_Record_Flag2", 00565);
            d1PLC2addresses.Add("Stack_Record_Flag3", 00566);
            d1PLC2addresses.Add("Stack_Record_Flag4", 00567);
            d1PLC2addresses.Add("Stack_Record_Flag5", 00568);
            d1PLC2addresses.Add("Stack_Record_Flag6", 00569);
            d1PLC2addresses.Add("Pos_Start", 00570);
            d1PLC2addresses.Add("Pos_Froce", 00571);
            d1PLC2addresses.Add("Pos_Rdy", 00572);
            d1PLC2addresses.Add("Pos_Start_Warning", 00573);
            d1PLC2addresses.Add("Pos_Runing", 00574);
            d1PLC2addresses.Add("Pos_Runing_Fault", 00575);
            d1PLC2addresses.Add("Pos_Runing_Finish", 00576);
            d1PLC2addresses.Add("Pos_TakeDevice_En", 00577);
            d1PLC2addresses.Add("Pos_StackDevice_En", 00578);
            d1PLC2addresses.Add("WorkArea_NotSelect", 00579);
            d1PLC2addresses.Add("Pos_DcREV_CMD1", 00580);
            d1PLC2addresses.Add("Pos_LuffUp_CMD1", 00581);
            d1PLC2addresses.Add("Pos_LuffUp_CMD2", 00582);
            d1PLC2addresses.Add("Pos_LuffUp_CMD3", 00583);
            d1PLC2addresses.Add("Pos_Slew_R_CMD1", 00584);
            d1PLC2addresses.Add("Pos_Slew_L_CMD1", 00585);
            d1PLC2addresses.Add("Pos_DcFWD_Dest_CMD", 00586);
            d1PLC2addresses.Add("Pos_DcREV_Dest_CMD", 00587);
            d1PLC2addresses.Add("Pos_DcFWD_Dest", 00588);
            d1PLC2addresses.Add("Pos_DcREV_Dest", 00589);
            d1PLC2addresses.Add("Pos_DC_Finish", 00590);
            d1PLC2addresses.Add("Pos_DC_HightSpeed", 00591);
            d1PLC2addresses.Add("Pos_LuffDown_CMD1", 00592);
            d1PLC2addresses.Add("Pos_LuffUp_CMD", 00593);
            d1PLC2addresses.Add("Pos_L_Slew_CMD2", 00594);
            d1PLC2addresses.Add("Pos_R_Slew_CMD2", 00595);
            d1PLC2addresses.Add("Pos_LuffUpMax_CMD", 00596);
            d1PLC2addresses.Add("Pos_L_Slew_CMD3", 00597);
            d1PLC2addresses.Add("Pos_R_Slew_CMD3", 00598);
            d1PLC2addresses.Add("Pos_L_Slew_Dest", 00599);
            d1PLC2addresses.Add("Pos_R_Slew_Dest", 00600);
            d1PLC2addresses.Add("Pos_Slew_Finish", 00601);
            d1PLC2addresses.Add("Pos_StartTakeDecive", 00602);
            d1PLC2addresses.Add("Pos_StartStackDecive", 00603);
            d1PLC2addresses.Add("Pos_Take_LuffU_CMD", 00604);
            d1PLC2addresses.Add("Pos_Take_LuffD_CMD", 00605);
            d1PLC2addresses.Add("Pos_Take_LuffU_Dest", 00606);
            d1PLC2addresses.Add("Pos_Take_LuffD_Dest", 00607);
            d1PLC2addresses.Add("Pos_Take_Luff_Finish", 00608);
            d1PLC2addresses.Add("Pos_Stack_LuffD_CMD", 00609);
            d1PLC2addresses.Add("Pos_Stack_LuffD_Dest", 00610);
            d1PLC2addresses.Add("Pos_StackLuff_Finish", 00611);
            d1PLC2addresses.Add("Pos_Finish_Missing", 00612);
            d1PLC2addresses.Add("Pos_Finish_Confmiss", 00613);
            d1PLC2addresses.Add("Pos_Execute_Onse", 00614);
            d1PLC2addresses.Add("Pos_Take_Starting", 00615);
            d1PLC2addresses.Add("Pos_Take_Outtime", 00616);
            d1PLC2addresses.Add("Pos_TakeBelt_AO_CMD", 00618);
            d1PLC2addresses.Add("Pos_Bucket_AO_CMD", 00619);
            d1PLC2addresses.Add("Pos_TakeBelt_AO", 00620);
            d1PLC2addresses.Add("Pos_Bucket_AO", 00621);
            d1PLC2addresses.Add("Pos_Take_Stopting", 00622);
            d1PLC2addresses.Add("Pos_TakeBelt_AC_CMD", 00623);
            d1PLC2addresses.Add("Pos_Bucket_AC_CMD", 00624);
            d1PLC2addresses.Add("Pos_TakeBelt_AC", 00625);
            d1PLC2addresses.Add("Pos_Bucket_AC", 00626);
            d1PLC2addresses.Add("Pos_Stack_Starting", 00627);
            d1PLC2addresses.Add("Pos_Stack_Outtime", 00628);
            d1PLC2addresses.Add("Pos_StackBelt_AO_CMD", 00630);
            d1PLC2addresses.Add("Pos_StackBelt_AO", 00633);
            d1PLC2addresses.Add("Pos_StackBelt_AC_CMD", 00636);
            d1PLC2addresses.Add("Pos_PassStarting", 00643);
            d1PLC2addresses.Add("Pos_PassBelt_AO_CMD", 00646);
            d1PLC2addresses.Add("Pos_PassBelt_AO", 00649);
            d1PLC2addresses.Add("Pos_PassBelt_AC_CMD", 00652);
            d1PLC2addresses.Add("Pos_PassStopting", 00658);
            d1PLC2addresses.Add("Pos_StartPassDecive", 00659);
            d1PLC2addresses.Add("Pass_BeltTake_AC_CMD", 00660);
            d1PLC2addresses.Add("Pass_SmaBelt_AC_CMD", 00664);
            d1PLC2addresses.Add("Pass_Sque_Stoping", 00668);
            d1PLC2addresses.Add("Pass_Starting", 00669);
            d1PLC2addresses.Add("Pos_Take_Startfinish", 00672);
            d1PLC2addresses.Add("Pos_Stac_Startfinish", 00673);
            d1PLC2addresses.Add("Pos_Stop", 00674);

            d1PLC2addresses.Add("Pos_STEP", 40322); //short
            d1PLC2addresses.Add("BucketStart_RTM", 40324);
            d1PLC2addresses.Add("StackStoping_RTM", 40329);
            d1PLC2addresses.Add("PassStart_TimeOut", 40330);
            d1PLC2addresses.Add("Pass_Stoping_RTM", 40333);
            d1PLC2addresses.Add("Pos_StackDevice_RTM", 40334);
            d1PLC2addresses.Add("Pos_Finish_FTM", 40335);
            d1PLC2addresses.Add("SEL_WorkArea", 40336);
            d1PLC2addresses.Add("SEL_WorkClass", 40337);
            d1PLC2addresses.Add("SEL_WorkTier", 40338);
            d1PLC2addresses.Add("Pos_Start_RTM", 40339);
            d1PLC2addresses.Add("Pos_TakeDevice_RTM", 40340);
            d1PLC2addresses.Add("Pos_SBCH_Finish", 40341);
            d1PLC2addresses.Add("Pos_ForceSBCH_Finish", 40342);

            d1PLC2addresses.Add("Pos_DCTarget", 400344); //float
            d1PLC2addresses.Add("Pos_SlewTarget", 400346);
            d1PLC2addresses.Add("Pos_LuffTarget", 400348);
            d1PLC2addresses.Add("Pos_DCTarget_SP", 400350);
            d1PLC2addresses.Add("Pos_SlewTarget_SP", 400352);
            d1PLC2addresses.Add("Pos_LuffTarget_SP", 400354);
            d1PLC2addresses.Add("Pos_DcBack", 400356);
            d1PLC2addresses.Add("Pos_LuffSA_SP", 400358);
            d1PLC2addresses.Add("Pos_SlewSA", 400360);
            d1PLC2addresses.Add("Pos_SlewSA_R_SP", 400362);
            d1PLC2addresses.Add("Pos_SlewSA_L_SP", 400364);
            d1PLC2addresses.Add("Pos_DCTarget_Mid", 400366);
            d1PLC2addresses.Add("Pos_Safe_LA_SP", 400368);
            d1PLC2addresses.Add("Pos_HCSafe_LA", 400370);
            d1PLC2addresses.Add("Pos_MaxStack_LA_SP", 400372);
            d1PLC2addresses.Add("Pos_SlewTarget_Mid", 400374);
            d1PLC2addresses.Add("MAC_Start_DcPos", 400376);
            d1PLC2addresses.Add("MAC_Start_SlewAngle", 400378);
            d1PLC2addresses.Add("MAC_Start_LuffAngle", 400380);
            d1PLC2addresses.Add("MAC_End_DcPos", 400382);
            d1PLC2addresses.Add("DCTarget_Record", 400384);
            d1PLC2addresses.Add("SlewTarget_Record", 400386);
            d1PLC2addresses.Add("LuffTarget_Record", 400388);
            d1PLC2addresses.Add("Take_PU_ACC", 400390);
            d1PLC2addresses.Add("Take_Current_ACC", 400392);
            d1PLC2addresses.Add("Take_Last_ACC", 400394);
            d1PLC2addresses.Add("Take_Total_ACC", 400396);
            d1PLC2addresses.Add("Stack_PU_ACC", 400398);
            d1PLC2addresses.Add("Stack_Current_ACC", 400400);
            d1PLC2addresses.Add("Stack_Last_ACC", 400402);
            d1PLC2addresses.Add("Encoder_PMW", 400448);
            d1PLC2addresses.Add("DC_SAS_RF_DSV", 400450);
            d1PLC2addresses.Add("DC_SAS_LF_DSV", 400452);
            d1PLC2addresses.Add("DC_SAS_RB_DSV", 400454);
            d1PLC2addresses.Add("DC_SAS_LB_DSV", 400456);
            d1PLC2addresses.Add("Boom_SAS_R_Radar_DSV", 400458);
            d1PLC2addresses.Add("Boom_SAS_L_Radar_DSV", 400460);
            d1PLC2addresses.Add("Boom_SAS_R_Ult_DSV", 400462);
            d1PLC2addresses.Add("Boom_SAS_L_Ult_DSV", 400464);
            d1PLC2addresses.Add("OverBelt_R_SPASV", 400466);
            d1PLC2addresses.Add("OverBelt_L_SPASV", 400468);
            d1PLC2addresses.Add("OverBelt_D_SPASV", 400470);
            d1PLC2addresses.Add("DC_FWD_SPSV", 400472);
            d1PLC2addresses.Add("DC_REV_SPSV", 400474);
            d1PLC2addresses.Add("Slew_R_SPSV", 400476);
            d1PLC2addresses.Add("Slew_L_SPSV", 400478);
            d1PLC2addresses.Add("Luff_U_SPSV", 400480);
            d1PLC2addresses.Add("Luff_D_SPSV", 400482);
            d1PLC2addresses.Add("Take_R_RB_01_SP", 400484);

            d1PLC2addresses.Add("Take_R_LB_01_SP", 400486); //float
            d1PLC2addresses.Add("Take_R_RB_02_SP", 400488);
            d1PLC2addresses.Add("Take_R_LB_02_SP", 400490);
            d1PLC2addresses.Add("Take_R_RB_03_SP", 400492);
            d1PLC2addresses.Add("Take_R_LB_03_SP", 400494);
            d1PLC2addresses.Add("Take_R_RB_04_SP", 400496);
            d1PLC2addresses.Add("Take_R_LB_04_SP", 400498);
            d1PLC2addresses.Add("Take_L_RB_01_SP", 400500);
            d1PLC2addresses.Add("Take_L_LB_01_SP", 400502);
            d1PLC2addresses.Add("Take_L_RB_02_SP", 400504);
            d1PLC2addresses.Add("Take_L_LB_02_SP", 400506);
            d1PLC2addresses.Add("Take_L_RB_03_SP", 400508);
            d1PLC2addresses.Add("Take_L_LB_03_SP", 400510);
            d1PLC2addresses.Add("Take_L_RB_04_SP", 400512);
            d1PLC2addresses.Add("Take_L_LB_04_SP", 400514);
            d1PLC2addresses.Add("Take_Luff_01_SP", 400516);
            d1PLC2addresses.Add("Take_Luff_02_SP", 400518);
            d1PLC2addresses.Add("Take_Luff_03_SP", 400520);
            d1PLC2addresses.Add("Take_Luff_04_SP", 400522);
            d1PLC2addresses.Add("Stack_R_RB_01_SP", 400524);
            d1PLC2addresses.Add("Stack_R_LB_01_SP", 400526);
            d1PLC2addresses.Add("Stack_R_RB_02_SP", 400528);
            d1PLC2addresses.Add("Stack_R_LB_02_SP", 400530);
            d1PLC2addresses.Add("Stack_R_RB_03_SP", 400532);
            d1PLC2addresses.Add("Stack_R_LB_03_SP", 400534);
            d1PLC2addresses.Add("Stack_R_RB_04_SP", 400536);
            d1PLC2addresses.Add("Stack_R_LB_04_SP", 400538);
            d1PLC2addresses.Add("Stack_L_RB_01_SP", 400540);
            d1PLC2addresses.Add("Stack_L_LB_01_SP", 400542);
            d1PLC2addresses.Add("Stack_L_RB_02_SP", 400544);
            d1PLC2addresses.Add("Stack_L_LB_02_SP", 400546);
            d1PLC2addresses.Add("Stack_L_RB_03_SP", 400548);
            d1PLC2addresses.Add("Stack_L_LB_03_SP", 400550);
            d1PLC2addresses.Add("Stack_L_RB_04_SP", 400552);
            d1PLC2addresses.Add("Stack_L_LB_04_SP", 400554);
            d1PLC2addresses.Add("Stack_Luff_01_SP", 400556);
            d1PLC2addresses.Add("Stack_Luff_02_SP", 400558);
            d1PLC2addresses.Add("Stack_Luff_03_SP", 400560);
            d1PLC2addresses.Add("Stack_Luff_04_SP", 400562);

            d1PLC2addresses.Add("Encoder_SAM", 00683); //bool
            d1PLC2addresses.Add("DC_Encoder_Enable", 00684);
            d1PLC2addresses.Add("Slew_Encoder_Enable", 00685);
            d1PLC2addresses.Add("DC_Encoder_Adjust", 00686);
            d1PLC2addresses.Add("Slew_Encoder_Adjust", 00687);
            d1PLC2addresses.Add("Encoder_BY2", 00688);
            d1PLC2addresses.Add("FAULT_RESET", 00707);




            #endregion

            #region 键值对d2PLC1

            #endregion

            #region 键值对d2PLC2

            #endregion


            #endregion


            //Number(4000113);


        }


        #region 全局变量定义
        //对象实例化

        //各用户控件对象
        public static UserControl_Page1 uc1 = new UserControl_Page1();

        public static UserControl_Page2 uc2 = new UserControl_Page2();
        public static UserControl_Page2_1 uc2_1 = new UserControl_Page2_1();
        public static UserControl_Page2_2 uc2_2 = new UserControl_Page2_2();
        public static UserControl_Page2_3 uc2_3 = new UserControl_Page2_3();
        public static UserControl_Page2_4 uc2_4 = new UserControl_Page2_4();
        public static UserControl_Page2_5 uc2_5 = new UserControl_Page2_5();
        public static UserControl_Page2_6 uc2_6 = new UserControl_Page2_6();

        public static UserControl_Page3 uc3 = new UserControl_Page3();
        public static UserControl_Page3_1 uc3_1 = new UserControl_Page3_1();
        public static UserControl_Page3_2 uc3_2 = new UserControl_Page3_2();
        public static UserControl_Page3_3 uc3_3 = new UserControl_Page3_3();

        public static UserControl_Page5 uc5 = new UserControl_Page5();
        

        
        public static ModbusHelper modbusHelper_D1PLC1 = new ModbusHelper(uc5);//施耐德D1PLC1对象
        public static ModbusHelper modbusHelper_D1PLC2 = new ModbusHelper(uc5);//施耐德D1PLC2对象
        public static ModbusHelper modbusHelper_D2PLC1 = new ModbusHelper(uc5);//施耐德D2PLC1对象
        public static ModbusHelper modbusHelper_D2PLC2 = new ModbusHelper(uc5);//施耐德D2PLC2对象

        public static D1PLC1Variables d1PLC1Variables = new D1PLC1Variables(); //D1PLC1变量对象
        public static D1PLC2Variables d1PLC2Variables = new D1PLC2Variables(); //D1PLC2变量对象
        public static D2PLC1Variables d2PLC1Variables = new D2PLC1Variables(); //D2PLC1变量对象
        public static D2PLC2Variables d2PLC2Variables = new D2PLC2Variables(); //D2PLC2变量对象
        public static SystemVariables systemVariables = new SystemVariables(); //PLC全体变量对象



        public static SystemCommand systemCommand = new SystemCommand(); //系统命令对象
        public static MySqlConnection mySqlConnection;//数据库对象

        // 字典定义
        Dictionary<string, int> d1PLC1addresses = new Dictionary<string, int>(); //D1PLC1变量地址
        Dictionary<string, int> d1PLC2addresses = new Dictionary<string, int>(); //D1PLC2变量地址
        Dictionary<string, int> d2PLC1addresses = new Dictionary<string, int>(); //D2PLC1变量地址
        Dictionary<string, int> d2PLC2addresses = new Dictionary<string, int>(); //D2PLC2变量地址

        private List<Button> buttons;

        string websocketIp = "127.0.0.1:11000";

        private DateTime startTime;
        IPAddress hostIP;
        IPEndPoint port;
        int point;
        Socket socketWatcher;
        bool flag;
        /*  变量定义
         *  startTime:程序启动时刻
         *  hostIP:用于存放服务器（本机）IP地址
         *  port:服务器的网络终结点，即IP:端口号
         *  point:存放用户输入的端口号
         *  socketWatcher:监听子系统的套接字
         *  flag:循环控制旗
         */



        //时间显示
        private void UpdateClock(object sender, EventArgs e)
        {
            // 获取当前时间并在 Label 控件上显示
            DateTime currentTime = DateTime.Now;

            // 获取今天是星期几
            DayOfWeek dayOfWeek = currentTime.DayOfWeek;

            // 将 DayOfWeek 转换为对应的字符串
            string dayOfWeekString;

            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dayOfWeekString = "日";
                    break;
                case DayOfWeek.Monday:
                    dayOfWeekString = "一";
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeekString = "二";
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeekString = "三";
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeekString = "四";
                    break;
                case DayOfWeek.Friday:
                    dayOfWeekString = "五";
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeekString = "六";
                    break;
                default:
                    dayOfWeekString = " ";
                    break;
            }

            TimeLabel.Text = currentTime.ToString("F")+" 星期"+ dayOfWeekString;
        }

        // 设置序列化选项
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented // 设置缩进格式
        };


        #endregion


        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //线程模块


        #region 与数据库连接线程
        /// <summary>
        /// 与数据库连接线程
        /// </summary>
        private void ConnectToDatabase()
        {
            // 连接数据库
            string IP = "127.0.0.1";
            string Database = "csr";
            string Username = "root";
            string Password = "123456";
            string connetStr = $"server={IP};database={Database};username={Username};password={Password};Charset=utf8";
            mySqlConnection = new MySqlConnection(connetStr);

            while (true)
            {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    try
                    {
                        mySqlConnection.Open(); // 连接数据库

                        uc3.mySqlConnection = mySqlConnection;


                        UpdateText(uc3.MySqlConnectionLabel, "已连接");

                        UpdateText(uc5.MySqlConnectionLabel, "已连接");

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK); // 显示错误信息

                        UpdateText(uc3.MySqlConnectionLabel, "已断开");

                        UpdateText(uc5.MySqlConnectionLabel, "已断开");
                    }
                }

                Thread.Sleep(5000);
            }
        }

        public void UpdateText(Label label, string text)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                label.Text = text;
            }));
        }

        #endregion

        #region 与PLC连接线程 （施耐德）
        /// <summary>
        /// 与PLC连接线程
        /// </summary>
        private void Process_PLC_Main()
        {
            while (true)
            {
                //D1PLC1
                try
                {
                    if (modbusHelper_D1PLC1.isConnectPLC == false)
                    {
                        if (modbusHelper_D1PLC1.ConnectPLC("127.0.0.1", int.Parse("502")))
                        {
                            DisplayRichTextboxContentAndScroll("D1PLC1连接Modbus_TCP成功");
                        }
                        else
                        {
                            DisplayRichTextboxContentAndScroll("D1PLC1连接Modbus_TCP失败,请检查网络以及IP设置");

                            UpdateText(uc5.PlcConnectionLabel, "D1PLC1连接失败！");
                        }
                    }

                    if (modbusHelper_D1PLC1.isConnectPLC == true)
                    {
                        UpdateText(uc5.PlcConnectionLabel, "D1PLC1已连接！");
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }
                Thread.Sleep(500);

                //D1PLC2
                try
                {
                    if (modbusHelper_D1PLC2.isConnectPLC == false)
                    {
                        if (modbusHelper_D1PLC2.ConnectPLC("127.0.0.1", int.Parse("502")))
                        {
                            DisplayRichTextboxContentAndScroll("D1PLC2连接Modbus_TCP成功");
                        }
                        else
                        {
                            DisplayRichTextboxContentAndScroll("D1PLC2连接Modbus_TCP失败,请检查网络以及IP设置");

                            UpdateText(uc5.PlcConnectionLabel, "D1PLC2连接失败！");
                        }
                    }

                    if (modbusHelper_D1PLC2.isConnectPLC == true)
                    {
                        UpdateText(uc5.PlcConnectionLabel, "D1PLC2已连接！");
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }
                Thread.Sleep(500);

                //D2PLC1
                try
                {
                    if (modbusHelper_D2PLC1.isConnectPLC == false)
                    {
                        if (modbusHelper_D2PLC1.ConnectPLC("127.0.0.1", int.Parse("502")))
                        {
                            DisplayRichTextboxContentAndScroll("D2PLC1连接Modbus_TCP成功");
                        }
                        else
                        {
                            DisplayRichTextboxContentAndScroll("D2PLC1连接Modbus_TCP失败,请检查网络以及IP设置");

                            UpdateText(uc5.PlcConnectionLabel, "D2PLC1连接失败！");
                        }
                    }

                    if (modbusHelper_D2PLC1.isConnectPLC == true)
                    {
                        UpdateText(uc5.PlcConnectionLabel, "D2PLC1已连接！");
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }
                Thread.Sleep(500);

                //D2PLC2
                try
                {
                    if (modbusHelper_D2PLC2.isConnectPLC == false)
                    {
                        if (modbusHelper_D2PLC2.ConnectPLC("127.0.0.1", int.Parse("502")))
                        {
                            DisplayRichTextboxContentAndScroll("D2PLC2连接Modbus_TCP成功");
                        }
                        else
                        {
                            DisplayRichTextboxContentAndScroll("D2PLC2连接Modbus_TCP失败,请检查网络以及IP设置");

                            UpdateText(uc5.PlcConnectionLabel, "D2PLC2连接失败！");
                        }
                    }

                    if (modbusHelper_D2PLC2.isConnectPLC == true)
                    {
                        UpdateText(uc5.PlcConnectionLabel, "D2PLC2已连接！");
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }
                Thread.Sleep(500);
            }
        }
        #endregion

        #region PLC数据获取线程 （施耐德）
        /// <summary>
        /// PLC数据获取线程
        /// </summary>
        private void Process_D1PLC1_DataGet()
        {
            while (true)
            {
                //Stopwatch stopwatch1 = new Stopwatch();
                //stopwatch1.Start();

                d1PLC1ShareRes.WaitMutex();

                if (modbusHelper_D1PLC1.socket != null && modbusHelper_D1PLC1.isConnectPLC)
                {
                    ReadD1PLC1();
                }

                d1PLC1ShareRes.ReleaseMutex();

                //stopwatch1.Stop();

                //复制变量
                CopyPropertiesD1PLC1(d1PLC1Variables, systemVariables);

                using (Form form = new Form())
                {
                    string responseMessage = JsonConvert.SerializeObject(systemVariables, settings);
                    form.Width = 400;
                    form.Height = 300;
                    form.Text = "Object Properties";

                    TextBox textBox = new TextBox();
                    textBox.Multiline = true;
                    textBox.ScrollBars = ScrollBars.Vertical;
                    textBox.Dock = DockStyle.Fill;
                    textBox.ReadOnly = true;
                    textBox.Text = responseMessage;
                    form.Controls.Add(textBox);


                    //form.ShowDialog();
                    //MessageBox.Show(stopwatch1.ElapsedMilliseconds.ToString());
                }



                Thread.Sleep(1000);
            }
        }
        private void Process_D1PLC2_DataGet()
        {
            while (true)
            {
                d1PLC2ShareRes.WaitMutex();

                if (modbusHelper_D1PLC2.socket != null && modbusHelper_D1PLC2.isConnectPLC == true)
                {
                    ReadD1PLC2();
                }

                d1PLC2ShareRes.ReleaseMutex();

                //复制变量
                CopyPropertiesD1PLC2(d1PLC2Variables, systemVariables);

                try
                {
                    //检测PLC连接状态
                    if (modbusHelper_D1PLC2.socket == null || modbusHelper_D1PLC2.isConnectPLC == false)
                    {
                        systemVariables.D1PLC2CommunicationState = false;
                    }
                    else if (modbusHelper_D1PLC2.socket != null && modbusHelper_D1PLC2.isConnectPLC == true)
                    {
                        systemVariables.D1PLC2CommunicationState = true;
                    }
                }
                catch { }



                Thread.Sleep(1000);
            }
        }
        private void Process_D2PLC1_DataGet()
        {
            while (true)
            {
                d2PLC1ShareRes.WaitMutex();

                if (modbusHelper_D2PLC1.socket != null && modbusHelper_D2PLC1.isConnectPLC == true)
                {
                    ReadD2PLC1();
                }

                d2PLC1ShareRes.ReleaseMutex();

                //复制变量
                CopyPropertiesD2PLC1(d2PLC1Variables, systemVariables);

                try
                {
                    //检测PLC连接状态
                    if (modbusHelper_D2PLC1.socket == null || modbusHelper_D2PLC1.isConnectPLC == false)
                    {
                        systemVariables.D2PLC1CommunicationState = false;
                    }
                    else if (modbusHelper_D2PLC1.socket != null && modbusHelper_D2PLC1.isConnectPLC == true)
                    {
                        systemVariables.D2PLC1CommunicationState = true;
                    }
                }
                catch { }



                Thread.Sleep(1000);
            }
        }
        private void Process_D2PLC2_DataGet()
        {
            while (true)
            {

                d2PLC2ShareRes.WaitMutex();

                if (modbusHelper_D2PLC2.socket != null && modbusHelper_D2PLC2.isConnectPLC == true)
                {
                    ReadD2PLC2();
                }

                d2PLC2ShareRes.ReleaseMutex();

                //复制变量
                CopyPropertiesD2PLC2(d2PLC2Variables, systemVariables);

                try
                {
                    //检测PLC连接状态
                    if (modbusHelper_D2PLC2.socket == null || modbusHelper_D2PLC2.isConnectPLC == false)
                    {
                        systemVariables.D2PLC2CommunicationState = false;
                    }
                    else if (modbusHelper_D2PLC2.socket != null && modbusHelper_D2PLC2.isConnectPLC == true)
                    {
                        systemVariables.D2PLC2CommunicationState = true;
                    }
                }
                catch { }



                Thread.Sleep(1000);
            }
        }
        #endregion

        #region WebSocket通信线程
        /// <summary>
        /// WebSocket通信线程
        /// </summary>
        public void Process_SocketListening()
        {
            Thread.Sleep(1000);


            WebSocketHelper websocket = new WebSocketHelper(this, systemVariables);
            
            websocket.Listen(websocketIp);


        }

        #endregion



        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //方法模块



        #region PLC单个写入方法 （施耐德）
        public void PLCWrite(ModbusHelper modbusHelper, int startAddress, int datatype, string data)
        {
            try
            {
                switch (datatype)
                {
                    case 0: //Boolean
                        modbusHelper.WriteSingleCoilStatus(startAddress, int.Parse(data));
                        break;

                    case 1: //Byte(Byte)
                        modbusHelper.WriteSingleHoldingRegisterValue<byte>(startAddress, byte.Parse(data));
                        break;

                    case 2: //"Int16(Int)"
                        modbusHelper.WriteSingleHoldingRegisterValue<short>(startAddress, short.Parse(data));
                        break;

                    case 3: //"UInt16(Word)"
                        modbusHelper.WriteSingleHoldingRegisterValue<ushort>(startAddress, ushort.Parse(data));
                        break;

                    case 4: //"Int32(DInt)"
                        modbusHelper.WriteSingleHoldingRegisterValue<int>(startAddress, int.Parse(data));
                        break;

                    case 5: //"UInt32(DWord)"
                        modbusHelper.WriteSingleHoldingRegisterValue<uint>(startAddress, uint.Parse(data));
                        break;

                    case 6: //"Float(Real)"
                        modbusHelper.WriteSingleHoldingRegisterValue<float>(startAddress, float.Parse(data));
                        break;

                    case 7: //"Int64"
                        modbusHelper.WriteSingleHoldingRegisterValue<long>(startAddress, long.Parse(data));
                        break;

                    case 8: //"UInt64"
                        modbusHelper.WriteSingleHoldingRegisterValue<ulong>(startAddress, ulong.Parse(data));
                        break;

                    case 9: //"Double"
                        modbusHelper.WriteSingleHoldingRegisterValue<double>(startAddress, double.Parse(data));
                        break;

                    default:
                        DisplayRichTextboxContentAndScroll("未选择数据类型\n");
                        break;
                }
            }
            catch (Exception ex)
            {
                DisplayRichTextboxContentAndScroll("写寄存器出现错误，请检查：\n" + ex.Message);
            }
        }

        #endregion

        #region PLC单个读取方法 （施耐德）
        public object PLCRead(ModbusHelper modbusHelper, int startAddress, Type datatype)
        {
            object result;
            object data = null;

            try
            {
                if (startAddress < 40000)
                {
                    switch (datatype.ToString())
                    {
                        case "System.Boolean": //Boolean
                            bool bo;
                            modbusHelper.ReadSingleCoilStatus(startAddress, out bo);

                            data = bo;
                            break;

                        case "System.Byte": //Byte(Byte)
                            byte b;
                            modbusHelper.ReadSingleHoldingRegisterValue<byte>(startAddress, out b);

                            data = b;
                            break;

                        case "System.Int16": //"Int16(Int)"
                            short s;
                            modbusHelper.ReadSingleHoldingRegisterValue<short>(startAddress, out s);

                            data = s;
                            break;

                        case "System.UInt16": //"UInt16(Word)"
                            ushort us;
                            modbusHelper.ReadSingleHoldingRegisterValue<ushort>(startAddress, out us);

                            data = us;
                            break;

                        case "System.Int32": //"Int32(DInt)"
                            int i;
                            modbusHelper.ReadSingleHoldingRegisterValue<int>(startAddress, out i);

                            data = i;
                            break;

                        case "System.UInt32": //"UInt32(DWord)"
                            uint ui;
                            modbusHelper.ReadSingleHoldingRegisterValue<uint>(startAddress, out ui);

                            data = ui;
                            break;

                        case "System.Single": //"Float(Real)"
                            float f;
                            modbusHelper.ReadSingleHoldingRegisterValue<float>(startAddress, out f);

                            data = f;
                            break;

                        case "System.Int64": //"Int64"
                            long l;
                            modbusHelper.ReadSingleHoldingRegisterValue<long>(startAddress, out l);

                            data = l;
                            break;

                        case "System.UInt64": //"UInt64"
                            ulong ul;
                            modbusHelper.ReadSingleHoldingRegisterValue<ulong>(startAddress, out ul);

                            data = ul;
                            break;

                        case "System.Double": //"Double"
                            double d;
                            modbusHelper.ReadSingleHoldingRegisterValue<double>(startAddress, out d);

                            data = d;
                            break;

                        default:
                            DisplayRichTextboxContentAndScroll("不支持的数据类型\n");
                            break;
                    }
                }
                else if (startAddress >= 40000 && startAddress <= 4000000)
                {
                    startAddress = startAddress - 40000;


                    switch (datatype.ToString())
                    {
                        case "System.Boolean": //Boolean
                            bool bo;
                            modbusHelper.ReadSingleCoilStatus(startAddress, out bo);

                            data = bo;
                            break;

                        case "System.Byte": //Byte(Byte)
                            byte b;
                            modbusHelper.ReadSingleHoldingRegisterValue<byte>(startAddress, out b);

                            data = b;
                            break;

                        case "System.Int16": //"Int16(Int)"
                            short s;
                            modbusHelper.ReadSingleHoldingRegisterValue<short>(startAddress, out s);

                            data = s;
                            break;

                        case "System.UInt16": //"UInt16(Word)"
                            ushort us;
                            modbusHelper.ReadSingleHoldingRegisterValue<ushort>(startAddress, out us);

                            data = us;
                            break;

                        case "System.Int32": //"Int32(DInt)"
                            int i;
                            modbusHelper.ReadSingleHoldingRegisterValue<int>(startAddress, out i);

                            data = i;
                            break;

                        case "System.UInt32": //"UInt32(DWord)"
                            uint ui;
                            modbusHelper.ReadSingleHoldingRegisterValue<uint>(startAddress, out ui);

                            data = ui;
                            break;

                        case "System.Single": //"Float(Real)"
                            float f;
                            modbusHelper.ReadSingleHoldingRegisterValue<float>(startAddress, out f);

                            data = f;
                            break;

                        case "System.Int64": //"Int64"
                            long l;
                            modbusHelper.ReadSingleHoldingRegisterValue<long>(startAddress, out l);

                            data = l;
                            break;

                        case "System.UInt64": //"UInt64"
                            ulong ul;
                            modbusHelper.ReadSingleHoldingRegisterValue<ulong>(startAddress, out ul);

                            data = ul;
                            break;

                        case "System.Double": //"Double"
                            double d;
                            modbusHelper.ReadSingleHoldingRegisterValue<double>(startAddress, out d);

                            data = d;
                            break;

                        default:
                            DisplayRichTextboxContentAndScroll("不支持的数据类型\n");
                            break;
                    }
                }
                // 4016013
                else if (startAddress >= 4000000)
                {
                    startAddress = startAddress - 4000000;// 16013

                    // 取剩下的位数（作为整数）
                    int remainingDigits = startAddress / 100;  // 160

                    // 取个位和十位
                    int tensUnits = startAddress % 100;  //  13



                    modbusHelper.ReadSingleHoldingRegisterBoolValue(remainingDigits, tensUnits, out bool bo);

                    data = bo;



                    //MessageBox.Show("一个Word中的第几位：" + tensUnits);
                    //MessageBox.Show("剩下的位数：" + remainingDigits);
                }
            }
            catch (Exception ex)
            {
                DisplayRichTextboxContentAndScroll("读寄存器出现错误，请检查：\n" + ex.Message);
            }


            result = data;

            return result;
        }
        #endregion

        #region PLC多个读取方法 （施耐德）
        //M区读取方法
        public void ReadLargeBoolBuffer(ModbusHelper modbusHelper, int startAddress, out bool[] value)
        {
            int totalLength = 2400;

            value = new bool[totalLength];

            int readLength = 800; // 每次读取 1000 位
            int offset = 0; // 当前写入位置的偏移量

            // 循环读取，直到读取完所有数据
            while (offset < totalLength)
            {
                bool[] readData;
                bool success = modbusHelper.ReadManyCoilStatus(startAddress, readLength, out readData);

                if (success)
                {
                    // 将读取到的数据拷贝到大缓冲区中的正确位置
                    Array.Copy(readData, 0, value, offset, readData.Length);
                    offset += readData.Length;
                }
                else
                {
                    throw new Exception();
                }

                startAddress += readLength; // 更新起始地址，准备下一次读取
            }
        }

        //MW区读取方法
        public void ReadLargeBuffer(ModbusHelper modbusHelper, int startAddress, int totalLength, int divideNumber, out byte[] value)
        {
            //int totalLength = 1020;// 目前最大支持1020

            value = new byte[totalLength];

            int readLength = totalLength / divideNumber; // 每次读取 204 字节
            int offset = 0; // 当前写入位置的偏移量

            // 循环读取，直到读取完所有数据
            while (offset < totalLength)
            {
                byte[] readData;
                bool success = modbusHelper.ReadManyHoldingRegisterValue(startAddress, readLength, out readData);

                if (success)
                {
                    // 将读取到的数据拷贝到大缓冲区中的正确位置
                    Array.Copy(readData, 0, value, offset, readData.Length);
                    offset += readData.Length;
                }
                else
                {
                    throw new Exception();
                }

                startAddress += readLength / 2; // 更新起始地址，准备下一次读取
            }
        }
        //public void ReadLargeBuffer(ModbusHelper modbusHelper, int startAddress, out byte[] value)
        //{
        //    int totalLength = 1020;// 目前最大支持1020

        //    value = new byte[totalLength];

        //    int readLength = 204; // 每次读取 204 字节
        //    int offset = 0; // 当前写入位置的偏移量

        //    // 循环读取，直到读取完所有数据
        //    while (offset < totalLength)
        //    {
        //        byte[] readData;
        //        bool success = modbusHelper.ReadManyHoldingRegisterValue(startAddress, readLength, out readData);

        //        if (success)
        //        {
        //            // 将读取到的数据拷贝到大缓冲区中的正确位置
        //            Array.Copy(readData, 0, value, offset, readData.Length);
        //            offset += readData.Length;
        //        }
        //        else
        //        {
        //            throw new Exception();
        //        }

        //        startAddress += 102; // 更新起始地址，准备下一次读取
        //    }
        //}
        #endregion

        #region D1PLC1数据映射方法
        public void ReadD1PLC1()
        {
            object result2 = null;

            
            //线圈
            bool[] plc1DdataBuffer1 = new bool[2100];
            //保持寄存器 目前最大支持1020
            byte[] plc1DdataBuffer2 = new byte[1020];

            //在这里把M区和MW区变量全部读进数组
            ReadLargeBoolBuffer(modbusHelper_D1PLC1, 1, out plc1DdataBuffer1);
            ReadLargeBuffer(modbusHelper_D1PLC1, 0, 1020, 5, out plc1DdataBuffer2);


            //遍历键值对d1PLC1
            foreach (int address in d1PLC1addresses.Values)
            {
                if (address < 40000)// 线圈
                {
                    try
                    {
                        string key = d1PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC1Variables).GetProperty(key);


                        object result = plc1DdataBuffer1[address];


                        result2 = result;
                        property.SetValue(d1PLC1Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                else if (address >=  40000 && address <= 4000000 )// %MWX，short类型
                {
                    try
                    {
                        string key = d1PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC1Variables).GetProperty(key);



                        int startAddress = address - 40000;


                        object result = BitConverter.ToInt16(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2 ] }, 0);


                        result2 = result;
                        property.SetValue(d1PLC1Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                // 4016013
                else if (address >= 4000000)// %MWX.X，bool类型
                {
                    try
                    {
                        string key = d1PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC1Variables).GetProperty(key);

                        int startAddress = address - 4000000;// 16013

                        // 取剩下的位数（作为整数）
                        int remainingDigits = startAddress / 100;  // 160

                        // 取个位和十位
                        int tensUnits = startAddress % 100;  //  13

                        //组合成16位二进制数
                        short s = BitConverter.ToInt16(new byte[] { plc1DdataBuffer2[remainingDigits* 2 + 1], plc1DdataBuffer2[remainingDigits * 2 ] }, 0);

                        int index = tensUnits; // 要读取的位
                        bool isBitSet = (s & (1 << index)) != 0;

                        object result = isBitSet;


                        result2 = result;
                        property.SetValue(d1PLC1Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
            }
        }
        #endregion

        #region D1PLC2数据映射方法
        public void ReadD1PLC2()
        {
            object result2 = null;


            //线圈
            bool[] plc1DdataBuffer1 = new bool[2100];
            //保持寄存器 目前最大支持1200
            byte[] plc1DdataBuffer2 = new byte[1200];

            //在这里把M区和MW区变量全部读进数组
            ReadLargeBoolBuffer(modbusHelper_D1PLC2, 1, out plc1DdataBuffer1);
            ReadLargeBuffer(modbusHelper_D1PLC2, 0, 1200, 5, out plc1DdataBuffer2);


            //遍历键值对d1PLC2
            foreach (int address in d1PLC2addresses.Values)
            {
                if (address < 40000)// 线圈
                {
                    try
                    {
                        string key = d1PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC2Variables).GetProperty(key);


                        bool result = plc1DdataBuffer1[address];


                        result2 = result;
                        property.SetValue(d1PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                else if (address >= 40000 && address < 400000)// %MWX，short类型
                {
                    try
                    {
                        string key = d1PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC2Variables).GetProperty(key);



                        int startAddress = address - 40000;


                        short result = BitConverter.ToInt16(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2] }, 0);
                        

                        result2 = result;
                        property.SetValue(d1PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                // 400126
                else if (address >= 400000 && address < 4000000) // %MWX，float类型
                {
                    try
                    {
                        string key = d1PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC2Variables).GetProperty(key);



                        int startAddress = address - 400000; //126


                        //有修改（全部加1查找）
                        float result = BitConverter.ToSingle(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2], plc1DdataBuffer2[startAddress * 2 + 3], plc1DdataBuffer2[startAddress * 2 + 2] }, 0);


                        result2 = result;
                        property.SetValue(d1PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                // 4000202
                else if (address >= 4000000) // %MWX，int类型
                {
                    try
                    {
                        string key = d1PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC2Variables).GetProperty(key);



                        int startAddress = address - 4000000; //202


                        int result = BitConverter.ToInt32(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2], plc1DdataBuffer2[startAddress * 2 + 3], plc1DdataBuffer2[startAddress * 2 + 2] }, 0);


                        result2 = result;
                        property.SetValue(d1PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
            }
        }
        #endregion

        #region D2PLC1数据映射方法
        public void ReadD2PLC1()
        {
            object result2 = null;


            //线圈
            bool[] plc1DdataBuffer1 = new bool[2100];
            //保持寄存器 目前最大支持1020
            byte[] plc1DdataBuffer2 = new byte[1020];

            //在这里把M区和MW区变量全部读进数组
            ReadLargeBoolBuffer(modbusHelper_D2PLC1, 1, out plc1DdataBuffer1);
            ReadLargeBuffer(modbusHelper_D2PLC1, 0, 1020, 5, out plc1DdataBuffer2);


            //遍历键值对d2PLC1
            foreach (int address in d2PLC1addresses.Values)
            {
                if (address < 40000)// 线圈
                {
                    try
                    {
                        string key = d2PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D2PLC1Variables).GetProperty(key);


                        object result = plc1DdataBuffer1[address];


                        result2 = result;
                        property.SetValue(d2PLC1Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                else if (address >= 40000 && address <= 4000000)// %MWX，short类型
                {
                    try
                    {
                        string key = d2PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D2PLC1Variables).GetProperty(key);



                        int startAddress = address - 40000;


                        object result = BitConverter.ToInt16(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2] }, 0);


                        result2 = result;
                        property.SetValue(d2PLC1Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                // 4016013
                else if (address >= 4000000)// %MWX.X，bool类型
                {
                    try
                    {
                        string key = d2PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D2PLC1Variables).GetProperty(key);

                        int startAddress = address - 4000000;// 16013

                        // 取剩下的位数（作为整数）
                        int remainingDigits = startAddress / 100;  // 160

                        // 取个位和十位
                        int tensUnits = startAddress % 100;  //  13

                        //组合成16位二进制数
                        short s = BitConverter.ToInt16(new byte[] { plc1DdataBuffer2[remainingDigits * 2 + 1], plc1DdataBuffer2[remainingDigits * 2] }, 0);

                        int index = tensUnits; // 要读取的位
                        bool isBitSet = (s & (1 << index)) != 0;

                        object result = isBitSet;


                        result2 = result;
                        property.SetValue(d2PLC1Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
            }
        }
        #endregion

        #region D2PLC2数据映射方法
        public void ReadD2PLC2()
        {
            object result2 = null;


            //线圈
            bool[] plc1DdataBuffer1 = new bool[2100];
            //保持寄存器 目前最大支持1200
            byte[] plc1DdataBuffer2 = new byte[1200];

            //在这里把M区和MW区变量全部读进数组
            ReadLargeBoolBuffer(modbusHelper_D2PLC2, 1, out plc1DdataBuffer1);
            ReadLargeBuffer(modbusHelper_D2PLC2, 0, 1200, 5, out plc1DdataBuffer2);


            //遍历键值对d2PLC2
            foreach (int address in d2PLC2addresses.Values)
            {
                if (address < 40000)// 线圈
                {
                    try
                    {
                        string key = d2PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D2PLC2Variables).GetProperty(key);


                        bool result = plc1DdataBuffer1[address];


                        result2 = result;
                        property.SetValue(d2PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                else if (address >= 40000 && address < 400000)// %MWX，short类型
                {
                    try
                    {
                        string key = d2PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D2PLC2Variables).GetProperty(key);



                        int startAddress = address - 40000;


                        short result = BitConverter.ToInt16(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2] }, 0);


                        result2 = result;
                        property.SetValue(d2PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                // 400126
                else if (address >= 400000 && address < 4000000) // %MWX，float类型
                {
                    try
                    {
                        string key = d2PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D2PLC2Variables).GetProperty(key);



                        int startAddress = address - 400000; //126


                        //有修改（全部加1查找）
                        float result = BitConverter.ToSingle(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2], plc1DdataBuffer2[startAddress * 2 + 3], plc1DdataBuffer2[startAddress * 2 + 2] }, 0);


                        result2 = result;
                        property.SetValue(d2PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
                // 4000202
                else if (address >= 4000000) // %MWX，int类型
                {
                    try
                    {
                        string key = d2PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D2PLC2Variables).GetProperty(key);



                        int startAddress = address - 4000000; //202


                        int result = BitConverter.ToInt32(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2], plc1DdataBuffer2[startAddress * 2 + 3], plc1DdataBuffer2[startAddress * 2 + 2] }, 0);


                        result2 = result;
                        property.SetValue(d2PLC2Variables, result);
                    }
                    catch (OverflowException e)
                    {
                        MessageBox.Show(result2.ToString());
                        MessageBox.Show(e.ToString());
                    }
                }
            }
        }
        #endregion

        #region 变量对象属性分类映射方法
        public static void CopyPropertiesD1PLC1(D1PLC1Variables a, SystemVariables c)
        {
            Type aType = typeof(D1PLC1Variables);
            Type cType = typeof(SystemVariables);

            PropertyInfo[] aProperties = aType.GetProperties();
            PropertyInfo[] cProperties = cType.GetProperties();

            foreach (PropertyInfo aProp in aProperties)
            {
                PropertyInfo cProp = cProperties.FirstOrDefault(p => p.Name == aProp.Name && p.PropertyType == aProp.PropertyType);
                if (cProp != null)
                {
                    cProp.SetValue(c, aProp.GetValue(a));
                }
            }
        }
        public static void CopyPropertiesD1PLC2(D1PLC2Variables b, SystemVariables c)
        {
            Type bType = typeof(D1PLC2Variables);
            Type cType = typeof(SystemVariables);

            PropertyInfo[] bProperties = bType.GetProperties();
            PropertyInfo[] cProperties = cType.GetProperties();

            foreach (PropertyInfo bProp in bProperties)
            {
                PropertyInfo cProp = cProperties.FirstOrDefault(p => p.Name == bProp.Name && p.PropertyType == bProp.PropertyType);
                if (cProp != null)
                {
                    cProp.SetValue(c, bProp.GetValue(b));
                }
            }
        }
        public static void CopyPropertiesD2PLC1(D2PLC1Variables b, SystemVariables c)
        {
            Type bType = typeof(D2PLC1Variables);
            Type cType = typeof(SystemVariables);

            PropertyInfo[] bProperties = bType.GetProperties();
            PropertyInfo[] cProperties = cType.GetProperties();

            foreach (PropertyInfo bProp in bProperties)
            {
                PropertyInfo cProp = cProperties.FirstOrDefault(p => p.Name == bProp.Name && p.PropertyType == bProp.PropertyType);
                if (cProp != null)
                {
                    cProp.SetValue(c, bProp.GetValue(b));
                }
            }
        }
        public static void CopyPropertiesD2PLC2(D2PLC2Variables b, SystemVariables c)
        {
            Type bType = typeof(D2PLC2Variables);
            Type cType = typeof(SystemVariables);

            PropertyInfo[] bProperties = bType.GetProperties();
            PropertyInfo[] cProperties = cType.GetProperties();

            foreach (PropertyInfo bProp in bProperties)
            {
                PropertyInfo cProp = cProperties.FirstOrDefault(p => p.Name == bProp.Name && p.PropertyType == bProp.PropertyType);
                if (cProp != null)
                {
                    cProp.SetValue(c, bProp.GetValue(b));
                }
            }
        }
        #endregion

        #region 数据库方法

        //数据库操作日志添加方法
        public static void OperationLogs(string operationinfo)
        {
            //记录操作日志，将数据存入数据库
            string insertSql = String.Format("insert into criticalparameter (time, M_ScraperMo) values('{0}','{1}')", DateTime.Now, operationinfo);//debug0328 (time, info)

            ExecuteSql(insertSql);


            //删除debug0328表三个月前的数据
            string deleteSql = String.Format("delete from criticalparameter where time < '{0}' ;", DateTime.Now.AddMonths(-3));//debug0328
            ExecuteSql(deleteSql);
        }

        public static string connstr = "server=" + "127.0.0.1" + ";database= " + "csr" + ";username=" + "root" + ";password=" + "123456" + ";Charset=utf8";

        //执行sql语句（MySQL短连接）
        public static void ExecuteSql(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        conn.Close();
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region 取bit位数方法
        public void Number(int startAddress)
        {
            startAddress = startAddress - 4000000;// 16013

            // 取剩下的位数（作为整数）
            int remainingDigits = startAddress / 100;  // 160

            // 取个位和十位
            int tensUnits = startAddress % 100;  //  13

            MessageBox.Show("一个Word中的第几位：" + tensUnits);
            MessageBox.Show("剩下的位数：" + remainingDigits);
        }
        #endregion



        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //其他模块



        #region 滚动式RichTextBox日志显示方法
        /// <summary>
        /// 大于100时 就清空文本框
        /// </summary>
        public int displaySequence = 0;
        /// <summary>
        /// 异常日志展示
        /// </summary>
        /// <param name="addContent">异常文本</param>
        public void DisplayRichTextboxContentAndScroll(object addContent)
        {
            try
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (displaySequence >= 100)
                    {
                        uc5.rtxtDisplay.Clear();
                        displaySequence = 0;
                    }
                    uc5.rtxtDisplay.AppendText(addContent + "\n");
                    uc5.rtxtDisplay.ScrollToCaret();
                }));
            }
            catch (Exception)
            {
                //MessageBox.Show("Invok错误调用");
            }
        }
        #endregion

        #region 用户控件UI事件

        //页面显示方法
        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;

            // 获取控件的名称
            string controlName = c.Name;

            try
            {
                PagePanel.Controls.Clear();
                PagePanel.Controls.Add(c);
            }
            catch { }

        }

        //颜色改变UI事件
        private void ChangeColor(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // 遍历所有按钮，将被点击的按钮文本颜色设为红色，其他按钮文本颜色设为默认颜色
            foreach (Button button in buttons)
            {
                if (button == clickedButton)
                {
                    button.ForeColor = Color.Yellow;
                    button.BackColor = Color.FromArgb(16, 78, 139);
                }
                else
                {
                    button.ForeColor = (Color)button.Tag;
                    button.BackColor = Color.FromArgb(51, 63, 98);
                }
            }
        }

        #region 页面切换按钮
        private void Page1Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc1);
        }
        private void Page2Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc2);
        }

        private void Page3Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc3);

            uc3_1.dateTimePickerEnd.Invoke(new MethodInvoker(() =>
            {
                uc3_1.dateTimePickerEnd.Value = DateTime.Now;
            }));
        }

        private void Page5Btn_Click(object sender, EventArgs e)
        {
            AddControlsToPanel(uc5);
        }
        #endregion

        #endregion

        #region 主窗体UI事件

        //退出确认
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //让用户选择点击
            DialogResult result = MessageBox.Show("是否确认关闭？", "正在关闭系统...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            //判断是否取消事件
            if (result == DialogResult.No)
            {
                //取消退出
                e.Cancel = true;
            }
        }
        //按钮退出系统
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否确认关闭？", "正在关闭系统...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                //关闭TCP连接
                //modbusHelper2.CloseConnect();


                Environment.Exit(0);
            }
            else if (dr == DialogResult.No)
            {
            }
        }

        //按钮最小化系统
        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        //主窗体退出时占用资源销毁
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            modbusHelper_D1PLC1.CloseConnect();
            modbusHelper_D1PLC2.CloseConnect();
            modbusHelper_D2PLC1.CloseConnect();
            modbusHelper_D2PLC2.CloseConnect();


            string processesName = "ShenYangRemoteSystem";
            System.Diagnostics.Process[] targetProcess = System.Diagnostics.Process.GetProcessesByName("ShenYangRemoteSystem");
            foreach (System.Diagnostics.Process process in targetProcess)
            {
                if (processesName == process.ProcessName)
                {
                    process.Kill();
                }
            }


            Environment.Exit(0);
        }
        #endregion

        #region 信号量
        public class d1PLC1ShareRes
        {
            public int count = 0;

            public static Mutex mutex = new Mutex();
            public static void WaitMutex()
            {
                mutex.WaitOne();

            }

            public static void ReleaseMutex()
            {
                mutex.ReleaseMutex();
            }
        }
        public class d1PLC2ShareRes
        {
            public int count = 0;

            public static Mutex mutex = new Mutex();
            public static void WaitMutex()
            {
                mutex.WaitOne();

            }

            public static void ReleaseMutex()
            {
                mutex.ReleaseMutex();
            }
        }
        public class d2PLC1ShareRes
        {
            public int count = 0;

            public static Mutex mutex = new Mutex();
            public static void WaitMutex()
            {
                mutex.WaitOne();

            }

            public static void ReleaseMutex()
            {
                mutex.ReleaseMutex();
            }
        }
        public class d2PLC2ShareRes
        {
            public int count = 0;

            public static Mutex mutex = new Mutex();
            public static void WaitMutex()
            {
                mutex.WaitOne();

            }

            public static void ReleaseMutex()
            {
                mutex.ReleaseMutex();
            }
        }
        #endregion



        //添加配置文件之默认信息
        private void button1_Click(object sender, EventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); //首先打开配置文件

            cfa.AppSettings.Settings["Port"].Value = "COM1";
            cfa.Save();  //保存配置文件
            ConfigurationManager.RefreshSection("appSettings");  //刷新配置文件

            MessageBox.Show(cfa.AppSettings.Settings["Port"].Value.ToString());
        }
    }
}
