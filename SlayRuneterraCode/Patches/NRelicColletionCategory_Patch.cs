using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;

namespace SlayRuneterra.Patches;



// Canys Patch
[HarmonyPatch(typeof(NRelicCollectionCategory), nameof(NRelicCollectionCategory.LoadRelics))]
public static class RelicCollectionTranspiler
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        
        // Find the error string as our anchor point
        int errorStringIndex = -1;
        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode == OpCodes.Ldstr && 
                codes[i].operand is string s && 
                s.Contains("act list"))
            {
                errorStringIndex = i;
                break;
            }
        }
        
        if (errorStringIndex == -1)
            return codes;
        
        // Find the throw after the error string
        int throwIndex = -1;
        for (int i = errorStringIndex; i < codes.Count && i < errorStringIndex + 5; i++)
        {
            if (codes[i].opcode == OpCodes.Throw)
            {
                throwIndex = i;
                break;
            }
        }
        
        if (throwIndex == -1)
            return codes;
        
        // Find the start of the block (ldc.i4.4 before error string)
        int blockStart = -1;
        for (int i = errorStringIndex - 1; i >= 0; i--)
        {
            if (codes[i].opcode == OpCodes.Ldc_I4_4)
            {
                blockStart = i;
                break;
            }
        }
        
        if (blockStart == -1)
            return codes;
        
        // Find the stloc that stores to the actModelList local
        CodeInstruction? stlocInstruction = null;
        for (int i = blockStart; i < errorStringIndex; i++)
        {
            if (codes[i].opcode == OpCodes.Stloc_S &&
                codes[i].operand is LocalBuilder lb &&
                lb.LocalType?.IsGenericType == true &&
                lb.LocalType.GetGenericTypeDefinition() == typeof(List<>))
            {
                stlocInstruction = codes[i].Clone();
                break;
            }
        }
        
        if (stlocInstruction == null)
            return codes;
        
        // Build replacement: ModelDb.Acts.ToList()
        var actModelType = typeof(ModelDb).Assembly.GetTypes().First(t => t.Name == "ActModel");
        var actsGetter = AccessTools.PropertyGetter(typeof(ModelDb), "Acts");
        var toListMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == "ToList" && m.GetParameters().Length == 1)
            .MakeGenericMethod(actModelType);
        
        var replacement = new List<CodeInstruction>
        {
            new CodeInstruction(OpCodes.Call, actsGetter),
            new CodeInstruction(OpCodes.Call, toListMethod),
            stlocInstruction
        };
        
        if (codes[blockStart].labels.Count > 0)
            replacement[0].labels.AddRange(codes[blockStart].labels);
        
        int removeCount = throwIndex - blockStart + 1;
        codes.RemoveRange(blockStart, removeCount);
        codes.InsertRange(blockStart, replacement);
        
        return codes;
    }
}