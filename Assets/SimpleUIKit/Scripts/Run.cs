using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts
{
    public class Run : MonoBehaviour
    {
        public BaseInterface Menu;

        public void Start()
        {
            Menu.Open();
        }
    }
}
