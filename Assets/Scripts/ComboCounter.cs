using UnityEngine;
using TMPro;

public class ComboCounter : MonoBehaviour
{
    private TextMeshPro tmp;
    private string comboSuffix = "<size=3.5><color=#B25959> Combo</color></size>";
    private int comboCount;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        ResetCombo();
    }

    public void ResetCombo()
    {
        SetCombo(0);
    }

    public void IncrementCombo()
    {
        SetCombo(comboCount + 1);
    }

    void SetCombo(int newCount)
    {
        comboCount = newCount;
        tmp.text = comboCount + comboSuffix;
        if (comboCount > ResultsInfo.combo)
        {
            ResultsInfo.combo = comboCount;
        }
    }
}
