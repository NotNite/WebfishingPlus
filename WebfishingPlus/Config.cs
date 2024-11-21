using System.Text.Json.Serialization;

namespace WebfishingPlus;

public class Config {
    [JsonInclude] public bool ControllerSupport;
    [JsonInclude] public bool ControllerVibration = true;
    [JsonInclude] public double ControllerVibrationStrength = 1.0;
    [JsonInclude] public bool ControllerVibrationCatch = true;
    [JsonInclude] public bool ControllerVibrationLand = true;
    [JsonInclude] public bool ControllerVibrationReel = true;
    [JsonInclude] public bool ControllerVibrationYank = true;

    [JsonInclude] public bool MenuTweaks = true;
    [JsonInclude] public bool SortInventory;
    [JsonInclude] public bool FixHotbar = true;
    [JsonInclude] public bool NetcodeImprover;
}
