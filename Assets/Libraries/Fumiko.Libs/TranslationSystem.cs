using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Fumiko.Systems.Debug;

namespace Fumiko.Localization
{
    public class TranslationSystem : MonoBehaviour
    {
        public static TranslationSystem instance;

        public Dictionary<string, string> actions = new Dictionary<string, string>();

        public Languages currentLanguage = Languages.ENGLISH;

        public Dictionary<string, string> descriptions = new Dictionary<string, string>();

        public Dictionary<string, string> templates = new Dictionary<string, string>();

        private string[] wordFiles = new string[] {
            "Templates",
            "Actions",
            "Descriptions"
        };

        public bool HasAction(string searchKey)
        {
            return actions.ContainsKey(searchKey);
        }

        public string GetAction(string searchKey)
        {
            if (actions.ContainsKey(searchKey))
            {
                return actions[searchKey];
            }

            return "{unknown action}";
        }

        public bool HasDescription(string searchKey)
        {
            return descriptions.ContainsKey(searchKey);
        }

        public string GetDescription(string searchKey)
        {
            if (descriptions.ContainsKey(searchKey))
            {
                return descriptions[searchKey];
            }

            return "{unknown description}";
        }

        public bool HasTemplate(string searchKey)
        {
            return templates.ContainsKey(searchKey);
        }

        public string GetTemplate(string searchKey)
        {
            if (templates.ContainsKey(searchKey))
            {
                return templates[searchKey];
            }

            return "{unknown string template}";
        }

        private void Awake()
        {
            if (instance != null)
            {
                LogSystem.instance.DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }

        private void getTranslationFiles()
        {
            templates.Clear();
            actions.Clear();
            descriptions.Clear();

            /*string subLanguageFolder = "Translation/";

            if (currentLanguage == Languages.ENGLISH)
            {
                subLanguageFolder += "English/";
            }
            else if (currentLanguage == Languages.GERMAN)
            {
                subLanguageFolder += "German/";
            }
            else if (currentLanguage == Languages.FRENCH)
            {
                subLanguageFolder += "French/";
            }

            for (int i = 0; i < wordFiles.Length; i++)
            {
                TextAsset asset = Resources.Load<TextAsset>(subLanguageFolder + wordFiles[i]);
                string words = asset.text;
                JsonTextReader reader = new JsonTextReader(new StringReader(words));
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = reader.Path;
                        reader.Skip();
                        string propertyValue = reader.Value.ToString();

                        if (wordFiles[i] == "Templates")
                        {
                            templates.Add(propertyName, propertyValue);
                        }

                        if (wordFiles[i] == "Actions")
                        {
                            actions.Add(propertyName, propertyValue);
                        }

                        if (wordFiles[i] == "Descriptions")
                        {
                            descriptions.Add(propertyName, propertyValue);
                        }
                    }
                }
            }*/
        }

        private void Start()
        {
            getTranslationFiles();
        }
    }
}