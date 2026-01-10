using ReleasePulse.Api.Models;

namespace ReleasePulse.Api.Dtos;

public record UpdateTestCaseDto(string? Actual, TestResult Result, string? TesterNote);
