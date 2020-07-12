using UnityEngine;

namespace Plants
{
    public class PlantingPlot : Interactable
    {
        public Plant script_plant { get; private set; }
        [SerializeField] private float ejection_velocity = 0f;
        
        private GameObject ref_placed_plant = null;

        /*
        public void PlaceOn(GameObject plantObj)
        {
            PlacedPlant = plantObj.GetComponent<Plant>();
            isTaken = true;
        }
        */

        public override void Interact(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
        {
            if (ref_held_obj == null) // Not holding anything
            {
                if (ref_placed_plant)
                {
                    // Eject with some velocity to make it more clear to the user
                    float random_angle = Random.Range(0f, Mathf.PI * 2);
                    ref_placed_plant.GetComponent<Rigidbody2D>().velocity = (new Vector2(Mathf.Cos(random_angle), Mathf.Sin(random_angle))) * ejection_velocity;

                    // Unroot the plant
                    ref_placed_plant.layer = LayerMask.NameToLayer("Seeds");
                    ref_placed_plant = null;
                    script_plant = null;
                }
            }
            else
            {
                // Check if plant
                if (original_tag == "Plant")
                {
                    if (ref_placed_plant == null)
                    {
                        // Assign held plant to plot
                        ref_placed_plant = ref_held_obj.gameObject;
                        ref_placed_plant.layer = LayerMask.NameToLayer("Planted"); // Prevents selection cursor from targetting this over Plot
                        script_plant = ref_placed_plant.GetComponent<Plant>();

                        // Free the previously held plant
                        ref_held_obj.tag = original_tag;
                        ref_held_obj = null;
                    }
                }

                // Check if watering can
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
    }
}
