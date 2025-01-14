﻿using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MediaLibrary.Commands;

public static class AddPlaylist
{
    public record AddPlaylistCommand(Guid UserId, Guid PlaylistId) : IRequest;

    public class Handler : IRequestHandler<AddPlaylistCommand>
    {
        private readonly MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(AddPlaylistCommand request, CancellationToken cancellationToken)
        {
            Domain.MusicUser? musicUser = await _context.MusicUsers.FindAsync(request.UserId);
            if (musicUser is null)
                throw new EntityNotFoundException(ExceptionMessages.UserCannotBeFound);

            Domain.Playlist? playlist = await _context.Playlists.FindAsync(request.PlaylistId);
            if (playlist is null)
                throw new EntityNotFoundException(ExceptionMessages.PlaylistCannotBeFound);

            musicUser.MediaLibrary.AddPlaylist(playlist);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}