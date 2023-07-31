using System.Collections;
using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.EditorMenus.Actions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{ 
    private void Start()
    {
        int i = 0;
        foreach (var stat in PlayFabManager.Instance.Player.Stats)
        {
            if (stat.Key == Attribute.DefIgnored) break;
            GameObject newGO = new GameObject(stat.Key.ToString());
            newGO.transform.SetParent(this.transform);
            newGO.transform.localScale = this.transform.localScale;
            newGO.transform.localPosition = new Vector3(-40, (i + 1) * -80 + 380);


            TextMeshProUGUI statText = newGO.AddComponent<TextMeshProUGUI>();
            statText.rectTransform.sizeDelta = new Vector2(400, 50);
            statText.fontSize = 36;
            statText.text = stat.Key + " : " + stat.Value.Value;
            // statText.transform.position = new Vector3(10, (i + 1) * 80 - 420);

            Debug.Log(stat.Key + " : " + stat.Value.Value);
            i++;
        }
    }
}