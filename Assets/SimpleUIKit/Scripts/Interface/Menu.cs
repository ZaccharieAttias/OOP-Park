using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interface
{
    public class Menu : BaseInterface
    {
        protected override void OnOpen()
        {
            //LocalizationManager.Read();
        }

        public void PopUpSpinRunExample()
        {
            PopupSpin.Instance.AskCount("How many seconds to spend?", () =>
            {
                PopupSpin.Instance.Close();
            }, 
                count =>
            {
                Debug.Log($"!{count}!");
                PopupSpin.Instance.Run(() => StartCoroutine(Wait(count)), "Waiting...");
            });

            IEnumerator Wait(int seconds)
            {
                yield return new WaitForSeconds(seconds);

                PopupSpin.Instance.Close();
                Popup.Instance.ShowMessage("PopUp Prefab");

                yield return new WaitForSeconds(2);

                Popup.Instance.ShowMessageTop(2f, null, "PopUp Prefab (Top)");
            }
        }
    }
}
