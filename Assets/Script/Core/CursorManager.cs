using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject _cursor;
    private RectTransform rectTransform;
    private static CursorManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Cursor.visible = false;
        rectTransform = _cursor.GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.position = Input.mousePosition;
    }
}