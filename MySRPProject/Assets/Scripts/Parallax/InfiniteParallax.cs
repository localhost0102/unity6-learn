using System;
using UnityEngine;

/// <summary>
/// Nakon sto se skripta doda objektu, potrebno je klonirati sprite
/// kao podobjekte, te pozicionirati jednog lijevo i jednog desno od parenta
/// </summary>
public class InfiniteParallax : MonoBehaviour
{
    [SerializeField] float parallaxEffect = 0.1f;
    [Range(0, 255)]
    float lenghtOfSprite;
    float startPosition;
    GameObject cam;

    void Start()
    {
        //cam = Camera.main; // Ako je CineMachine, ovdje ide GameObject.Find("Ime kamere"); i tip je GameObject umjesto Camera
        cam = GameObject.Find("CinemachineCamera");
        startPosition = transform.position.x;
        lenghtOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // temp ce spremiti gdje se kamera trenutno nalazi (jer pozadina se stalno krece)
        float temp = (cam.transform.position.x * (1 - parallaxEffect)); // parallaxEffect je zapravo offset i ako uzmemo 1-offset, dobijemo gdje se objekt nalazi u tom trenutku
        float distance = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        // Ako se kamera krece lijevo: trenutna pozicija kamere (temp) je desno od pocetne pozicije objekta
        // I sprite ce se prebaciti desno, odnosno lijevo za suprotni smjer
        
        // Calculate the half-width of camera view in world units (ovo je fix za spriteove manje od onog sto kamera vidi)
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        
        if (temp > startPosition + lenghtOfSprite + cameraHalfWidth)
        {
            startPosition += lenghtOfSprite;
        }
        else if (temp < startPosition - lenghtOfSprite - cameraHalfWidth)
        {
            startPosition -= lenghtOfSprite;
        }
        
        // Originalni code, radi samo za spritove vece od vidljivosti kamere
        // if (temp > startPosition + lenghtOfSprite)
        // {
        //     startPosition += lenghtOfSprite;
        // }
        // else if (temp < startPosition - lenghtOfSprite)
        // {
        //     startPosition -= lenghtOfSprite;
        // }
    }
}