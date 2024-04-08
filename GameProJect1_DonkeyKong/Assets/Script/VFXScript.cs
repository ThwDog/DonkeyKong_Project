using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScript : MonoBehaviour
{
    [SerializeField]GameObject particle;
    [Range(0f,30f)]public float playTime; // countdown to Deactivate Particle  
    private bool isPlaying = false;

    public void playParticle(Transform pos,Vector3 multiplyPos)
    {
        gameObject.transform.position = pos.position + multiplyPos;
        if(!isPlaying)
            StartCoroutine(enableParticle());        
    }

    IEnumerator enableParticle()
    {
        isPlaying = !isPlaying;
        particle.SetActive(true);
        particle.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(playTime);
        
        particle.GetComponent<ParticleSystem>().Stop();
        particle.SetActive(false);
        isPlaying = !isPlaying;
    }
}
