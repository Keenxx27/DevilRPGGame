using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInteraction : MonoBehaviour
    {
        private readonly List<DoorController> nearbyDoors = new List<DoorController>();
        private readonly List<InvestigationPoint> nearbyInvestigations =
            new List<InvestigationPoint>();
        private PlayerInventory inventory;

        private void Awake()
        {
            inventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }

        public void RegisterDoor(DoorController door)
        {
            if (door != null && !nearbyDoors.Contains(door))
            {
                nearbyDoors.Add(door);
            }
        }

        public void UnregisterDoor(DoorController door)
        {
            nearbyDoors.Remove(door);
        }

        public void ClearNearbyDoors()
        {
            nearbyDoors.Clear();
        }

        public void RegisterInvestigation(InvestigationPoint investigation)
        {
            if (investigation != null && !nearbyInvestigations.Contains(investigation))
            {
                nearbyInvestigations.Add(investigation);
            }
        }

        public void UnregisterInvestigation(InvestigationPoint investigation)
        {
            nearbyInvestigations.Remove(investigation);
        }

        public void ClearNearbyInvestigations()
        {
            nearbyInvestigations.Clear();
        }

        public bool TryInteract()
        {
            DoorController nearest = FindNearestDoor();
            if (nearest != null)
            {
                nearest.TryInteract(this);
                return true;
            }

            InvestigationPoint investigation = FindNearestInvestigation();
            if (investigation != null)
            {
                if (inventory != null && inventory.SelectedItem != null
                    && investigation.TryUseSelectedItem())
                {
                    return true;
                }

                investigation.TryInvestigate();
                return true;
            }

            return inventory != null && inventory.TryPickUpNearest();
        }

        public bool TryUseSelectedItemOnReadingTable()
        {
            InvestigationPoint nearest = null;
            float nearestDistance = float.PositiveInfinity;
            for (int index = nearbyInvestigations.Count - 1; index >= 0; index--)
            {
                InvestigationPoint investigation = nearbyInvestigations[index];
                if (investigation == null || !investigation.gameObject.activeInHierarchy)
                {
                    nearbyInvestigations.RemoveAt(index);
                    continue;
                }

                if (investigation.Kind != InvestigationKind.LibraryReadingTable)
                {
                    continue;
                }

                float distance = (investigation.transform.position - transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearest = investigation;
                    nearestDistance = distance;
                }
            }

            return nearest != null && nearest.TryUseSelectedItem();
        }

        private InvestigationPoint FindNearestInvestigation()
        {
            InvestigationPoint nearest = null;
            float nearestDistance = float.PositiveInfinity;

            for (int index = nearbyInvestigations.Count - 1; index >= 0; index--)
            {
                InvestigationPoint investigation = nearbyInvestigations[index];
                if (investigation == null || !investigation.gameObject.activeInHierarchy)
                {
                    nearbyInvestigations.RemoveAt(index);
                    continue;
                }

                if (!investigation.IsAvailable)
                {
                    continue;
                }

                float distance =
                    (investigation.transform.position - transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearest = investigation;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        private DoorController FindNearestDoor()
        {
            DoorController nearest = null;
            float nearestDistance = float.PositiveInfinity;

            for (int index = nearbyDoors.Count - 1; index >= 0; index--)
            {
                DoorController door = nearbyDoors[index];
                if (door == null)
                {
                    nearbyDoors.RemoveAt(index);
                    continue;
                }

                float distance = (door.transform.position - transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearest = door;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }
    }
}
