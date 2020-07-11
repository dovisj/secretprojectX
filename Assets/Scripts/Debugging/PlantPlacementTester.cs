using Plants;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Debugging
{
    public class PlantPlacementTester : MonoBehaviour
    {
        [SerializeField] private Plant _plant;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    Plant plant = Instantiate(_plant);
                    plant.PlacePlant(hit.point, Quaternion.identity);
                }
            }
        }
    }
}