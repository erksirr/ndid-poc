namespace ndid_poc.Application.DTOs;

public record VerifyResponse(
    string RequestId,
    string ReferenceId,
    string? Note,
    string NextStep);
