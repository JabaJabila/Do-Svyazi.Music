using System.Linq;
using DS.Common.Exceptions;
using DS.Domain;
using DS.Domain.Types;
using NUnit.Framework;

namespace DS.Tests.EntitiesTests;

[TestFixture]
public class SongTests
{
    private MusicUser _featuringUser;
    private MusicUser _author;
    private Song _song;
    
    [SetUp]
    public void Setup()
    {
        _featuringUser = new MusicUser();
        _author = new MusicUser();
        _song = new Song(new AuthoredSongType("Test", new SongGenre("Test"), _author, "Content"));
    }

    [Test]
    public void AddFeaturingUser_UserIsNotNull_Success()
    {
        _song.AddFeaturingUser(_featuringUser);
        Assert.Contains(_featuringUser, _song.Featuring.ToList());
    }

    [Test]
    public void DeleteFeaturingUser_UserIsFeaturing_UserDeleted()
    {
        _song.AddFeaturingUser(_featuringUser);
        _song.DeleteFeaturingUser(_featuringUser);
        
        Assert.False(_song.Featuring.Contains(_featuringUser));
    }

    [Test]
    public void DeleteFeaturingUser_UserIsNotFeaturing_ThrowError()
    {
        Assert.Catch<DoSvyaziMusicException>(() =>
        {
            _song.DeleteFeaturingUser(_featuringUser);
        });
    }
}