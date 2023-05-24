using DG.Tweening;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using UnityEngine.Rendering;

namespace Feofun.Extension
{
    public static class MaterialExt
    {
        private const string SRC_BLEND = "_SrcBlend";
        private const string DST_BLEND = "_DstBlend";
        private const string Z_WRITE = "_ZWrite";
        private const string ALPHATEST_ON = "_ALPHATEST_ON";
        private const string ALPHA_BLEND_ON = "_ALPHABLEND_ON";
        private const string ALPHA_PREMULTIPLY_ON = "_ALPHAPREMULTIPLY_ON";
        private const string RENDERING_MODE = "_RenderingMode";
        private const int OPAQUE_RENDER_QUEUE = 200;
        private const int TRANSPARENT_RENDER_QUEUE = 3000;
        
        public static void ToTransparent(this Material material)
        {
            if (material.HasRenderingMode())
            {
                material.SetFloat(RENDERING_MODE, 2f);
            }
            material.SetInt(SRC_BLEND, (int) BlendMode.One);
            material.SetInt(DST_BLEND, (int) BlendMode.OneMinusSrcAlpha);
            material.SetInt(Z_WRITE, 0);
            material.DisableKeyword(ALPHATEST_ON);
            material.DisableKeyword(ALPHA_BLEND_ON);
            material.EnableKeyword(ALPHA_PREMULTIPLY_ON);
            material.renderQueue = TRANSPARENT_RENDER_QUEUE;
        }

        public static void ToFade(this Material material)
        {
            if (material.HasRenderingMode())
            {
                material.SetFloat(RENDERING_MODE, 1f);
            }
            material.SetInt(SRC_BLEND, (int)BlendMode.SrcAlpha);
            material.SetInt(DST_BLEND, (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt(Z_WRITE, 0);
            material.DisableKeyword(ALPHATEST_ON);
            material.EnableKeyword(ALPHA_BLEND_ON);
            material.EnableKeyword(ALPHA_PREMULTIPLY_ON);
            material.renderQueue = TRANSPARENT_RENDER_QUEUE;
        }

        public static void ToOpaque(this Material material)
        {
            if (material.HasRenderingMode())
            {
                material.SetFloat(RENDERING_MODE, 0f);
            }
            material.SetInt(SRC_BLEND, (int)BlendMode.One);
            material.SetInt(DST_BLEND, (int)BlendMode.Zero);
            material.SetInt(Z_WRITE, 1);
            material.DisableKeyword(ALPHATEST_ON);
            material.DisableKeyword(ALPHA_BLEND_ON);
            material.DisableKeyword(ALPHA_PREMULTIPLY_ON);
            material.renderQueue = OPAQUE_RENDER_QUEUE;
        }

        public static bool IsTransparentOrFade(this Material material)
        {
            return material.HasInt(DST_BLEND) && material.GetInt(DST_BLEND) != (int) BlendMode.Zero;
        }

        public static bool HasRenderingMode(this Material material)
        {
            return material.HasFloat(RENDERING_MODE);
        }

        public static Color GetColor(this Material material, string colorPropertyName = null)
        {
            return !colorPropertyName.IsNullOrEmpty() ? material.GetColor(colorPropertyName) : material.color;
        }

        public static void SetColor(this Material material, Color color, string colorPropertyName = null)
        {
            if (!colorPropertyName.IsNullOrEmpty())
            {
                material.SetColor(colorPropertyName, color);
                return;
            }

            material.color = color;
        }
    }
}