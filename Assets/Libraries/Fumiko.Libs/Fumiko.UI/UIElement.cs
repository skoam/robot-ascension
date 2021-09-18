using UnityEngine;
using UnityEngine.UI;

namespace Fumiko.UI
{
    public class UIElement : MonoBehaviour
    {
        public bool active;

        [Header("Animation Style")]
        public bool fades = true;

        public UIElements identifiedBy;
        public Image[] imageRenderers;
        public Image mainImageRenderer;
        public Text mainTextRenderer;
        public float maximumAlpha;
        public float minimumAlpha;
        public float speedIn = 1;
        public float speedOut = 6;
        public float startingValue = 0;
        public Text[] textRenderers;
        private UIElementType type;
        private float visibility = 0;

        public Image getImage
        {
            get
            {
                return mainImageRenderer;
            }
        }

        public Text getText
        {
            get
            {
                return mainTextRenderer;
            }
        }

        public UIElementType getType
        {
            get
            {
                return type;
            }
        }

        public bool visible
        {
            get
            {
                return visibility > 0;
            }
        }

        public Color newColorForElement(Color color)
        {
            Color newColor = color;

            if (active)
            {
                if (fades)
                {
                    if (newColor.a < maximumAlpha)
                    {
                        newColor.a += speedIn * Time.deltaTime;
                    }
                }
                else
                {
                    newColor.a = maximumAlpha;
                }
            }
            else
            {
                if (fades)
                {
                    if (newColor.a > minimumAlpha)
                    {
                        newColor.a -= speedOut * Time.deltaTime;
                    }
                }
                else
                {
                    newColor.a = minimumAlpha;
                }
            }

            return newColor;
        }

        private void Start()
        {
            if (mainImageRenderer == null)
            {
                mainImageRenderer = GetComponent<Image>();
            }

            if (mainTextRenderer == null)
            {
                mainTextRenderer = GetComponent<Text>();
            }

            if (imageRenderers.Length == 0)
            {
                imageRenderers = GetComponentsInChildren<Image>();
            }

            if (textRenderers.Length == 0)
            {
                textRenderers = GetComponentsInChildren<Text>();
            }

            if (mainImageRenderer != null)
            {
                mainImageRenderer.color = new Color(mainImageRenderer.color.r, mainImageRenderer.color.g, mainImageRenderer.color.b, startingValue);
            }

            if (mainTextRenderer != null)
            {
                mainTextRenderer.color = new Color(mainTextRenderer.color.r, mainTextRenderer.color.g, mainTextRenderer.color.b, startingValue);
            }

            if (imageRenderers.Length > 0)
            {
                type = UIElementType.IMAGES;
                for (int i = 0; i < imageRenderers.Length; i++)
                {
                    imageRenderers[i].color = new Color(imageRenderers[i].color.r, imageRenderers[i].color.g, imageRenderers[i].color.b, startingValue);
                }
            }

            if (textRenderers.Length > 0)
            {
                type = UIElementType.TEXTS;
                for (int i = 0; i < textRenderers.Length; i++)
                {
                    textRenderers[i].color = new Color(textRenderers[i].color.r, textRenderers[i].color.g, textRenderers[i].color.b, startingValue);
                }
            }

            if (DynamicUISystem.instance.getUIElement(identifiedBy) == null)
            {
                DynamicUISystem.instance.addUIElement(this);
            }
        }

        private void Update()
        {
            for (int i = 0; i < imageRenderers.Length; i++)
            {
                imageRenderers[i].color = newColorForElement(imageRenderers[i].color);
            }

            for (int i = 0; i < textRenderers.Length; i++)
            {
                textRenderers[i].color = newColorForElement(textRenderers[i].color);
            }
        }
    }
}