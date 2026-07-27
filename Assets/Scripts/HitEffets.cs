using UnityEngine;

public class HitEffets : MonoBehaviour
{


    [SerializeField] float lifeTime = 0.3f;

    private SpriteRenderer sr;
    private float timer = 0f;
    private Color startColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
    }

 
    void Update()
    {
        timer = timer + Time.deltaTime;
        
        float ratio = timer / lifeTime;

        Color c = startColor;
        c.a = ratio;
        sr.color = c;

        transform.localScale = Vector3.one * (1f + ratio * 0.5f);

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
