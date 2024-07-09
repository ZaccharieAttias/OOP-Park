using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Elements
{
    public class MessageRow : MonoBehaviour
    {
        public Text Text;
        public Text Time;
        

        public MessageRow Init(string text, string time)
        {
            Text.text = text;
            Time.text = time;

            return this;
        }
    }
}
