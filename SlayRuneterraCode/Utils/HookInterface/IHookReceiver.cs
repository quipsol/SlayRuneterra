using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Runs;
using SlayRuneterra.Content.Events.DemaciaAct;
using SlayRuneterra.Nodes;

namespace SlayRuneterra.Utils.HookInterface;

/// <summary>
/// Marks a class as a receiver for runtime hook injection.
/// </summary>
/// <remarks>
/// Any non-abstract class implementing <c>IHookReceiver</c> will automatically be discovered
/// and registered for <c>RunStateHooks</c> and <c>CombatStateHooks</c><br></br>
/// <b>Requirements:</b>
/// <list type="bullet">
///   <item>  <description>  The class must have a corresponding entry in <see cref="ModelDb"/>. </description> </item>
/// </list>
/// This interface can be implemented on an abstract base class to implicitly include all derived types.<br></br>
/// </remarks>
public interface IHookReceiver
{
    /// <summary>
    /// Override to further specify the conditions of when your Model should get iterated over
    /// </summary>
    /// <param name="combatState"></param>
    /// <returns></returns>
    bool ReceiveHooks(CombatState combatState) => true;
}

[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public class ActModelPatch
{
    private static bool _alreadyExecuted = false;
    
    private static readonly List<AbstractModel> AllModels = new();
    
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
            AllModels.Add(abstractModel);
        }

        if (AllModels.Count <= 0) return;
        ModHelper.SubscribeForCombatStateHooks("SLAYRUNETERRA-I_HOOK_RECEIVER", HookSubModels);
    }
    
    public static IEnumerable<AbstractModel> HookSubModels(CombatState combatState)
    {
        return AllModels.Where(model => (model as IHookReceiver)!.ReceiveHooks(combatState));
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