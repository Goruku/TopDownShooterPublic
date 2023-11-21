using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIFeederInfo : MonoBehaviour
{
    public TextMeshProUGUI feederText;

    public void UpdateFeederText(int magazineSize, int defaultMagazineSize)
    {
        feederText.text = magazineSize.ToString() + "/" + defaultMagazineSize.ToString();
    }
}
