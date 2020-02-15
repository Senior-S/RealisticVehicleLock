using SDG.Framework.Landscapes;
using SDG.Unturned;
using System.Linq;
using UnityEngine;

namespace XPlugins.RealisticVehiclesLock
{
    public class RaycastHelper
    {
        public static RaycastHit[] RaycastAll(Ray ray, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            if ((layerMask & RayMasks.GROUND) == RayMasks.GROUND)
            {
                LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray, maxDistance, ref layerMask);
            }
            return Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
        }

        public static InteractableVehicle GetVehicleFromHits(Player caller, RaycastHit[] hits)
        {
            InteractableVehicle vehicle = null;
            int hitsCount = hits.Count();
            if (hitsCount > 0)
            {
                for (int i = 0; i < hitsCount; i++)
                {
                    InteractableVehicle vv = hits[i].transform.GetComponentInParent<InteractableVehicle>();
                    if (vv != caller)
                    {
                        vehicle = vv;
                        break;
                    }
                }
            }
            return vehicle;
        }
    }
}