using System;
using Rocket.Core.Plugins;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned.Player;
using Steamworks;
using Rocket.API.Collections;

namespace RealisticVehicleLock
{
    public class Main : RocketPlugin
    {
        protected override void Load()
        {
            SteamChannel.onTriggerSend += OnTriggerSend;
            VehicleManager.onEnterVehicleRequested += OnEnterVehicle;
            VehicleManager.onExitVehicleRequested += OnLeaveVehicle;
            Logger.Log(" Plugin loaded corrrectly!", ConsoleColor.Green);
            Logger.Log(" More plugins: www.dvtserver.xyz", ConsoleColor.Green);
        }

        private void OnLeaveVehicle(Player player, InteractableVehicle vehicle, ref bool cancel, ref Vector3 pendingLocation, ref float pendingYaw)
        {
            var user = UnturnedPlayer.FromPlayer(player);

            if (vehicle.isLocked == false)
            {
                return;
            }
            else
            {
                cancel = false;
                ChatManager.say(user.CSteamID, Translate("enter_vehicle_locked"), Color.red, false);
            }
        }

        private void OnEnterVehicle(Player player, InteractableVehicle vehicle, ref bool cancel)
        {
            var user = UnturnedPlayer.FromPlayer(player);

            if (vehicle.isLocked == false)
            {
                return;
            }
            else if(vehicle.isLocked == true)
            {
                cancel = false;
                ChatManager.say(user.CSteamID, Translate("leave_vehicle_locked"), Color.red, false);
            }
        }

        private void OnTriggerSend(SteamPlayer player, string W, ESteamCall mode, ESteamPacket type, object[] arg)
        {
            switch (W)
            {
                /* This replace the event "OnPlayerUpdateGesture" from RocketMod
                 */
                case "tellGesture":
                    if (arg[0].ToString() == "4" || arg[0].ToString() == "5")
                    {
                        var user = player.player;
                        var csteamid = player.playerID.steamID;
                        var steamgroupid = player.playerID.group;
                        InteractableVehicle vehicle = RaycastHelper.GetVehicleFromHits(RaycastHelper.RaycastAll(new Ray(user.look.aim.position, user.look.aim.forward), 4f, RayMasks.VEHICLE));
                        UnturnedPlayer uuser = UnturnedPlayer.FromPlayer(user);
                        if (uuser == null)
                        {
                            Logger.Log("uuser is null!");
                        }
                        if (vehicle != null && uuser != null)
                        {
                            if (vehicle.isDead || vehicle.isDrowned)
                            {
                                return;
                            }
                            if ((vehicle.lockedOwner != csteamid || vehicle.lockedGroup != steamgroupid) && vehicle.isLocked == true)
                            {
                                ChatManager.say(uuser.CSteamID, Translate("other_vehicle_locked"), Color.red, true);
                            }
                            if ((vehicle.lockedOwner == csteamid || vehicle.lockedGroup == steamgroupid) && (vehicle.lockedGroup != null || vehicle.lockedGroup != CSteamID.Nil) && vehicle.isLocked == true)
                            {
                                VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID, csteamid, steamgroupid, false);
                            }
                            else if ((vehicle.lockedOwner == csteamid || vehicle.lockedGroup == steamgroupid) && (vehicle.lockedGroup != null || vehicle.lockedGroup != CSteamID.Nil) && vehicle.isLocked == false)
                            {
                                VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID, csteamid, steamgroupid, true);
                                ChatManager.say(uuser.CSteamID, Translate("vehicle_locked"), Color.red, true);
                            }
                        }
                    }
                    break;
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    { "enter_vehicle_locked", "[RealisticVehicleLock] This vehicle is locked, you need to unlock it to enter in this vehicle."},
                    { "leave_vehicle_locked", "[RealisticVehicleLock] This vehicle is locked, you need to unlock it to leave the vehicle." },
                    { "vehicle_locked", "[RealisticVehicleLock] Now your vehicle is locked." },
                    { "vehicle_is_locked", "[RealisticVehicleLock] This vehicle is locked!" }
                };
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