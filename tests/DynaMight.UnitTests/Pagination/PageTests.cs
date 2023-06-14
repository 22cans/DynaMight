using DynaMight.Pagination;
using FluentAssertions;

namespace DynaMight.UnitTests.Pagination;

public class PageTests
{
    public class Origin
    {
        public Guid Guid { get; set; }
    }

    public class OriginDto : IConvertFrom<Origin, OriginDto>
    {
        public string Id { get; set; } = default!;
        public static OriginDto ConvertFrom(Origin original)
        {
            return new OriginDto
            {
                Id = original.Guid.ToString()
            };
        }
    }

    [Fact]
    public void ConvertTo()
    {
        const string id = "5e4c0992-7d56-4926-b712-3ca7a1230cda";
        const string token = "nextPage";
        var origin = new Origin()
        {
            Guid = new Guid(id)
        };

        var pageOrigin = new Page<Origin>(token, new List<Origin> { origin });
        var pageDto = pageOrigin.ConvertTo<OriginDto>();

        pageDto.Should().NotBeNull();
        pageDto.PageToken.Should().Be(token);
        pageDto.Results.Should().NotBeNull();
        pageDto.Results!.Count.Should().Be(1);
        pageDto.Results[0].Should().BeEquivalentTo(new OriginDto{ Id = id});
    }

    [Fact]
    public void Empty()
    {
        var emptyPage = Page<DynaMightTestClass.InternalClass>.Empty();

        emptyPage.Should().NotBeNull();
        emptyPage.PageToken.Should().BeEmpty();
        emptyPage.Results.Should().NotBeNull();
        emptyPage.Results!.Count.Should().Be(0);
    }
}