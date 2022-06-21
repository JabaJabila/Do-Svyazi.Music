﻿using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Song;
using Microsoft.AspNetCore.Mvc;

namespace DS.Application.DTO.Playlist;

public record PlaylistInfoDto
(
    string Name,
    IReadOnlyCollection<SongInfoDto> Songs,
    FileStreamResult? Cover,
    MusicUserInfoDto Author
)
{
    public PlaylistInfoDto() 
        : this(string.Empty, ArraySegment<SongInfoDto>.Empty, null, new MusicUserInfoDto()) { }
}