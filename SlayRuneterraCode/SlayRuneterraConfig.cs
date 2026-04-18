using BaseLib.Config;

namespace SlayRuneterra;

[ConfigHoverTipsByDefault]
public class SlayRuneterraConfig : SimpleModConfig
{
    [ConfigSection("Enable")]
    public static bool IsEnabled { get; set; } = true;
}