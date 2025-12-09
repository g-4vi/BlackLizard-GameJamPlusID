using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class FallingPlatform : SpecialPlatform
{
    [Header("Tremor")]
    [SerializeField] float tremorStrength = 1.0f;
    [SerializeField] float tremorDuration = 0.5f;
    [SerializeField] float tremorSpeed = 10f;

    [Header("Fall")]
    [SerializeField] float fallSpeed;
    [SerializeField] bool animateFall;

    [SerializeField] float respawnTime;

    Vector3 spawnPos;
    Coroutine fallCoroutine;

    private void Start()
    {
        spawnPos = transform.position;
    }

    IEnumerator ResetPlatform()
    {
        // stop fall loop
        if (fallCoroutine != null)
        {
            StopCoroutine(fallCoroutine);
            fallCoroutine = null;
        }

        yield return new WaitForSeconds(respawnTime);

        transform.position = spawnPos;

        gameObject.GetComponent<Collider2D>().enabled = true;

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        Color currentColor = sr.color;

        while (currentColor.a < 1f)
        {
            currentColor.a += Time.deltaTime * 5f;
            sr.color = currentColor;
            yield return null;
        }

        sr.color = Color.white;
    }

    public override void TriggerPlatform()
    {
        if (fallCoroutine == null)
            Fall();
    }

    public void Fall()
    {
        fallCoroutine=StartCoroutine(TremorPlatform());
    }

    IEnumerator TremorPlatform()
    {
        float timer = 0;

        Vector3 originalPos = transform.position;
        while (timer < tremorDuration)//ground tremble for indication its about to fall
        {
            Vector3 tremor = new Vector3(Random.Range(-tremorStrength, tremorStrength), Random.Range(-tremorStrength, tremorStrength), 0);

            transform.position = Vector2.Lerp(transform.position, originalPos + tremor, tremorSpeed*Time.deltaTime);
            originalPos = transform.position;

            timer += Time.deltaTime;

            yield return null;
        }

        //Fall
        gameObject.GetComponent<Collider2D>().enabled = false;
     
        if (animateFall)
        {
            while (true)
            {
                transform.Translate(0, -fallSpeed * Time.deltaTime, 0);
                yield return null;
            }
                    
        }
        else
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            Color c = sr.color;

            while (c.a > 0)
            {
                c.a -= Time.deltaTime * 5f;
                sr.color = c;
                yield return null;
            }

            StartCoroutine(ResetPlatform());

            yield break;//exit coroutine
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("PlatformCollector"))
        {
            

            gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
            StartCoroutine(ResetPlatform());
        }
    }
}
