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

            thread1.Start();
            thread2.Start();
            //thread3.Start();
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
            d1PLC1addresses.Add("LargeCarElectricCurrent", 40444);
            d1PLC1addresses.Add("RotaryElectricCurrent", 40454);
            d1PLC1addresses.Add("SuspensionBeltElectricCurrent", 40464);
            d1PLC1addresses.Add("BucketWheelElectricCurrent", 40474);
            d1PLC1addresses.Add("LargeCarTravelDistance", 40400);
            d1PLC1addresses.Add("RotaryAngle", 40418);
            d1PLC1addresses.Add("VariableAmplitudeAngle", 40428);

            d1PLC1addresses.Add("VacuumCircuitBreakerClosed", 10100);
            d1PLC1addresses.Add("LowVoltageControlPowerClosed", 10102);
            d1PLC1addresses.Add("LowVoltagePowerClosed", 10103);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationLowOilLevel", 10105);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationOilBlockage", 10106);
            d1PLC1addresses.Add("AllowBucketWheelMaterialLoading", 10112);
            d1PLC1addresses.Add("AllowBucketWheelMaterialUnloading", 10113);
            d1PLC1addresses.Add("LargeCarMainCircuitBreaker", 10121);
            d1PLC1addresses.Add("LargeCarMotorCircuitBreaker", 10122);
            d1PLC1addresses.Add("LargeCarBrakeCircuitBreaker", 10123);
            d1PLC1addresses.Add("LargeCarFrequencyConverterContact", 10125);
            d1PLC1addresses.Add("LargeCarBrakeContact", 10126);
            d1PLC1addresses.Add("LargeCarFrequencyConverterFault", 10128);
            d1PLC1addresses.Add("LargeCarBrakeResistorOverheatSwitch", 10129);
            d1PLC1addresses.Add("LargeCarForwardLimit", 10130);
            d1PLC1addresses.Add("LargeCarReverseLimit", 10131);
            d1PLC1addresses.Add("LargeCarForwardExtremeLimit", 10132);
            d1PLC1addresses.Add("LargeCarReverseExtremeLimit", 10133);
            d1PLC1addresses.Add("CableReelMainCircuitBreaker", 10140);
            d1PLC1addresses.Add("CableReelMotorOverload", 10141);
            d1PLC1addresses.Add("PowerReelContact", 10142);
            d1PLC1addresses.Add("ReelOverTensionLimit1", 10200);
            d1PLC1addresses.Add("ReelOverLooseLimit1", 10201);
            d1PLC1addresses.Add("VibrationMotorMainCircuitBreaker", 10186);
            d1PLC1addresses.Add("RotaryBrakeOverload", 10163);
            d1PLC1addresses.Add("RotaryMainCircuitBreaker", 10162);
            d1PLC1addresses.Add("ClampMotorOverload", 10149);
            d1PLC1addresses.Add("LeftAnchorLiftLimit", 10150);
            d1PLC1addresses.Add("RightAnchorLiftLimit", 10151);
            d1PLC1addresses.Add("LeftClampRelaxLimit", 10152);
            d1PLC1addresses.Add("RightClampRelaxLimit", 10153);
            d1PLC1addresses.Add("BucketWheelMotorMainCircuitBreaker", 10171);
            d1PLC1addresses.Add("RotaryFanContact", 10167);
            d1PLC1addresses.Add("RotaryBrakeContact", 10166);
            d1PLC1addresses.Add("RotaryFrequencyConverterContact", 10165);
            d1PLC1addresses.Add("SystemInterlockSwitch", 10270);
            d1PLC1addresses.Add("VariableAmplitudeMainCircuitBreaker", 10180);
            d1PLC1addresses.Add("VariableAmplitudeMotorOverload", 10181);
            d1PLC1addresses.Add("VariableAmplitudeMotorContact", 10182);
            d1PLC1addresses.Add("VariableAmplitudeHeaterContact", 10183);
            d1PLC1addresses.Add("VariableAmplitudeFanContact", 10184);
            d1PLC1addresses.Add("SuspensionBeltMainCircuitBreaker", 10188);
            d1PLC1addresses.Add("SuspensionBeltMotorOverload", 10189);

            //51
            d1PLC1addresses.Add("SuspensionBeltMaterialLoadingRunningContact", 10190);
            d1PLC1addresses.Add("SuspensionBeltMaterialUnloadingRunningContact", 10191);
            d1PLC1addresses.Add("SuspensionBeltBrakeContact", 10192);
            d1PLC1addresses.Add("CentralMaterialDustDetectionSwitch", 10175);
            d1PLC1addresses.Add("DiversionBaffleMainCircuitBreaker", 10220);
            d1PLC1addresses.Add("VibrationMotorOverload", 10187);
            d1PLC1addresses.Add("ClampMainCircuitBreaker", 10148);
            d1PLC1addresses.Add("BucketWheelMotorOverload", 10172);
            d1PLC1addresses.Add("BucketWheelLubricationPumpContact", 10174);
            d1PLC1addresses.Add("RotaryLeftTurnLimit", 10304);
            d1PLC1addresses.Add("RotaryRightTurnLimit", 10305);
            d1PLC1addresses.Add("RotaryLeftTurnExtremeLimit", 10306);
            d1PLC1addresses.Add("RotaryRightTurnExtremeLimit", 10307);
            d1PLC1addresses.Add("RotaryLeftTurnForbiddenZoneLimit", 10308);
            d1PLC1addresses.Add("RotaryRightTurnForbiddenZoneLimit", 10309);
            d1PLC1addresses.Add("RotaryZeroPositionLimit", 10312);
            d1PLC1addresses.Add("BucketWheelOverTorqueSwitch", 10355);
            d1PLC1addresses.Add("BucketWheelForcedLubricationFlowSwitch", 10354);
            d1PLC1addresses.Add("VariableAmplitudeUpperLimit", 10360);
            d1PLC1addresses.Add("VariableAmplitudeLowerLimit", 10361);
            d1PLC1addresses.Add("VariableAmplitudeUpperExtremeLimit", 10362);
            d1PLC1addresses.Add("VariableAmplitudeLowerExtremeLimit", 10363);
            d1PLC1addresses.Add("VariableAmplitudeLowerForbiddenZoneLimit", 10364);
            d1PLC1addresses.Add("CabinFrontBalanceLimit", 10365);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterStartup", 10326);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterStop", 10327);
            d1PLC1addresses.Add("VariableAmplitudeFanStop", 10329);
            d1PLC1addresses.Add("VariableAmplitudeFanStartup", 10328);
            d1PLC1addresses.Add("VariableAmplitudeOilLevelLowSignal", 10332);
            d1PLC1addresses.Add("VariableAmplitudePumpStationOverheatAlarm", 10330);
            d1PLC1addresses.Add("VariableAmplitudeOilLevelVeryLowSignal", 10331);
            d1PLC1addresses.Add("RotaryCentralizedLubricationLowOilLevelFault", 10349);
            d1PLC1addresses.Add("LargeCarFrequencyConverterPowerOn", 10409);
            d1PLC1addresses.Add("LargeCarBrakeOpen", 10410);
            d1PLC1addresses.Add("LargeCarFrequencyConverterFaultReset", 10414);
            d1PLC1addresses.Add("LargeCarReverseCommand", 10413);
            d1PLC1addresses.Add("LargeCarHighLowSpeedSelection", 10415);
            d1PLC1addresses.Add("BucketWheelMaterialLoadingRunning", 10403);
            d1PLC1addresses.Add("BucketWheelFault", 10406);
            d1PLC1addresses.Add("SuspensionBeltFirstLevelDeviationSwitch", 10340);
            d1PLC1addresses.Add("SuspensionBeltSecondLevelDeviationSwitch", 10341);
            d1PLC1addresses.Add("SuspensionBeltEmergencyStopSwitch", 10342);
            d1PLC1addresses.Add("SuspensionBeltSpeedDetectionSwitch", 10343);
            d1PLC1addresses.Add("SuspensionBeltMaterialFlowDetectionSwitch", 10344);
            d1PLC1addresses.Add("SuspensionBeltLongitudinalTearSwitch", 10345);
            d1PLC1addresses.Add("RotaryCentralizedLubricationOilBlockageFault", 10350);
            d1PLC1addresses.Add("LargeCarForwardCommand", 10412);
            d1PLC1addresses.Add("VariableAmplitudeOilPumpMotorRunning", 10440);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterRunning", 10441);
            d1PLC1addresses.Add("VariableAmplitudeFanRunning", 10442);

            //101
            d1PLC1addresses.Add("LeftClampPumpRunning", 10423);
            d1PLC1addresses.Add("RightClampPumpRunning", 10424);
            d1PLC1addresses.Add("LeftClampElectromagneticValveOpen", 10425);
            d1PLC1addresses.Add("RightClampElectromagneticValveOpen", 10426);
            d1PLC1addresses.Add("RotaryFrequencyConverterPowerOn", 10427);
            d1PLC1addresses.Add("RotaryBrakeOpen", 10428);
            d1PLC1addresses.Add("RotaryLeftTurnCommand", 10429);
            d1PLC1addresses.Add("RotaryRightTurnCommand", 10430);
            d1PLC1addresses.Add("RotaryFrequencyConverterFaultReset", 10431);
            d1PLC1addresses.Add("RotarySpeedGivenSelection", 10432);
            d1PLC1addresses.Add("RotaryFanRunning", 10433);
            d1PLC1addresses.Add("VariableAmplitudeLowerElectromagneticValveOpen", 10445);
            d1PLC1addresses.Add("RiseCount", 40500);
            d1PLC1addresses.Add("SingleAction", 10053);
            d1PLC1addresses.Add("LinkAction", 10054);
            d1PLC1addresses.Add("Automatic", 10055);
            d1PLC1addresses.Add("LargeCarFault", 11000);
            d1PLC1addresses.Add("LargeCarForwardLimiting", 11001);
            d1PLC1addresses.Add("LargeCarReverseLimiting", 11002);
            d1PLC1addresses.Add("AnchorClamp", 11003);
            d1PLC1addresses.Add("LargeCarForward", 11004);
            d1PLC1addresses.Add("LargeCarReverse", 11005);
            d1PLC1addresses.Add("RotaryFault", 11020);
            d1PLC1addresses.Add("RotaryLeftTurnLimiting", 11021);
            d1PLC1addresses.Add("RotaryRightTurnLimiting", 11022);
            d1PLC1addresses.Add("RotaryLeftTurn", 11024);
            d1PLC1addresses.Add("RotaryRightTurn", 11025);
            d1PLC1addresses.Add("VariableAmplitudeFault", 11040);
            d1PLC1addresses.Add("VariableAmplitudeUpperLimiting", 11041);
            d1PLC1addresses.Add("VariableAmplitudeLowerLimiting", 11042);
            d1PLC1addresses.Add("VariableAmplitudeUpper", 11044);
            d1PLC1addresses.Add("VariableAmplitudeLower", 11045);
            d1PLC1addresses.Add("SuspensionBeltFault", 11060);
            d1PLC1addresses.Add("SuspensionBeltManualLoading", 11063);
            d1PLC1addresses.Add("SuspensionBeltManualUnloading", 11064);
            d1PLC1addresses.Add("SuspensionBeltLinkLoading", 11065);
            d1PLC1addresses.Add("SuspensionBeltLinkUnloading", 11066);
            d1PLC1addresses.Add("BucketWheelFaulting", 11080);
            d1PLC1addresses.Add("BucketWheelSingleStartup", 11082);
            d1PLC1addresses.Add("BucketWheelLinkStartup", 11083);
            d1PLC1addresses.Add("ClampFault", 11100);
            d1PLC1addresses.Add("ClampRelax", 11103);
            d1PLC1addresses.Add("CentralBaffleFault", 11140);
            d1PLC1addresses.Add("TailCarBeltFault", 11160);
            d1PLC1addresses.Add("MaterialLevelMeter", 11503);
            d1PLC1addresses.Add("ManualIntervention", 12020);
            d1PLC1addresses.Add("InterventionRelease", 12021);
            d1PLC1addresses.Add("SuspensionBeltLoadingButton", 12000);
            d1PLC1addresses.Add("SuspensionBeltStopButton", 12001);
            d1PLC1addresses.Add("SuspensionBeltUnloadingButton", 12002);

            //151
            d1PLC1addresses.Add("BucketWheelStartupButton", 12003);
            d1PLC1addresses.Add("BucketWheelStopButton", 12004);
            d1PLC1addresses.Add("RotaryCount", 40504);
            d1PLC1addresses.Add("LeftAnchorNotLifted", 11700);
            d1PLC1addresses.Add("RightAnchorNotLifted", 11701);
            d1PLC1addresses.Add("ClampNotRelaxed", 11702);
            d1PLC1addresses.Add("LargeCarBrakeNotOpen", 11704);
            d1PLC1addresses.Add("LargeCarFrequencyConverterNotPowered", 11706);
            d1PLC1addresses.Add("LargeCarBrakeContactAuxiliaryFault", 11707);
            d1PLC1addresses.Add("LargeCarFrequencyConverterContactAuxiliaryFault", 11708);
            d1PLC1addresses.Add("RotaryFrequencyConverterNotPowered", 41720);
            d1PLC1addresses.Add("RotaryFrequencyConverterContactAuxiliaryFault", 41722);
            d1PLC1addresses.Add("RotaryBrakeContactAuxiliaryFault", 41723);
            d1PLC1addresses.Add("VariableAmplitudeOilPumpMotorNotRunning", 11730);
            d1PLC1addresses.Add("SuspensionBeltBrakeContactAuxiliaryFault", 11741);
            d1PLC1addresses.Add("SuspensionBeltLoadingContactAuxiliaryFault", 11742);
            d1PLC1addresses.Add("SuspensionBeltUnloadingContactAuxiliaryFault", 11743);
            d1PLC1addresses.Add("SuspensionBeltFirstLevelDeviation", 11750);
            d1PLC1addresses.Add("BucketWheelLubricationPumpContactAuxiliaryFault", 11751);
            d1PLC1addresses.Add("WindproofSystemCableLimit1", 40145);
            d1PLC1addresses.Add("RotaryFrequencyConverterFault", 40168);
            d1PLC1addresses.Add("RotaryFanOverload", 40164);
            d1PLC1addresses.Add("RotaryBrakeResistorOverheatSwitch", 40170);
            d1PLC1addresses.Add("DiversionBaffleMotorOverload", 40221);
            d1PLC1addresses.Add("TailCarFirstLevelDeviationSwitch", 40224);
            d1PLC1addresses.Add("TailCarSecondLevelDeviationSwitch", 40225);
            d1PLC1addresses.Add("TailCarEmergencyStopSwitch", 40226);
            d1PLC1addresses.Add("RotaryLeftTurnForbiddenLimit", 40310);
            d1PLC1addresses.Add("RotaryRightTurnForbiddenLimit", 40311);
            d1PLC1addresses.Add("BucketWheelMaterialUnloadingRunning", 40404);
            d1PLC1addresses.Add("VariableAmplitudeUpperElectromagneticValveOpen", 40446);
            d1PLC1addresses.Add("SuspensionBeltLoadingRunning", 40449);
            d1PLC1addresses.Add("SuspensionBeltUnloadingRunning", 40450);
            d1PLC1addresses.Add("SuspensionBeltBrakeOpen", 40451);
            d1PLC1addresses.Add("BucketWheelMotorRunning", 40465);
            d1PLC1addresses.Add("BucketWheelLubricationPumpRunning", 40466);
            d1PLC1addresses.Add("DiversionBaffleDownRunning", 40460);
            d1PLC1addresses.Add("DiversionBaffleUpRunning", 40461);
            d1PLC1addresses.Add("VibrationMotorRunning", 40454);
            d1PLC1addresses.Add("VariableAmplitudeBoostValveOpen", 40446);
            d1PLC1addresses.Add("BaffleDownLimit", 40322);
            d1PLC1addresses.Add("BaffleUpLimit", 40323);
            d1PLC1addresses.Add("RotaryOverTorque", 40380);
            d1PLC1addresses.Add("VariableAmplitudeOilPumpMotorContactFault", 11732);
            d1PLC1addresses.Add("BucketWheelMotorContactAuxiliaryFault", 11752);
            d1PLC1addresses.Add("TailCarOilPumpMotorContactAuxiliaryFault", 11753);
            d1PLC1addresses.Add("VibrationMotorFault", 11200);
            d1PLC1addresses.Add("ReelEmptySwitch", 10202);
            d1PLC1addresses.Add("WindproofSystemCableNotOpen", 11703);
            d1PLC1addresses.Add("LargeCarLimitAction", 11754);

            //201
            d1PLC1addresses.Add("RotaryLimitAction", 11755);
            d1PLC1addresses.Add("VariableAmplitudeLimitAction", 11756);
            d1PLC1addresses.Add("ForbiddenZoneLimitAction", 11757);
            d1PLC1addresses.Add("RotaryCrashSwitchAction", 11758);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationLowOilLevelAlarm", 11760);
            d1PLC1addresses.Add("LargeCarCentralizedLubricationOilBlockageAlarm", 11761);
            d1PLC1addresses.Add("RotaryCentralizedLubricationLowOilLevelAlarm", 11762);
            d1PLC1addresses.Add("RotaryCentralizedLubricationOilBlockageAlarm", 11763);
            d1PLC1addresses.Add("StrongWindPreAlarm", 11764);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationLowOilLevelAlarm", 11765);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationOilBlockageAlarm", 11767);
            d1PLC1addresses.Add("ManualGuideSlotLiftButton", 12015);
            d1PLC1addresses.Add("ManualBucketWheelSlotStopButton", 12016);
            d1PLC1addresses.Add("ManualBucketWheelSlotDownButton", 12017);
            d1PLC1addresses.Add("CentralBaffleManualLiftButton", 12005);
            d1PLC1addresses.Add("CentralBaffleManualStopButton", 12006);
            d1PLC1addresses.Add("CentralBaffleManualDownButton", 12007);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterManualStartupButton", 12011);
            d1PLC1addresses.Add("VariableAmplitudeOilHeaterManualStopButton", 12012);
            d1PLC1addresses.Add("VariableAmplitudeFanManualStartupButton", 12013);
            d1PLC1addresses.Add("VariableAmplitudeFanManualStopButton", 12014);
            d1PLC1addresses.Add("ElectricRoomEmergencyStopButtonAction", 40052);
            d1PLC1addresses.Add("CabinEmergencyStopButtonAction", 40052);     // %MW52X01
            d1PLC1addresses.Add("EmergencyStopRelayNot", 40052);               // %MW52X02
            d1PLC1addresses.Add("TransformerOverheatAlarm", 40052);            // %MW52X03
            d1PLC1addresses.Add("ElectricRoomPLCModulePowerFault", 40052);     // %MW52X04
            d1PLC1addresses.Add("CabinPLCModulePowerFault", 40052);            // %MW52X05
            d1PLC1addresses.Add("ElectricRoomFireAlarm", 40052);
            d1PLC1addresses.Add("CabinFireAlarm", 40052);
            d1PLC1addresses.Add("SuspensionBeltEmergencyStop", 40052);
            d1PLC1addresses.Add("TailCarBeltEmergencyStopSwitch", 40052);
            d1PLC1addresses.Add("LargeCarMainCircuitBreakerFault", 40100);
            d1PLC1addresses.Add("LargeCarMotorCircuitBreakerFault", 40100);
            d1PLC1addresses.Add("LargeCarBrakeCircuitBreakerFault", 40100);
            d1PLC1addresses.Add("Spare1", 40100);
            d1PLC1addresses.Add("CarFrequencyConverterFault", 40100);
            d1PLC1addresses.Add("LargeCarBrakeResistorOverheatJump", 40100);
            d1PLC1addresses.Add("CableReelMainCircuitBreakerFault", 40100);
            d1PLC1addresses.Add("CableReelMotorOverloading", 40100);
            d1PLC1addresses.Add("PowerReelCableOverLooseAlarm", 40100);
            d1PLC1addresses.Add("PowerReelCableOverTightAlarm", 40100);
            d1PLC1addresses.Add("PowerReelFullDiskAlarm", 40100);
            d1PLC1addresses.Add("PowerReelEmptyDiskAlarm", 40100);
            d1PLC1addresses.Add("LargeCarOperationHandleFault", 40100);
            d1PLC1addresses.Add("RotaryMainCircuitBreakerFault", 40100);
            d1PLC1addresses.Add("RotaryBrakeOverloadAlarm", 40100);
            d1PLC1addresses.Add("RotaryFanOverloadAlarm", 40100);
            d1PLC1addresses.Add("RotaryFrequencyConverterFaulting", 40100);
            d1PLC1addresses.Add("RotaryBrakeResistorOverheatSwitching", 40100);
            d1PLC1addresses.Add("RotaryOverTorqueSwitch", 40100);

            //251
            d1PLC1addresses.Add("ReversalHandleFault", 40120);
            d1PLC1addresses.Add("LinkedBucketWheelNotRunning", 40120);
            d1PLC1addresses.Add("VariableFrequencyMainCircuitBreakerFault", 40140);
            d1PLC1addresses.Add("VariableFrequencyMotorOverload", 40140);
            d1PLC1addresses.Add("VariableFrequencyPumpClogged", 40140);
            d1PLC1addresses.Add("VariableFrequencyPumpStationHighTemperatureAlarm", 40140);
            d1PLC1addresses.Add("VariableFrequencyOilTankLowLevelAlarm", 40140);
            d1PLC1addresses.Add("Spare2", 40140);
            d1PLC1addresses.Add("VariableFrequencyHandleFault", 40160);
            d1PLC1addresses.Add("SuspendedBeltCircuitBreakerFault", 40160);
            d1PLC1addresses.Add("SuspendedBeltMotorOverload", 40160);
            d1PLC1addresses.Add("SuspendedBeltSecondLevelDeviationSwitch", 40160);
            d1PLC1addresses.Add("SuspendedBeltEmergencyStop", 40160);
            d1PLC1addresses.Add("SuspendedBeltSlip", 40160);
            d1PLC1addresses.Add("SuspendedBeltLongitudinalTearSwitch", 40160);
            d1PLC1addresses.Add("CentralHopperCloggedDetectionSwitch", 40160);
            d1PLC1addresses.Add("StackingSwitchFault", 40160);
            d1PLC1addresses.Add("CentralControlRoomNoStackingCommand", 40160);
            d1PLC1addresses.Add("BucketWheelMotorMainCircuitBreakerFault", 40180);
            d1PLC1addresses.Add("BucketWheelMotorOverloading", 40180);
            d1PLC1addresses.Add("BucketWheelOverTorqueSwitching", 40180);
            d1PLC1addresses.Add("BucketWheelTemperatureUpperLimitAlarm", 40180);
            d1PLC1addresses.Add("ClampingDeviceMainCircuitBreakerFault", 40200);
            d1PLC1addresses.Add("ClampingDeviceMotorOverload", 40200);
            d1PLC1addresses.Add("LeftClampingDeviceTimeout", 40200);
            d1PLC1addresses.Add("RightClampingDeviceTimeout", 40200);
            d1PLC1addresses.Add("StrongWindAlarm", 40200);
            d1PLC1addresses.Add("DryFogSystemLowAirPressure", 40220);
            d1PLC1addresses.Add("DryFogSystemLowWaterPressure", 40220);
            d1PLC1addresses.Add("DryFogSystemFilterClogged", 40220);
            d1PLC1addresses.Add("DryFogSystemWaterTankLowLevel", 40220);
            d1PLC1addresses.Add("DiversionPlateCircuitBreakerFault", 40240);
            d1PLC1addresses.Add("DiversionPlateMotorOverload", 40240);
            d1PLC1addresses.Add("DiversionPlateTimeout", 40240);
            d1PLC1addresses.Add("CentralControlRoomNoStackingOrDiversionCommand", 40240);
            d1PLC1addresses.Add("BucketWheelFeederCircuitBreakerFault", 40250);
            d1PLC1addresses.Add("BucketWheelFeederMotorOverload", 40250);
            d1PLC1addresses.Add("BucketWheelFeederTimeout", 40250);
            d1PLC1addresses.Add("CentralControlRoomNoStackingUnloadingCommand", 40250);
            d1PLC1addresses.Add("TailCarBeltFirstLevelDeviation", 40260);
            d1PLC1addresses.Add("TailCarBeltSecondLevelDeviation", 40260);
            d1PLC1addresses.Add("TailCarBeltLongitudinalTear", 40260);
            d1PLC1addresses.Add("Spare3", 40260);
            d1PLC1addresses.Add("VibrationMotorCircuitBreakerFault", 40280);
            d1PLC1addresses.Add("VibrationMotorOverloading", 40280);
            d1PLC1addresses.Add("DriverRoomEmergencyStopButton", 10260);
            d1PLC1addresses.Add("ElectricalRoomEmergencyStopButton", 10109);
            d1PLC1addresses.Add("EmergencyStopRelay", 10110);
            d1PLC1addresses.Add("TwoMachineCollisionAlarm", 10144);
            d1PLC1addresses.Add("RollerFullDiskSwitch", 10203);

            //301
            d1PLC1addresses.Add("RollerMiddleSwitch", 10204);
            d1PLC1addresses.Add("BucketWheelMotorContactor", 10173);
            d1PLC1addresses.Add("VariableFrequencyOilBlockageSignal", 10333);
            d1PLC1addresses.Add("VariableFrequencyOverpressureStop", 10335);
            d1PLC1addresses.Add("VariableFrequencyPumpStationOverpressureAlarm", 10334);
            d1PLC1addresses.Add("PowerRollerRunning", 10420);
            d1PLC1addresses.Add("DriverRoomLevelingContactor", 10185);
            d1PLC1addresses.Add("DryFogSystemIsLowAirPressure", 10208);
            d1PLC1addresses.Add("DryFogSystemIsLowWaterPressure", 10209);
            d1PLC1addresses.Add("WaterTankLowLevelSwitch", 10211);
            d1PLC1addresses.Add("DriverRoomRiseValve", 10447);
            d1PLC1addresses.Add("DriverRoomDescentValve", 10448);
            d1PLC1addresses.Add("PowerCableRollerNotRunning", 11705);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingTemperatureUpperLimitAlarm", 11768);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingTemperatureLowerLimitAlarm", 11769);
            d1PLC1addresses.Add("AllowBucketWheelDiversion", 10114);
            d1PLC1addresses.Add("WindproofSystemCableLimit2", 10146);
            d1PLC1addresses.Add("WindproofSystemCableLimit3", 10147);
            d1PLC1addresses.Add("RollerOverTightLimit2", 10206);
            d1PLC1addresses.Add("RollerOverLooseLimit2", 10207);
            d1PLC1addresses.Add("DryFogSystemFilterIsClogged", 10210);
            d1PLC1addresses.Add("DryFogSystemAutoRun", 10212);
            d1PLC1addresses.Add("DryFogSystemManualRun", 10213);
            d1PLC1addresses.Add("DryFogSystemSprayStatus", 10214);
            d1PLC1addresses.Add("DryFogSystemHeatRun", 10215);
            d1PLC1addresses.Add("BucketWheelSlotMainCircuitBreaker", 10222);
            d1PLC1addresses.Add("BucketWheelSlotMotorOverload", 10223);
            d1PLC1addresses.Add("TailCarBeltLongitudinalTearing", 10227);
            d1PLC1addresses.Add("ReversalBrakeRelease", 10313);
            d1PLC1addresses.Add("BucketWheelSlotLiftLimit", 10320);
            d1PLC1addresses.Add("BucketWheelSlotLowerLimit", 10321);
            d1PLC1addresses.Add("DiversionPlateLimit", 10324);
            d1PLC1addresses.Add("SuspendedBeltBrakeRelease", 10346);
            d1PLC1addresses.Add("BrokenBeltCaptureAlarm", 10347);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationLowOilLevel", 10351);
            d1PLC1addresses.Add("BucketWheelCentralizedLubricationClogged", 10352);
            d1PLC1addresses.Add("DriverRoomRearBalanceLimit", 10366);
            d1PLC1addresses.Add("BucketWheelDiversionRunning", 10405);
            d1PLC1addresses.Add("DriverRoomLevelingPumpRunning", 10443);
            d1PLC1addresses.Add("BucketWheelSlotLift", 10452);
            d1PLC1addresses.Add("BucketWheelSlotLower", 10453);
            d1PLC1addresses.Add("RemoteEmergencyStop", 10107);
            d1PLC1addresses.Add("DiversionPlateAngle", 44006);
            d1PLC1addresses.Add("DryFogDustSuppressionStackingRunning", 10480);
            d1PLC1addresses.Add("DryFogDustSuppressionReclaimingRunning", 10481);
            d1PLC1addresses.Add("DryFogDustSuppressionDiversionRunning", 10482);
            d1PLC1addresses.Add("DryFogDustSuppressionRemoteStartRunning", 10483);
            d1PLC1addresses.Add("DryFogDustSuppressionRemoteStopRunning", 10484);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingUpperLimitAlarm", 10250);
            d1PLC1addresses.Add("TailCarDrivenRollerBearingLowerLimitAlarm", 10251);

            //351
            d1PLC1addresses.Add("UnmannedEmergencyStop", 44052);
            d1PLC1addresses.Add("RemoteEmergencyStoping", 44052);
            d1PLC1addresses.Add("LargeVehicleMotor1OvertemperatureAlarm", 44102);
            d1PLC1addresses.Add("LargeVehicleMotor2OvertemperatureAlarm", 44102);
            d1PLC1addresses.Add("LargeVehicleMotor3OvertemperatureAlarm", 44102);
            d1PLC1addresses.Add("LargeVehicleMotor4OvertemperatureAlarm", 44102);
            d1PLC1addresses.Add("LargeVehicleMotor5OvertemperatureAlarm", 44102);
            d1PLC1addresses.Add("LargeVehicleMotor6OvertemperatureAlarm", 44102);
            d1PLC1addresses.Add("WalkingReducerBearingTemperatureUpperLimitAlarm", 44102);
            d1PLC1addresses.Add("WalkingReducerBearingTemperatureLowerLimitAlarm", 44102);
            d1PLC1addresses.Add("WalkingReducerOilTemperatureUpperLimitAlarm", 44102);
            d1PLC1addresses.Add("WalkingReducerOilTemperatureLowerLimitAlarm", 44102);
            d1PLC1addresses.Add("ReversalTemperatureUpperLimitAlarm", 44120);
            d1PLC1addresses.Add("ReversalTemperatureLowerLimitAlarm", 44120);
            d1PLC1addresses.Add("BrokenBeltCaptureAlarming", 44160);
            d1PLC1addresses.Add("SuspendedBeltTemperatureUpperLimitAlarm", 44160);
            d1PLC1addresses.Add("SuspendedBeltTemperatureLowerLimitAlarm", 44160);
            d1PLC1addresses.Add("SuspendedBeltRollerBearingTemperatureUpperLimitAlarm", 44160);
            d1PLC1addresses.Add("SuspendedBeltRollerBearingTemperatureLowerLimitAlarm", 44160);
            d1PLC1addresses.Add("BucketWheelTemperatureLowerLimitAlarm", 44180);
            d1PLC1addresses.Add("CableRollerContactorAuxiliaryContactFault", 11709);
            d1PLC1addresses.Add("DriverRoomBalancePumpMotorNotRunning", 11731);
            d1PLC1addresses.Add("DriverRoomBalancePumpMotorAuxiliaryContactFault", 11733);
            d1PLC1addresses.Add("Remote", 10295);
            d1PLC1addresses.Add("TwoMachineDistance", 40484);
            d1PLC1addresses.Add("DriverRoomAngle", 40498);
            d1PLC1addresses.Add("DriverRoomRiseButton", 12018);
            d1PLC1addresses.Add("DriverRoomDescentButton", 12022);



            #endregion

            #region 键值对d1PLC2

            #endregion

            #region 键值对d2PLC1

            #endregion

            #region 键值对d2PLC2

            #endregion

            #endregion
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

        public static PLC1Variables d1PLC1Variables = new PLC1Variables(); //D1PLC1变量对象
        public static PLC2Variables d1PLC2Variables = new PLC2Variables(); //D1PLC2变量对象
        public static PLC1Variables d2PLC1Variables = new PLC1Variables(); //D2PLC1变量对象
        public static PLC2Variables d2PLC2Variables = new PLC2Variables(); //D2PLC2变量对象
        public static SystemVariables systemVariables = new SystemVariables(); //PLC全体变量对象


        public static SiemensHelper siemensHelper;//西门子PLC1对象
        public static SiemensHelper siemensHelper2;//西门子PLC2对象

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

                //复制变量
                CopyPropertiesD1PLC1(d1PLC1Variables, systemVariables);


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
                //CopyPropertiesD1PLC2(d1PLC2Variables, systemVariables);

                #region 随机数
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
                Random random = new Random();

                // 遍历所有属性，给它们赋随机数值
                foreach (var property in systemVariables.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(systemVariables, random.Next(2) == 0);
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        property.SetValue(systemVariables, (float)random.NextDouble());
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        property.SetValue(systemVariables, random.Next());
                    }
                    else if (property.PropertyType == typeof(ushort))
                    {
                        property.SetValue(systemVariables, (ushort)random.Next(500));
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(systemVariables, (int)random.Next(500));
                    }
                }
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
            Thread.Sleep(2000);


            WebSocketHelper websocket = new WebSocketHelper(this, systemVariables);
            
            websocket.Listen("127.0.0.1:11000");


        }

        #endregion


        //*******************************************************************************************************************************//
        //*******************************************************************************************************************************//
        //方法模块

        #region D1PLC数据映射方法
        public void ReadD1PLC()
        {
            object result2 = null;

            //遍历键值对d1PLC1
            foreach (int address in d1PLC1addresses.Values)
            {
                try
                {
                    string key = d1PLC1addresses.FirstOrDefault(x => x.Value == address).Key;
                    PropertyInfo property = typeof(PLC1Variables).GetProperty(key);

                    // 读取地址并处理返回的数据
                    object result = PLCRead(modbusHelper_D1PLC1, address, property.PropertyType);

                    result2 = result;
                    property.SetValue(d1PLC1Variables, result);
                }
                catch (OverflowException e)
                {
                    MessageBox.Show(result2.ToString());
                    MessageBox.Show(e.ToString());
                }
            }
            //遍历键值对d1PLC2
            foreach (int address in d1PLC2addresses.Values)
            {
                try
                {
                    string key = d1PLC2addresses.FirstOrDefault(x => x.Value == address).Key;
                    PropertyInfo property = typeof(PLC2Variables).GetProperty(key);

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
                    PropertyInfo property = typeof(PLC1Variables).GetProperty(key);

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
                    PropertyInfo property = typeof(PLC2Variables).GetProperty(key);

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
            catch (Exception ex)
            {
                DisplayRichTextboxContentAndScroll("写寄存器出现错误，请检查：\n" + ex.Message);
            }


            result = data;

            return result;
        }
        #endregion

        #region 对象属性映射方法
        public static void CopyPropertiesD1PLC1(PLC1Variables a, SystemVariables c)
        {
            Type aType = typeof(PLC1Variables);
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
        public static void CopyPropertiesD1PLC2(PLC2Variables b, SystemVariables c)
        {
            Type bType = typeof(PLC2Variables);
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

        #region 与PLC连接线程 （西门子）
        private void Process_SiemensPLC_Main()
        {
            try
            {
                siemensHelper.plc = new Plc(CpuType.S71500, "192.168.1.66", 0, 1);
                siemensHelper2.plc = new Plc(CpuType.S71500, "192.168.1.150", 0, 1);

                siemensHelper.plc.Open();
                siemensHelper2.plc.Open();
            }
            catch { }

            while (true)
            {
                //PLC1
                try
                {
                    //检查plc是否连接上
                    if (siemensHelper.plc.IsConnected == true)
                    {
                        if (uc5.PlcConnectionLabel != null)
                        {
                            UpdateText(uc5.PlcConnectionLabel, "PLC1已连接！");
                        }
                    }
                    else
                    {
                        UpdateText(uc5.PlcConnectionLabel, "PLC1连接失败！");
                        DisplayRichTextboxContentAndScroll("PLC1连接失败！");

                        siemensHelper.plc.Open();//再次尝试
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }

                //PLC2
                try
                {
                    //检查plc是否连接上
                    if (siemensHelper2.plc.IsConnected)
                    {
                        if (uc5.PlcConnectionLabel != null)
                        {
                            UpdateText(uc5.PlcConnectionLabel, "PLC2已连接！");
                        }
                    }
                    else
                    {
                        UpdateText(uc5.PlcConnectionLabel, "PLC2连接失败！");
                        DisplayRichTextboxContentAndScroll("PLC2连接失败！");

                        siemensHelper2.plc.Open();
                    }
                }
                catch (Exception ex)
                {
                    DisplayRichTextboxContentAndScroll("错误： " + ex.Message);
                }

                Thread.Sleep(5000);
            }
        }
        #endregion

        #region PLC数据获取线程 （西门子）
        private void Process_SiemensPLC_DataGet1()
        {
            while (true)
            {
                if (siemensHelper.plc != null && siemensHelper.plc.IsConnected)
                {
                    d1PLC1Variables.TimeStamp = DateTime.Now; // 记录程序启动时间

                    //ReadPLC1();
                }

                //复制变量
                //CopyProperties1(plc1Variables, systemVariables);

                Thread.Sleep(1000);
            }
        }
        private void Process_SiemensPLC_DataGet2()
        {
            while (true)
            {
                ShareRes.WaitMutex();//YCQ, 20231223

                if (siemensHelper2.plc != null && siemensHelper2.plc.IsConnected)
                {
                    //ReadPLC2();

                    //GlobalVariables2.globalVariables2 = plc2Variables;
                }
                ShareRes.ReleaseMutex();//YCQ, 20231223

                #region 随机数

                systemVariables.TimeStamp = DateTime.Now;

                Random random = new Random();

                // 遍历所有属性，给它们赋随机数值
                foreach (var property in systemVariables.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(systemVariables, random.Next(2) == 0);
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        property.SetValue(systemVariables, (float)random.NextDouble());
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        property.SetValue(systemVariables, random.Next());
                    }
                    else if (property.PropertyType == typeof(ushort))
                    {
                        property.SetValue(systemVariables, (ushort)random.Next(500));
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(systemVariables, (int)random.Next(500));
                    }
                }


                //OperationLogs(systemVariables.MaterialFeederRotationSpeed.ToString());


                #endregion

                Thread.Sleep(500);

                //复制变量
                //CopyProperties2(plc2Variables, systemVariables);



                //string test = JsonConvert.SerializeObject(systemVariables, settings);
                //MessageBox.Show(test);
            }
        }
        #endregion
    }
}
