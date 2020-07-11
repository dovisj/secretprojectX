using UnityEngine;

namespace Managers
{
    public class PlantManager : MonoBehaviour
    {
        public GameObject[] branches;
        public GameObject[] stems;
        public GameObject[] extras;
        protected static PlantManager instance;
        public static PlantManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (PlantManager)FindObjectOfType(typeof(PlantManager));

                    if (instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(PlantManager) + " is needed in the scene, but there is none.");
                    }
                }
                return instance;
            }
        }

        public GameObject GetRandomStem()
        {
            return stems[Random.Range(0, stems.Length)];
        }
    
        public GameObject GetRandomBranch()
        {
            return branches[Random.Range(0, branches.Length)];
        }
    
        public GameObject GetRandomExtra()
        {
            return extras[Random.Range(0, extras.Length)];
        }
    }
}
