using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Analytics;
using NaughtyAttributes;

public class CharacterHover : MonoBehaviour
{
    [SerializeField, Scene] private string _mainMenu;
    [SerializeField, Dropdown(nameof(GetGendersValues))] private int _gender;

    public Image characterImage;
    public Image hoverPanel;
    public TextMeshProUGUI characterName;
    public string Name;

    private Vector3 originalScale;
    private Vector3 targerScale;
    private Color originalPanelColor;

    public float zoomSpeed = 5.0f;

    private void Start()
    {
        originalScale = characterImage.transform.localScale;
        originalPanelColor = hoverPanel.color;
        targerScale = originalScale * 0.9f;

        hoverPanel.gameObject.SetActive(false);
        characterName.gameObject.SetActive(false);
    }
    private DropdownList<int> GetGendersValues()
    {
        return new()
        {
            { "Female", 1 },
            { "Male", 2 }
        };
    }

    public void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomImage(targerScale));

        hoverPanel.gameObject.SetActive(true);
        characterName.gameObject.SetActive(true);
        characterName.text = Name;
    }

    public void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomImage(originalScale));

        hoverPanel.gameObject.SetActive(false);
        characterName.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        PlayFabManager.Instance.SetGender(_gender);
        ScenesManager.Instance.SetScene(_mainMenu);
    }

    private IEnumerator ZoomImage(Vector3 target)
    {
        float elapsedTime = 0;
        Vector3 startingScale = characterImage.transform.localScale;

        while (elapsedTime < 1.0f)
        {
            characterImage.transform.localScale = Vector3.Lerp(startingScale, target, elapsedTime);
            elapsedTime += Time.deltaTime * zoomSpeed;
            yield return null;
        }

        characterImage.transform.localScale = target;
    }
}