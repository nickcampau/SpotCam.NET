namespace SpotCam.Interop
{
    public enum CoreParameter : short
    {
        /// <summary>
        /// An invalid parameter
        /// </summary>
        Unknown = 0,
        
        ///<summary>
        /// 
        ///</summary>
        AutoExpose                              =  100,

        ///<summary>
        /// 
        ///</summary>
        BrightnessAdjustment                    =  101,

        ///<summary>
        /// 
        ///</summary>
        AutoGainLimit                           =  102,

        ///<summary>
        /// 
        ///</summary>
        BinSize                                 =  103,

        ///<summary>
        /// 
        ///</summary>
        ImageRect                               =  104,

        ///<summary>
        /// 
        ///</summary>
        ColorEnable                             =  108,

        ///<summary>
        /// 
        ///</summary>
        ColorOrder                              =  109,

        ///<summary>
        /// 
        ///</summary>
        WhiteBalance                            =  110,

        ///<summary>
        /// 
        ///</summary>
        ImageType                               =  111,

        ///<summary>
        /// 
        ///</summary>
        CorrectChipDefects                      =  112,

        ///<summary>
        /// 
        ///</summary>
        BitDepth                                =  113,

        ///<summary>
        /// 
        ///</summary>
        MessageEnable                           =  114,

        ///<summary>
        /// 
        ///</summary>
        LiveBrightnessAdj                       =  118,

        ///<summary>
        /// 
        ///</summary>
        BrightnessAdjustmentLimits              =  120,

        ///<summary>
        /// 
        ///</summary>
        BinSizeLimits                           =  121,

        ///<summary>
        /// 
        ///</summary>
        MaxImageRectSize                        =  122,

        ///<summary>
        /// 
        ///</summary>
        Port0GainValues8                        =  123,

        ///<summary>
        /// 
        ///</summary>
        BitDepths                               =  124,

        ///<summary>
        /// 
        ///</summary>
        Port0LiveGainValues                     =  128,

        ///<summary>
        /// 
        ///</summary>
        Port0GainValues16                       =  129,

        ///<summary>
        /// 
        ///</summary>
        MaxLiveAccelerationLevel                =  130,

        ///<summary>
        /// 
        ///</summary>
        MaxWhiteBalanceRatio                    =  131,

        ///<summary>
        /// 
        ///</summary>
        MinExposureIncrement                    =  133,

        ///<summary>
        /// 
        ///</summary>
        ExposureConversionFactor                =  134,

        ///<summary>
        /// 
        ///</summary>
        MosaicPattern                           =  136,

        ///<summary>
        /// 
        ///</summary>
        ExternalTriggerDelayLimits              =  137,

        ///<summary>
        /// 
        ///</summary>
        RegulatedTemperatureLimits              =  138,

        ///<summary>
        /// 
        ///</summary>
        HorizontalReadoutFrequencies            =  139,

        ///<summary>
        /// 
        ///</summary>
        TtlOutputDelayLimits                    =  140,

        ///<summary>
        /// 
        ///</summary>
        ExposureResolution                      =  141,

        ///<summary>
        /// 
        ///</summary>
        MaxGainPortNumber                       =  144,

        ///<summary>
        /// 
        ///</summary>
        Port1GainValueLimits                    =  145,

        ///<summary>
        /// 
        ///</summary>
        Port1LiveGainValueLimits                =  146,

        ///<summary>
        /// 
        ///</summary>
        Port2GainValueLimits                    =  147,

        ///<summary>
        /// 
        ///</summary>
        Port2LiveGainValueLimits                =  148,

        ///<summary>
        /// 
        ///</summary>
        Port3GainValueLimits                    =  149,

        ///<summary>
        /// 
        ///</summary>
        Port3LiveGainValueLimits                =  150,

        ///<summary>
        /// 
        ///</summary>
        BinSizes                                =  151,

        ///<summary>
        /// 
        ///</summary>
        MaxPixelResolutionLevel                 =  152,

        ///<summary>
        /// 
        ///</summary>
        AcquiredImageSize                       =  153,

        ///<summary>
        /// 
        ///</summary>
        AcquiredLiveImageSize                   =  154,

        ///<summary>
        /// 
        ///</summary>
        MinImageSize                            =  155,

        ///<summary>
        /// 
        ///</summary>
        ColorBinningSizes                       =  156,

        ///<summary>
        /// 
        ///</summary>
        LiveAutoGainLimit                       =  200,

        ///<summary>
        /// 
        ///</summary>
        SubtractBlackLevel                      =  201,

        ///<summary>
        /// 
        ///</summary>
        MonitorFilterPosition                   =  202,

        ///<summary>
        /// 
        ///</summary>
        ColorEnable2                            =  203,

        ///<summary>
        /// 
        ///</summary>
        DriverDeviceNumber                      =  204,

        ///<summary>
        /// 
        ///</summary>
        EnableTtlOutput                         =  205,

        ///<summary>
        /// 
        ///</summary>
        TtlOutputDelayMs                        =  206,

        ///<summary>
        /// 
        ///</summary>
        LiveGammaAdjustment                     =  207,

        ///<summary>
        /// 
        ///</summary>
        LiveExposure64                          =  209,

        ///<summary>
        /// 
        ///</summary>
        MinimumExposureMsec                     =  211,

        ///<summary>
        /// 
        ///</summary>
        ReturnRawMosaicData                     =  212,

        ///<summary>
        /// 
        ///</summary>
        LiveAccelerationLevel                   =  213,

        ///<summary>
        /// 
        ///</summary>
        EnhanceColors                           =  214,

        ///<summary>
        /// 
        ///</summary>
        FlatfieldCorrect                        =  215,

        ///<summary>
        /// 
        ///</summary>
        NoiseFilterThresholdPercent             =  216,

        ///<summary>
        /// 
        ///</summary>
        LiveAutoBrightness                      =  217,

        ///<summary>
        /// 
        ///</summary>
        LiveAutoBrightnessAdjustment            =  218,

        ///<summary>
        /// 
        ///</summary>
        LiveMaxExposureMsec                     =  220,

        ///<summary>
        /// 
        ///</summary>
        ExposureIncrement                       =  221,

        ///<summary>
        /// 
        ///</summary>
        ExternalTriggerMode                     =  222,

        ///<summary>
        /// 
        ///</summary>
        MaxExposureMsec                         =  223,

        ///<summary>
        /// 
        ///</summary>
        WhiteBalanceComputationRect             =  224,

        ///<summary>
        /// 
        ///</summary>
        BiasFrameSubtract                       =  225,

        ///<summary>
        /// 
        ///</summary>
        DeviceUid                               =  226,

        ///<summary>
        /// 
        ///</summary>
        BusBandwidth                            =  227,

        ///<summary>
        /// 
        ///</summary>
        ExposureComputationRect                 =  228,

        ///<summary>
        /// 
        ///</summary>
        ExternalTriggerActiveState              =  229,

        ///<summary>
        /// 
        ///</summary>
        ExternalTriggerDelay                    =  230,

        ///<summary>
        /// 
        ///</summary>
        RegulateTemperature                     =  231,

        ///<summary>
        /// 
        ///</summary>
        RegulatedTemperature                    =  232,

        ///<summary>
        /// 
        ///</summary>
        HorizontalReadoutFrequency              =  233,

        ///<summary>
        /// 
        ///</summary>
        TtlOutputActiveState                    =  234,

        ///<summary>
        /// 
        ///</summary>
        TtlOutputDelay                          =  235,

        ///<summary>
        /// 
        ///</summary>
        BackgroundImageSubtract                 =  236,

        ///<summary>
        /// 
        ///</summary>
        GainPortNumber                          =  237,

        ///<summary>
        /// 
        ///</summary>
        ColorRenderingIntent                    =  238,

        ///<summary>
        /// 
        ///</summary>
        LiveEnhanceColors                       =  239,

        ///<summary>
        /// 
        ///</summary>
        PixelResolutionLevel                    =  240,

        ///<summary>
        /// 
        ///</summary>
        InputColorProfile                       =  241,

        ///<summary>
        /// 
        ///</summary>
        OutputColorProfile                      =  242,

        ///<summary>
        /// 
        ///</summary>
        ColorBinSize                            =  243,

        ///<summary>
        /// 
        ///</summary>
        SensorClearMode                         =  244,

        ///<summary>
        /// 
        ///</summary>
        CoolingLevel                            =  245,

        ///<summary>
        /// 
        ///</summary>
        FanSpeed                                =  246,

        ///<summary>
        /// 
        ///</summary>
        FanExposureSpeed                        =  247,

        ///<summary>
        /// 
        ///</summary>
        FanExposureDelayMs                      =  248,

        ///<summary>
        /// 
        ///</summary>
        PreAmpGainValue                         =  249,

        ///<summary>
        /// 
        ///</summary>
        VerticalShiftPeriod                     =  250,

        ///<summary>
        /// 
        ///</summary>
        VerticalClockVoltageBoost               =  251,

        ///<summary>
        /// 
        ///</summary>
        ReadoutCircuit                          =  252,

        ///<summary>
        /// 
        ///</summary>
        NumberSkipLines                         =  253,

        ///<summary>
        /// 
        ///</summary>
        ShutterMode                             =  254,

        ///<summary>
        /// 
        ///</summary>
        EnablePowerStateControl                 =  260,

        ///<summary>
        /// 
        ///</summary>
        ForceSingleChannelLiveMode              =  261,

        ///<summary>
        /// 
        ///</summary>
        LiveImageScaling                        =  262,

        ///<summary>
        /// 
        ///</summary>
        LiveHistogram                           =  263,

        ///<summary>
        /// 
        ///</summary>
        SensorResponseMode                      =  264,

        ///<summary>
        /// 
        ///</summary>
        SequentialImageDiskCachePath            =  265,

        ///<summary>
        /// 
        ///</summary>
        LiveSubtractBlackLevel                  =  267,

        ///<summary>
        /// 
        ///</summary>
        LivePixelResolutionLevel                =  268,

        ///<summary>
        /// 
        ///</summary>
        CoolerModeOnExit                        =  269,

        ///<summary>
        /// 
        ///</summary>
        Readout8Bit                             =  271,

        ///<summary>
        /// 
        ///</summary>
        ReadoutChannelMode                      =  272,

        ///<summary>
        /// 
        ///</summary>
        Exposure64                              =  273,

        ///<summary>
        /// 
        ///</summary>
        SequentialImageExposureDurations64      =  274,

        ///<summary>
        /// 
        ///</summary>
        AutoGainMinimum                         =  280,

        ///<summary>
        /// 
        ///</summary>
        LiveAutoGainMinimum                     =  281,

        ///<summary>
        /// 
        ///</summary>
        LabPixelAdjustments                     =  290,

        ///<summary>
        /// 
        ///</summary>
        PixelSizeNm                             =  401,

        ///<summary>
        /// 
        ///</summary>
        CoolingLevels                           =  403,

        ///<summary>
        /// 
        ///</summary>
        FanSpeeds                               =  404,

        ///<summary>
        /// 
        ///</summary>
        NumberReadoutCircuits                   =  405,

        ///<summary>
        /// 
        ///</summary>
        PreAmpGainValues                        =  406,

        ///<summary>
        /// 
        ///</summary>
        VerticalShiftPeriods                    =  407,

        ///<summary>
        /// 
        ///</summary>
        MaxVerticalClockVoltageBoost            =  408,

        ///<summary>
        /// 
        ///</summary>
        Port0GainAttributes                     =  411,

        ///<summary>
        /// 
        ///</summary>
        Port1GainAttributes                     =  412,

        ///<summary>
        /// 
        ///</summary>
        Port2GainAttributes                     =  413,

        ///<summary>
        /// 
        ///</summary>
        Port3GainAttributes                     =  414,

        ///<summary>
        /// 
        ///</summary>
        ReadoutCircuitDescription               =  415,

        ///<summary>
        /// 
        ///</summary>
        ImageSensorModelDescription             =  416,

        ///<summary>
        /// 
        ///</summary>
        ImageSensorType                         =  417,

        ///<summary>
        /// 
        ///</summary>
        MinFastSequentialImageExposureDuration64=  419,

        ///<summary>
        /// 
        ///</summary>
        MaxNumberSkipLines                      =  421,

        ///<summary>
        /// 
        ///</summary>
        SensorResponseModes                     =  422,

        ///<summary>
        /// 
        ///</summary>
        SensorClearModes                        =  423,

        ///<summary>
        /// 
        ///</summary>
        MinPixelResolutionLevel                 =  424,

        ///<summary>
        /// 
        ///</summary>
        MaxNumberSequentialImageExposures       =  425,

        ///<summary>
        /// 
        ///</summary>
        PixelResolutionImageSizeFactors         =  426,

        ///<summary>
        /// 
        ///</summary>
        ReadoutChannelModes                     =  427,

        ///<summary>
        /// 
        ///</summary>
        ExposureLimits64                        =  428,

        ///<summary>
        /// 
        ///</summary>
        WaitForStatusChanges                    =  501,

        ///<summary>
        /// 
        ///</summary>
        ImageOrientation                        =  701,

        ///<summary>
        /// 
        ///</summary>
        ImageBufferFormat24Bpp                  =  702,

        ///<summary>
        /// 
        ///</summary>
        NumberBytesPerImageRow                  =  703,
    }
}