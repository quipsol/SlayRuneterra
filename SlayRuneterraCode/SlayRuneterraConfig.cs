using BaseLib.Config;

namespace SlayRuneterra;

[HoverTipsByDefault]
public class SlayRuneterraConfig : SimpleModConfig
{
    [ConfigSection("Enable")]
    public static bool IsEnabled { get; set; } = true;
}