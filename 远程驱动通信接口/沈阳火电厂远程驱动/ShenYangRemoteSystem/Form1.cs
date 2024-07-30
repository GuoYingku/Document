using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            

            Thread thread1 = new Thread(new ThreadStart(ConnectToDatabase));
            Thread thread2 = new Thread(new ThreadStart(Process_PLC_Main));
            Thread thread3 = new Thread(new ThreadStart(Process_D1PLC_DataGet));
            Thread thread4 = new Thread(new ThreadStart(Process_D2PLC_DataGet));
            Thread thread5 = new Thread(new ThreadStart(Process_SocketListening));


            //thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();


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

            //222  Boolean
            d1PLC1addresses.Add("ElectricRoomEmergencyStopButtonAction", 4005200);// Word
            d1PLC1addresses.Add("CabinEmergencyStopButtonAction", 4005201);// Word
            d1PLC1addresses.Add("EmergencyStopRelayNot", 4005202);// Word
            d1PLC1addresses.Add("TransformerOverheatAlarm", 4005203);// Word
            d1PLC1addresses.Add("ElectricRoomPLCModulePowerFault", 4005204);// Word
            d1PLC1addresses.Add("CabinPLCModulePowerFault", 4005205);// Word
            d1PLC1addresses.Add("ElectricRoomFireAlarm", 4005206);// Word
            d1PLC1addresses.Add("CabinFireAlarm", 4005207);// Word
            d1PLC1addresses.Add("SuspensionBeltEmergencyStop", 4005208);// Word
            d1PLC1addresses.Add("TailCarBeltEmergencyStopSwitch", 4005209);// Word
            d1PLC1addresses.Add("LargeCarMainCircuitBreakerFault", 4010000);// Word
            d1PLC1addresses.Add("LargeCarMotorCircuitBreakerFault", 4010001);// Word
            d1PLC1addresses.Add("LargeCarBrakeCircuitBreakerFault", 4010002);// Word
            d1PLC1addresses.Add("Spare1", 4010003);// Word
            d1PLC1addresses.Add("CarFrequencyConverterFault", 4010004);// Word
            d1PLC1addresses.Add("LargeCarBrakeResistorOverheatJump", 4010005);// Word
            d1PLC1addresses.Add("CableReelMainCircuitBreakerFault", 4010006);// Word
            d1PLC1addresses.Add("CableReelMotorOverloading", 4010007);// Word
            d1PLC1addresses.Add("PowerReelCableOverLooseAlarm", 4010008);// Word
            d1PLC1addresses.Add("PowerReelCableOverTightAlarm", 4010009);// Word
            d1PLC1addresses.Add("PowerReelFullDiskAlarm", 4010010);// Word
            d1PLC1addresses.Add("PowerReelEmptyDiskAlarm", 4010011);// Word
            d1PLC1addresses.Add("LargeCarOperationHandleFault", 4010012);// Word
            d1PLC1addresses.Add("RotaryMainCircuitBreakerFault", 4012000);// Word
            d1PLC1addresses.Add("RotaryBrakeOverloadAlarm", 4012001);// Word
            d1PLC1addresses.Add("RotaryFanOverloadAlarm", 4012002);// Word
            d1PLC1addresses.Add("RotaryFrequencyConverterFaulting", 4012003);// Word
            d1PLC1addresses.Add("RotaryBrakeResistorOverheatSwitching", 4012004);// Word
            d1PLC1addresses.Add("RotaryOverTorqueSwitch", 4012005);// Word
            d1PLC1addresses.Add("ReversalHandleFault", 4012006);// Word
            d1PLC1addresses.Add("LinkedBucketWheelNotRunning", 4012007);// Word
            d1PLC1addresses.Add("VariableFrequencyMainCircuitBreakerFault", 4014000);// Word
            d1PLC1addresses.Add("VariableFrequencyMotorOverload", 4014001);// Word
            d1PLC1addresses.Add("VariableFrequencyPumpClogged", 4014005);// Word
            d1PLC1addresses.Add("VariableFrequencyPumpStationHighTemperatureAlarm", 4014006);// Word
            d1PLC1addresses.Add("VariableFrequencyOilTankLowLevelAlarm", 4014007);// Word
            d1PLC1addresses.Add("Spare2", 4014008);// Word
            d1PLC1addresses.Add("VariableFrequencyHandleFault", 4014009);// Word
            d1PLC1addresses.Add("SuspendedBeltCircuitBreakerFault", 4016000);// Word
            d1PLC1addresses.Add("SuspendedBeltMotorOverload", 4016001);// Word
            d1PLC1addresses.Add("SuspendedBeltSecondLevelDeviationSwitch", 4016002);// Word
            d1PLC1addresses.Add("SuspendedBeltEmergencyStop", 4016003);// Word
            d1PLC1addresses.Add("SuspendedBeltSlip", 4016004);// Word
            d1PLC1addresses.Add("SuspendedBeltLongitudinalTearSwitch", 4016005);// Word
            d1PLC1addresses.Add("CentralHopperCloggedDetectionSwitch", 4016006);// Word
            d1PLC1addresses.Add("StackingSwitchFault", 4016007);// Word
            d1PLC1addresses.Add("CentralControlRoomNoStackingCommand", 4016008);// Word
            d1PLC1addresses.Add("BucketWheelMotorMainCircuitBreakerFault", 4018000);// Word
            d1PLC1addresses.Add("BucketWheelMotorOverloading", 4018001);// Word
            d1PLC1addresses.Add("BucketWheelOverTorqueSwitching", 4018002);// Word
            d1PLC1addresses.Add("BucketWheelTemperatureUpperLimitAlarm", 4018003);// Word
            d1PLC1addresses.Add("ClampingDeviceMainCircuitBreakerFault", 4020000);// Word
            d1PLC1addresses.Add("ClampingDeviceMotorOverload", 4020001);// Word
            d1PLC1addresses.Add("LeftClampingDeviceTimeout", 4020002);// Word
            d1PLC1addresses.Add("RightClampingDeviceTimeout", 4020003);// Word
            d1PLC1addresses.Add("StrongWindAlarm", 4020004);// Word
            d1PLC1addresses.Add("DryFogSystemLowAirPressure", 4022000);// Word
            d1PLC1addresses.Add("DryFogSystemLowWaterPressure", 4022001);// Word
            d1PLC1addresses.Add("DryFogSystemFilterClogged", 4022002);// Word
            d1PLC1addresses.Add("DryFogSystemWaterTankLowLevel", 4022003);// Word
            d1PLC1addresses.Add("DiversionPlateCircuitBreakerFault", 4024000);// Word
            d1PLC1addresses.Add("DiversionPlateMotorOverload", 4024001);// Word
            d1PLC1addresses.Add("DiversionPlateTimeout", 4024002);// Word
            d1PLC1addresses.Add("CentralControlRoomNoStackingOrDiversionCommand", 4024003);// Word
            d1PLC1addresses.Add("BucketWheelFeederCircuitBreakerFault", 4025000);// Word
            d1PLC1addresses.Add("BucketWheelFeederMotorOverload", 4025001);// Word
            d1PLC1addresses.Add("BucketWheelFeederTimeout", 4025002);// Word
            d1PLC1addresses.Add("CentralControlRoomNoStackingUnloadingCommand", 4025003);// Word
            d1PLC1addresses.Add("TailCarBeltFirstLevelDeviation", 4026000);// Word
            d1PLC1addresses.Add("TailCarBeltSecondLevelDeviation", 4026001);// Word
            d1PLC1addresses.Add("TailCarBeltLongitudinalTear", 4026002);// Word
            d1PLC1addresses.Add("Spare3", 4026003);// Word
            d1PLC1addresses.Add("VibrationMotorCircuitBreakerFault", 4028000);// Word
            d1PLC1addresses.Add("VibrationMotorOverloading", 4028001);// Word

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

            //351  Boolean
            d1PLC1addresses.Add("UnmannedEmergencyStop", 4005210);// Word
            d1PLC1addresses.Add("RemoteEmergencyStoping", 4005211);// Word
            d1PLC1addresses.Add("LargeVehicleMotor1OvertemperatureAlarm", 4010200);// Word
            d1PLC1addresses.Add("LargeVehicleMotor2OvertemperatureAlarm", 4010201);// Word 
            d1PLC1addresses.Add("LargeVehicleMotor3OvertemperatureAlarm", 4010202);// Word
            d1PLC1addresses.Add("LargeVehicleMotor4OvertemperatureAlarm", 4010203);// Word
            d1PLC1addresses.Add("LargeVehicleMotor5OvertemperatureAlarm", 4010204);// Word
            d1PLC1addresses.Add("LargeVehicleMotor6OvertemperatureAlarm", 4010205);// Word
            d1PLC1addresses.Add("WalkingReducerBearingTemperatureUpperLimitAlarm", 4010206);// Word
            d1PLC1addresses.Add("WalkingReducerBearingTemperatureLowerLimitAlarm", 4010207);// Word
            d1PLC1addresses.Add("WalkingReducerOilTemperatureUpperLimitAlarm", 4010208);// Word
            d1PLC1addresses.Add("WalkingReducerOilTemperatureLowerLimitAlarm", 4010209);// Word
            d1PLC1addresses.Add("ReversalTemperatureUpperLimitAlarm", 4012008);// Word
            d1PLC1addresses.Add("ReversalTemperatureLowerLimitAlarm", 4012009);// Word
            d1PLC1addresses.Add("BrokenBeltCaptureAlarming", 4016009);// Word
            d1PLC1addresses.Add("SuspendedBeltTemperatureUpperLimitAlarm", 4016010);// Word
            d1PLC1addresses.Add("SuspendedBeltTemperatureLowerLimitAlarm", 4016011);// Word
            d1PLC1addresses.Add("SuspendedBeltRollerBearingTemperatureUpperLimitAlarm", 4016012);// Word
            d1PLC1addresses.Add("SuspendedBeltRollerBearingTemperatureLowerLimitAlarm", 4016013);// Word
            d1PLC1addresses.Add("BucketWheelTemperatureLowerLimitAlarm", 4018004);// Word

            //371
            d1PLC1addresses.Add("CableRollerContactorAuxiliaryContactFault", 1709);
            d1PLC1addresses.Add("DriverRoomBalancePumpMotorNotRunning", 1731);
            d1PLC1addresses.Add("DriverRoomBalancePumpMotorAuxiliaryContactFault", 1733);
            d1PLC1addresses.Add("Remote", 295);


            d1PLC1addresses.Add("DriverRoomRiseButton", 2018);
            d1PLC1addresses.Add("DriverRoomDescentButton", 2022);

            #endregion

            #region 键值对d1PLC2

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
                        if (modbusHelper_D1PLC2.ConnectPLC("84.3.243.106", int.Parse("502")))
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
                        if (modbusHelper_D2PLC1.ConnectPLC("84.3.243.106", int.Parse("502")))
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
                        if (modbusHelper_D2PLC2.ConnectPLC("84.3.243.106", int.Parse("502")))
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
        private void Process_D1PLC_DataGet()
        {
            while (true)
            {
                //Stopwatch stopwatch1 = new Stopwatch();
                //stopwatch1.Start();


                ShareRes.WaitMutex();
                //if (modbusHelper_D1PLC1.socket != null && modbusHelper_D1PLC1.isConnectPLC == true && modbusHelper_D1PLC2.socket != null && modbusHelper_D1PLC2.isConnectPLC == true)
                //{
                //    ReadD1PLC();
                //}
                if (modbusHelper_D1PLC1.socket != null && modbusHelper_D1PLC1.isConnectPLC)
                {
                    ReadD1PLC();
                }
                ShareRes.ReleaseMutex();

                ////stopwatch1.Stop();


                //复制变量
                CopyPropertiesD1PLC1(d1PLC1Variables, systemVariables);

                string responseMessage = JsonConvert.SerializeObject(systemVariables, settings);

                using (Form form = new Form())
                {
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
                }


                //MessageBox.Show(stopwatch1.ElapsedMilliseconds.ToString());


                Thread.Sleep(1000);
            }
        }
        
        private void Process_D2PLC_DataGet()
        {
            while (true)
            {
                ShareRes.WaitMutex();

                if (modbusHelper_D2PLC1.socket != null && modbusHelper_D2PLC1.isConnectPLC == true && modbusHelper_D2PLC2.socket != null && modbusHelper_D2PLC2.isConnectPLC == true)
                {
                    ReadD2PLC();
                }

                ShareRes.ReleaseMutex();

                //复制变量
                CopyPropertiesD2PLC1(d2PLC1Variables, systemVariables);

                try
                {
                    //检测PLC连接状态
                    if (modbusHelper_D2PLC1.socket == null || modbusHelper_D2PLC1.isConnectPLC == false && modbusHelper_D2PLC2.socket == null || modbusHelper_D2PLC2.isConnectPLC == false)
                    {   
                        //systemVariables.PLCSignal = false;
                    }
                    else if (modbusHelper_D2PLC1.socket != null && modbusHelper_D2PLC1.isConnectPLC == true && modbusHelper_D2PLC2.socket != null && modbusHelper_D2PLC2.isConnectPLC == true)
                    {
                        //systemVariables.PLCSignal = true;
                    }
                }
                catch { }

                #region 随机数

                //Random random = new Random();

                //// 遍历所有属性，给它们赋随机数值
                //foreach (var property in systemVariables.GetType().GetProperties())
                //{
                //    if (property.PropertyType == typeof(bool))
                //    {
                //        property.SetValue(systemVariables, random.Next(2) == 0);
                //    }
                //    else if (property.PropertyType == typeof(float))
                //    {
                //        property.SetValue(systemVariables, (float)random.NextDouble());
                //    }
                //    else if (property.PropertyType == typeof(long))
                //    {
                //        property.SetValue(systemVariables, random.Next());
                //    }
                //    else if (property.PropertyType == typeof(ushort))
                //    {
                //        property.SetValue(systemVariables, (ushort)random.Next(500));
                //    }
                //    else if (property.PropertyType == typeof(int))
                //    {
                //        property.SetValue(systemVariables, (int)random.Next(500));
                //    }
                //}


                #endregion

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
            
            websocket.Listen("127.0.0.1:11000");


        }

        #endregion


        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //方法模块



        #region PLC多个读取方法 （施耐德）
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

        public void ReadLargeBuffer(ModbusHelper modbusHelper, int startAddress, out byte[] value)
        {
            int totalLength = 1020;// 目前最大支持1020

            value = new byte[totalLength];

            int readLength = 204; // 每次读取 200 字节
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

                startAddress += 102; // 更新起始地址，准备下一次读取
            }
        }
        #endregion

        #region D1PLC数据映射方法
        public void ReadD1PLC()
        {
            object result2 = null;



            
            //线圈
            bool[] plc1DdataBuffer1 = new bool[2100];
            //保持寄存器 目前最大支持1020
            byte[] plc1DdataBuffer2 = new byte[1020];

            //在这里把M区和MW区变量全部读进数组
            ReadLargeBoolBuffer(modbusHelper_D1PLC1, 1, out plc1DdataBuffer1);
            ReadLargeBuffer(modbusHelper_D1PLC1, 0, out plc1DdataBuffer2);



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
                else if (address >=  40000 && address <= 4000000 )// %MWX
                {
                    try
                    {
                        string key = d1PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                        PropertyInfo property = typeof(D1PLC1Variables).GetProperty(key);



                        int startAddress = address - 40000;


                        object result = BitConverter.ToUInt16(new byte[] { plc1DdataBuffer2[startAddress * 2 + 1], plc1DdataBuffer2[startAddress * 2 ] }, 0);


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
                else if (address >= 4000000)// %MWX.X
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
                        ushort us = BitConverter.ToUInt16(new byte[] { plc1DdataBuffer2[remainingDigits* 2 + 1], plc1DdataBuffer2[remainingDigits * 2 ] }, 0);

                        int index = tensUnits; // 要读取的位
                        bool isBitSet = (us & (1 << index)) != 0;

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










            ////遍历键值对d1PLC1
            //foreach (int address in d1PLC1addresses.Values)
            //{
            //    try
            //    {
            //        string key = d1PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
            //        PropertyInfo property = typeof(D1PLC1Variables).GetProperty(key);

            //        // 读取地址并处理返回的数据
            //        object result = PLCRead(modbusHelper_D1PLC1, address, property.PropertyType);

            //        result2 = result;
            //        property.SetValue(d1PLC1Variables, result);
            //    }
            //    catch (OverflowException e)
            //    {
            //        MessageBox.Show(result2.ToString());
            //        MessageBox.Show(e.ToString());
            //    }
            //}




            //遍历键值对d1PLC2
            foreach (int address in d1PLC2addresses.Values)
            {
                try
                {
                    string key = d1PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                    PropertyInfo property = typeof(D1PLC2Variables).GetProperty(key);

                    // 读取地址并处理返回的数据
                    object result = PLCRead(modbusHelper_D1PLC2, address, property.PropertyType);

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
        #endregion

        #region D2PLC数据映射方法
        public void ReadD2PLC()
        {
            object result2 = null;

            //遍历键值对d2PLC1
            foreach (int address in d2PLC1addresses.Values)
            {
                try
                {
                    string key = d2PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                    PropertyInfo property = typeof(D2PLC1Variables).GetProperty(key);

                    // 读取地址并处理返回的数据
                    object result = PLCRead(modbusHelper_D2PLC1, address, property.PropertyType);

                    result2 = result;
                    property.SetValue(d2PLC1Variables, result);
                }
                catch (OverflowException e)
                {
                    MessageBox.Show(result2.ToString());
                    MessageBox.Show(e.ToString());
                }
            }
            //遍历键值对d2PLC2
            foreach (int address in d2PLC2addresses.Values)
            {
                try
                {
                    string key = d2PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                    PropertyInfo property = typeof(D2PLC2Variables).GetProperty(key);

                    // 读取地址并处理返回的数据
                    object result = PLCRead(modbusHelper_D2PLC2, address, property.PropertyType);

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
        #endregion

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
                else if(startAddress >= 40000 && startAddress <= 4000000)
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

        #region 对象属性映射方法
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

        #region 取位数方法
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
        public class ShareRes
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


        //添加配置文件信息
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
