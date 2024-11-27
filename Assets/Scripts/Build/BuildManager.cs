using UnityEngine.Tilemaps;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject buildingPanel;
    private UIManager uiManager;
    public static BuildManager buildManager;
    public Tilemap tilemap;
    private GameObject currentBuildingPreview;
    public GameObject prefab;

    private void Awake()
    {
        if (buildManager == null)
        {
            buildManager = this;
        }
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        buildingPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShowPanel();
        }
    }

    private void ShowPanel()
    {
        if (buildingPanel.activeSelf)
        {
            buildingPanel.SetActive(false);
        }
        else if (!uiManager.IsAnyPanelOpen())
        {
            buildingPanel.SetActive(true);
        }
    }

    public void ChooseBuilding(GameObject building)
    {
        var currentRequirements = building.GetComponent<RequirementManager>();
        prefab = building.GetComponent<RequirementManager>().prefab;

        if (!currentRequirements.CheckRequirements())
        {
            uiManager.ShakeButton(building, 0.5f, 7f);
            return;
        }

        if (buildingPanel.activeSelf)
        {
            ShowPanel();
        }
        currentBuildingPreview = Instantiate(currentRequirements.prefab);
        var previewScript = currentBuildingPreview.GetComponent<BuildingPreview>();
        previewScript.StartPreview();

    }

    public Vector3Int GetTilePositionFromMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return tilemap.WorldToCell(mouseWorldPosition);
    }
}
