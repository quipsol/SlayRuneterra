using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SlayRuneterra.Extensions;

namespace SlayRuneterra.Models;

// TODO: Lots of patchwork in CardModel GetDescriptionForPile and Title
/*
 
 Description:
 Prepend -> add beore
 Append -> add after
 Replace -> replace entire text
 
 Title:
 Prepend -> add beore
 Append -> add after
 Replace -> replace entire text
 
 */

public abstract class CustomAfflictionModel : AfflictionModel
{
 public virtual string? CustomOverlayPath => $"res://{MainFile.ModId}/scenes/cards/overlays/afflictions/{this.Id.Entry.ToLowerInvariant()}.tscn";
}

[HarmonyPatch(typeof(AfflictionModel), nameof(AfflictionModel.OverlayPath), MethodType.Getter)]
class CustomAffliction_OverlayPath_Patch
{
 [HarmonyPrefix]
 static bool UseAltOverlay(AfflictionModel __instance, ref string? __result)
 { 
  if (__instance is not CustomAfflictionModel customAffliction) return true;
  if (customAffliction.CustomOverlayPath == null) return true;
  __result = customAffliction.CustomOverlayPath;
  return false;
 }
}