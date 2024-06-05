using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Canvas canvas;

    [SerializeField] public GameObject menuUIPrefab;
    [SerializeField] public GameObject inventoryPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(canvas.gameObject.name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var findCanvasInScene = FindObjectsOfType<Canvas>();
        if (findCanvasInScene.Length > 0)
        {
            canvas = findCanvasInScene[0];
        }
        else
        {
            GameObject canvasGO = new GameObject();
            canvasGO.AddComponent<Canvas>();
            canvasGO.name = "Canvas";
            canvas = canvasGO.GetComponent<Canvas>();
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void DestroyCurrentWidget(GameObject _GO)
    {
        Destroy(_GO);
    }

    public List<GameObject> FindChildrenComponentsInParent(GameObject parent, System.Type componentType)
    {
        List<GameObject> gameObjectsWithComponent = new List<GameObject>();

        Component[] components = parent.GetComponentsInChildren(componentType, true);

        // Iterate over each component and add its game object to the list
        foreach (Component component in components)
        {
            gameObjectsWithComponent.Add(component.gameObject);
        }
        return gameObjectsWithComponent;
    }    

    public List<GameObject> FindChildrenGOInParent(Transform parent)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (Transform child in parent)
        {
            gameObjects.Add(child.gameObject);
        }
        return gameObjects;
    }

    public GameObject GetWidgetManager()
    {
        List<GameObject> gameObjectsWithComponent = new List<GameObject>();
        gameObjectsWithComponent = FindChildrenComponentsInParent(canvas.gameObject, typeof(WidgetManager));
        if (gameObjectsWithComponent.Count > 0)
        {
            return gameObjectsWithComponent[0];
        }
        else
        {
            return null;
        }
        
    }

    //! Open Backpack
    public void OpenBackpack()
    {
        if (GetWidgetManager().GetComponentsInChildren<InventoryUI>().Length <= 0)
        {
            GameObject _inventoryPrefab = Instantiate(inventoryPrefab);
            _inventoryPrefab.transform.parent = GetWidgetManager().transform;
            _inventoryPrefab.transform.localPosition = Vector3.zero;
        }

    }

}
