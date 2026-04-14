using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Utils;

namespace SlayRuneterra.Patches;


// This is horrible
// I just... don't know how to patch it properly
[HarmonyPatch(typeof(CardPileCmd), "CreateCardNodeAndUpdateVisuals")]
public static class CardPileCmd_CreateCardNodeAndUpdateVisuals_Patch
{
    static void Prefix(CardModel card, PileType targetPileType, bool owningPlayerIsLocal)
    {
        if (targetPileType == PileType.Hand)
        {
            if (card.CombatState == null)
                return;
//            CustomHooks.BeforeCardDrawnSync(card.CombatState, card,  targetPileType, owningPlayerIsLocal);
            CustomHooks.BeforeCardDrawn(card.CombatState, card,  targetPileType, owningPlayerIsLocal);
        }
    }
}


//
// [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.CardPileCmd) )]
//  public class CardPileCmd_Patch
//  {
//
//      static MethodInfo AddMethod = 
//                  AccessTools.Method(
//                  typeof(CardPileCmd),
//                  "Add",
//                  new Type[]
//                  {
//                              typeof(CardModel),
//                              typeof(CardPile),
//                              typeof(CardPilePosition),
//                              typeof(AbstractModel),
//                              typeof(bool)
//                  });
//
//      static MethodBase TargetMethod()
//      {
//          return AccessTools.Method(typeof(CardPileCmd).GetNestedType("<Draw>d__16", BindingFlags.NonPublic), "MoveNext");
//      }
//
//      static MethodInfo BeforeMethod =
//                  //AccessTools.Method(typeof(CustomHooks), nameof(CustomHooks.BeforeCardDrawnSync));
//                  AccessTools.Method(typeof(CustomHooks), nameof(CustomHooks.BeforeCardDrawn));
//      
//    
//      
//      
//      static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase original)
//      {
//          MainFile.Logger.Warn("=======  Transpiler  ========");
//          var list = new List<CodeInstruction>(instructions);
//          var stateMachineType = original.DeclaringType;
//          
//          var combatStateField = AccessTools.Field(stateMachineType, "<combatState>5__2");
//          var choiceContextField = AccessTools.Field(stateMachineType, "choiceContext");
//          var fromHandDrawField = AccessTools.Field(stateMachineType, "fromHandDraw");
//          var cardField = AccessTools.Field(stateMachineType, "<card>5__8");
//          
//          MainFile.Logger.Warn($"combatStateField = {combatStateField}");
//          MainFile.Logger.Warn($"choiceContextField = {choiceContextField}");
//          MainFile.Logger.Warn($"fromHandDrawField = {fromHandDrawField}");
//          MainFile.Logger.Warn($"cardField = {cardField}");
//          
//          //var awaiterLocal = il.DeclareLocal(typeof(TaskAwaiter));
//          var awaiterType = typeof(Task<>)
//                      .MakeGenericType(typeof(CardPileAddResult))
//                      .GetMethod("GetAwaiter")
//                      .ReturnType; // TaskAwaiter<CardPileAddResult>
//
//          var awaiterLocal = il.DeclareLocal(awaiterType);
//          MainFile.Logger.Warn($"awaiterLocal = {awaiterLocal}");
//          
//          var moveNext = AccessTools.Method(typeof(CardPileCmd).GetNestedType("<Draw>d__16", BindingFlags.NonPublic), "MoveNext");
//          
//          for (int i = 0; i < list.Count; i++)
//          {
//              var instr = list[i];
//              
//             MainFile.Logger.Info($"IL {i}: {instr.opcode} {instr.operand}");
//              
//              if (instr.opcode == OpCodes.Call &&
//                  instr.operand is MethodInfo mi &&
//                  mi == AddMethod)
//              {
//                  MainFile.Logger.Warn("=======  AddMethod found  ========");
//                  // 🔥 INSERT BEFORE Add(card, hand)
//
//                  // Load combatState (<combatState>5__2)
//                  yield return new CodeInstruction(OpCodes.Ldarg_0);
//                  yield return new CodeInstruction(OpCodes.Ldfld,combatStateField);
//
//                  // Load choiceContext (field)
//                  yield return new CodeInstruction(OpCodes.Ldarg_0);
//                  yield return new CodeInstruction(OpCodes.Ldfld,choiceContextField);
//
//             
//                  // yield return new CodeInstruction(OpCodes.Ldarg_0);
//                  // yield return new CodeInstruction(OpCodes.Ldfld, cardField); 
//                  
//                  
//                  // Load fromHandDraw (field)
//                  yield return new CodeInstruction(OpCodes.Ldarg_0);
//                  yield return new CodeInstruction(OpCodes.Ldfld,fromHandDrawField);
//
//                  // Call your method
//                  yield return new CodeInstruction(OpCodes.Call, BeforeMethod);
//                  
//                  // ----- new from here -----
//                  
//                  // Begin async await pattern
//                  var getAwaiter = typeof(Task).GetMethod("GetAwaiter");
//                  yield return new CodeInstruction(OpCodes.Callvirt, getAwaiter);       // call GetAwaiter()
//                  yield return new CodeInstruction(OpCodes.Stloc, awaiterLocal);        // store in local
//                  yield return new CodeInstruction(OpCodes.Ldloca_S, awaiterLocal);    // load address
//                  
//                  //yield return new CodeInstruction(OpCodes.Call, typeof(TaskAwaiter).GetMethod("get_IsCompleted"));
//                  yield return new CodeInstruction(OpCodes.Call, awaiterType.GetProperty("IsCompleted").GetGetMethod());
//                  
//                  
//                  var label = il.DefineLabel();
//                  yield return new CodeInstruction(OpCodes.Brtrue_S, label); // skip if completed
//
//                  // Save state and await
//                  yield return new CodeInstruction(OpCodes.Ldarg_0);
//                  yield return new CodeInstruction(OpCodes.Ldloc, awaiterLocal);
//                  
//                  
//                  // var awaitUnsafe = typeof(AsyncTaskMethodBuilder<IEnumerable<CardModel>>)
//                  //             .GetMethod("AwaitUnsafeOnCompleted")
//                  //             .MakeGenericMethod(typeof(TaskAwaiter), stateMachineType);
//                  var awaitUnsafe = typeof(AsyncTaskMethodBuilder<IEnumerable<CardModel>>)
//                              .GetMethod("AwaitUnsafeOnCompleted")
//                              .MakeGenericMethod(awaiterType, stateMachineType);
//                  
//                  
//                  yield return new CodeInstruction(OpCodes.Call, awaitUnsafe);
//                  yield return new CodeInstruction(OpCodes.Ret);
//
//                  // Mark continuation label
//                  var contInstr = new CodeInstruction(OpCodes.Nop) { labels = new List<Label> { label } };
//                  yield return contInstr;
//              }
//
//              yield return instr;
//          }
//      }
//      
//      
//      
//  }