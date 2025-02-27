using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownNecklaceSet : MonoBehaviour
{
    public void SetNecklace(bool b) => PlayerAnimator.SetNecklace(b);
    public void SetCrown(bool b) => PlayerAnimator.SetCrown(b);
}
