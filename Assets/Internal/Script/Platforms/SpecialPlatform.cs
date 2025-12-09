using Unity.VisualScripting;
using UnityEngine;

public abstract class SpecialPlatform: MonoBehaviour
{
    public virtual void TriggerPlatform()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            TriggerPlatform();
        }
    }
   
}
