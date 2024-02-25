using System;
using UnityEngine;

namespace OctanGames.Background
{
    [Serializable]
    public class ParallaxComponent
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Vector2 _parallaxWeight;

        private float _textureUnitSizeX;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Vector2 ParallaxWeight => _parallaxWeight;

        public float TextureUnitSizeX
        {
            get
            {
                if (_textureUnitSizeX == 0)
                {
                    Sprite sprite = _spriteRenderer.sprite;
                    Texture2D texture = sprite.texture;
                    _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
                }

                return _textureUnitSizeX;
            }
        }
    }
}