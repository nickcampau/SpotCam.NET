
namespace SpotCam.Interop
{
    /// <summary>
    /// All return code values defined in the SpotCam API up to version 5.0
    /// </summary>
    /// <remarks>Negative values are only warnings</remarks>
    public enum SpotCamReturnCode : int
    {
        /// <summary>
        /// Operation completed successfully
        /// </summary>
        Success                                 =    0,

        /// <summary>
        /// The service-configuration contains settings that are not recognized by this version of the library
        /// </summary>
        WarnServiceConfigurationUnknown         = -104,

        /// <summary>
        /// The color enhancement library could not be loaded
        /// </summary>
        WarnColorLibNotLoaded                   = -103,

        /// <summary>
        /// The output color profile for the camera cannot be found or is invalid
        /// </summary>
        WarnInvalidOutputIcc                    = -102,

        /// <summary>
        /// The input color profile for the camera cannot be found or is invalid
        /// </summary>
        WarnInvalidInputIcc                     = -101,

        /// <summary>
        /// The camera has features which are not supported by this software version
        /// </summary>
        WarnUnsupportedCameraFeatures           = -100,

        /// <summary>
        /// Operation was aborted by the caller
        /// </summary>
        Abort                                   =  100,

        /// <summary>
        /// Memory allocation failure
        /// </summary>
        ErrorOutOfMemory                        =  101,

        /// <summary>
        /// Exposure times is too short
        /// </summary>
        ErrorExposureTooShort                   =  102,

        /// <summary>
        /// Exposure times is too long
        /// </summary>
        ErrorExposureTooLong                    =  103,

        /// <summary>
        /// Camera is not responding to commands
        /// </summary>
        ErrorNoCameraResponse                   =  104,

        /// <summary>
        /// Specified value is out of valid range
        /// </summary>
        ErrorValueOutOfRange                    =  105,

        /// <summary>
        /// Specified parameter number is not valid
        /// </summary>
        ErrorInvalidParam                       =  106,

        /// <summary>
        /// SpotInit has not yet been successfully called
        /// </summary>
        ErrorDriverNotInitialized               =  107,

        /// <summary>
        /// Error reading from the system registry (Windows Only)
        /// </summary>
        ErrorRegistryQuery                      =  109,

        /// <summary>
        /// Error writing to the system registry (Windows Only)
        /// </summary>
        ErrorRegistrySet                        =  110,

        /// <summary>
        /// Error loading device driver
        /// </summary>
        ErrorDeviveDriverLoad                   =  112,

        /// <summary>
        /// Camera is malfunctioning
        /// </summary>
        ErrorCameraError                        =  114,

        /// <summary>
        /// SpotInit has already been successfully called
        /// </summary>
        ErrorDriverAlreadyInit                  =  115,

        /// <summary>
        /// The DMA buffer could not be setup
        /// </summary>
        ErrorDmaSetup                           =  117,

        /// <summary>
        /// There was an error reading the camera's configuration information
        /// </summary>
        ErrorReadingCameraInfo                  =  118,

        /// <summary>
        /// The camera or driver is not capable of performing the command
        /// </summary>
        ErrorNotCapable                         =  119,

        /// <summary>
        /// The color filter is not in the IN position
        /// </summary>
        ErrorColorFilterNotIn                   =  120,

        /// <summary>
        /// The color filter is not in the OUT position
        /// </summary>
        ErrorColorFilterNotOut                  =  121,

        /// <summary>
        ///  The camera is currently in another operation
        /// </summary>
        ErrorCameraBusy                         =  122,

        /// <summary>
        /// The camera model is not supported by this version
        /// </summary>
        ErrorCameraNotSupported                 =  123,

        /// <summary>
        /// There is no image available
        /// </summary>
        ErrorNoImageAvailable                   =  125,

        /// <summary>
        /// The specified file cannot be opened or created
        /// </summary>
        ErrorFileOpen                           =  126,

        /// <summary>
        /// The specified flatfield is incompatible with the current camera/parameters
        /// </summary>
        ErrorFlatfieldIncompatible              =  127,

        /// <summary>
        /// No SPOT interface cards or cameras were found
        /// </summary>
        ErrorNoDevicesFound                     =  128,

        /// <summary>
        /// The brightness changed while exposure was being computed
        /// </summary>
        ErrorBrightnessChanged                  =  129,

        /// <summary>
        /// The camera is incompatible with the interface card
        /// </summary>
        ErrorCameraAndCardIncompatible          =  130,

        /// <summary>
        /// The specified bias frame is incompatible with the current camera/parameters
        /// </summary>
        ErrorBiasFrameIncompatible              =  131,

        /// <summary>
        /// The specified background image is incompatible with the current camera/parameters
        /// </summary>
        ErrorBackgroundImageIncompatible        =  132,

        /// <summary>
        /// The background is too bright to acquire a background image
        /// </summary>
        ErrorBackgroundTooBright                =  133,

        /// <summary>
        /// The specified file is invalid
        /// </summary>
        ErrorInvalidFile                        =  134,

        /// <summary>
        /// miscellaneous error, error details unknown
        /// </summary>
        ErrorMisc                               =  135,

        /// <summary>
        /// The image is too bright
        /// </summary>
        ErrorImageTooBright                     =  136,

        /// <summary>
        /// There is nothing to do (polling mode only)
        /// </summary>
        ErrorNothingToDo                        =  137,

        /// <summary>
        /// The camera is powered off
        /// </summary>
        ErrorNoCameraPower                      =  138,

        /// <summary>
        /// There is insufficient isochronous bandwidth available on the IEEE-1394 bus
        /// </summary>
        ErrorInsuf1394IsocBandwidth             =  201,

        /// <summary>
        /// There is insufficient IEEE-1394 isochronous resources available
        /// </summary>
        ErrorInsuf1394IsocResources             =  202,

        /// <summary>
        /// There is no isochronous channel available on the IEEE-1394 bus
        /// </summary>
        ErrorNo1394IsocChannel                  =  203,

        /// <summary>
        /// The USB bus version is lower than 2.0
        /// </summary>
        ErrorUsbVersionLowerThan2               =  204,

        /// <summary>
        /// SpotStartUp has already been successfully called
        /// </summary>
        ErrorStartupAlreadyDone                 =  210,

        /// <summary>
        /// SpotStartUp has not yet been successfully called or SpotShutDown was already called
        /// </summary>
        ErrorStartupNotDone                     =  211,

        /// <summary>
        /// Unable to locate any SpotCam service
        /// </summary>
        ErrorSpotcamServiceNotFound             =  212,

        /// <summary>
        /// Unable to locate the requested BaseService service
        /// </summary>
        ErrorWrongSpotcamServiceVersion         =  213,

        /// <summary>
        /// The operation is not supported by the SpotCam service
        /// </summary>
        ErrorOperationNotSupported              =  214,

        /// <summary>
        /// Could not read the service-configuration data file
        /// </summary>
        ErrorServiceConfigurationFileError      =  215,

        /// <summary>
        /// The service-configuration data is not formed correctly
        /// </summary>
        ErrorServiceConfigurationSyntaxError    =  216,

        /// <summary>
        /// The service-configuration data specifies invalid settings
        /// </summary>
        ErrorServiceConfigurationInvalid        =  217
    }

}
