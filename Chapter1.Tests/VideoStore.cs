using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System;
using Xunit;

namespace Chapter1.Tests
{
    public class VideoStore
    {
        public class UnitTests
        {
            private IImmutableDictionary<string, Play> _plays;
            private Invoice _invoice;

            public UnitTests()
            {
                Init();
            }

            private void Init()
            {
                _plays = new Dictionary<string, Play>
                {
                    {"hamlet", new Play("Hamlet", PayType.Tragedy)},
                    {"as-like", new Play("As You Like It", PayType.Comedy)},
                    {"othello", new Play("Othello", PayType.Tragedy)}
                }.ToImmutableDictionary();

                _invoice = new Invoice("BigCo", new List<Performance>
                {
                    new Performance("hamlet", 55),
                    new Performance("as-like", 35),
                    new Performance("othello", 40)
                });
            }

            [Theory]
            [MemberData(nameof(StatementImplementations))]
            public void RenderPlainTextTest(IVideoStore implementation)
            {
                var expected = "Statement for BigCo" + Environment.NewLine +
                               "  Hamlet: $650.00 (55 seats)" + Environment.NewLine +
                               "  As You Like It: $580.00 (35 seats)" + Environment.NewLine +
                               "  Othello: $500.00 (40 seats)" + Environment.NewLine +
                               "Amount owed is $1,730.00" + Environment.NewLine +
                               "You earned 47 credits";
                               
                Assert.Equal(expected, implementation.Statement(_invoice, _plays));
            }

            [Theory]
            [MemberData(nameof(HtmlStatementImplementations))]
            public void RenderHtmlTest(IHtmlVideoStore implementation)
            {
                Assert.NotEmpty(implementation.HtmlStatement(_invoice, _plays));
            }

            public static IEnumerable<object[]> StatementImplementations => new StatementImplementationProvider();

            public static IEnumerable<object[]> HtmlStatementImplementations => new HtmlStatementImplementationProvider();

            private class StatementImplementationProvider : IEnumerable<object[]>
            {
                public IEnumerator<object[]> GetEnumerator()
                {
                    yield return new object[] { new VideoStore0BeforeRefactor() };
                    yield return new object[] { new VideoStore1DecomposeMethod() };
                    yield return new object[] { new VideoStore2RemoveVariable() };
                    yield return new object[] { new VideoStore3ExtractMethod() };
                    yield return new object[] { new VideoStore4RemoveVariable() };
                    yield return new object[] { new VideoStore5RemoveLoops() };
                    yield return new object[] { new VideoStore6SplitPhase() };
                    yield return new object[] { new VideoStore7Polymorphism() };
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }

            private class HtmlStatementImplementationProvider : IEnumerable<object[]>
            {
                public IEnumerator<object[]> GetEnumerator()
                {
                    yield return new object[] { new VideoStore6SplitPhase() };
                    yield return new object[] { new VideoStore7Polymorphism() };
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }
        }
    }
}
