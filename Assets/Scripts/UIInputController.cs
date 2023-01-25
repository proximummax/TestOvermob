using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInputController : MonoBehaviour
{
    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;

    [SerializeField] private Canvas _canvasScoreList;

    public UnityAction<ScoreListElement> ElementIsClicked;

    void Start()
    {
        _raycaster = _canvasScoreList.GetComponent<GraphicRaycaster>();

        _eventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _pointerEventData = new PointerEventData(_eventSystem);

            _pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            _raycaster.Raycast(_pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (!result.gameObject.TryGetComponent(out ScoreListElement listElement))
                    continue;

                ElementIsClicked?.Invoke(listElement);
                
                break;
            }
        }
    }
}
