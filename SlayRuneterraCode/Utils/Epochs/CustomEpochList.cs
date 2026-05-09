using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Timeline;
using SlayRuneterra.Models;

namespace SlayRuneterra.Utils.Epochs;

public static class CustomEpochList
{
    private static List<CustomEpochModel>? _customEpochs;
    public static List<CustomEpochModel> CustomEpochs => _customEpochs ??= GetCustomEpochs();


    private static bool _alreadyInitialized = false;
    public static void InsertCustomEpochModelsToDictionaries()
    {
        if (_alreadyInitialized) return;
        _alreadyInitialized = true;
        
        var type = typeof(EpochModel);
        Dictionary<string, Type> epochTypeDictionary = (AccessTools.Field(type, "_epochTypeDictionary").GetValue(null) as Dictionary<string, Type>)!;
        Dictionary<Type, string> typeToIdDictionary = (AccessTools.Field(type, "_typeToIdDictionary").GetValue(null) as Dictionary<Type, string>)!;

        foreach (var customEpochModel in CustomEpochs)
        {
            MainFile.Logger.Info($"CustomEpoch Type: {customEpochModel.GetType().Name}");
            epochTypeDictionary[customEpochModel.Id] = customEpochModel.GetType();
            typeToIdDictionary[customEpochModel.GetType()] = customEpochModel.Id;
        }
    }
    
    public static List<CustomEpochModel> GetCustomEpochs()
    {
        var interfaceType = typeof(CustomEpochModel);

        return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly =>
                    {
                        try
                        {
                            return assembly.GetTypes();
                        }
                        catch (ReflectionTypeLoadException e)
                        {
                            return e.Types.Where(t => t != null);
                        }
                    })
                    .Where(type => interfaceType.IsAssignableFrom(type) &&
                                   type is { IsClass: true, IsAbstract: false })
                    .Select(type =>
                                {
                                    try
                                    {
                                        return Activator.CreateInstance(type) as CustomEpochModel;
                                    }
                                    catch
                                    {
                                        MainFile.Logger.Warn($"Activator.CreateInstance and subsequent casting to 'CustomEpochModel' failed for type {type}");
                                        return null;
                                    }
                                })
                    .Where(type => type is not null)
                    .ToList();
    }
}