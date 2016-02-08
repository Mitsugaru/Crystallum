using UnityEngine;
using UnityEngine.UI;

public class ToggleUIScript : MonoBehaviour
{

    public GameObject Target;

    // Use this for initialization
    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Toggle);
    }

    public void Toggle()
    {
        Target.SetActive(!Target.activeInHierarchy);
    }
}
