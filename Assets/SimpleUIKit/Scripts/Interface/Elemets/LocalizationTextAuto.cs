using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Elements
{   
    /// <summary>
    /// Localize text component.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizationTextAuto : MonoBehaviour
    {
        public string LocalizationText;

        public void Start()
        {
            Localize();
            //LocalizationManagerAuto.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            //LocalizationManagerAuto.LocalizationChanged -= Localize;
        }

        private void Localize()
        {
            //LocalizationManagerAuto.Instance.TryTranslate(LocalizationText, GetComponent<Text>());
        }
    }
}

