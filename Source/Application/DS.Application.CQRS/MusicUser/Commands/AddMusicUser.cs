﻿using DS.Application.DTO.MusicUser;
using DS.DataAccess;
using DS.DataAccess.Context;
using MediatR;

namespace DS.Application.CQRS.MusicUser.Commands;

public static class AddMusicUser
{
    public record AddUserCommand(MusicUserCreationInfoDto MusicUserCreationInfo) : IRequest;

    public class Handler : IRequestHandler<AddUserCommand>
    {
        private readonly MusicDbContext _context;
        private readonly IContentStorage _storage;
        
        public Handler(MusicDbContext context, IContentStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            MusicUserCreationInfoDto dto = request.MusicUserCreationInfo;
            
            string? profilePictureUri = null;
            // Force unwrapping is ok here because if cover is null
            // we wont get inside this condition
            if (Helpers.Helpers.ShouldGenerateUri(dto.ProfilePicture))
                profilePictureUri = _storage.GenerateUri(dto.ProfilePicture!.FileName);

            var musicUser = new Domain.MusicUser
            (
                dto.Id,
                dto.Name,
                profilePictureUri
             );
            
            _context.MusicUsers.Add(musicUser);

            await _context.SaveChangesAsync(cancellationToken);
            
            if (dto.ProfilePicture is null || dto.ProfilePicture.Length == 0)
                return Unit.Value;
            
            await Helpers.Helpers.UploadFile
            (
                dto.ProfilePicture.OpenReadStream(),
                musicUser.ProfilePictureUri,
                _storage
            );
            
            return Unit.Value;
        }
    }
}