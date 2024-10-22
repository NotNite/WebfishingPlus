using System.Reflection;
using GDWeave;
using WebfishingPlus.Mods;

namespace WebfishingPlus;

public class Mod : IMod {
    private static readonly Version VersionObject = Assembly.GetExecutingAssembly().GetName().Version!;
    private static readonly string Version = $"v{VersionObject.Major}.{VersionObject.Minor}.{VersionObject.Build}";

    public static Config Config = null!;

    public Mod(IModInterface modInterface) {
        Config = modInterface.ReadConfig<Config>();

        if (Config.ControllerSupport) {
            modInterface.RegisterScriptMod(new ControllerInput.PlayerModifier());
            modInterface.RegisterScriptMod(new ControllerInput.Fishing3Modifier());
            modInterface.RegisterScriptMod(new ControllerInput.InputRemapButtonModifier());
        }

        if (Config.MenuTweaks) modInterface.RegisterScriptMod(new MenuTweaks());
        if (Config.FixHotbar) modInterface.RegisterScriptMod(new FixHotbar());
        if (Config.SortInventory) modInterface.RegisterScriptMod(new InventorySorter());
        if (Config.MeteorNotice) modInterface.RegisterScriptMod(new MeteorNotice());
    }

    public void Dispose() { }
}
