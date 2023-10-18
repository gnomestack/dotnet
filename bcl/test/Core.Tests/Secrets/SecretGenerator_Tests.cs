using GnomeStack.Secrets;

namespace Tests;

public static class SecretGenerator_Tests
{
    [UnitTest]
    public static void GenerateString()
    {
        var sg = new SecretGenerator();
        sg.Add(SecretCharacterSets.LatinAlphaLowerCase);
        sg.Add(SecretCharacterSets.LatinAlphaUpperCase);
        sg.Add(SecretCharacterSets.Digits);
        sg.SetValidator(_ => true);

        for (var i = 0; i < 100; i++)
        {
            var secret = sg.GenerateAsString(10);
            Assert.Equal(10, secret.Length);
            Assert.True(secret.All(char.IsLetterOrDigit));
        }
    }

    [UnitTest]
    public static void GenerateChars()
    {
        var sg = new SecretGenerator();
        sg.Add(SecretCharacterSets.LatinAlphaLowerCase);
        sg.Add(SecretCharacterSets.LatinAlphaUpperCase);
        sg.Add(SecretCharacterSets.Digits);
        sg.SetValidator(_ => true);

        for (var i = 0; i < 100; i++)
        {
            var secret = sg.Generate(11);
            Assert.Equal(11, secret.Length);
            Assert.True(secret.All(char.IsLetterOrDigit));
        }
    }

    [UnitTest]
    public static void GenerateBytes()
    {
        var sg = new SecretGenerator();
        sg.Add(SecretCharacterSets.LatinAlphaLowerCase);
        sg.Add(SecretCharacterSets.LatinAlphaUpperCase);
        sg.Add(SecretCharacterSets.Digits);
        sg.SetValidator(_ => true);

        for (var i = 0; i < 100; i++)
        {
            var secret = sg.GenerateAsBytes(15);
            Assert.Equal(15, secret.Length);
            Assert.True(secret.All(o => char.IsLetterOrDigit((char)o)));
        }
    }
}