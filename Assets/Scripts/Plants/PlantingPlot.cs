using System.Collections.Generic;
using MEC;
using Misc;
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
            if (ref_held_obj == null) // Interacting entity not holding anything
            {
                if (ref_placed_plant && script_plant.HasGrown)
                {
                    // Eject with some velocity to make it more clear to the user
                    float random_angle = Random.Range(0f, Mathf.PI * 2);
                    ref_placed_plant.GetComponent<Rigidbody2D>().velocity = (new Vector2(Mathf.Cos(random_angle), Mathf.Sin(random_angle))) * ejection_velocity;
                    // Unroot the plant
                    ref_placed_plant.tag = "Plant";
                    ref_placed_plant.layer = LayerMask.NameToLayer("Seeds");
                    ref_placed_plant.GetComponent<Holdable>().Interact(ref ref_held_obj,ref original_tag, ref original_layer);
                    ref_placed_plant = null;
                    script_plant = null;
                }
            }
            else
            {
                // Check if plant
                if (original_tag == "Plant")
                {
                    if (ref_placed_plant == null) // No plants are currently on the plot
                    {
                        // Assign held plant to plot
                        ref_placed_plant = ref_held_obj.gameObject;
                        ref_placed_plant.tag = "Planted";
                        ref_placed_plant.GetComponent<Rigidbody2D>().simulated = false;
                        ref_placed_plant.GetComponent<Plant>().IsPlaced = true;
                        ref_placed_plant.transform.rotation =Quaternion.identity;
                        ref_placed_plant.layer = LayerMask.NameToLayer("Planted"); // Prevents selection cursor from targetting this over Plot
                        script_plant = ref_placed_plant.GetComponent<Plant>();

                        // Free the previously held plant
                        ref_held_obj = null;
                        SoundManager.Instance.PlayRandomFlowerGetSound();
                    }
                }

                // Check if watering can
                if (original_tag == "WaterCan" && script_plant != null)
                {
                    WateringCan wateringCan = ref_held_obj.GetComponent<WateringCan>();
                    if (wateringCan.WaterAmount <= 0)
                    {
                        UIManager.Instance.SendAMessage("Water can is empty!");
                    }
                    else
                    {
                        script_plant.currentWater = 100f;
                        wateringCan.WaterAmount -= 25f;
                        SoundManager.Instance.PlayPourWater();
                    }
            
                }
            }
        }

        public override void InteractEvil(ref GameObject ref_held_obj, ref string original_tag, ref LayerMask original_layer)
        {
            float random = Random.Range(0, 100);
            if (random > 50 && script_plant != null)
            {
                Timing.RunCoroutine(Trample());
            }
            if (ref_held_obj != null)
            {
                // Check if watering can
                if (original_tag == "WaterCan")
                {
                    WateringCan wateringCan = ref_held_obj.GetComponent<WateringCan>();
                    if (wateringCan.WaterAmount != 0)
                    {
                        wateringCan.WaterAmount = 0;
                        SoundManager.Instance.PlayPourWater();
                        if (script_plant != null)
                        {
                            script_plant.currentWater = 100f;
                        }
                    }
          
                }
            }
    
           
        }

        IEnumerator<float> Trample()
        {
            script_plant.KillPlant();
            SoundManager.Instance.PlayRandomFlowerGetSound();
            yield return  0f;
        }

        public override void StopInteracting()
        { 
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
