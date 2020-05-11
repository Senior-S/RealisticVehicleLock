using System;
using Rocket.Core.Plugins;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned.Player;
using Steamworks;
using System.Linq;
using Rocket.Unturned.Events;
using Rocket.API;
using Rocket.API.Collections;

namespace XPlugins.RealisticVehiclesLock
{
    public class Main : RocketPlugin
    {
        public static Main Instance;
        protected override void Load()
        {
            Instance = this;
            Logger.Log("###########################################", ConsoleColor.Green);
            Logger.Log("#[RealisticVehiclesLock] Loaded correctly!#", ConsoleColor.Green);
            Logger.Log("###########################################", ConsoleColor.Green);

            VehicleManager.onEnterVehicleRequested += OnEnterVehicle;
            VehicleManager.onExitVehicleRequested += OnLeaveVehicle;
            UnturnedPlayerEvents.OnPlayerUpdateGesture += OnGesture;
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "Vehicle-Locked", "Now your vehicle is locked!" },
            { "Other-Vehicle-Locked", "This vehicle is locked!" }
        };

        private void OnLeaveVehicle(Player player, InteractableVehicle vehicle, ref bool cancel, ref Vector3 pendingLocation, ref float pendingYaw)
        {
            if (vehicle.isLocked == false)
            {
                return;
            }
            else
            {
                cancel = false;
                ChatManager.say("[RealisticVehiclesLock] This vehicle is locked, you need to unlock it to leave this vehicle!", Color.red, false);
            }
        }

        private void OnEnterVehicle(Player player, InteractableVehicle vehicle, ref bool cancel)
        {
            if (vehicle.isLocked == false)
            {
                return;
            }
            else if (vehicle.isLocked == true)
            {
                cancel = false;
                ChatManager.say("[RealisticVehiclesLock] This vehicle is locked, you need to unlock it to get in this vehicle!", Color.red, false);
            }
        }

        private void OnGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if (gesture == UnturnedPlayerEvents.PlayerGesture.PunchLeft | gesture == UnturnedPlayerEvents.PlayerGesture.PunchRight)
            {
                Player user = player.Player;

                InteractableVehicle vehicle1 = RaycastHelper.GetVehicleFromHits(user, RaycastHelper.RaycastAll(new Ray(user.look.aim.position, user.look.aim.forward), 4f, RayMasks.VEHICLE));

                if (vehicle1 != null)
                {
                    if ((vehicle1.lockedOwner != player.CSteamID || vehicle1.lockedGroup != player.SteamGroupID) && vehicle1.isLocked == true)
                    {
                        ChatManager.say(player.CSteamID, Translate("Other-Vehicle-Locked"), Color.red, false);
                    }
                    if ((vehicle1.lockedOwner == player.CSteamID || vehicle1.lockedGroup == player.SteamGroupID) && vehicle1.isLocked == true)
                    {
                        VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle1.instanceID, player.CSteamID, player.SteamGroupID, false);
                    }
                    else if ((vehicle1.lockedOwner == player.CSteamID || vehicle1.lockedGroup == player.SteamGroupID) && vehicle1.isLocked == false)
                    {
                        VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle1.instanceID, player.CSteamID, player.SteamGroupID, true);
                        ChatManager.say(player.CSteamID, Translate("Vehicle-Locked"), Color.red, false);
                    }
                }
            }
        }

        protected override void Unload()
        {
            Logger.Log("#############################################", ConsoleColor.Green);
            Logger.Log("#[RealisticVehiclesLock] Unloaded correctly!#", ConsoleColor.Green);
            Logger.Log("#############################################", ConsoleColor.Green);
        }
    }
}