using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Test.BazzarOn.UnitTest;

public static class AutoFixtureExtensions
{
    public static IFixture WithAutoNSubstitutions(this IFixture fixture)
    {
        return fixture.Customize(new AutoNSubstituteCustomization
        {
            ConfigureMembers = true
        });
    }
}