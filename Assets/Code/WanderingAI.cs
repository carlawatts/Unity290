using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float obsticleRange = 5.0f;
    private bool isAlive;
    [SerializeField] GameObject fireballPrefab;
    private GameObject fireball;
    public MeshRenderer[] renderers;



    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        //  Color newColor = Random.ColorHSV();
        //  Color newColor = Random.ColorHSV(0f, .5f);
        Color newColor = Random.ColorHSV(0f, 0.25f, 0.4f, 1f);
        ApplyMaterial(newColor, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (isAlive)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            // if (Physics.SphereCast(ray, 0.75f, out hit))
            // {
            //     if (hit.distance < obsticleRange)
            //     {
            //         float angle = Random.Range(-110, 110);
            //         transform.Rotate(0, angle, 0);
            //     }
            // }
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if (fireball == null)
                    {
                        fireball = Instantiate(fireballPrefab) as GameObject;
                        fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        fireball.transform.rotation = transform.rotation;
                    }
                }
                else if (hit.distance < obsticleRange)
                {
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }





    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }

    public void ReactToHit()
    {
        WanderingAI behavior = GetComponent<WanderingAI>();
        if (behavior != null)
        {
            behavior.SetAlive(false);
        }
        StartCoroutine(Die());
    }
    private IEnumerator Die()
    {
        this.transform.Rotate(-75, 0, 0);
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    void ApplyMaterial(Color color, int targetMaterialIndex)
    {
        Material generatedMaterial = new Material(Shader.Find("Standard"));
        generatedMaterial.SetColor("_Color", color);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = generatedMaterial;
        }
    }
}
