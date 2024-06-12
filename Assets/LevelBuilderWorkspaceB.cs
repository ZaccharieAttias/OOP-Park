using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBuilderWorkspaceB : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public LevelBuilderB LevelBuilder;

    private Vector3 _pointerDown, _camPosition;
    public bool canDraw = false;
    public GameObject BuildingHUD;

    public void Update()
    {
        if (!canDraw)
            return;
        LevelBuilder.MoveCursor(Input.mousePosition);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canDraw)
            return;
        LevelBuilder.EnableCursor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!canDraw)
            return;
        LevelBuilder.EnableCursor(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDraw)
            return;
        if (Input.GetMouseButton(0))
        {
            LevelBuilder.Draw(eventData.position);
        }
        else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))
        {
            SetCanDraw();
        }
        else if (Input.GetMouseButton(1))
        {
            _pointerDown = eventData.position;
            _camPosition = Camera.main.transform.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDraw)
            return;
        if (Input.GetMouseButton(0))
        {
            LevelBuilder.Draw(eventData.position);
        }
        else if (Input.GetMouseButton(1))
        {
            Camera.main.transform.position = _camPosition + Camera.main.ScreenToWorldPoint(_pointerDown) - Camera.main.ScreenToWorldPoint(eventData.position);
        }
    }
    public void SetCanDraw()
    {
        this.canDraw = !this.canDraw;
        if (this.canDraw)
        {
            BuildingHUD.SetActive(true);
        }
        else
        {
            BuildingHUD.SetActive(false);
            LevelBuilder.EnableCursor(false);
        }
    }
}
