using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeResidueManager : MonoBehaviour
{
    public static SlimeResidueManager Instance;

    [SerializeField] private int poolSize;
    [SerializeField] private SlimeResidue slimePrefab;
    
    private List<SlimeResidue> _slimeObjects = new List<SlimeResidue>();


    public static int MaxActiveResidues
    {
        get { return Instance.poolSize; }
    }
    
    public static int ActiveEnemies
    {
        get
        {
            int numActiveResidues = 0;
            foreach (var slime in Instance._slimeObjects)
            {
                if (slime.gameObject.activeSelf)
                {
                    numActiveResidues++;
                }
            }

            return numActiveResidues;
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var slime = Instantiate(slimePrefab, transform);
            slime.gameObject.SetActive(false);
            _slimeObjects.Add(slime.GetComponent<SlimeResidue>());
        }
    }

    public static void Spawn(Vector3 position, Color color, bool isFlipped)
    {
        SlimeResidue oldestSlime = Instance._slimeObjects[0];
        bool slimeFound = false;
        foreach (var slime in Instance._slimeObjects)
        {
            if (slime.Age > oldestSlime.Age)
            {
                oldestSlime = slime;
            }
            if (!slime.gameObject.activeSelf)
            {
                slime.transform.position = position;
                slime.SetColor(color);
                slime.ResetAge();
                slime.Flipped(isFlipped);
                slime.gameObject.SetActive(true);
                slimeFound = true;
            }
        }

        if (!slimeFound)
        {
            oldestSlime.transform.position = position;
            oldestSlime.SetColor(color);
            oldestSlime.ResetAge();
            oldestSlime.Flipped(isFlipped);
        }   
    }
}
