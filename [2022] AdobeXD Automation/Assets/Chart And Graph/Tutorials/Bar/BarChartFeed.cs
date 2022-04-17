#define Graph_And_Chart_PRO
using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class BarChartFeed : MonoBehaviour {
	void Start () {
        BarChart barChart = GetComponent<BarChart>();
        if (barChart != null)
        {
            barChart.DataSource.SetValue("Player 1", "Value 1", 10);

            //barChart.DataSource.SetValue("Player 1", "Value 1", Random.value * 20);
            barChart.DataSource.SlideValue("Player 2", "Value 1", 10, 2f);
            
            //barChart.DataSource.SlideValue("Player 2", "Value 1", Random.value * 20, 40f);
        }
    }
    private void Update()
    {
    }
}
