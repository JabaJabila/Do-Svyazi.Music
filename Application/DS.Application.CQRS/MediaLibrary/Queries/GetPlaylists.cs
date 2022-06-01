﻿using DS.Application.DTO.Playlist;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetPlaylists
{
    public record Query(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> PlaylistsInfo);

    // public class Handler : IRequestHandler<Query, Response>
    // {
    //     public async Task<Response> Handle(Query request, CancellationToken cancellationToken) { }
    // }
}