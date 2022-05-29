﻿using DS.Domain;
using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess.Context;

public sealed class MusicDbContext : DbContext, IMusicContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    
    public DbSet<ListeningQueue> ListeningQueues { get; private set; } = null!;
    public DbSet<MediaLibrary> MediaLibraries { get; private set; } = null!;
    public DbSet<MusicUser> MusicUsers { get; private set; } = null!;
    public DbSet<Playlist> Playlists { get; private set; } = null!;
    public DbSet<PlaylistSongNode> PlaylistSongNodes { get; private set; } = null!;
    public DbSet<PlaylistSongs> PlaylistSongs { get; private set; } = null!;
    public DbSet<Song> Songs { get; private set; } = null!;
    public DbSet<SongGenre> SongGenres { get; private set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureMusicUser(modelBuilder);
        ConfigureSong(modelBuilder);
        ConfigureSongGenre(modelBuilder);
        ConfigurePlaylistSongNode(modelBuilder);
        ConfigurePlaylistSongs(modelBuilder);
        ConfigurePlaylist(modelBuilder);
        ConfigureListeningQueue(modelBuilder);
        ConfigureMediaLibrary(modelBuilder);
    }

    private static void ConfigureMusicUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MusicUser>().Property(mu => mu.Id).ValueGeneratedNever();
        // modelBuilder.Entity<MusicUser>().HasOne(mu => mu.MediaLibrary);
        // modelBuilder.Entity<MusicUser>().HasOne(mu => mu.ListeningQueue);
    }

    private static void ConfigureSong(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>().Property(s => s.Id).ValueGeneratedNever();
        // modelBuilder.Entity<Song>().HasOne(s => s.Author);
        // modelBuilder.Entity<Song>().HasOne(s => s.Genre);
        modelBuilder.Entity<Song>().Navigation(s => s.Featuring).HasField("_featuring");
    }

    private static void ConfigureSongGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SongGenre>().Property(sg => sg.Id).ValueGeneratedNever();
    }

    private static void ConfigurePlaylistSongNode(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaylistSongNode>().Property(psn => psn.Id).ValueGeneratedNever();
        // modelBuilder.Entity<PlaylistSongNode>().HasOne(psn => psn.Song);
        // modelBuilder.Entity<PlaylistSongNode>().HasOne(psn => psn.NextSongNode);
    }

    private static void ConfigurePlaylistSongs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaylistSongs>().Property(ps => ps.Id).ValueGeneratedNever();
        modelBuilder.Entity<PlaylistSongs>().Property("_head");
        modelBuilder.Entity<PlaylistSongs>().Property("_tail");
    }

    private static void ConfigurePlaylist(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Playlist>().Property(p => p.Id).ValueGeneratedNever();
        modelBuilder.Entity<Playlist>().Navigation(p => p.Songs).HasField("_songs");
        // modelBuilder.Entity<Playlist>().HasOne(p => p.Author);
    }

    private static void ConfigureListeningQueue(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ListeningQueue>().HasKey(lq => lq.OwnerId);
    }

    private static void ConfigureMediaLibrary(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MediaLibrary>().HasKey(ml => ml.OwnerId);
        
        modelBuilder.Entity<MediaLibrary>()
            .Navigation(ml => ml.Songs)
            .HasField("_songs");
        
        modelBuilder.Entity<MediaLibrary>()
            .Navigation(ml => ml.AuthoredSongs)
            .HasField("_authoredSongs");
        
        modelBuilder.Entity<MediaLibrary>()
            .Navigation(ml => ml.Playlists)
            .HasField("_playlists");
        
        modelBuilder.Entity<MediaLibrary>()
            .Navigation(ml => ml.AuthoredPlaylists)
            .HasField("_authoredPlaylists");
    }
}