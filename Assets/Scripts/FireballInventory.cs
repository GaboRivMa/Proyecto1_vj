using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballInventory : MonoBehaviour
{

    public static FireballInventory Instance;
    public int currentFireballs = 0;
    public int maxFireballs = 2;

    [Header("UI Settings")]
    public GameObject circlePrefab; 
    public Transform container;     


    private List<GameObject> activeIcons = new List<GameObject>();

    void Awake() {
        Instance = this;
    }

    public void AddFireball() {
        if (currentFireballs < maxFireballs) {
            currentFireballs++;
            UpdateUI();
        } 
    }

    public void UseFireball() {
        if (currentFireballs > 0) {
            currentFireballs--;
            UpdateUI();
        }
    }

    void UpdateUI() {
        // Limpiamos los iconos actuales
        foreach (GameObject icon in activeIcons) {
            Destroy(icon);
        }
        activeIcons.Clear();

        // Creamos los nuevos círculos según el contador actual
        for (int i = 0; i < currentFireballs; i++) {
            GameObject newIcon = Instantiate(circlePrefab, container);
            activeIcons.Add(newIcon);
        }
    }
}

