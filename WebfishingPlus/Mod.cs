using System.Reflection;
using GDWeave;
using WebfishingPlus.Mods;

namespace WebfishingPlus;

public class Mod : IMod {
    private static readonly Version VersionObject = Assembly.GetExecutingAssembly().GetName().Version!;
    private static readonly string Version = $"v{VersionObject.Major}.{VersionObject.Minor}.{VersionObject.Build}";

    public static Config Config = null!;

    public Mod(IModInterface modInterface) {
        Console.WriteLine($"WebfishingPlus {Version} initialized");

        Config = modInterface.ReadConfig<Config>();

        if (Config.ControllerSupport) {
            Console.WriteLine("WebfishingPlus: Controller support enabled");
            modInterface.RegisterScriptMod(new ControllerInput.PlayerModifier());
            modInterface.RegisterScriptMod(new ControllerInput.Fishing3Modifier());
            modInterface.RegisterScriptMod(new ControllerInput.InputRemapButtonModifier());
        }

        if (Config.MenuTweaks) {
            Console.WriteLine("WebfishingPlus: Menu tweaks enabled");
            modInterface.RegisterScriptMod(new MenuTweaks());
        }
        if (Config.FixHotbar) {
            Console.WriteLine("WebfishingPlus: Hotbar fix enabled");
            modInterface.RegisterScriptMod(new FixHotbar());
        }
        if (Config.SortInventory) {
            Console.WriteLine("WebfishingPlus: Inventory sorting enabled");
            modInterface.RegisterScriptMod(new InventorySorter());
        }
        if (Config.MeteorNotice) {
            Console.WriteLine("WebfishingPlus: Meteor notice enabled");
            modInterface.RegisterScriptMod(new MeteorNotice());
        }
    }

    public void Dispose() { }
}
