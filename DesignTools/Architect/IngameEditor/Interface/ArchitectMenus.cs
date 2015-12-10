using UnityEngine;
using System.Collections.Generic;
using Pseudo.Internal.Input;

namespace Pseudo
{
    public class ArchitectMenus : MonoBehaviour
    {

        private Architect architect;

        void Awake()
        {
            architect = GetComponentInParent<Architect>();
        }

        void Update()
        {

            if (InputUtility.GetKeysDown(KeyCode.LeftControl, KeyCode.A))
                Debug.Log("NICE");
            if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.A))
            {
                Debug.Log("asd");
            }
        }

        public void Save()
        {
            architect.Save();
        }

        public void New()
        {
            architect.New();
        }

        public void Open()
        {
            architect.Open("map.txt");
        }
    }

}
