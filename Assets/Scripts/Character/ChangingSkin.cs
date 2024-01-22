using UnityEngine;


public class ChangingSkin : MonoBehaviour
{
    public AnimatorOverrideController BnW;
    public AnimatorOverrideController Normal;
    public AnimatorOverrideController Pink;


    public void BlackAndWhiteSkin() { GetComponent<Animator>().runtimeAnimatorController = BnW; }
    public void NormalSkin() { GetComponent<Animator>().runtimeAnimatorController = Normal; }
    public void PinkSkin() { GetComponent<Animator>().runtimeAnimatorController = Pink; }
}
