using System;
using TMPro;
using UnityEngine;

namespace UIElements
{
    public class MessageCenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textholder;
        [SerializeField] private Transform followTransform;
        private Vector3 initalOffest;
        public void SetMessage(string message)
        {
            textholder.text = message;
        }

        private void Awake()
        {
            initalOffest = transform.position - followTransform.position;
        }

        private void Update()
        {
            transform.position = followTransform.position+initalOffest;
        }
    }
}
