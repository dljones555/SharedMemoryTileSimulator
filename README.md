# SharedMemoryTileSimulator

Ever wondered how GPUs and high-performance CPUs manage memory for fast matrix operations? **SharedMemoryTileSimulator** is a .NET 9 console app that lets you peek under the hood! ??

- **Tile Loading**: See how a submatrix (tile) is loaded into fast, stack-allocated memory.
- **Cache Line Visualization**: Explore how your data maps to cache lines, and why it matters for performance.
- **Memory Layout Inspection**: Dump the actual memory addresses of your tile data.
- **Reuse Simulation**: Simulate how warps/threads reuse shared memory for efficient computation.
- **Async Copy**: Try out async-style tile copying and see the results instantly.

This project is perfect for:
- Students learning about memory hierarchies and cache effects
- Developers curious about low-level performance
- Anyone who wants to see .NET's stackalloc and Span<T> in action

---

**How to run:**
1. Build with .NET 9 (`dotnet build`)
2. Run the app (`dotnet run`)
3. Follow the console output for a step-by-step simulation

---

*Inspired by GPU programming, but runs on your CPU!*
