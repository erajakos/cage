using System;
using UnityEngine;

public class Grid
{
    private int cols;
    private int rows;
    private float perlinNoiseMultiplier;

    public Grid(int cols, int rows, float perlinNoiseMultiplier = 4f)
    {
        if (cols < 1)
        {
            throw new ArithmeticException("cols should be greater or equal to 1");
        }

        if (rows < 1)
        {
            throw new ArithmeticException("rows should be greater or equal to 1");
        }

        this.cols = cols;
        this.rows = rows;

        this.perlinNoiseMultiplier = perlinNoiseMultiplier;
    }

    public float[,] GenerateGrid(int sampleOffset = 0)
    {
        float[,] gridArray = new float[cols, rows];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                gridArray[col, row] = GetSample(col, row, sampleOffset);
            }
        }

        return gridArray;
    }

    float GetSample(int x, int y, int sampleOffset = 0)
    {
        x += sampleOffset;
        y += sampleOffset;

        float xCoord = (float)x / cols;
        float yCoord = (float)y / rows;

        float sample = Mathf.PerlinNoise(xCoord * perlinNoiseMultiplier, yCoord * perlinNoiseMultiplier);

        return sample;
    }
}
