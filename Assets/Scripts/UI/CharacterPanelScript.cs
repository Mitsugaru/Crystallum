using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelScript : MonoBehaviour
{

    public Text NameField;

    public Text LevelField;

    public Text HPField;

    public Text VirtueField;

    public Text ResolveField;

    public Text SpiritField;

    public Text DeftField;

    public Text VitalityField;

    private Image background;

    public Entity Character { get; set; }

    void Start()
    {
        background = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Character != null)
        {
            NameField.text = Character.Name;
            LevelField.text = Character.Level.ToString();
            HPField.text = Character.HP.ToString();
            VirtueField.text = Character.Virtue.ToString();
            ResolveField.text = Character.Resolve.ToString();
            SpiritField.text = Character.Spirit.ToString();
            DeftField.text = Character.Deft.ToString();
            VitalityField.text = Character.Vitality.ToString();
            if (Character.HP == 0)
            {
                background.color = Color.red;
            }
        }
    }

    public void SetBackgroundColor(Color color)
    {
        background.color = color;
    }
}
