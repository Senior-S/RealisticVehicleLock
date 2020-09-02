using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using SDG.Unturned;
using OpenMod.Unturned.Users;
using OpenMod.API.Users;
using OpenMod.Core.Users;
using OpenMod.Core.Helpers;
using UnityEngine;
using Steamworks;

[assembly: PluginMetadata("SS.RealisticVehicleLock", DisplayName = "RealisticVehicleLock")]
namespace RealisticVehicleLock
{
    public class RealisticVehicleLock : OpenModUnturnedPlugin
    {
        private readonly ILogger<RealisticVehicleLock> ro_Logger;
        private readonly IStringLocalizer ro_StringLocalizer;
        private readonly IUserManager ro_UserManager;

        public RealisticVehicleLock(
            ILogger<RealisticVehicleLock> logger,
            IStringLocalizer stringLocalizer,
            IUserManager usermanager,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ro_Logger = logger;
            ro_StringLocalizer = stringLocalizer;
            ro_UserManager = usermanager;
        }

        protected override async UniTask OnLoadAsync()
        {
			await UniTask.SwitchToMainThread();
            SteamChannel.onTriggerSend += OnTriggerSend;
            VehicleManager.onEnterVehicleRequested += OnEnterVehicle;
            VehicleManager.onExitVehicleRequested += OnLeaveVehicle;
            ro_Logger.LogInformation("Plugin loaded correctly!");
            ro_Logger.LogInformation("If you have any error you can contact the owner in discord: Senior S#9583");
        }

        private void OnEnterVehicle(Player user, InteractableVehicle vehicle, ref bool cancel)
        {
            if (!vehicle.isLocked)
            {
                return;
            }
            else if (vehicle.isLocked)
            {
                cancel = false;
                AsyncHelper.RunSync(async () =>
                {
                    UnturnedUser uuser = (UnturnedUser)await ro_UserManager.FindUserAsync(KnownActorTypes.Player, user.channel.owner.playerID.steamID.ToString(), UserSearchMode.FindById);
                    await uuser.PrintMessageAsync(ro_StringLocalizer["plugin_translations:enter_vehicle_locked"]);
                });
            }
        }

        private void OnLeaveVehicle(Player user, InteractableVehicle vehicle, ref bool cancel, ref Vector3 pendingLocation, ref float pendingYaw)
        {
            if (!vehicle.isLocked)
            {
                return;
            }
            else if (vehicle.isLocked)
            {
                cancel = false;
                AsyncHelper.RunSync(async () =>
                {
                    UnturnedUser uuser = (UnturnedUser)await ro_UserManager.FindUserAsync(KnownActorTypes.Player, user.channel.owner.playerID.steamID.ToString(), UserSearchMode.FindById);
                    await uuser.PrintMessageAsync(ro_StringLocalizer["plugin_translations:leave_vehicle_locked"]);
                });
            }
        }

        private void OnTriggerSend(SteamPlayer player, string W, ESteamCall mode, ESteamPacket type, object[] arg)
        {
            switch (W)
            {
                case "tellGesture":
                    if (arg[0].ToString() == "4" || arg[0].ToString() == "5")
                    {
                        var user = player.player;
                        var csteamid = player.playerID.steamID;
                        var steamgroupid = player.playerID.group;
                        InteractableVehicle vehicle = RaycastHelper.GetVehicleFromHits(RaycastHelper.RaycastAll(new Ray(user.look.aim.position, user.look.aim.forward), 4f, RayMasks.VEHICLE));
                        AsyncHelper.RunSync(async () =>
                        {
                            UnturnedUser uuser = (UnturnedUser)await ro_UserManager.FindUserAsync(KnownActorTypes.Player, csteamid.ToString(), UserSearchMode.FindById);
                            if (uuser == null)
                            {
                                ro_Logger.LogInformation("uuser is null!");
                            }
                            if (vehicle != null && uuser != null)
                            {
                                if (vehicle.isDead || vehicle.isDrowned)
                                {
                                    return;
                                }
                                if ((vehicle.lockedOwner != csteamid || vehicle.lockedGroup != steamgroupid) && vehicle.isLocked == true)
                                {
                                    await uuser.PrintMessageAsync(ro_StringLocalizer["plugin_translations:other_vehicle_locked"], System.Drawing.Color.Red);
                                }
                                if ((vehicle.lockedOwner == csteamid || vehicle.lockedGroup == steamgroupid) && (vehicle.lockedGroup != null || vehicle.lockedGroup != CSteamID.Nil) && vehicle.isLocked == true )
                                {
                                    VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID, csteamid, steamgroupid, false);
                                }
                                else if ((vehicle.lockedOwner == csteamid || vehicle.lockedGroup == steamgroupid) && (vehicle.lockedGroup != null || vehicle.lockedGroup != CSteamID.Nil) && vehicle.isLocked == false)
                                {
                                    VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID, csteamid, steamgroupid, true);
                                    await uuser.PrintMessageAsync(ro_StringLocalizer["plugin_translations:vehicle_locked"], System.Drawing.Color.Red);
                                }
                            }
                        });
                    }
                    break;
            }

        }

        protected override async UniTask OnUnloadAsync()
        {
            await UniTask.SwitchToMainThread();
            SteamChannel.onTriggerSend -= OnTriggerSend;
            VehicleManager.onEnterVehicleRequested -= OnEnterVehicle;
            VehicleManager.onExitVehicleRequested -= OnLeaveVehicle;
            ro_Logger.LogInformation("Plugin unloaded correctly!");
        }
    }
}
