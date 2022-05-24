using System;
using UnityEngine;
using System.Linq;

namespace TeamZ.Code.Game.Highlighting
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Highlighter : MonoBehaviour
    {
        public bool ShowOnTop = true;
        public Vector3 Size = Vector3.one;
        public Color Color = Color.green;
        
        public GameObject Template;
        
        private GameObject highlightingGameObject;

        private void Start()
        {
            if (!this.Template)
            {
                return;
            }
            
            var spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.highlightingGameObject = GameObject.Instantiate(this.Template, this.gameObject.transform);
            highlightingGameObject.transform.localScale = this.Size;
            
            var highlightingRenderer = highlightingGameObject.GetComponent<SpriteRenderer>();
            highlightingRenderer.sprite = spriteRenderer.sprite;
            highlightingRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
            
            var layerOrderShift = (this.ShowOnTop ? 1 : -1);
            highlightingRenderer.sortingOrder = spriteRenderer.sortingOrder + layerOrderShift;
            highlightingRenderer.color = this.Color;
        }

        private void OnDestroy()
        {
            this.highlightingGameObject.Destroy();
        }
    }
}