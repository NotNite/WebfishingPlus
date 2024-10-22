using System.Text.Json.Serialization;

namespace WebfishingPlus;

public class Config {
    [JsonInclude] public bool ControllerSupport = true;
    [JsonInclude] public bool ControllerVibration = true;
    [JsonInclude] public double ControllerVibrationStrength = 1.0;
    [JsonInclude] public bool ControllerVibrationLand = true;
    [JsonInclude] public bool ControllerVibrationReel = true;
    [JsonInclude] public bool ControllerVibrationYank = true;

    [JsonInclude] public bool MenuTweaks;
    [JsonInclude] public bool SortInventory;
    [JsonInclude] public bool FixHotbar = true;
    [JsonInclude] public bool MeteorNotice = true;
}
