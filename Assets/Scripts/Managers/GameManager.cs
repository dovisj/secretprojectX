using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            EventManager.TriggerEvent("GAME_STARTED");
        }
    }
}
