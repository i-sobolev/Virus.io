using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const int _totalNumberOfPeople = 200;
    private const int _totalNumberOfBots = 9;
    
    [Header("Prefabs")]
    public GameObject peopleFab;
    public GameObject botFab;

    [Header("Sprites")]
    public Sprite[] peopleSprites;
    public Sprite[] headsSprites;
    public Sprite[] bodiesSprites;
    
    private int _spiteNum = 0;

    private const float _areaRange = 24.5f;

    [HideInInspector]
    public static Spawner Singletone { get; private set; }

    private void Awake()
    {
        Singletone = this;

        for (int i = _totalNumberOfPeople; i >= 0; i--)
            SpawnPeople();

        for (int i = _totalNumberOfBots; i >= 0; i--)
            SpawnBot(true);
    }

    private void SpawnPeople()
    {
        Vector3 spawnPos = new Vector2(Random.Range(-_areaRange, _areaRange), Random.Range(-_areaRange, _areaRange));

        GameObject instPeople = Instantiate(peopleFab, spawnPos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        ChangeSprite(instPeople);
    }

    public void SpawnPeopleOnVirusDeath(Vector3 currentPosition, int howMany) 
    { 
        for (int i = howMany; i > 0; i--)
        {
            var instPeople = SpawnPeople(currentPosition);
            instPeople.name = "D_People";
            ChangeSprite(instPeople);
        }
    }

    public void SpawnBot(bool isInstantietedOnStart = false)
    {
        Vector3 spawnPosition = new Vector2(Random.Range(-_areaRange + 2, _areaRange - 2), Random.Range(-_areaRange + 2, _areaRange - 2));
        int randomSkinNumber = Random.Range(0, headsSprites.Length);
        AIVirusHead bot = Instantiate(botFab, spawnPosition, Quaternion.identity).GetComponent<AIVirusHead>();
        
        bot.SetSprites(headsSprites[randomSkinNumber], bodiesSprites[randomSkinNumber]);
        bot.isInstantietedOnStart = isInstantietedOnStart;
    }

    public GameObject SpawnPeople(Vector3 spawnPosition)
    {
        GameObject gamObj = Instantiate(peopleFab, spawnPosition, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        return gamObj;
    }

    private void ChangeSprite(GameObject peopleGameObject)
    {
        peopleGameObject.GetComponent<SpriteRenderer>().sprite = peopleSprites[_spiteNum];
        _spiteNum++;

        if (_spiteNum >= peopleSprites.Length - 1)
            _spiteNum = 0;
    }

    public static Vector2 NewPeoplePosition()
    {
        return new Vector3(Random.Range(-_areaRange, _areaRange), Random.Range(-_areaRange, _areaRange));
    }
}
