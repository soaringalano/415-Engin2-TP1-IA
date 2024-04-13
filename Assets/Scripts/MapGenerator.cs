using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_collectiblePrefab;

	public const int WORKER_COST = 20;

	[field: SerializeField]
	public static int Seed { get; private set; }
	[SerializeField]
	private bool m_useRandomSeed = true;
    private int rejectionSamples = 30;

    [field: SerializeField]
	public static RandomIntBetweenRange CampCost { get; private set; } = new RandomIntBetweenRange(5, 80); //Valeur � laquelle vos workers peuvent acc�der
    [SerializeField]
	private RandomIntBetweenRange m_mapDimension; //Valeur � laquelle vos workers peuvent acc�der
    [SerializeField]
	private RandomIntBetweenRange m_nodesDensity;	//Valeur INCONNUE de vos workers
	public static RandomIntBetweenRange SimulationDuration { get; private set; } = new RandomIntBetweenRange(10, 1000); //In seconds. Between 10 and 1000
		//Valeur � laquelle vos workers peuvent acc�der



    private void Awake()
    {
		GenerateValues();
		GenerateMap();
		ShiftMap();
    }

    private void Update()
    {
        if (SimulationDuration.Value < Time.timeSinceLevelLoad)
		{
			TeamOrchestrator._Instance.OnGameEnded();
		}
    }

    private void GenerateValues()
	{
		if (m_useRandomSeed)
		{
            Seed = (int)DateTime.Now.Ticks;
        }
		UnityEngine.Random.InitState(Seed);


		CampCost.RollValue();
		m_mapDimension.RollValue();
		m_nodesDensity.RollValue();
		SimulationDuration.RollValue();
    }

    private void GenerateMap()
    {
        List<Vector2> points;
        points = PoissonDiscSampling.GeneratePoints(m_nodesDensity.Value, Vector2.one * m_mapDimension.Value, rejectionSamples);

		foreach (var point in points)
		{
			Instantiate(m_collectiblePrefab, new Vector3(point.x, point.y, 0), Quaternion.identity, transform);
		}
		Debug.Log("Points generated: " + points.Count.ToString());
    }

    private void ShiftMap()
    {
        transform.Translate(-m_mapDimension.Value / 2, -m_mapDimension.Value / 2, 0);
    }
}

[System.Serializable]
public class RandomIntBetweenRange
{
	[SerializeField]
	private int m_minimumValue = 0;
	[SerializeField]
	private int m_maximumValue = 100;
	[field:SerializeField]
	public int Value { get; private set; }

	public RandomIntBetweenRange()
	{

	}

	public RandomIntBetweenRange(int min, int max)
	{
		m_minimumValue = min; m_maximumValue = max;
    }

	public void RollValue()
	{
        Value = UnityEngine.Random.Range(m_minimumValue, m_maximumValue + 1);
	}
}