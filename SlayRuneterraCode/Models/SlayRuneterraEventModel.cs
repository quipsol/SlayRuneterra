using System.Reflection;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Models;

public abstract class SlayRuneterraEventModel(bool autoAdd = false) : CustomEventModel(autoAdd)
{
    protected string OptionKey(string pageName, string optionName)
    {
        return $"SLAYRUNETERRA-{StringHelper.Slugify(this.GetType().Name)}.pages.{pageName}.options.{optionName}";
    }
    
    protected EventOption EmptyOption(string optionKey, string pageKey = "INITIAL")
    { 
        return new EventOption(this, null, $"{this.Id.Entry}.pages.{pageKey}.options.{optionKey}", []);
    }

    protected LocString GetDescription(string pageName, string optionName = "")
    {
        return GetEntry(pageName,  optionName);
    }
    
    protected LocString GetTitle(string pageName, string optionName = "")
    {
        return GetEntry(pageName,  optionName, "title");

    }
    
    protected LocString GetEntry(string pageName, string optionName = "", string entryName = "description")
    {
        if(optionName == "")
            return L10NLookup($"SLAYRUNETERRA-{StringHelper.Slugify(this.GetType().Name)}.pages.{pageName}.{entryName}");
        else
            return L10NLookup($"SLAYRUNETERRA-{StringHelper.Slugify(this.GetType().Name)}.pages.{pageName}.options.{optionName}.{entryName}");
    }
    

    protected EventOption AllowOrLockOption(Func<bool> evaluate, Func<Task>? onChosen, IEnumerable<IHoverTip> tips, string pageKey = "INITIAL")
    {
        MethodInfo? method = onChosen?.Method;
        string optionKey = "UNKNOWN";
        if (method == null)
        {
            MainFile.Logger.Error("Unable to get delegate method for CustomEventModel.Option; provide an explicit title and description LocString if not passing a method directly.");
        }
        else
        {
            if (method.IsSpecialName)
                MainFile.Logger.Warn("Method passed as delegate to CustomEventModel.Option has special name; recommended to directly pass declared method or provide an explicit title and description LocString.");
            optionKey = method.Name;
        }

        optionKey = StringHelper.Slugify(optionKey);
        if (evaluate.Invoke())
            return new EventOption(this, onChosen, $"{this.Id.Entry}.pages.{pageKey}.options.{optionKey}", tips);
        optionKey += "_LOCKED";
        return EmptyOption(optionKey, pageKey);
        
    }
    
}