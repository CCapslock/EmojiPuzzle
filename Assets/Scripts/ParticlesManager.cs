using UnityEngine;
using System.Collections.Generic;

public class ParticlesManager : MonoBehaviour
{
    [HideInInspector] public static ParticlesManager Current;

    public ParticleSystem ConfettiParticles;
    private ParticleSystem _confettiParticles;

    public ParticleSystem BigSimpleSmileParticles;
    private ParticleSystem _bigParticle;

    public ParticleSystem SimpleSmileParticles;
    private ParticleSystem[] _emojiParticle;

    private void Awake()
    {
        Current = this;
        _confettiParticles = Instantiate(ConfettiParticles, new Vector3(0, -10, 0f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
        PrepareParticles(BigSimpleSmileParticles, SimpleSmileParticles);
    }

    public void PrepareParticles(ParticleSystem bigParticle, ParticleSystem particles)
    {
        _bigParticle = Instantiate(bigParticle, new Vector3(0, -10, 0f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
        _emojiParticle = new ParticleSystem[10];
        for (int i = 0; i < _emojiParticle.Length; i++)
        {
            _emojiParticle[i] = Instantiate(particles, new Vector3(0, -10, 0f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
        }
    }

    public void MakeLittleParticle(Vector3 position, bool isRandom)
    {
        if (!isRandom)
        {
            for (int i = 0; i < _emojiParticle.Length; i++)
            {
                if (!_emojiParticle[i].isPlaying || i == _emojiParticle.Length - 1)
                {
                    _emojiParticle[i].transform.position = position;
                    _emojiParticle[i].Play();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < _emojiParticle.Length; i++)
            {
                if (!_emojiParticle[i].isPlaying || i == _emojiParticle.Length - 1)
                {
                    _emojiParticle[i].Play();
                    StartCoroutine(MakeRandomParticle(i, position, _emojiParticle));
                    break;
                }
            }
        }
    }
    IEnumerator<WaitForSeconds> MakeRandomParticle(int num, Vector3 position, ParticleSystem[] particles)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
        particles[num].Stop();
        particles[num].transform.position = position + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0f);
        particles[num].Play();
    }
    public void MakeBigParticle()
    {
        _bigParticle.transform.position = new Vector3(-50f, 0, 0);
        _bigParticle.Play();
    }
    public void MakeConfettiParticles()
    {
        _confettiParticles.transform.position = new Vector3(-50f, 0, 0);
        _confettiParticles.Play();
    }
}
