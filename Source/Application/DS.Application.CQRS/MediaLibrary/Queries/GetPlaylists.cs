﻿using AutoMapper;
using DS.Application.DTO.Playlist;
using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Queries;

public static class GetPlaylists
{
    public record GetPlaylistsQuery(Guid UserId) : IRequest<Response>;

    public record Response(IReadOnlyCollection<PlaylistInfoDto> PlaylistsInfo);

    public class Handler : IRequestHandler<GetPlaylistsQuery, Response>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
        {
            Domain.MusicUser? user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            return new Response(_mapper.Map<IReadOnlyCollection<PlaylistInfoDto>>(user.MediaLibrary.Playlists));
        }
    }
}