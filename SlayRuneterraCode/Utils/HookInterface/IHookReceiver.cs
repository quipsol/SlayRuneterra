using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Content.Events.DemaciaAct;

namespace SlayRuneterra.Utils.HookInterface;

/// <summary>
/// Marks a class as a receiver for runtime hook injection.
/// </summary>
/// <remarks>
/// Any non-abstract class implementing <c>IHookReceiver</c> will automatically be discovered
/// and registered for <c>RunStateHooks</c> and <c>CombatStateHooks</c><br></br>
/// <b>Requirements:</b>
/// <list type="bullet">
///   <item>  <description>  The class must be concrete (not <c>abstract</c>).  </description>   </item>
///   <item>  <description>  The class must have a corresponding entry in <see cref="ModelDb"/>. </description> </item>
/// </list>
/// This interface can be implemented on an abstract base class to implicitly include all derived types.<br></br>
/// </remarks>
public interface IHookReceiver { }

[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public class ActModelPatch
{
    private static bool _alreadyExecuted = false;
    
    private static List<AbstractModel> allModels = new();
    
    [HarmonyPostfix]
    public static void Postfix()
    {
        if (_alreadyExecuted) return;
        _alreadyExecuted = true;
        
        MainFile.Logger.Info("Getting IHookReceiver implementations");
        List<Type?> hookReceivers = GetHookReceivers();
        foreach (Type? type in hookReceivers)
        {
            if (type is null)
            {
                MainFile.Logger.Info("IHookReceivers: Type was null");
                continue;
            }
            AbstractModel? abstractModel = ModelDb.GetById<AbstractModel>(ModelDb.GetId(type));
            MainFile.Logger.Info("Model: " + abstractModel.Id.Entry);
            if(abstractModel is null)
                continue;
            allModels.Add(abstractModel);
        }

        if (allModels.Count <= 0) return;
        ModHelper.SubscribeForRunStateHooks("SLAYRUNETERRA-I_HOOK_RECEIVER", RunSubModels);
        ModHelper.SubscribeForCombatStateHooks("SLAYRUNETERRA-I_HOOK_RECEIVER", CombatSubModels);
    }
    
    public static IEnumerable<AbstractModel> RunSubModels(RunState runState)
    {
        return allModels;
    }
    public static IEnumerable<AbstractModel> CombatSubModels(CombatState combatState)
    {
        return allModels;
    }
    
    
    public static List<Type?> GetHookReceivers()
    {
        var interfaceType = typeof(IHookReceiver);

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
                    .ToList();
    }
}