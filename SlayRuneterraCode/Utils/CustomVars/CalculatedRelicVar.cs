using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Utils.CustomVars;

public class CalculatedRelicVar(string name) : DynamicVar(name, 55M)
{
  private Func<RelicModel, Decimal>? _multiplierCalc;

  public override void SetOwner(AbstractModel owner)
  {
    base.SetOwner(owner);
    this.UpdateValues();
  }

  public CalculatedRelicVar WithMultiplier(Func<RelicModel, Decimal> multiplierCalc)
  {
    if (this._multiplierCalc != null)
      throw new InvalidOperationException($"Tried to set extra multiplier calc on {this} twice!");
    this._multiplierCalc = !(multiplierCalc.Target is AbstractModel) ? multiplierCalc : throw new InvalidOperationException("Multiplier calc must be static!");
    return this;
  }

  public Decimal Calculate()
  {
    if (this._multiplierCalc == null)
      throw new InvalidOperationException("Extra multiplier calc must be specified!");
    RelicModel owner = (RelicModel) this._owner!;
    Decimal num = this._multiplierCalc(owner);
    return this.GetBaseVar().BaseValue + this.GetExtraVar().BaseValue * num;
  }

  public void RecalculateForUpgradeOrEnchant()
  {
    Decimal baseValue = this.GetBaseVar().BaseValue;
    if (baseValue != this.BaseValue)
      this.WasJustUpgraded = true;
    this.BaseValue = baseValue;
  }

  public void UpdatePreviewVar(bool runGlobalHooks)
  {
    this.PreviewValue = this.Calculate();
  }

  
  
  protected virtual DynamicVar GetBaseVar()
  {
    return ((RelicModel) this._owner!).DynamicVars.CalculationBase;
  }

  protected virtual DynamicVar GetExtraVar()
  {
    return ((RelicModel) this._owner!).DynamicVars.CalculationExtra;
  }

  protected override Decimal GetBaseValueForIConvertible() => this.Calculate();

  public override string ToString() => this.Calculate().ToString();

  private void UpdateValues()
  {
    if (this._owner == null)
      return;
    this.BaseValue = this.GetBaseVar().BaseValue;
  }
}