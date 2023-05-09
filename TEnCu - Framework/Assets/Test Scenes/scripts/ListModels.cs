using System.Collections.Generic;
using Test_Scenes.models;
using UnityEngine;
using UnityEngine.UI;

namespace Test_Scenes.scripts
{
    public class ListModels: MonoBehaviour
    {
        public GameObject modelPrefab;
        public GameObject container;
        private List<Model> _models;
        private void Awake()
        {
            Startup.Local.AssetBundlesManager.LoadAllAssetBundlesFromStorage();
            var text = Resources.Load<TextAsset>("models").text;
            _models = JsonUtility.FromJson<ModelsList>(text).models;
        }

        private void Start()
        {
            container.GetComponent<RectTransform>().sizeDelta = new Vector2(0, _models.Count * modelPrefab.GetComponent<RectTransform>().sizeDelta.y);
            foreach (var model in _models)
            {
                var modelButton = Instantiate(modelPrefab, container.transform);
                modelButton.GetComponent<Button>().onClick.AddListener(() => OpenModel.OpenModelScene(model.assetBundleName));
                modelButton.GetComponentInChildren<Text>().text = model.name;
            }
        }
    }
}