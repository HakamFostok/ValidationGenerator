using BenchmarkDotNet.Running;

BenchmarkRunner.Run(typeof(Program).Assembly);

// command to execute benchmark
// dotnet run -c Release --filter