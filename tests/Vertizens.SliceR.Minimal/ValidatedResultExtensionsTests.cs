using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal.Tests;

public class ValidatedResultExtensionsTests
{
    [Fact]
    public void SuccessfulResultNoContent()
    {
        var result = new ValidatedResult();

        var httpResult = result.ToHttpResult();
        var expectedResult = TypedResults.StatusCode(StatusCodes.Status204NoContent);

        Assert.IsType(expectedResult.GetType(), httpResult);
        Assert.True(((StatusCodeHttpResult)httpResult).StatusCode == expectedResult.StatusCode);
    }
}