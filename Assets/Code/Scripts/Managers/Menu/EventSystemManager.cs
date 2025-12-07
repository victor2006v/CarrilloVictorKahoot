using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class EventSystemManager : MonoBehaviour{

    [Header("Focus GameObjects")]
    private GameObject initialFocusObject;
    [HideInInspector] public GameObject lastSelectedObject;

    [Header("Input Actions Configurations")]
    private InputAction clickAction;
    private InputSystemUIInputModule uIInputModule;

    private void Awake(){
        if (uIInputModule == null) uIInputModule = GetComponent<InputSystemUIInputModule>();
    }

    private void Start() {
        if(EventSystem.current != null) initialFocusObject = EventSystem.current.firstSelectedGameObject;

        //If the focus is not null the EventSystem is gonna focus the first gameObject
        if (initialFocusObject != null){
            EventSystem.current.SetSelectedGameObject(initialFocusObject);
            lastSelectedObject = initialFocusObject;
        }
        else return;

        if (uIInputModule != null){
            clickAction = uIInputModule.actionsAsset.FindAction("UI/Click");

            if (clickAction != null) clickAction.performed += OnClick;
            else Debug.LogWarning("Action UI/Click not found.", this);
        }
    }

    private void Update(){
        //Here it overrides the focus everytime you move
        var current = EventSystem.current.currentSelectedGameObject;
        if (current != null && current != lastSelectedObject){
            lastSelectedObject = current;
        }
    }
    private void OnDestroy() {
        if(clickAction != null) clickAction.performed -= OnClick;
    }
    
    private void OnClick(InputAction.CallbackContext ctx){ 
        //This piece of code helps to not lose the focus if we click in other area.
        if(EventSystem.current.currentSelectedGameObject == null && lastSelectedObject != null && lastSelectedObject.activeInHierarchy){ 
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        }
    }
}
