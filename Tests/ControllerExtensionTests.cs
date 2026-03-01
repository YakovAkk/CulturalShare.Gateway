using CulturalShare.Foundation.AspNetCore.Extensions.Constants;
using CulturalShare.Gateway.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CulturalShare.Gateway.Tests;

public class ControllerExtensionTests
{
    private readonly ControllerBase _controller = new FakeController();

    // gRPC Metadata normalises all keys to lowercase.
    private const string AuthMetadataKey = "authorization";
    private const string CorrelationIdMetadataKey = "correlationid"; // LoggingConsts.CorrelationIdHeaderName.ToLower()

    private static DefaultHttpContext CreateHttpContext(
        string? authHeaderValue = null,
        string? correlationId = null)
    {
        var context = new DefaultHttpContext();

        if (authHeaderValue is not null)
            context.Request.Headers["Authorization"] = authHeaderValue;

        if (correlationId is not null)
            context.Request.Headers[LoggingConsts.CorrelationIdHeaderName] = correlationId;

        return context;
    }

    // -------------------------------------------------------------------------
    // Authorization header extraction
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenValidBearerToken_AddsAuthorizationEntryWithToken()
    {
        var context = CreateHttpContext(authHeaderValue: "Bearer mytoken123");

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        var authEntry = metadata.FirstOrDefault(e => e.Key == AuthMetadataKey);
        Assert.NotNull(authEntry);
        Assert.Equal("Bearer mytoken123", authEntry.Value);
    }

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenBearerPrefixIsLowercase_ExtractsTokenCaseInsensitively()
    {
        var context = CreateHttpContext(authHeaderValue: "bearer mytoken123");

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        var authEntry = metadata.FirstOrDefault(e => e.Key == AuthMetadataKey);
        Assert.NotNull(authEntry);
        Assert.Equal("Bearer mytoken123", authEntry.Value);
    }

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenTokenHasLeadingWhitespace_TrimsToken()
    {
        // "Bearer " (7 chars) + "  mytoken123" => Substring gives "  mytoken123", Trim() gives "mytoken123"
        var context = CreateHttpContext(authHeaderValue: "Bearer   mytoken123");

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        var authEntry = metadata.FirstOrDefault(e => e.Key == AuthMetadataKey);
        Assert.NotNull(authEntry);
        Assert.Equal("Bearer mytoken123", authEntry.Value);
    }

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenAuthorizationHeaderIsMissing_AddsAuthorizationEntryWithEmptyToken()
    {
        // GetAuthToken returns null → AddAuthHeader(null) → "Bearer " (empty token)
        var context = CreateHttpContext();

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        var authEntry = metadata.FirstOrDefault(e => e.Key == AuthMetadataKey);
        Assert.NotNull(authEntry);
        Assert.Equal("Bearer ", authEntry.Value);
    }

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenAuthorizationSchemeIsNotBearer_AddsAuthorizationEntryWithEmptyToken()
    {
        var context = CreateHttpContext(authHeaderValue: "Basic dXNlcjpwYXNz");

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        var authEntry = metadata.FirstOrDefault(e => e.Key == AuthMetadataKey);
        Assert.NotNull(authEntry);
        Assert.Equal("Bearer ", authEntry.Value);
    }

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenHttpContextIsNull_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _controller.CreateSecureHeaderWithCorrelationId(null!));
    }

    // -------------------------------------------------------------------------
    // CorrelationId header forwarding
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenCorrelationIdPresentInRequest_ForwardsItToMetadata()
    {
        const string correlationId = "test-correlation-id-123";
        var context = CreateHttpContext(authHeaderValue: "Bearer token", correlationId: correlationId);

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        var correlationEntry = metadata.FirstOrDefault(e => e.Key == CorrelationIdMetadataKey);
        Assert.NotNull(correlationEntry);
        Assert.Equal(correlationId, correlationEntry.Value);
    }

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_WhenCorrelationIdAbsentInRequest_GeneratesNewGuidAsCorrelationId()
    {
        var context = CreateHttpContext(authHeaderValue: "Bearer token");

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        var correlationEntry = metadata.FirstOrDefault(e => e.Key == CorrelationIdMetadataKey);
        Assert.NotNull(correlationEntry);
        Assert.True(Guid.TryParse(correlationEntry.Value, out _), "CorrelationId should be a valid GUID when not provided by the caller.");
    }

    // -------------------------------------------------------------------------
    // Metadata structure
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateSecureHeaderWithCorrelationId_Always_ReturnsTwoMetadataEntries()
    {
        var context = CreateHttpContext(authHeaderValue: "Bearer token", correlationId: "some-id");

        var metadata = await _controller.CreateSecureHeaderWithCorrelationId(context);

        Assert.Equal(2, metadata.Count);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private sealed class FakeController : ControllerBase { }
}
