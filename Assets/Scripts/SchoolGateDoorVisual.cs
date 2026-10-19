using UnityEngine;

namespace RPG
{
    /// <summary>
    /// 学校大门打开后收起关闭状态的玻璃门板。
    /// 门的通行判定仍完全由 DoorController 负责。
    /// </summary>
    public sealed class SchoolGateDoorVisual : MonoBehaviour
    {
        [SerializeField] private DoorController door;
        [SerializeField] private GameObject closedDoorVisual;

        private void Update()
        {
            if (door != null && door.IsFullyOpen && closedDoorVisual != null)
            {
                closedDoorVisual.SetActive(false);
            }
        }
    }
}
