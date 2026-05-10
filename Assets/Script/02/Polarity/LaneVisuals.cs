using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LaneVisuals : MonoBehaviour
{
    public Color positiveColor =
        new Color(1f, 0f, 0f, 0.9f);

    public Color negativeColor =
        new Color(0f, 0.5f, 1f, 0.9f);

    LanePolarity polarity;

    SpriteRenderer glowRenderer;
    SpriteRenderer lineRenderer;
    Light2D glowLight;

    public Material particleMaterial;
    ParticleSystem particles;

    void Start()
    {
        polarity = GetComponent<LanePolarity>();

        CreateGlow();
        CreateLine();
        CreateParticles();

        ApplyVisual();
    }

    void CreateGlow()
    {
        GameObject glow =
            new GameObject("GlowLight");

        glow.transform.SetParent(transform);

        glow.transform.localPosition =
            new Vector3(0, 0.15f, 0);

        glowLight =
            glow.AddComponent<Light2D>();

        glowLight.lightType =
            Light2D.LightType.Point;

        glowLight.intensity = 1.5f;

        glowLight.pointLightOuterRadius = 2f;

        glowLight.pointLightInnerRadius = 0.5f;

        glowLight.falloffIntensity = 1f;
    }

    void CreateLine()
    {
        GameObject line = new GameObject("Line");

        line.transform.SetParent(transform);

        line.transform.localPosition =
            Vector3.zero;

        lineRenderer =
            line.AddComponent<SpriteRenderer>();

        SpriteRenderer original =
            GetComponent<SpriteRenderer>();

        if (original != null)
        {
            lineRenderer.sprite = original.sprite;
        }

        line.transform.localScale =
            new Vector3(1f, 0.015f, 1f);

        lineRenderer.sortingOrder = -4;
    }

    void CreateParticles()
    {
        GameObject particleObj =
            new GameObject("Particles");

        particleObj.transform.SetParent(transform);

        particleObj.transform.localPosition =
            new Vector3(0, 0.538f, 0);

        // ✅ สร้าง ParticleSystem ก่อน
        particles =
            particleObj.AddComponent<ParticleSystem>();

        // ✅ Renderer
        ParticleSystemRenderer renderer =
            particles.GetComponent<ParticleSystemRenderer>();

        renderer.sortingOrder = 3;

        // ✅ Main
        var main = particles.main;

        main.startLifetime = 0.8f;
        main.startSpeed = 0.05f;
        main.startSize = 0.15f;
        main.loop = true;

        // ✅ Emission
        var emission = particles.emission;
        emission.rateOverTime = 10;

        // ✅ Shape
        var shape = particles.shape;

        shape.shapeType =
            ParticleSystemShapeType.Box;

        shape.scale =
            new Vector3(0.8f, 0.02f, 0f);

        // ✅ Magnetic movement
        var velocity =
            particles.velocityOverLifetime;

        velocity.enabled = true;

        velocity.orbitalZ = 0.5f;

        if (particleMaterial != null)
        {
            renderer.material = particleMaterial;
        }
    }
    public void ApplyVisual()
    {
        if (polarity == null) return;

        Color targetColor =
            polarity.lanePolarity ==
            PolarityType.Positive
            ? positiveColor
            : negativeColor;

        if (glowLight != null)
        {
            glowLight.color = targetColor;
        }
        if (lineRenderer != null)
        {
            lineRenderer.color = targetColor;
        }

        if (particles != null)
        {
            var main = particles.main;
            main.startColor = targetColor;
        }
    }

    void Update()
    {
        if (glowLight != null)
        {
            glowLight.intensity =
                1.2f +
                Mathf.Sin(Time.time * 2f)
                * 0.3f;
        }
    }
}