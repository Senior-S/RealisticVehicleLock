using Rocket.API;

namespace ItemAbilities
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string item_effects;
        public string clothing_effects;
        public void LoadDefaults()
        {
            item_effects = "140,speed3,81,gravity0";
            clothing_effects = "310,jump2";
        }
    }
}
