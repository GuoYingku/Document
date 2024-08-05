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

        
        public bool PLCCommunicationState { get; set; }//可编程逻辑控制器通信状态
        public bool RCCommunicationState { get; set; }//远程驱动子系统通信状态
        public bool MCCommunicationState { get; set; }//综合运控子系统通信状态
        public bool PCCommunicationState { get; set; }//任务规划子系统通信状态
        public bool LOCCommunicationState { get; set; }//运动定位子系统通信状态
        public bool CACommunicationState { get; set; }//安全防护子系统通信状态
        public bool SCANCommunicationState { get; set; }//三维扫描子系统通信状态
        public bool FDCommunicationState { get; set; }//流量检测子系统通信状态
        public bool SMCommunicationState { get; set; }//系统管理子系统通信状态
        public bool VMCommunicationState { get; set; }//视频监测子系统通信状态




        // D1PLC1
        public ushort LargeCarElectricCurrent { get; set; }// ID 1
        public ushort RotaryElectricCurrent { get; set; }
        public ushort SuspensionBeltElectricCurrent { get; set; }
        public ushort BucketWheelElectricCurrent { get; set; }
        public ushort LargeCarTravelDistance { get; set; } //大车行走距离
        public ushort RotaryAngle { get; set; } //回转角度
        public ushort VariableAmplitudeAngle { get; set; } //变幅角度
        public bool VacuumCircuitBreakerClosed { get; set; }
        public bool LowVoltageControlPowerClosed { get; set; }
        public bool LowVoltagePowerClosed { get; set; }
        public bool LargeCarCentralizedLubricationLowOilLevel { get; set; }
        public bool LargeCarCentralizedLubricationOilBlockage { get; set; }
        public bool AllowBucketWheelMaterialLoading { get; set; }
        public bool AllowBucketWheelMaterialUnloading { get; set; }
        public bool LargeCarMainCircuitBreaker { get; set; }
        public bool LargeCarMotorCircuitBreaker { get; set; }
        public bool LargeCarBrakeCircuitBreaker { get; set; }
        public bool LargeCarFrequencyConverterContact { get; set; }
        public bool LargeCarBrakeContact { get; set; }
        public bool LargeCarFrequencyConverterFault { get; set; }
        public bool LargeCarBrakeResistorOverheatSwitch { get; set; }
        public bool LargeCarForwardLimit { get; set; }
        public bool LargeCarReverseLimit { get; set; }
        public bool LargeCarForwardExtremeLimit { get; set; }
        public bool LargeCarReverseExtremeLimit { get; set; }
        public bool CableReelMainCircuitBreaker { get; set; }
        public bool CableReelMotorOverload { get; set; }
        public bool PowerReelContact { get; set; }
        public bool ReelOverTensionLimit1 { get; set; }
        public bool ReelOverLooseLimit1 { get; set; }
        public bool VibrationMotorMainCircuitBreaker { get; set; }
        public bool RotaryBrakeOverload { get; set; }
        public bool RotaryMainCircuitBreaker { get; set; }
        public bool ClampMotorOverload { get; set; }
        public bool LeftAnchorLiftLimit { get; set; }
        public bool RightAnchorLiftLimit { get; set; }
        public bool LeftClampRelaxLimit { get; set; }
        public bool RightClampRelaxLimit { get; set; }
        public bool BucketWheelMotorMainCircuitBreaker { get; set; }
        public bool RotaryFanContact { get; set; }
        public bool RotaryBrakeContact { get; set; }
        public bool RotaryFrequencyConverterContact { get; set; }
        public bool SystemInterlockSwitch { get; set; }
        public bool VariableAmplitudeMainCircuitBreaker { get; set; }
        public bool VariableAmplitudeMotorOverload { get; set; }
        public bool VariableAmplitudeMotorContact { get; set; }
        public bool VariableAmplitudeHeaterContact { get; set; }
        public bool VariableAmplitudeFanContact { get; set; }
        public bool SuspensionBeltMainCircuitBreaker { get; set; }
        public bool SuspensionBeltMotorOverload { get; set; }
        public bool SuspensionBeltMaterialLoadingRunningContact { get; set; }
        public bool SuspensionBeltMaterialUnloadingRunningContact { get; set; }
        public bool SuspensionBeltBrakeContact { get; set; }
        public bool CentralMaterialDustDetectionSwitch { get; set; }
        public bool DiversionBaffleMainCircuitBreaker { get; set; }
        public bool VibrationMotorOverload { get; set; }
        public bool ClampMainCircuitBreaker { get; set; }
        public bool BucketWheelMotorOverload { get; set; }
        public bool BucketWheelLubricationPumpContact { get; set; }
        public bool RotaryLeftTurnLimit { get; set; }
        public bool RotaryRightTurnLimit { get; set; }
        public bool RotaryLeftTurnExtremeLimit { get; set; }
        public bool RotaryRightTurnExtremeLimit { get; set; }
        public bool RotaryLeftTurnForbiddenZoneLimit { get; set; }
        public bool RotaryRightTurnForbiddenZoneLimit { get; set; }
        public bool RotaryZeroPositionLimit { get; set; }
        public bool BucketWheelOverTorqueSwitch { get; set; }
        public bool BucketWheelForcedLubricationFlowSwitch { get; set; }
        public bool VariableAmplitudeUpperLimit { get; set; }
        public bool VariableAmplitudeLowerLimit { get; set; }
        public bool VariableAmplitudeUpperExtremeLimit { get; set; }
        public bool VariableAmplitudeLowerExtremeLimit { get; set; }
        public bool VariableAmplitudeLowerForbiddenZoneLimit { get; set; }
        public bool CabinFrontBalanceLimit { get; set; }
        public bool VariableAmplitudeOilHeaterStartup { get; set; }
        public bool VariableAmplitudeOilHeaterStop { get; set; }
        public bool VariableAmplitudeFanStop { get; set; }
        public bool VariableAmplitudeFanStartup { get; set; }
        public bool VariableAmplitudeOilLevelLowSignal { get; set; }
        public bool VariableAmplitudePumpStationOverheatAlarm { get; set; }
        public bool VariableAmplitudeOilLevelVeryLowSignal { get; set; }
        public bool RotaryCentralizedLubricationLowOilLevelFault { get; set; }
        public bool LargeCarFrequencyConverterPowerOn { get; set; }
        public bool LargeCarBrakeOpen { get; set; }
        public bool LargeCarFrequencyConverterFaultReset { get; set; }
        public bool LargeCarReverseCommand { get; set; }
        public bool LargeCarHighLowSpeedSelection { get; set; }
        public bool BucketWheelMaterialLoadingRunning { get; set; }
        public bool BucketWheelFault { get; set; }
        public bool SuspensionBeltFirstLevelDeviationSwitch { get; set; }
        public bool SuspensionBeltSecondLevelDeviationSwitch { get; set; }
        public bool SuspensionBeltEmergencyStopSwitch { get; set; }
        public bool SuspensionBeltSpeedDetectionSwitch { get; set; }
        public bool SuspensionBeltMaterialFlowDetectionSwitch { get; set; }
        public bool SuspensionBeltLongitudinalTearSwitch { get; set; }
        public bool RotaryCentralizedLubricationOilBlockageFault { get; set; }
        public bool LargeCarForwardCommand { get; set; }
        public bool VariableAmplitudeOilPumpMotorRunning { get; set; }
        public bool VariableAmplitudeOilHeaterRunning { get; set; }
        public bool VariableAmplitudeFanRunning { get; set; }
        public bool LeftClampPumpRunning { get; set; }
        public bool RightClampPumpRunning { get; set; }
        public bool LeftClampElectromagneticValveOpen { get; set; }
        public bool RightClampElectromagneticValveOpen { get; set; }
        public bool RotaryFrequencyConverterPowerOn { get; set; }
        public bool RotaryBrakeOpen { get; set; }
        public bool RotaryLeftTurnCommand { get; set; }
        public bool RotaryRightTurnCommand { get; set; }
        public bool RotaryFrequencyConverterFaultReset { get; set; }
        public bool RotarySpeedGivenSelection { get; set; }
        public bool RotaryFanRunning { get; set; }
        public bool VariableAmplitudeLowerElectromagneticValveOpen { get; set; }
        public ushort RiseCount { get; set; }
        public bool SingleAction { get; set; }
        public bool LinkAction { get; set; }
        public bool Automatic { get; set; }
        public bool LargeCarFault { get; set; }
        public bool LargeCarForwardLimiting { get; set; }// 118
        public bool LargeCarReverseLimiting { get; set; }
        public bool AnchorClamp { get; set; }
        public bool LargeCarForward { get; set; }
        public bool LargeCarReverse { get; set; }
        public bool RotaryFault { get; set; }
        public bool RotaryLeftTurnLimiting { get; set; }
        public bool RotaryRightTurnLimiting { get; set; }
        public bool RotaryLeftTurn { get; set; }
        public bool RotaryRightTurn { get; set; }
        public bool VariableAmplitudeFault { get; set; }
        public bool VariableAmplitudeUpperLimiting { get; set; }
        public bool VariableAmplitudeLowerLimiting { get; set; }
        public bool VariableAmplitudeUpper { get; set; }
        public bool VariableAmplitudeLower { get; set; }
        public bool SuspensionBeltFault { get; set; }
        public bool SuspensionBeltManualLoading { get; set; }
        public bool SuspensionBeltManualUnloading { get; set; }
        public bool SuspensionBeltLinkLoading { get; set; }
        public bool SuspensionBeltLinkUnloading { get; set; }
        public bool BucketWheelFaulting { get; set; }
        public bool BucketWheelSingleStartup { get; set; }
        public bool BucketWheelLinkStartup { get; set; }
        public bool ClampFault { get; set; }
        public bool ClampRelax { get; set; }
        public bool CentralBaffleFault { get; set; }
        public bool TailCarBeltFault { get; set; }
        public bool MaterialLevelMeter { get; set; }
        public bool ManualIntervention { get; set; }
        public bool InterventionRelease { get; set; }
        public bool SuspensionBeltLoadingButton { get; set; }
        public bool SuspensionBeltStopButton { get; set; }
        public bool SuspensionBeltUnloadingButton { get; set; }
        public bool BucketWheelStartupButton { get; set; }
        public bool BucketWheelStopButton { get; set; }
        public ushort RotaryCount { get; set; }
        public bool LeftAnchorNotLifted { get; set; }
        public bool RightAnchorNotLifted { get; set; }
        public bool ClampNotRelaxed { get; set; }
        public bool LargeCarBrakeNotOpen { get; set; }
        public bool LargeCarFrequencyConverterNotPowered { get; set; }
        public bool LargeCarBrakeContactAuxiliaryFault { get; set; }
        public bool LargeCarFrequencyConverterContactAuxiliaryFault { get; set; }
        public bool RotaryFrequencyConverterNotPowered { get; set; }
        public bool RotaryFrequencyConverterContactAuxiliaryFault { get; set; }
        public bool RotaryBrakeContactAuxiliaryFault { get; set; }
        public bool VariableAmplitudeOilPumpMotorNotRunning { get; set; }
        public bool SuspensionBeltBrakeContactAuxiliaryFault { get; set; }
        public bool SuspensionBeltLoadingContactAuxiliaryFault { get; set; }
        public bool SuspensionBeltUnloadingContactAuxiliaryFault { get; set; }
        public bool SuspensionBeltFirstLevelDeviation { get; set; }
        public bool BucketWheelLubricationPumpContactAuxiliaryFault { get; set; }
        public bool WindproofSystemCableLimit1 { get; set; }
        public bool RotaryFrequencyConverterFault { get; set; }
        public bool RotaryFanOverload { get; set; }
        public bool RotaryBrakeResistorOverheatSwitch { get; set; }
        public bool DiversionBaffleMotorOverload { get; set; }
        public bool TailCarFirstLevelDeviationSwitch { get; set; }
        public bool TailCarSecondLevelDeviationSwitch { get; set; }
        public bool TailCarEmergencyStopSwitch { get; set; }
        public bool RotaryLeftTurnForbiddenLimit { get; set; }
        public bool RotaryRightTurnForbiddenLimit { get; set; }
        public bool BucketWheelMaterialUnloadingRunning { get; set; }
        public bool VariableAmplitudeUpperElectromagneticValveOpen { get; set; }
        public bool SuspensionBeltLoadingRunning { get; set; }
        public bool SuspensionBeltUnloadingRunning { get; set; }
        public bool SuspensionBeltBrakeOpen { get; set; }
        public bool BucketWheelMotorRunning { get; set; }
        public bool BucketWheelLubricationPumpRunning { get; set; }
        public bool DiversionBaffleDownRunning { get; set; }
        public bool DiversionBaffleUpRunning { get; set; }
        public bool VibrationMotorRunning { get; set; }
        public bool VariableAmplitudeBoostValveOpen { get; set; }
        public bool BaffleDownLimit { get; set; }
        public bool BaffleUpLimit { get; set; }
        public bool RotaryOverTorque { get; set; }
        public bool VariableAmplitudeOilPumpMotorContactFault { get; set; }
        public bool BucketWheelMotorContactAuxiliaryFault { get; set; }
        public bool TailCarOilPumpMotorContactAuxiliaryFault { get; set; }
        public bool VibrationMotorFault { get; set; }
        public bool ReelEmptySwitch { get; set; }
        public bool WindproofSystemCableNotOpen { get; set; }
        public bool LargeCarLimitAction { get; set; }// 200

        // 201
        public bool RotaryLimitAction { get; set; }
        public bool VariableAmplitudeLimitAction { get; set; }
        public bool ForbiddenZoneLimitAction { get; set; }
        public bool RotaryCrashSwitchAction { get; set; }
        public bool LargeCarCentralizedLubricationLowOilLevelAlarm { get; set; }
        public bool LargeCarCentralizedLubricationOilBlockageAlarm { get; set; }
        public bool RotaryCentralizedLubricationLowOilLevelAlarm { get; set; }
        public bool RotaryCentralizedLubricationOilBlockageAlarm { get; set; }
        public bool StrongWindPreAlarm { get; set; }
        public bool BucketWheelCentralizedLubricationLowOilLevelAlarm { get; set; }
        public bool BucketWheelCentralizedLubricationOilBlockageAlarm { get; set; }
        public bool ManualGuideSlotLiftButton { get; set; }
        public bool ManualBucketWheelSlotStopButton { get; set; }
        public bool ManualBucketWheelSlotDownButton { get; set; }
        public bool CentralBaffleManualLiftButton { get; set; }
        public bool CentralBaffleManualStopButton { get; set; }
        public bool CentralBaffleManualDownButton { get; set; }
        public bool VariableAmplitudeOilHeaterManualStartupButton { get; set; }
        public bool VariableAmplitudeOilHeaterManualStopButton { get; set; }
        public bool VariableAmplitudeFanManualStartupButton { get; set; }
        public bool VariableAmplitudeFanManualStopButton { get; set; }
        public bool ElectricRoomEmergencyStopButtonAction { get; set; }
        public bool CabinEmergencyStopButtonAction { get; set; }
        public bool EmergencyStopRelayNot { get; set; }
        public bool TransformerOverheatAlarm { get; set; }
        public bool ElectricRoomPLCModulePowerFault { get; set; }
        public bool CabinPLCModulePowerFault { get; set; }
        public bool ElectricRoomFireAlarm { get; set; }
        public bool CabinFireAlarm { get; set; }
        public bool SuspensionBeltEmergencyStop { get; set; }
        public bool TailCarBeltEmergencyStopSwitch { get; set; }
        public bool LargeCarMainCircuitBreakerFault { get; set; }
        public bool LargeCarMotorCircuitBreakerFault { get; set; }
        public bool LargeCarBrakeCircuitBreakerFault { get; set; }
        public bool Spare1 { get; set; }
        public bool CarFrequencyConverterFault { get; set; }
        public bool LargeCarBrakeResistorOverheatJump { get; set; }
        public bool CableReelMainCircuitBreakerFault { get; set; }
        public bool CableReelMotorOverloading { get; set; }
        public bool PowerReelCableOverLooseAlarm { get; set; }
        public bool PowerReelCableOverTightAlarm { get; set; }
        public bool PowerReelFullDiskAlarm { get; set; }
        public bool PowerReelEmptyDiskAlarm { get; set; }
        public bool LargeCarOperationHandleFault { get; set; }
        public bool RotaryMainCircuitBreakerFault { get; set; }
        public bool RotaryBrakeOverloadAlarm { get; set; }
        public bool RotaryFanOverloadAlarm { get; set; }
        public bool RotaryFrequencyConverterFaulting { get; set; }
        public bool RotaryBrakeResistorOverheatSwitching { get; set; }
        public bool RotaryOverTorqueSwitch { get; set; }
        public bool ReversalHandleFault { get; set; }
        public bool LinkedBucketWheelNotRunning { get; set; }
        public bool VariableFrequencyMainCircuitBreakerFault { get; set; }
        public bool VariableFrequencyMotorOverload { get; set; }
        public bool VariableFrequencyPumpClogged { get; set; }
        public bool VariableFrequencyPumpStationHighTemperatureAlarm { get; set; }
        public bool VariableFrequencyOilTankLowLevelAlarm { get; set; }
        public bool Spare2 { get; set; }
        public bool VariableFrequencyHandleFault { get; set; }
        public bool SuspendedBeltCircuitBreakerFault { get; set; }
        public bool SuspendedBeltMotorOverload { get; set; }
        public bool SuspendedBeltSecondLevelDeviationSwitch { get; set; }
        public bool SuspendedBeltEmergencyStop { get; set; }
        public bool SuspendedBeltSlip { get; set; }
        public bool SuspendedBeltLongitudinalTearSwitch { get; set; }
        public bool CentralHopperCloggedDetectionSwitch { get; set; }
        public bool StackingSwitchFault { get; set; }
        public bool CentralControlRoomNoStackingCommand { get; set; }
        public bool BucketWheelMotorMainCircuitBreakerFault { get; set; }
        public bool BucketWheelMotorOverloading { get; set; }
        public bool BucketWheelOverTorqueSwitching { get; set; }
        public bool BucketWheelTemperatureUpperLimitAlarm { get; set; }
        public bool ClampingDeviceMainCircuitBreakerFault { get; set; }
        public bool ClampingDeviceMotorOverload { get; set; }
        public bool LeftClampingDeviceTimeout { get; set; }
        public bool RightClampingDeviceTimeout { get; set; }
        public bool StrongWindAlarm { get; set; }
        public bool DryFogSystemLowAirPressure { get; set; }
        public bool DryFogSystemLowWaterPressure { get; set; }
        public bool DryFogSystemFilterClogged { get; set; }
        public bool DryFogSystemWaterTankLowLevel { get; set; }
        public bool DiversionPlateCircuitBreakerFault { get; set; }
        public bool DiversionPlateMotorOverload { get; set; }
        public bool DiversionPlateTimeout { get; set; }
        public bool CentralControlRoomNoStackingOrDiversionCommand { get; set; }
        public bool BucketWheelFeederCircuitBreakerFault { get; set; }
        public bool BucketWheelFeederMotorOverload { get; set; }
        public bool BucketWheelFeederTimeout { get; set; }
        public bool CentralControlRoomNoStackingUnloadingCommand { get; set; }
        public bool TailCarBeltFirstLevelDeviation { get; set; }
        public bool TailCarBeltSecondLevelDeviation { get; set; }
        public bool TailCarBeltLongitudinalTear { get; set; }
        public bool Spare3 { get; set; }//293
        public bool VibrationMotorCircuitBreakerFault { get; set; }
        public bool VibrationMotorOverloading { get; set; }
        public bool DriverRoomEmergencyStopButton { get; set; }
        public bool ElectricalRoomEmergencyStopButton { get; set; }
        public bool EmergencyStopRelay { get; set; }
        public bool TwoMachineCollisionAlarm { get; set; }
        public bool RollerFullDiskSwitch { get; set; }
        public bool RollerMiddleSwitch { get; set; }
        public bool BucketWheelMotorContactor { get; set; }
        public bool VariableFrequencyOilBlockageSignal { get; set; }
        public bool VariableFrequencyOverpressureStop { get; set; }
        public bool VariableFrequencyPumpStationOverpressureAlarm { get; set; }
        public bool PowerRollerRunning { get; set; }
        public bool DriverRoomLevelingContactor { get; set; }
        public bool DryFogSystemIsLowAirPressure { get; set; }
        public bool DryFogSystemIsLowWaterPressure { get; set; }
        public bool WaterTankLowLevelSwitch { get; set; }
        public bool DriverRoomRiseValve { get; set; }
        public bool DriverRoomDescentValve { get; set; }
        public bool PowerCableRollerNotRunning { get; set; }
        public bool TailCarDrivenRollerBearingTemperatureUpperLimitAlarm { get; set; }
        public bool TailCarDrivenRollerBearingTemperatureLowerLimitAlarm { get; set; }
        public bool AllowBucketWheelDiversion { get; set; }
        public bool WindproofSystemCableLimit2 { get; set; }
        public bool WindproofSystemCableLimit3 { get; set; }
        public bool RollerOverTightLimit2 { get; set; }
        public bool RollerOverLooseLimit2 { get; set; }
        public bool DryFogSystemFilterIsClogged { get; set; }
        public bool DryFogSystemAutoRun { get; set; }
        public bool DryFogSystemManualRun { get; set; }
        public bool DryFogSystemSprayStatus { get; set; }
        public bool DryFogSystemHeatRun { get; set; }
        public bool BucketWheelSlotMainCircuitBreaker { get; set; }
        public bool BucketWheelSlotMotorOverload { get; set; }
        public bool TailCarBeltLongitudinalTearing { get; set; }
        public bool ReversalBrakeRelease { get; set; }
        public bool BucketWheelSlotLiftLimit { get; set; }
        public bool BucketWheelSlotLowerLimit { get; set; }
        public bool DiversionPlateLimit { get; set; }
        public bool SuspendedBeltBrakeRelease { get; set; }
        public bool BrokenBeltCaptureAlarm { get; set; }
        public bool BucketWheelCentralizedLubricationLowOilLevel { get; set; }
        public bool BucketWheelCentralizedLubricationClogged { get; set; }
        public bool DriverRoomRearBalanceLimit { get; set; }
        public bool BucketWheelDiversionRunning { get; set; }
        public bool DriverRoomLevelingPumpRunning { get; set; }
        public bool BucketWheelSlotLift { get; set; }
        public bool BucketWheelSlotLower { get; set; }
        public bool RemoteEmergencyStop { get; set; }
        public ushort DiversionPlateAngle { get; set; }
        public bool DryFogDustSuppressionStackingRunning { get; set; }
        public bool DryFogDustSuppressionReclaimingRunning { get; set; }
        public bool DryFogDustSuppressionDiversionRunning { get; set; }
        public bool DryFogDustSuppressionRemoteStartRunning { get; set; }
        public bool DryFogDustSuppressionRemoteStopRunning { get; set; }// 348
        public bool TailCarDrivenRollerBearingUpperLimitAlarm { get; set; }
        public bool TailCarDrivenRollerBearingLowerLimitAlarm { get; set; }
        public bool UnmannedEmergencyStop { get; set; }
        public bool RemoteEmergencyStoping { get; set; }
        public bool LargeVehicleMotor1OvertemperatureAlarm { get; set; }
        public bool LargeVehicleMotor2OvertemperatureAlarm { get; set; }
        public bool LargeVehicleMotor3OvertemperatureAlarm { get; set; }
        public bool LargeVehicleMotor4OvertemperatureAlarm { get; set; }
        public bool LargeVehicleMotor5OvertemperatureAlarm { get; set; }
        public bool LargeVehicleMotor6OvertemperatureAlarm { get; set; }
        public bool WalkingReducerBearingTemperatureUpperLimitAlarm { get; set; }
        public bool WalkingReducerBearingTemperatureLowerLimitAlarm { get; set; }
        public bool WalkingReducerOilTemperatureUpperLimitAlarm { get; set; }
        public bool WalkingReducerOilTemperatureLowerLimitAlarm { get; set; }
        public bool ReversalTemperatureUpperLimitAlarm { get; set; }
        public bool ReversalTemperatureLowerLimitAlarm { get; set; }
        public bool BrokenBeltCaptureAlarming { get; set; }
        public bool SuspendedBeltTemperatureUpperLimitAlarm { get; set; }
        public bool SuspendedBeltTemperatureLowerLimitAlarm { get; set; }
        public bool SuspendedBeltRollerBearingTemperatureUpperLimitAlarm { get; set; }
        public bool SuspendedBeltRollerBearingTemperatureLowerLimitAlarm { get; set; }
        public bool BucketWheelTemperatureLowerLimitAlarm { get; set; }
        public bool CableRollerContactorAuxiliaryContactFault { get; set; }
        public bool DriverRoomBalancePumpMotorNotRunning { get; set; }
        public bool DriverRoomBalancePumpMotorAuxiliaryContactFault { get; set; }
        public bool Remote { get; set; }
        public ushort TwoMachineDistance { get; set; } //两机距离
        public ushort DriverRoomAngle { get; set; } //司机室角度
        public bool DriverRoomRiseButton { get; set; }
        public bool DriverRoomDescentButton { get; set; }










        // D1PLC2









        // D2PLC1
        public ushort LargeCarElectricCurrent_2 { get; set; } // 1
        public ushort RotaryElectricCurrent_2 { get; set; }
        public ushort SuspensionBeltElectricCurrent_2 { get; set; }
        public ushort BucketWheelElectricCurrent_2 { get; set; }
        public ushort LargeCarTravelDistance_2 { get; set; } //大车行走距离
        public ushort RotaryAngle_2 { get; set; } //回转角度
        public ushort VariableAmplitudeAngle_2 { get; set; } //变幅角度
        public bool VacuumCircuitBreakerClosed_2 { get; set; }
        public bool LowVoltageControlPowerClosed_2 { get; set; }
        public bool LowVoltagePowerClosed_2 { get; set; }
        public bool LargeCarCentralizedLubricationLowOilLevel_2 { get; set; }
        public bool LargeCarCentralizedLubricationOilBlockage_2 { get; set; }
        public bool AllowBucketWheelMaterialLoading_2 { get; set; }
        public bool AllowBucketWheelMaterialUnloading_2 { get; set; }
        public bool LargeCarMainCircuitBreaker_2 { get; set; }
        public bool LargeCarMotorCircuitBreaker_2 { get; set; }
        public bool LargeCarBrakeCircuitBreaker_2 { get; set; }
        public bool LargeCarFrequencyConverterContact_2 { get; set; }
        public bool LargeCarBrakeContact_2 { get; set; }
        public bool LargeCarFrequencyConverterFault_2 { get; set; }
        public bool LargeCarBrakeResistorOverheatSwitch_2 { get; set; }
        public bool LargeCarForwardLimit_2 { get; set; }
        public bool LargeCarReverseLimit_2 { get; set; }
        public bool LargeCarForwardExtremeLimit_2 { get; set; }
        public bool LargeCarReverseExtremeLimit_2 { get; set; }
        public bool CableReelMainCircuitBreaker_2 { get; set; }
        public bool CableReelMotorOverload_2 { get; set; }
        public bool PowerReelContact_2 { get; set; }
        public bool ReelOverTensionLimit1_2 { get; set; }
        public bool ReelOverLooseLimit1_2 { get; set; }
        public bool VibrationMotorMainCircuitBreaker_2 { get; set; }
        public bool RotaryBrakeOverload_2 { get; set; }
        public bool RotaryMainCircuitBreaker_2 { get; set; }
        public bool ClampMotorOverload_2 { get; set; }
        public bool LeftAnchorLiftLimit_2 { get; set; }
        public bool RightAnchorLiftLimit_2 { get; set; }
        public bool LeftClampRelaxLimit_2 { get; set; }
        public bool RightClampRelaxLimit_2 { get; set; }
        public bool BucketWheelMotorMainCircuitBreaker_2 { get; set; }
        public bool RotaryFanContact_2 { get; set; }
        public bool RotaryBrakeContact_2 { get; set; }
        public bool RotaryFrequencyConverterContact_2 { get; set; }
        public bool SystemInterlockSwitch_2 { get; set; }
        public bool VariableAmplitudeMainCircuitBreaker_2 { get; set; }
        public bool VariableAmplitudeMotorOverload_2 { get; set; }
        public bool VariableAmplitudeMotorContact_2 { get; set; }
        public bool VariableAmplitudeHeaterContact_2 { get; set; }
        public bool VariableAmplitudeFanContact_2 { get; set; }
        public bool SuspensionBeltMainCircuitBreaker_2 { get; set; }
        public bool SuspensionBeltMotorOverload_2 { get; set; }
        public bool SuspensionBeltMaterialLoadingRunningContact_2 { get; set; }
        public bool SuspensionBeltMaterialUnloadingRunningContact_2 { get; set; }
        public bool SuspensionBeltBrakeContact_2 { get; set; }
        public bool CentralMaterialDustDetectionSwitch_2 { get; set; }
        public bool DiversionBaffleMainCircuitBreaker_2 { get; set; }
        public bool VibrationMotorOverload_2 { get; set; }
        public bool ClampMainCircuitBreaker_2 { get; set; }
        public bool BucketWheelMotorOverload_2 { get; set; }
        public bool BucketWheelLubricationPumpContact_2 { get; set; }
        public bool RotaryLeftTurnLimit_2 { get; set; }
        public bool RotaryRightTurnLimit_2 { get; set; }
        public bool RotaryLeftTurnExtremeLimit_2 { get; set; }
        public bool RotaryRightTurnExtremeLimit_2 { get; set; }
        public bool RotaryLeftTurnForbiddenZoneLimit_2 { get; set; }
        public bool RotaryRightTurnForbiddenZoneLimit_2 { get; set; }
        public bool RotaryZeroPositionLimit_2 { get; set; }
        public bool BucketWheelOverTorqueSwitch_2 { get; set; }
        public bool BucketWheelForcedLubricationFlowSwitch_2 { get; set; }
        public bool VariableAmplitudeUpperLimit_2 { get; set; }
        public bool VariableAmplitudeLowerLimit_2 { get; set; }
        public bool VariableAmplitudeUpperExtremeLimit_2 { get; set; }
        public bool VariableAmplitudeLowerExtremeLimit_2 { get; set; }
        public bool VariableAmplitudeLowerForbiddenZoneLimit_2 { get; set; }
        public bool CabinFrontBalanceLimit_2 { get; set; }
        public bool VariableAmplitudeOilHeaterStartup_2 { get; set; }
        public bool VariableAmplitudeOilHeaterStop_2 { get; set; }
        public bool VariableAmplitudeFanStop_2 { get; set; }
        public bool VariableAmplitudeFanStartup_2 { get; set; }
        public bool VariableAmplitudeOilLevelLowSignal_2 { get; set; }
        public bool VariableAmplitudePumpStationOverheatAlarm_2 { get; set; }
        public bool VariableAmplitudeOilLevelVeryLowSignal_2 { get; set; }
        public bool RotaryCentralizedLubricationLowOilLevelFault_2 { get; set; }
        public bool LargeCarFrequencyConverterPowerOn_2 { get; set; }
        public bool LargeCarBrakeOpen_2 { get; set; }
        public bool LargeCarFrequencyConverterFaultReset_2 { get; set; }
        public bool LargeCarReverseCommand_2 { get; set; }
        public bool LargeCarHighLowSpeedSelection_2 { get; set; }
        public bool BucketWheelMaterialLoadingRunning_2 { get; set; }
        public bool BucketWheelFault_2 { get; set; }
        public bool SuspensionBeltFirstLevelDeviationSwitch_2 { get; set; }
        public bool SuspensionBeltSecondLevelDeviationSwitch_2 { get; set; }
        public bool SuspensionBeltEmergencyStopSwitch_2 { get; set; }
        public bool SuspensionBeltSpeedDetectionSwitch_2 { get; set; }
        public bool SuspensionBeltMaterialFlowDetectionSwitch_2 { get; set; }
        public bool SuspensionBeltLongitudinalTearSwitch_2 { get; set; }
        public bool RotaryCentralizedLubricationOilBlockageFault_2 { get; set; }
        public bool LargeCarForwardCommand_2 { get; set; }
        public bool VariableAmplitudeOilPumpMotorRunning_2 { get; set; }
        public bool VariableAmplitudeOilHeaterRunning_2 { get; set; }
        public bool VariableAmplitudeFanRunning_2 { get; set; }
        public bool LeftClampPumpRunning_2 { get; set; }
        public bool RightClampPumpRunning_2 { get; set; }
        public bool LeftClampElectromagneticValveOpen_2 { get; set; }
        public bool RightClampElectromagneticValveOpen_2 { get; set; }
        public bool RotaryFrequencyConverterPowerOn_2 { get; set; }
        public bool RotaryBrakeOpen_2 { get; set; }
        public bool RotaryLeftTurnCommand_2 { get; set; }
        public bool RotaryRightTurnCommand_2 { get; set; }
        public bool RotaryFrequencyConverterFaultReset_2 { get; set; }
        public bool RotarySpeedGivenSelection_2 { get; set; }
        public bool RotaryFanRunning_2 { get; set; }
        public bool VariableAmplitudeLowerElectromagneticValveOpen_2 { get; set; }
        public ushort RiseCount_2 { get; set; }
        public bool SingleAction_2 { get; set; }
        public bool LinkAction_2 { get; set; }
        public bool Automatic_2 { get; set; }
        public bool LargeCarFault_2 { get; set; }
        public bool LargeCarForwardLimiting_2 { get; set; }
        public bool LargeCarReverseLimiting_2 { get; set; }
        public bool AnchorClamp_2 { get; set; }
        public bool LargeCarForward_2 { get; set; }
        public bool LargeCarReverse_2 { get; set; }
        public bool RotaryFault_2 { get; set; }
        public bool RotaryLeftTurnLimiting_2 { get; set; }
        public bool RotaryRightTurnLimiting_2 { get; set; }
        public bool RotaryLeftTurn_2 { get; set; }
        public bool RotaryRightTurn_2 { get; set; }
        public bool VariableAmplitudeFault_2 { get; set; }
        public bool VariableAmplitudeUpperLimiting_2 { get; set; }
        public bool VariableAmplitudeLowerLimiting_2 { get; set; }
        public bool VariableAmplitudeUpper_2 { get; set; }
        public bool VariableAmplitudeLower_2 { get; set; }
        public bool SuspensionBeltFault_2 { get; set; }
        public bool SuspensionBeltManualLoading_2 { get; set; }
        public bool SuspensionBeltManualUnloading_2 { get; set; }
        public bool SuspensionBeltLinkLoading_2 { get; set; }
        public bool SuspensionBeltLinkUnloading_2 { get; set; }
        public bool BucketWheelFaulting_2 { get; set; }
        public bool BucketWheelSingleStartup_2 { get; set; }
        public bool BucketWheelLinkStartup_2 { get; set; }
        public bool ClampFault_2 { get; set; }
        public bool ClampRelax_2 { get; set; }
        public bool CentralBaffleFault_2 { get; set; }
        public bool TailCarBeltFault_2 { get; set; }
        public bool MaterialLevelMeter_2 { get; set; }
        public bool ManualIntervention_2 { get; set; }
        public bool InterventionRelease_2 { get; set; }
        public bool SuspensionBeltLoadingButton_2 { get; set; }
        public bool SuspensionBeltStopButton_2 { get; set; }
        public bool SuspensionBeltUnloadingButton_2 { get; set; }
        public bool BucketWheelStartupButton_2 { get; set; }
        public bool BucketWheelStopButton_2 { get; set; }
        public ushort RotaryCount_2 { get; set; }
        public bool LeftAnchorNotLifted_2 { get; set; }
        public bool RightAnchorNotLifted_2 { get; set; }
        public bool ClampNotRelaxed_2 { get; set; }
        public bool LargeCarBrakeNotOpen_2 { get; set; }
        public bool LargeCarFrequencyConverterNotPowered_2 { get; set; }
        public bool LargeCarBrakeContactAuxiliaryFault_2 { get; set; }
        public bool LargeCarFrequencyConverterContactAuxiliaryFault_2 { get; set; }
        public bool RotaryFrequencyConverterNotPowered_2 { get; set; }
        public bool RotaryFrequencyConverterContactAuxiliaryFault_2 { get; set; }
        public bool RotaryBrakeContactAuxiliaryFault_2 { get; set; }
        public bool VariableAmplitudeOilPumpMotorNotRunning_2 { get; set; }
        public bool SuspensionBeltBrakeContactAuxiliaryFault_2 { get; set; }
        public bool SuspensionBeltLoadingContactAuxiliaryFault_2 { get; set; }
        public bool SuspensionBeltUnloadingContactAuxiliaryFault_2 { get; set; }
        public bool SuspensionBeltFirstLevelDeviation_2 { get; set; }
        public bool BucketWheelLubricationPumpContactAuxiliaryFault_2 { get; set; }
        public bool WindproofSystemCableLimit1_2 { get; set; }
        public bool RotaryFrequencyConverterFault_2 { get; set; }
        public bool RotaryFanOverload_2 { get; set; }
        public bool RotaryBrakeResistorOverheatSwitch_2 { get; set; }
        public bool DiversionBaffleMotorOverload_2 { get; set; }
        public bool TailCarFirstLevelDeviationSwitch_2 { get; set; }
        public bool TailCarSecondLevelDeviationSwitch_2 { get; set; }
        public bool TailCarEmergencyStopSwitch_2 { get; set; }
        public bool RotaryLeftTurnForbiddenLimit_2 { get; set; }
        public bool RotaryRightTurnForbiddenLimit_2 { get; set; }
        public bool BucketWheelMaterialUnloadingRunning_2 { get; set; }
        public bool VariableAmplitudeUpperElectromagneticValveOpen_2 { get; set; }
        public bool SuspensionBeltLoadingRunning_2 { get; set; }
        public bool SuspensionBeltUnloadingRunning_2 { get; set; }
        public bool SuspensionBeltBrakeOpen_2 { get; set; }
        public bool BucketWheelMotorRunning_2 { get; set; }
        public bool BucketWheelLubricationPumpRunning_2 { get; set; }
        public bool DiversionBaffleDownRunning_2 { get; set; }
        public bool DiversionBaffleUpRunning_2 { get; set; }
        public bool VibrationMotorRunning_2 { get; set; }
        public bool VariableAmplitudeBoostValveOpen_2 { get; set; }
        public bool BaffleDownLimit_2 { get; set; }
        public bool BaffleUpLimit_2 { get; set; }
        public bool RotaryOverTorque_2 { get; set; }
        public bool VariableAmplitudeOilPumpMotorContactFault_2 { get; set; }
        public bool BucketWheelMotorContactAuxiliaryFault_2 { get; set; }
        public bool TailCarOilPumpMotorContactAuxiliaryFault_2 { get; set; }
        public bool VibrationMotorFault_2 { get; set; }
        public bool ReelEmptySwitch_2 { get; set; }
        public bool WindproofSystemCableNotOpen_2 { get; set; }
        public bool LargeCarLimitAction_2 { get; set; }// 200

        // 201
        public bool RotaryLimitAction_2 { get; set; }
        public bool VariableAmplitudeLimitAction_2 { get; set; }
        public bool ForbiddenZoneLimitAction_2 { get; set; }
        public bool RotaryCrashSwitchAction_2 { get; set; }
        public bool LargeCarCentralizedLubricationLowOilLevelAlarm_2 { get; set; }
        public bool LargeCarCentralizedLubricationOilBlockageAlarm_2 { get; set; }
        public bool RotaryCentralizedLubricationLowOilLevelAlarm_2 { get; set; }
        public bool RotaryCentralizedLubricationOilBlockageAlarm_2 { get; set; }
        public bool StrongWindPreAlarm_2 { get; set; }
        public bool BucketWheelCentralizedLubricationLowOilLevelAlarm_2 { get; set; }
        public bool BucketWheelCentralizedLubricationOilBlockageAlarm_2 { get; set; }
        public bool ManualGuideSlotLiftButton_2 { get; set; }
        public bool ManualBucketWheelSlotStopButton_2 { get; set; }
        public bool ManualBucketWheelSlotDownButton_2 { get; set; }
        public bool CentralBaffleManualLiftButton_2 { get; set; }
        public bool CentralBaffleManualStopButton_2 { get; set; }
        public bool CentralBaffleManualDownButton_2 { get; set; }
        public bool VariableAmplitudeOilHeaterManualStartupButton_2 { get; set; }
        public bool VariableAmplitudeOilHeaterManualStopButton_2 { get; set; }
        public bool VariableAmplitudeFanManualStartupButton_2 { get; set; }
        public bool VariableAmplitudeFanManualStopButton_2 { get; set; }
        public bool ElectricRoomEmergencyStopButtonAction_2 { get; set; }
        public bool CabinEmergencyStopButtonAction_2 { get; set; }
        public bool EmergencyStopRelayNot_2 { get; set; }
        public bool TransformerOverheatAlarm_2 { get; set; }
        public bool ElectricRoomPLCModulePowerFault_2 { get; set; }
        public bool CabinPLCModulePowerFault_2 { get; set; }
        public bool ElectricRoomFireAlarm_2 { get; set; }
        public bool CabinFireAlarm_2 { get; set; }
        public bool SuspensionBeltEmergencyStop_2 { get; set; }
        public bool TailCarBeltEmergencyStopSwitch_2 { get; set; }
        public bool LargeCarMainCircuitBreakerFault_2 { get; set; }
        public bool LargeCarMotorCircuitBreakerFault_2 { get; set; }
        public bool LargeCarBrakeCircuitBreakerFault_2 { get; set; }
        public bool Spare1_2 { get; set; }
        public bool CarFrequencyConverterFault_2 { get; set; }
        public bool LargeCarBrakeResistorOverheatJump_2 { get; set; }
        public bool CableReelMainCircuitBreakerFault_2 { get; set; }
        public bool CableReelMotorOverloading_2 { get; set; }
        public bool PowerReelCableOverLooseAlarm_2 { get; set; }
        public bool PowerReelCableOverTightAlarm_2 { get; set; }
        public bool PowerReelFullDiskAlarm_2 { get; set; }
        public bool PowerReelEmptyDiskAlarm_2 { get; set; }
        public bool LargeCarOperationHandleFault_2 { get; set; }
        public bool RotaryMainCircuitBreakerFault_2 { get; set; }
        public bool RotaryBrakeOverloadAlarm_2 { get; set; }
        public bool RotaryFanOverloadAlarm_2 { get; set; }
        public bool RotaryFrequencyConverterFaulting_2 { get; set; }
        public bool RotaryBrakeResistorOverheatSwitching_2 { get; set; }
        public bool RotaryOverTorqueSwitch_2 { get; set; }
        public bool ReversalHandleFault_2 { get; set; }
        public bool LinkedBucketWheelNotRunning_2 { get; set; }
        public bool VariableFrequencyMainCircuitBreakerFault_2 { get; set; }
        public bool VariableFrequencyMotorOverload_2 { get; set; }
        public bool VariableFrequencyPumpClogged_2 { get; set; }
        public bool VariableFrequencyPumpStationHighTemperatureAlarm_2 { get; set; }
        public bool VariableFrequencyOilTankLowLevelAlarm_2 { get; set; }
        public bool Spare2_2 { get; set; }
        public bool VariableFrequencyHandleFault_2 { get; set; }
        public bool SuspendedBeltCircuitBreakerFault_2 { get; set; }
        public bool SuspendedBeltMotorOverload_2 { get; set; }
        public bool SuspendedBeltSecondLevelDeviationSwitch_2 { get; set; }
        public bool SuspendedBeltEmergencyStop_2 { get; set; }
        public bool SuspendedBeltSlip_2 { get; set; }
        public bool SuspendedBeltLongitudinalTearSwitch_2 { get; set; }
        public bool CentralHopperCloggedDetectionSwitch_2 { get; set; }
        public bool StackingSwitchFault_2 { get; set; }
        public bool CentralControlRoomNoStackingCommand_2 { get; set; }
        public bool BucketWheelMotorMainCircuitBreakerFault_2 { get; set; }
        public bool BucketWheelMotorOverloading_2 { get; set; }
        public bool BucketWheelOverTorqueSwitching_2 { get; set; }
        public bool BucketWheelTemperatureUpperLimitAlarm_2 { get; set; }
        public bool ClampingDeviceMainCircuitBreakerFault_2 { get; set; }
        public bool ClampingDeviceMotorOverload_2 { get; set; }
        public bool LeftClampingDeviceTimeout_2 { get; set; }
        public bool RightClampingDeviceTimeout_2 { get; set; }
        public bool StrongWindAlarm_2 { get; set; }
        public bool DryFogSystemLowAirPressure_2 { get; set; }
        public bool DryFogSystemLowWaterPressure_2 { get; set; }
        public bool DryFogSystemFilterClogged_2 { get; set; }
        public bool DryFogSystemWaterTankLowLevel_2 { get; set; }
        public bool DiversionPlateCircuitBreakerFault_2 { get; set; }
        public bool DiversionPlateMotorOverload_2 { get; set; }
        public bool DiversionPlateTimeout_2 { get; set; }
        public bool CentralControlRoomNoStackingOrDiversionCommand_2 { get; set; }
        public bool BucketWheelFeederCircuitBreakerFault_2 { get; set; }
        public bool BucketWheelFeederMotorOverload_2 { get; set; }
        public bool BucketWheelFeederTimeout_2 { get; set; }
        public bool CentralControlRoomNoStackingUnloadingCommand_2 { get; set; }
        public bool TailCarBeltFirstLevelDeviation_2 { get; set; }
        public bool TailCarBeltSecondLevelDeviation_2 { get; set; }
        public bool TailCarBeltLongitudinalTear_2 { get; set; }
        public bool Spare3_2 { get; set; }//293
        public bool VibrationMotorCircuitBreakerFault_2 { get; set; }
        public bool VibrationMotorOverloading_2 { get; set; }
        public bool DriverRoomEmergencyStopButton_2 { get; set; }
        public bool ElectricalRoomEmergencyStopButton_2 { get; set; }
        public bool EmergencyStopRelay_2 { get; set; }
        public bool TwoMachineCollisionAlarm_2 { get; set; }
        public bool RollerFullDiskSwitch_2 { get; set; }
        public bool RollerMiddleSwitch_2 { get; set; }
        public bool BucketWheelMotorContactor_2 { get; set; }
        public bool VariableFrequencyOilBlockageSignal_2 { get; set; }
        public bool VariableFrequencyOverpressureStop_2 { get; set; }
        public bool VariableFrequencyPumpStationOverpressureAlarm_2 { get; set; }
        public bool PowerRollerRunning_2 { get; set; }
        public bool DriverRoomLevelingContactor_2 { get; set; }
        public bool DryFogSystemIsLowAirPressure_2 { get; set; }
        public bool DryFogSystemIsLowWaterPressure_2 { get; set; }
        public bool WaterTankLowLevelSwitch_2 { get; set; }
        public bool DriverRoomRiseValve_2 { get; set; }
        public bool DriverRoomDescentValve_2 { get; set; }
        public bool PowerCableRollerNotRunning_2 { get; set; }
        public bool TailCarDrivenRollerBearingTemperatureUpperLimitAlarm_2 { get; set; }
        public bool TailCarDrivenRollerBearingTemperatureLowerLimitAlarm_2 { get; set; }
        public bool AllowBucketWheelDiversion_2 { get; set; }
        public bool WindproofSystemCableLimit2_2 { get; set; }
        public bool WindproofSystemCableLimit3_2 { get; set; }
        public bool RollerOverTightLimit2_2 { get; set; }
        public bool RollerOverLooseLimit2_2 { get; set; }
        public bool DryFogSystemFilterIsClogged_2 { get; set; }
        public bool DryFogSystemAutoRun_2 { get; set; }
        public bool DryFogSystemManualRun_2 { get; set; }
        public bool DryFogSystemSprayStatus_2 { get; set; }
        public bool DryFogSystemHeatRun_2 { get; set; }
        public bool BucketWheelSlotMainCircuitBreaker_2 { get; set; }
        public bool BucketWheelSlotMotorOverload_2 { get; set; }
        public bool TailCarBeltLongitudinalTearing_2 { get; set; }
        public bool ReversalBrakeRelease_2 { get; set; }
        public bool BucketWheelSlotLiftLimit_2 { get; set; }
        public bool BucketWheelSlotLowerLimit_2 { get; set; }
        public bool DiversionPlateLimit_2 { get; set; }
        public bool SuspendedBeltBrakeRelease_2 { get; set; }
        public bool BrokenBeltCaptureAlarm_2 { get; set; }
        public bool BucketWheelCentralizedLubricationLowOilLevel_2 { get; set; }
        public bool BucketWheelCentralizedLubricationClogged_2 { get; set; }
        public bool DriverRoomRearBalanceLimit_2 { get; set; }
        public bool BucketWheelDiversionRunning_2 { get; set; }
        public bool DriverRoomLevelingPumpRunning_2 { get; set; }
        public bool BucketWheelSlotLift_2 { get; set; }
        public bool BucketWheelSlotLower_2 { get; set; }
        public bool RemoteEmergencyStop_2 { get; set; }
        public ushort DiversionPlateAngle_2 { get; set; }
        public bool DryFogDustSuppressionStackingRunning_2 { get; set; }
        public bool DryFogDustSuppressionReclaimingRunning_2 { get; set; }
        public bool DryFogDustSuppressionDiversionRunning_2 { get; set; }
        public bool DryFogDustSuppressionRemoteStartRunning_2 { get; set; }
        public bool DryFogDustSuppressionRemoteStopRunning_2 { get; set; }// 348
        public bool TailCarDrivenRollerBearingUpperLimitAlarm_2 { get; set; }
        public bool TailCarDrivenRollerBearingLowerLimitAlarm_2 { get; set; }
        public bool UnmannedEmergencyStop_2 { get; set; }
        public bool RemoteEmergencyStoping_2 { get; set; }
        public bool LargeVehicleMotor1OvertemperatureAlarm_2 { get; set; }
        public bool LargeVehicleMotor2OvertemperatureAlarm_2 { get; set; }
        public bool LargeVehicleMotor3OvertemperatureAlarm_2 { get; set; }
        public bool LargeVehicleMotor4OvertemperatureAlarm_2 { get; set; }
        public bool LargeVehicleMotor5OvertemperatureAlarm_2 { get; set; }
        public bool LargeVehicleMotor6OvertemperatureAlarm_2 { get; set; }
        public bool WalkingReducerBearingTemperatureUpperLimitAlarm_2 { get; set; }
        public bool WalkingReducerBearingTemperatureLowerLimitAlarm_2 { get; set; }
        public bool WalkingReducerOilTemperatureUpperLimitAlarm_2 { get; set; }
        public bool WalkingReducerOilTemperatureLowerLimitAlarm_2 { get; set; }
        public bool ReversalTemperatureUpperLimitAlarm_2 { get; set; }
        public bool ReversalTemperatureLowerLimitAlarm_2 { get; set; }
        public bool BrokenBeltCaptureAlarming_2 { get; set; }
        public bool SuspendedBeltTemperatureUpperLimitAlarm_2 { get; set; }
        public bool SuspendedBeltTemperatureLowerLimitAlarm_2 { get; set; }
        public bool SuspendedBeltRollerBearingTemperatureUpperLimitAlarm_2 { get; set; }
        public bool SuspendedBeltRollerBearingTemperatureLowerLimitAlarm_2 { get; set; }
        public bool BucketWheelTemperatureLowerLimitAlarm_2 { get; set; }
        public bool CableRollerContactorAuxiliaryContactFault_2 { get; set; }
        public bool DriverRoomBalancePumpMotorNotRunning_2 { get; set; }
        public bool DriverRoomBalancePumpMotorAuxiliaryContactFault_2 { get; set; }
        public bool Remote_2 { get; set; }
        public ushort TwoMachineDistance_2 { get; set; } //两机距离
        public ushort DriverRoomAngle_2 { get; set; } //司机室角度
        public bool DriverRoomRiseButton_2 { get; set; }
        public bool DriverRoomDescentButton_2 { get; set; }






















        // D2PLC2

































    }
}
