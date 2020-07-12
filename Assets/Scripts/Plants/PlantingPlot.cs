using UnityEngine;

namespace Plants
{
    public class PlantingPlot : Interactable
    {
        public Plant script_plant { get; private set; }
        
        private GameObject ref_placed_plant = null;

        /*
        public void PlaceOn(GameObject plantObj)
        {
            PlacedPlant = plantObj.GetComponent<Plant>();
            isTaken = true;
        }
        */

        public override void Interact(int action)
        {
            Debug.Log("Interact");
            if (ref_placed_plant)
            {
                // Unroot the plant
                ref_placed_plant = null;
                script_plant = null;
            }
        }

        private void Awake()
        {
            script_plant = null;
        }

        private void Update()
        {
            if (ref_placed_plant != null)
            {
                ref_placed_plant.transform.position = transform.position;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ref_placed_plant == null)
            {
                if (collision.tag == "Plant")
                {
                    ref_placed_plant = collision.gameObject;
                    script_plant = ref_placed_plant.GetComponent<Plant>();
                }
            }
        }
    }
}
