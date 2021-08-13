using UnityEngine;
using System.Collections.Generic;

public class ParticlesManager : MonoBehaviour
{
    [HideInInspector] public static ParticlesManager Current;

    public ParticleSystem ConfettiParticles;
    private ParticleSystem _confettiParticles;

    public ParticleSystem BigSimpleSmileParticles;
    private ParticleSystem _bigSimpleSmile;

    public ParticleSystem SimpleSmileParticles;
    private ParticleSystem[] _simpleSmile;

    private void Awake()
    {
        Current = this;
        _simpleSmile = new ParticleSystem[10];
        _bigSimpleSmile = Instantiate(BigSimpleSmileParticles, new Vector3(0, -10, 0f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
        _confettiParticles = Instantiate(ConfettiParticles, new Vector3(0, -10, 0f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
        for (int i = 0; i < _simpleSmile.Length; i++)
        {
            _simpleSmile[i] = Instantiate(SimpleSmileParticles, new Vector3(0, -10, 0f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
        }
    }
    public void MakeSimpleSmile(Vector3 position, bool isRandom)
    {
        if (!isRandom)
        {
            for (int i = 0; i < _simpleSmile.Length; i++)
            {
                if (!_simpleSmile[i].isPlaying || i == _simpleSmile.Length - 1)
                {
                    _simpleSmile[i].transform.position = position;
                    _simpleSmile[i].Play();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < _simpleSmile.Length; i++)
            {
                if (!_simpleSmile[i].isPlaying || i == _simpleSmile.Length - 1)
                {
                    _simpleSmile[i].Play();
                    StartCoroutine(MakeRandomParticle(i, position, _simpleSmile));
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
    public void MakeBigSimpleSmile()
    {
        _bigSimpleSmile.transform.position = new Vector3(-50f, 0, 0);
        _bigSimpleSmile.Play();
    }
    public void MakeConfettiParticles()
    {
        _confettiParticles.transform.position = new Vector3(-50f, 0, 0);
        _confettiParticles.Play();
    }
}
