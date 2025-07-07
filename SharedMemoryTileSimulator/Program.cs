// Entry point for the Shared Memory Tile Simulator application.
// Demonstrates tile loading, memory layout, and cache line visualization.
//
// Author: David Jones+Copilot+ChatGPT initial code gen/ideas
// Date: 7/7/2025
// Prompt session: https://chatgpt.com/share/686be76e-1580-800d-80ca-3011ba2858e3
//
// This file initializes a matrix and runs the TileSimulator to demonstrate shared memory tile operations.

// SharedMemoryTileSim.cs
// Refactored to use object-oriented approach with memory address and cache layout inspection

using System;
using System.Runtime.CompilerServices;
using SharedMemoryTileSimulator;

const int MatrixSize = 8;
const int TileSize = 4;

var matrix = InitializeMatrix(MatrixSize);
var simulator = new TileSimulator(matrix, TileSize);
simulator.Run(tileX: 2, tileY: 3);

Console.WriteLine("\nPress any key to exit...");
Console.ReadLine();
static int[,] InitializeMatrix(int size)
{
    var matrix = new int[size, size];
    for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
            matrix[y, x] = y * size + x;
    return matrix;
}

