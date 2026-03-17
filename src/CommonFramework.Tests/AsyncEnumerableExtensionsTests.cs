namespace CommonFramework.Tests;

public class AsyncEnumerableExtensionsTests
{
    private static async IAsyncEnumerable<string> GetStrings()
    {
        yield return "a";
        yield return "bb";
        yield return "ccc";
        await Task.CompletedTask;
    }

    private static async IAsyncEnumerable<KeyValuePair<int, string>> GetPairs()
    {
        yield return new KeyValuePair<int, string>(1, "a");
        yield return new KeyValuePair<int, string>(2, "b");
        await Task.CompletedTask;
    }

    private static async IAsyncEnumerable<int> GetNumbers()
    {
        yield return 1;
        yield return 1;
        await Task.CompletedTask;
    }

    [Fact]
    public async Task ToImmutableDictionary_Source_KeyValue_WithComparer()
    {
        var result = await GetStrings()
            .ToImmutableDictionaryAsync(x => x.Length, x => x, EqualityComparer<int>.Default, CancellationToken.None);

        result.Should().HaveCount(3);
        result[1].Should().Be("a");
        result[2].Should().Be("bb");
        result[3].Should().Be("ccc");
    }

    [Fact]
    public async Task ToImmutableDictionary_Source_KeyValue_WithoutComparer()
    {
        var result = await GetStrings()
            .ToImmutableDictionaryAsync(x => x.Length, x => x, CancellationToken.None);

        result.Should().ContainKey(1).WhoseValue.Should().Be("a");
    }

    [Fact]
    public async Task ToImmutableDictionary_Source_KeyOnly_WithoutComparer()
    {
        var result = await GetStrings()
            .ToImmutableDictionaryAsync(x => x.Length, CancellationToken.None);

        result[2].Should().Be("bb");
    }

    [Fact]
    public async Task ToImmutableDictionary_Source_KeyOnly_WithComparer()
    {
        var result = await GetStrings()
            .ToImmutableDictionaryAsync(x => x.Length, EqualityComparer<int>.Default, CancellationToken.None);

        result[3].Should().Be("ccc");
    }

    [Fact]
    public async Task ToImmutableDictionary_KeyValuePair_WithoutComparer()
    {
        var result = await GetPairs()
            .ToImmutableDictionaryAsync(CancellationToken.None);

        result.Should().HaveCount(2);
        result[1].Should().Be("a");
    }

    [Fact]
    public async Task ToImmutableDictionary_KeyValuePair_WithComparer()
    {
        var result = await GetPairs()
            .ToImmutableDictionaryAsync(EqualityComparer<int>.Default, CancellationToken.None);

        result[2].Should().Be("b");
    }

    //[Fact]
    //public async Task ToImmutableDictionary_Should_Throw_On_Duplicate_Key()
    //{
    //    Func<Task> act = async () =>
    //        await GetNumbers().ToImmutableDictionaryAsync(x => x, x => x, CancellationToken.None);

    //    await act.Should().ThrowAsync<ArgumentException>();
    //}

    //[Fact]
    //public async Task ToImmutableDictionary_Should_Respect_Cancellation()
    //{
    //    using var cts = new CancellationTokenSource();
    //    await cts.CancelAsync();

    //    Func<Task> act = async () =>
    //        await GetStrings().ToImmutableDictionaryAsync(x => x.Length, x => x, cts.Token);

    //    await act.Should().ThrowAsync<OperationCanceledException>();
    //}
}