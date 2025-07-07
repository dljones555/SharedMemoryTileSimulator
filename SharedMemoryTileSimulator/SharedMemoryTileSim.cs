// TileSimulator.cs
// Simulates shared memory tile operations, memory layout, and cache line visualization in C#.
//
// Author: [Your Name]
// Date: [Date]
//
// This class provides methods to load a tile from a matrix, inspect memory addresses, visualize cache lines, and simulate reuse patterns.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedMemoryTileSimulator
{
    /// <summary>
    /// Simulates shared memory tile operations and cache line visualization.
    /// </summary>
    class TileSimulator
    {
        private readonly int[,] matrix;
        private readonly int tileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileSimulator"/> class.
        /// </summary>
        /// <param name="matrix">The matrix to operate on.</param>
        /// <param name="tileSize">The size of the tile.</param>
        public TileSimulator(int[,] matrix, int tileSize)
        {
            this.matrix = matrix;
            this.tileSize = tileSize;
        }

        /// <summary>
        /// Runs the tile simulation for the specified tile coordinates.
        /// </summary>
        /// <param name="tileX">The X-coordinate of the tile.</param>
        /// <param name="tileY">The Y-coordinate of the tile.</param>
        public void Run(int tileX, int tileY)
        {
            Span<int> tile = stackalloc int[tileSize * tileSize];
            LoadTile(tile, tileX, tileY);
            PrintTile(tile, "Initial Shared Tile");

            DumpTileMemoryAddresses(tile);
            VisualizeTileCacheLines(tile);

            int sum = ComputeSum(tile);
            Console.WriteLine($"\n✅ Reused tile to compute sum: {sum}");

            int dot = DotProductWithOnes(tile);
            Console.WriteLine($"\n📐 Dot Product with tile of ones: {dot}");

            SimulateWarpReuse(tile);
            SimulateAsyncCopy(tile);
        }

        /// <summary>
        /// Loads a tile from the matrix into a stack-allocated span.
        /// </summary>
        /// <param name="tile">The span to load the tile into.</param>
        /// <param name="tileX">The X-coordinate of the tile.</param>
        /// <param name="tileY">The Y-coordinate of the tile.</param>
        private void LoadTile(Span<int> tile, int tileX, int tileY)
        {
            Console.WriteLine($"\n🔄 Loading tile at ({tileX},{tileY}) into shared memory...\n");

            for (int y = 0; y < tileSize; y++)
            {
                for (int x = 0; x < tileSize; x++)
                {
                    int val = matrix[tileY + y, tileX + x];
                    tile[y * tileSize + x] = val;
                }
            }
        }

        /// <summary>
        /// Computes the sum of all elements in the tile.
        /// </summary>
        /// <param name="tile">The tile to compute the sum for.</param>
        /// <returns>The sum of the tile elements.</returns>
        private int ComputeSum(Span<int> tile)
        {
            int sum = 0;
            foreach (var v in tile)
                sum += v;
            return sum;
        }

        /// <summary>
        /// Computes the dot product of the tile with a tile of ones.
        /// </summary>
        /// <param name="tile">The tile to compute the dot product for.</param>
        /// <returns>The dot product result.</returns>
        private int DotProductWithOnes(Span<int> tile)
        {
            Span<int> tileB = stackalloc int[tileSize * tileSize];
            for (int i = 0; i < tileB.Length; i++)
                tileB[i] = 1;

            int dot = 0;
            for (int i = 0; i < tile.Length; i++)
                dot += tile[i] * tileB[i];

            return dot;
        }

        /// <summary>
        /// Simulates warp-level reuse of the tile.
        /// </summary>
        /// <param name="tile">The tile to reuse.</param>
        private void SimulateWarpReuse(Span<int> tile)
        {
            for (int warp = 0; warp < 3; warp++)
            {
                int warpSum = 0;
                foreach (var v in tile)
                    warpSum += (v + warp);
                Console.WriteLine($"⚙️  Warp {warp} reused tile to compute: {warpSum}");
            }
        }

        /// <summary>
        /// Simulates an asynchronous copy of the tile.
        /// </summary>
        /// <param name="tile">The tile to copy.</param>
        private void SimulateAsyncCopy(Span<int> tile)
        {
            Span<int> asyncTile = stackalloc int[tileSize * tileSize];
            tile.CopyTo(asyncTile);
            int asyncSum = ComputeSum(asyncTile);
            Console.WriteLine($"\n📦 Async-copied tile sum: {asyncSum}");
        }

        /// <summary>
        /// Prints the tile to the console with a label.
        /// </summary>
        /// <param name="tile">The tile to print.</param>
        /// <param name="label">The label for the tile.</param>
        private void PrintTile(Span<int> tile, string label)
        {
            Console.WriteLine($"📦 {label}:");
            for (int y = 0; y < tileSize; y++)
            {
                for (int x = 0; x < tileSize; x++)
                    Console.Write($"{tile[y * tileSize + x],3} ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Dumps the memory addresses of the tile elements.
        /// </summary>
        /// <param name="tile">The tile to inspect.</param>
        private void DumpTileMemoryAddresses(Span<int> tile)
        {
            Console.WriteLine("\n🔬 Memory Layout of Tile (stackalloc'd span):");
            unsafe
            {
                fixed (int* ptr = tile)
                {
                    for (int i = 0; i < tile.Length; i++)
                    {
                        long offset = (long)(ptr + i) - (long)ptr;
                        Console.WriteLine($"Index {i,2}: Addr={(ulong)(ptr + i):X} Offset={offset * sizeof(int)} bytes");
                    }
                }
            }
        }

        /// <summary>
        /// Visualizes the cache line layout of the tile.
        /// </summary>
        /// <param name="tile">The tile to visualize.</param>
        private void VisualizeTileCacheLines(Span<int> tile)
        {
            Console.WriteLine("\n🧩 Cache Line Layout of Tile:");
            unsafe
            {
                fixed (int* basePtr = tile)
                {
                    const int lineSize = 64;
                    ulong lastLine = (ulong)(basePtr) / (ulong)lineSize;

                    for (int i = 0; i < tile.Length; i++)
                    {
                        ulong addr = (ulong)(basePtr + i);
                        ulong line = addr / lineSize;
                        string marker = (line == lastLine) ? " " : "↯";
                        Console.WriteLine($"{marker} Index {i,2} → Addr: 0x{addr:X} (Line {line})");
                        lastLine = line;
                    }
                }
            }
        }
    }
}