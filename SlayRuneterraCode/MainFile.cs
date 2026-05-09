using System.Reflection;
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using SlayRuneterra.Models;
using SlayRuneterra.Utils.Epochs;

namespace SlayRuneterra;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "SlayRuneterra"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        
        Harmony harmony = new(ModId);
        harmony.PatchAll();


        ModConfigRegistry.Register(ModId, new SlayRuneterraConfig());
        RegisterSavedPropertyTypes();
        
        MainFile.Logger.Info("Initializing all custom Epochs");
        CustomEpochList.InsertCustomEpochModelsToDictionaries();
        MainFile.Logger.Info("Custom Epochs:");
        foreach (var customEpochModel in CustomEpochList.CustomEpochs)
        {
            MainFile.Logger.Info(customEpochModel.Id);
        }
    }
    
    static void RegisterSavedPropertyTypes()
    {
        var cacheMethod = typeof(SavedPropertiesTypeCache)
                    .GetMethod("CachePropertiesForType", BindingFlags.NonPublic | BindingFlags.Static);

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.GetProperties(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic)
                .Any(p => p.GetCustomAttribute<SavedPropertyAttribute>() != null))
            {
                cacheMethod!.Invoke(null, [type]);
            }
        }
    }
    
    
    
}