﻿using DS.Common.Exceptions;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.Song.Commands;

public static class AddFeaturing
{
    public record AddFeaturingCommand(Guid UserId, Guid SongId, Guid FeaturingUserId) : IRequest;
    
    public class Handler : IRequestHandler<AddFeaturingCommand>
    {
        private MusicDbContext _context;
        public Handler(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddFeaturingCommand request, CancellationToken cancellation)
        {
            var user = await _context.MusicUsers.FindAsync(request.UserId);
            if (user is null)
                throw new EntityNotFoundException($"User {request.UserId} does not exist");
            
            var song = await _context.Songs.FindAsync(request.SongId);
            if (song is null)
                throw new EntityNotFoundException("Song cannot be found in the database");
            
            song.AddFeaturingUser(user);
            await _context.SaveChangesAsync(cancellation);
            
            return Unit.Value;
        }
    }
}