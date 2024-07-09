using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Elements
{
    public class Avatar : MonoBehaviour
    {
        public Image AvatarIcon;
        public List<Sprite> Avatars;

        public void ScrollAvatar(int direction)
        {
            var index = Avatars.IndexOf(AvatarIcon.sprite);

            if (index + direction < 0)
            {
                AvatarIcon.sprite = Avatars.Last();
            }
            else if (index + direction >= Avatars.Count)
            {
                AvatarIcon.sprite = Avatars.First();
            }
            else
            {
                AvatarIcon.sprite = Avatars[index + direction];
            }
        }
    }
}
