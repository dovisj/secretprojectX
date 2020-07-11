using UnityEngine;

namespace Plants
{
    public class PlantingPlot : MonoBehaviour
    {
        public bool isTaken;
        public Plant PlacedPlant { get; private set; }

        public void PlaceOn(GameObject plantObj)
        {
            PlacedPlant = plantObj.GetComponent<Plant>();
            isTaken = true;
        }
    }
}
