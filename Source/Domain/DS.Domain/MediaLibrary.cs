using DS.Common.Enums;
using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class MediaLibrary : IEquatable<MediaLibrary>
{
    private List<MediaLibrarySong> _songs;
    private List<MediaLibraryPlaylist> _playlists;

#pragma warning disable CS8618
    protected MediaLibrary() { }
#pragma warning restore CS8618
    
    public MediaLibrary(Guid ownerId)
    {
        if (ownerId == Guid.Empty)
            throw new GuidIsEmptyException(nameof(Guid));

        Id = ownerId;
        _songs = new List<MediaLibrarySong>();
        _playlists = new List<MediaLibraryPlaylist>();
    }
    
    public Guid Id { get; private init; }
    public IReadOnlyCollection<Song> Songs => _songs.Select(s => s.Song).ToList();
    public IReadOnlyCollection<Song> AuthoredSongs => Songs.Where(s => s.Author.Id == Id).ToList();
    public IReadOnlyCollection<Playlist> Playlists => _playlists.Select(p => p.Playlist).ToList();
    public IReadOnlyCollection<Playlist> AuthoredPlaylists => Playlists.Where(p => p.Author.Id == Id).ToList();

    public void AddSong(Song song)
    {
        song.ThrowIfNull();
        if (Songs.Contains(song))
            throw new DoSvyaziMusicException(ExceptionMessages.SongAlreadyExists);

        _songs.Add(new MediaLibrarySong(this, song));
    }

    public void DeleteSong(Song song)
    {
        song.ThrowIfNull();
        if (!_songs.Remove(new MediaLibrarySong(this, song)))
            throw new EntityNotFoundException(nameof(Song));
    }

    public void AddPlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();
        if (Playlists.Contains(playlist))
            throw new DoSvyaziMusicException(ExceptionMessages.PlaylistAlreadyExists);

        _playlists.Add(new MediaLibraryPlaylist(this, playlist));
    }

    public void DeletePlaylist(Playlist playlist)
    {
        playlist.ThrowIfNull();
        if (!_playlists.Remove(new MediaLibraryPlaylist(this, playlist)))
            throw new EntityNotFoundException(nameof(Playlist));
    }

    public bool Equals(MediaLibrary? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as MediaLibrary);
    public override int GetHashCode() => Id.GetHashCode();
}