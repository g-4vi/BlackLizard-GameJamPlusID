using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    void Start()
    {
        SfxID clickSfx = AudioManager.Instance.ButtonSFX;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(clickSfx);
        });
    }
}
