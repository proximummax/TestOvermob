using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreList : MonoBehaviour
{
    [SerializeField] private ScoreListElement _scoreListElementTemplate;
    [System.Serializable]
    public class ScoreListElementParameters
    {
        public Sprite _icon;
        public int _score;
    }
    [SerializeField] private List<ScoreListElementParameters> _scoreListElementsParameters;

    [SerializeField] private ScrollRect _scoreList;
    [SerializeField] private ScoreListElement _savedScoreListElementTop;
    [SerializeField] private ScoreListElement _savedScoreListElementBottom;

    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _defaultColor;

    [SerializeField] private UIInputController _inputController;

    private ScoreListElement _currentSelectedElement;
    private List<ScoreListElement> _scoreListElements = new List<ScoreListElement>();

    private void OnEnable()
    {
        _inputController.ElementIsClicked += SelectElement;
    }

    private void OnDisable()
    {
        _inputController.ElementIsClicked -= SelectElement;
    }

    private void Start()
    {
        foreach (var elementParameters in _scoreListElementsParameters)
        {
            ScoreListElement currentScoreListElement = Instantiate(_scoreListElementTemplate, _scoreList.content);
            SetElementParameters(elementParameters, ref currentScoreListElement);

            _scoreListElements.Add(currentScoreListElement);
        }
    }

    private void Update()
    {
        if (_currentSelectedElement != null)
        {
            if (_currentSelectedElement.RendererComponent.isVisible)
                CloseSavedElements();
            else
                ShowSavedElement();
        }

    }

    private void SetElementParameters(ScoreListElementParameters parameters, ref ScoreListElement currentElement)
    {
        currentElement.Image.sprite = parameters._icon;
        currentElement.ScoreText.text = parameters._score.ToString();
        currentElement.IndexText.text = (_scoreListElementsParameters.IndexOf(parameters) + 1).ToString();
    }
    private void SetElementParametersByAnotherElement(ScoreListElement elementFrom, ref ScoreListElement elementTo)
    {
        elementTo.Image.sprite = elementFrom.Image.sprite;
        elementTo.ScoreText.text = elementFrom.ScoreText.text;
        elementTo.IndexText.text = elementFrom.IndexText.text;
    }
    

    private int GetIndexOfFirstShowingElementOnScreen()
    {
        ScoreListElement firstShowingElement = null;

        foreach (var element in _scoreListElements)
        {
            if (!element.RendererComponent.isVisible)
                continue;

            firstShowingElement = element;

            break;

        }
        return Convert.ToInt32(firstShowingElement.IndexText.text);
    }

    private void ShowSavedElement()
    {
        int selectedElementIndex = Convert.ToInt32(_currentSelectedElement.IndexText.text);

        int showingElement = GetIndexOfFirstShowingElementOnScreen();

        if (selectedElementIndex < showingElement)
        {
            SetElementParametersByAnotherElement(_currentSelectedElement, ref _savedScoreListElementTop)
            _savedScoreListElementTop.gameObject.SetActive(true);
        }

        else if (selectedElementIndex > showingElement)
        {
            SetElementParametersByAnotherElement(_currentSelectedElement, ref _savedScoreListElementBottom)
            _savedScoreListElementBottom.gameObject.SetActive(true);
        }

    }
    private void CloseSavedElements()
    {
        if (_savedScoreListElementBottom.gameObject.activeSelf)
            _savedScoreListElementBottom.gameObject.SetActive(false);

        else if (_savedScoreListElementTop.gameObject.activeSelf)
            _savedScoreListElementTop.gameObject.SetActive(false);
    }
    private void SelectElement(ScoreListElement selectedElement)
    {
        if (_currentSelectedElement != null)
            SetDefaultColor();

        _currentSelectedElement = selectedElement;

        SetSelectedColor(_currentSelectedElement);

    }

    private void SetSelectedColor(ScoreListElement element)
    {
        var backgroundImage = element.gameObject.GetComponent<Image>();
        if (backgroundImage == null)
            return;

        backgroundImage.color = _selectedColor;
    }

    private void SetDefaultColor()
    {
        foreach (var element in _scoreListElements)
        {
            var backgroundImage = element.gameObject.GetComponent<Image>();
            if (backgroundImage == null)
                break;

            backgroundImage.color = _defaultColor;
        }
    }
}
