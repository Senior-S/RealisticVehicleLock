using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using Logger = Rocket.Core.Logging.Logger;

namespace ItemAbilities
{
    public class ItemAbilities : RocketPlugin<Configuration>
    {
        protected override void Load()
        {
            Provider.onEnemyConnected += OnEnemyConnected;
            Provider.onEnemyDisconnected += OnEnemyDisconnected;
            Logger.Log("[ItemAbilities] Plugin loaded correctly!");
            Logger.Log("[ItemAbilities] If you have any error you can contact the owner in discord: Senior S#9583");
        }

        #region Events
        private void OnEnemyConnected(SteamPlayer player)
        {
            player.player.equipment.onEquipRequested += OnEquipRequested;
            player.player.equipment.onDequipRequested += OnDequipRequested;
            player.player.clothing.onBackpackUpdated += (newBackpack, newBackpackQuality, newBackpackState) => OnBackpackUpdated(player.player, newBackpack, newBackpackQuality, newBackpackState);
            player.player.clothing.onGlassesUpdated += (newGlasses, newGlassesQuality, newGlassesState) => OnGlassesUpdated(player.player, newGlasses, newGlassesQuality, newGlassesState);
            player.player.clothing.onHatUpdated += (newHat, newHatQuality, newHatState) => OnHatUpdated(player.player, newHat, newHatQuality, newHatState);
            player.player.clothing.onMaskUpdated += (newMask, newMaskQuality, newMaskState) => OnMaskUpdated(player.player, newMask, newMaskQuality, newMaskState);
            player.player.clothing.onPantsUpdated += (newPants, newPantsQuality, newPantsState) => OnPantsUpdated(player.player, newPants, newPantsQuality, newPantsState);
            player.player.clothing.onShirtUpdated += (newShirt, newShirtQuality, newShirtState) => OnShirtUpdated(player.player, newShirt, newShirtQuality, newShirtState);
            player.player.clothing.onVestUpdated += (newVest, newVestQuality, newVestState) => OnVestUpdated(player.player, newVest, newVestQuality, newVestState);
        }

        private void OnVestUpdated(Player player, ushort newVest, byte newVestQuality, byte[] newVestState)
        {
            player.movement.sendPluginGravityMultiplier(1);
            player.movement.sendPluginJumpMultiplier(1);
            player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = player.equipment.itemID.ToString();
            SendEffectItems(itemsI, id, player);
            SendEffectClothing(itemsC, newVest.ToString(), player);
        }

        private void OnShirtUpdated(Player player, ushort newShirt, byte newShirtQuality, byte[] newShirtState)
        {
            player.movement.sendPluginGravityMultiplier(1);
            player.movement.sendPluginJumpMultiplier(1);
            player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = player.equipment.itemID.ToString();
            SendEffectItems(itemsI, id, player);
            SendEffectClothing(itemsC, newShirt.ToString(), player);
        }

        private void OnPantsUpdated(Player player, ushort newPants, byte newPantsQuality, byte[] newPantsState)
        {
            player.movement.sendPluginGravityMultiplier(1);
            player.movement.sendPluginJumpMultiplier(1);
            player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = player.equipment.itemID.ToString();
            SendEffectItems(itemsI, id, player);
            SendEffectClothing(itemsC, newPants.ToString(), player);
        }

        private void OnMaskUpdated(Player player, ushort newMask, byte newMaskQuality, byte[] newMaskState)
        {
            player.movement.sendPluginGravityMultiplier(1);
            player.movement.sendPluginJumpMultiplier(1);
            player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = player.equipment.itemID.ToString();
            SendEffectItems(itemsI, id, player);
            SendEffectClothing(itemsC, newMask.ToString(), player);
        }

        private void OnHatUpdated(Player player, ushort newHat, byte newHatQuality, byte[] newHatState)
        {
            player.movement.sendPluginGravityMultiplier(1);
            player.movement.sendPluginJumpMultiplier(1);
            player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = player.equipment.itemID.ToString();
            SendEffectItems(itemsI, id, player);
            SendEffectClothing(itemsC, newHat.ToString(), player);
        }

