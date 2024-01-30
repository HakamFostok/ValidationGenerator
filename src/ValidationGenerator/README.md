Source Generator to generate validation <br />
Aternative to [Fluent Validation](https://github.com/FluentValidation/FluentValidation) package

Performance comparision With [Fluent Validation](https://github.com/FluentValidation/FluentValidation)

```
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.3930/22H2/2022Update)
11th Gen Intel Core i7-1165G7 2.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```

| Method                  | Mean        | Error      | StdDev     | Median      | Ratio  | RatioSD | Allocated | Alloc Ratio |
|------------------------ |------------:|-----------:|-----------:|------------:|-------:|--------:|----------:|------------:|
| Fluent Validation       | 5,525.53 ns | 109.579 ns | 249.567 ns | 5,420.75 ns | 131.51 |    8.11 |   10024 B |       35.80 |
| Generation Validation   |    41.86 ns |   0.871 ns |   1.699 ns |    40.99 ns |   1.00 |    0.00 |     280 B |        1.00 |


**Fluent Validation is slower 131 times** <br/>
**Fluent Validation allocates 35 times more** <br/>