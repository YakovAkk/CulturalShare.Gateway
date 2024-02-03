﻿using AuthenticationProto;
using CulturalShare.Common.Helper;
using CulturalShare.Gateway.Extensions;
using CulturalShare.Gateway.Models.Model.Request;
using CulturalShare.GatewayCommon;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsReadProto;
using PostsWriteProto;

namespace CulturalShare.Gateway.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly PostsRead.PostsReadClient _postReadClient;
    private readonly PostsWrite.PostsWriteClient _postWriteClient;
    private readonly Authentication.AuthenticationClient _authClient;

    public PostsController(PostsRead.PostsReadClient postsClient, PostsWrite.PostsWriteClient postWriteClient, Authentication.AuthenticationClient authClient)
    {
        _postReadClient = postsClient;
        _postWriteClient = postWriteClient;
        _authClient = authClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync(CancellationToken cancellationToken)
    {
        try 
        {
            var userId = HttpHelper.GetCustomerId(HttpContext);
            var headers = await this.CreateSecureHeader(HttpContext, _authClient);
            var request = new GetPostsRequest()
            {
                UserId = userId,
            };

            var result = await _postReadClient.GetPostsAsync(request, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (RpcException)
        {
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] int Id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = HttpHelper.GetCustomerId(HttpContext);
            var headers = await this.CreateSecureHeader(HttpContext, _authClient);
            var request = new GetPostByIdRequest()
            {
                UserId = userId,
                Id = Id
            };

            var result = await _postReadClient.GetPostByIdAsync(request, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePostAsync([FromBody] CreatePostRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = HttpHelper.GetCustomerId(HttpContext);
            var headers = await this.CreateSecureHeader(HttpContext, _authClient);

            var createPostRequest = request.MapTo<CreatePostRequest>();
            createPostRequest.UserId = userId;

            var result = await _postWriteClient.CreatePostAsync(createPostRequest, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePostAsync([FromBody] UpdatePostRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = HttpHelper.GetCustomerId(HttpContext);
            var headers = await this.CreateSecureHeader(HttpContext, _authClient);

            var updatePostRequest = request.MapTo<UpdatePostRequest>();
            updatePostRequest.UserId = userId;

            var result = await _postWriteClient.UpdatePostAsync(updatePostRequest, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeletePostAsync([FromRoute] int Id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = HttpHelper.GetCustomerId(HttpContext);
            var headers = await this.CreateSecureHeader(HttpContext, _authClient);

            var request = new DeletePostRequest()
            {
                UserId = userId,
                PostId = Id
            };
            var result = await _postWriteClient.DeletePostAsync(request, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
