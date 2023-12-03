using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingSkin : MonoBehaviour
{
    public AnimatorOverrideController bandw;
    public AnimatorOverrideController normal;

    public void BlackAndWhiteSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = bandw as RuntimeAnimatorController;
    }

        public void NormalSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = normal as RuntimeAnimatorController;
    }
}