        private void OnGlassesUpdated(Player player, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
        {
            player.movement.sendPluginGravityMultiplier(1);
            player.movement.sendPluginJumpMultiplier(1);
            player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = player.equipment.itemID.ToString();
            SendEffectItems(itemsI, id, player);
            SendEffectClothing(itemsC, newGlasses.ToString(), player);
        }

        private void OnBackpackUpdated(Player player, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState)
        {
            player.movement.sendPluginGravityMultiplier(1);
            player.movement.sendPluginJumpMultiplier(1);
            player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = player.equipment.itemID.ToString();
            SendEffectItems(itemsI, id, player);
            SendEffectClothing(itemsC, newBackpack.ToString(), player);
        }

        private void OnEnemyDisconnected(SteamPlayer player)
        {
            player.player.equipment.onEquipRequested -= OnEquipRequested;
            player.player.equipment.onDequipRequested -= OnDequipRequested;
            player.player.clothing.onBackpackUpdated -= (newBackpack, newBackpackQuality, newBackpackState) => OnBackpackUpdated(player.player, newBackpack, newBackpackQuality, newBackpackState);
            player.player.clothing.onGlassesUpdated -= (newGlasses, newGlassesQuality, newGlassesState) => OnGlassesUpdated(player.player, newGlasses, newGlassesQuality, newGlassesState);
            player.player.clothing.onHatUpdated -= (newHat, newHatQuality, newHatState) => OnHatUpdated(player.player, newHat, newHatQuality, newHatState);
            player.player.clothing.onMaskUpdated -= (newMask, newMaskQuality, newMaskState) => OnMaskUpdated(player.player, newMask, newMaskQuality, newMaskState);
            player.player.clothing.onPantsUpdated -= (newPants, newPantsQuality, newPantsState) => OnPantsUpdated(player.player, newPants, newPantsQuality, newPantsState);
            player.player.clothing.onShirtUpdated -= (newShirt, newShirtQuality, newShirtState) => OnShirtUpdated(player.player, newShirt, newShirtQuality, newShirtState);
            player.player.clothing.onVestUpdated -= (newVest, newVestQuality, newVestState) => OnVestUpdated(player.player, newVest, newVestQuality, newVestState);
        }

        private void OnDequipRequested(PlayerEquipment equipment, ref bool shouldAllow)
        {
            equipment.player.movement.sendPluginGravityMultiplier(1);
            equipment.player.movement.sendPluginJumpMultiplier(1);
            equipment.player.movement.sendPluginSpeedMultiplier(1);
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var clothing = equipment.player.clothing;
            SendEffectClothing(itemsC, clothing.backpack.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.glasses.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.hat.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.mask.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.pants.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.shirt.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.vest.ToString(), equipment.player);
        }

        private void OnEquipRequested(PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow)
        {
            equipment.player.movement.sendPluginGravityMultiplier(1);
            equipment.player.movement.sendPluginJumpMultiplier(1);
            equipment.player.movement.sendPluginSpeedMultiplier(1);
            var itemsI = Configuration.Instance.item_effects.Split(',');
            var itemsC = Configuration.Instance.clothing_effects.Split(',');
            var id = jar.item.id.ToString();
            var clothing = equipment.player.clothing;
            SendEffectItems(itemsI, id, equipment.player);
            SendEffectClothing(itemsC, clothing.backpack.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.glasses.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.hat.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.mask.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.pants.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.shirt.ToString(), equipment.player);
            SendEffectClothing(itemsC, clothing.vest.ToString(), equipment.player);
        }
        #endregion Events

        #region Functions
        private void SendEffectItems(String[] items, string id, Player player)
        {
            if (items.Contains(id))
            {
                int a = Array.IndexOf(items, id) + 1;
                string EffectName = items[a];
                List<string> Effect = new List<string>();
                if (EffectName.StartsWith("s"))
                {
                    Effect.Add("speed");
                    string ret = EffectName.Replace("speed", "");
                    Effect.Add(ret);
                    Effect.ToArray();
                }
                else if (EffectName.StartsWith("j"))
                {
                    string s = "jump";
                    Effect.Add(s);
                    string ret = EffectName.Replace("jump", "");
                    Effect.Add(ret);
                    Effect.ToArray();
                }
                else if (EffectName.StartsWith("g"))
                {
                    string s = "gravity";
                    Effect.Add(s);
                    string ret = EffectName.Replace("gravity", "");
                    Effect.Add(ret);
                    Effect.ToArray();
                }
                if (Effect != null)
                {
                    float multiplier = Convert.ToSingle(Effect[1]);
                    switch (Effect[0])
                    {
                        case "speed":
                            player.movement.sendPluginSpeedMultiplier(multiplier);
                            break;
                        case "jump":
                            player.movement.sendPluginJumpMultiplier(multiplier);
                            break;
                        case "gravity":
                            player.movement.sendPluginGravityMultiplier(multiplier);
                            break;
                    }
                }
                else
                {
                    Logger.Log($"Error, the effect of the item: {id} are wrong");
                }
            }
        }

        private void SendEffectClothing(String[] items, string id, Player player)
        {
            if (items.Contains(id))
            {
                int a = Array.IndexOf(items, id) + 1;
                string EffectName = items[a];
                List<string> Effect = new List<string>();
                if (EffectName.StartsWith("s"))
                {
                    Effect.Add("speed");
                    string ret = EffectName.Replace("speed", "");
                    Effect.Add(ret);
                    Effect.ToArray();
                }
                else if (EffectName.StartsWith("j"))
                {
                    string s = "jump";
                    Effect.Add(s);
                    string ret = EffectName.Replace("jump", "");
                    Effect.Add(ret);
                    Effect.ToArray();
                }
                else if (EffectName.StartsWith("g"))
                {
                    string s = "gravity";
                    Effect.Add(s);
                    string ret = EffectName.Replace("gravity", "");
                    Effect.Add(ret);
                    Effect.ToArray();
                }
                if (Effect != null)
                {
                    float multiplier = Convert.ToSingle(Effect[1]);
                    switch (Effect[0])
                    {
                        case "speed":
                            player.movement.sendPluginSpeedMultiplier(multiplier);
                            break;
                        case "jump":
                            player.movement.sendPluginJumpMultiplier(multiplier);
                            break;
                        case "gravity":
                            player.movement.sendPluginGravityMultiplier(multiplier);
                            break;
                    }
                }
                else
                {
                    Logger.Log($"Error, the effect of the item: {id} are wrong");
                }
            }
        }
        #endregion

        protected override void Unload()
        {
            Provider.onEnemyConnected -= OnEnemyConnected;
            Provider.onEnemyDisconnected -= OnEnemyDisconnected;
            Logger.Log("[ItemAbilities] Plugin unloaded correctly!");
        }
    }
}