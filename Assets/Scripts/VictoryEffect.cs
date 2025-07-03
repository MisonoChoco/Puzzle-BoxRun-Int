using UnityEngine;

public class VictoryEffect : MonoBehaviour
{
    public static void SpawnAt(Vector3 position)
    {
        GameObject go = new GameObject("VictoryEffect");
        go.transform.position = position;

        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        var main = ps.main;

        // Configure all 'main' properties before calling Play or Stop
        main.playOnAwake = false;
        main.duration = 3f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(3f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(60f);
        main.startSize = new ParticleSystem.MinMaxCurve(2f, 3f);  // Combined definition
        main.startColor = new ParticleSystem.MinMaxGradient(Color.white);

        // Configure remaining modules
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 0f;
        shape.radius = 2f;
        shape.rotation = new Vector3(-90f, 0f, 0f);

        var emission = ps.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(300f);

        var lights = ps.lights;
        lights.enabled = true;
        lights.range = 10f;
        lights.intensityMultiplier = 2f;

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        Material mat = new Material(Shader.Find("Particles/Standard Unlit"));
        mat.SetColor("_Color", Color.white);
        renderer.material = mat;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        // Ensure system is fully stopped before playback
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Now safe to play
        ps.Play();

        float totalTime = main.duration + main.startLifetime.constantMax;
        Object.Destroy(go, totalTime);
    }
}