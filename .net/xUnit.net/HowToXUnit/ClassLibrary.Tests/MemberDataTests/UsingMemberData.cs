namespace ClassLibrary.Tests.MemberDataTests;

public class UsingMemberData
{
    public static IEnumerable<object[]> ObjectArrayProperty =>
    [
        [
            "foo",
            5,
            new Data("bar")
        ],
        [
            "foobar",
            6,
            new Data("baz")
        ]
    ];

    public static Task<IEnumerable<object[]>> ObjectArrayTaskProperty =>
        Task.FromResult<IEnumerable<object[]>>(
        [
            [
                "foo",
                5,
                new Data("bar")
            ],
            [
                "foobar",
                6,
                new Data("baz")
            ]
        ]);

    public static IEnumerable<ITheoryDataRow> TheoryDataRowProperty =>
    [
        new TheoryDataRow(
        [
            "foo",
            5,
            new Data("bar")
        ]),
        new TheoryDataRow(
        [
            "foobar",
            6,
            new Data("baz")
        ])
    ];

    public static Task<IEnumerable<ITheoryDataRow>> TheoryDataRowPropertyTask =>
        Task.FromResult<IEnumerable<ITheoryDataRow>>(
        [
            new TheoryDataRow(
            [
                "foo",
                5,
                new Data("bar")
            ]),
            new TheoryDataRow(
            [
                "foobar",
                6,
                new Data("baz")
            ])
        ]);

    public static IEnumerable<object[]> ObjectArrayFunc()
    {
        return
        [
            [
                "foo",
                5,
                new Data("bar")
            ],
            [
                "foobar",
                6,
                new Data("baz")
            ]
        ];
    }

    public static async IAsyncEnumerable<object[]> ObjectArrayFuncAsync()
    {
        yield return await Task.FromResult(
            new object[]
            {
                "foo",
                5,
                new Data("bar")
            });
        yield return await Task.FromResult(
            new object[]
            {
                "foobar",
                7,
                new Data("baz")
            });
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.ObjectArrayFuncAsync))]
    public void ObjectArrayFuncAsyncTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.ObjectArrayFunc))]
    public void ObjectArrayFuncTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.ObjectArrayProperty))]
    public void ObjectArrayPropertyTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    public static async Task<IEnumerable<object[]>> ObjectArrayTaskFunc()
    {
        return await Task.FromResult<IEnumerable<object[]>>(
        [
            [
                "foo",
                5,
                new Data("bar")
            ],
            [
                "foobar",
                6,
                new Data("baz")
            ]
        ]);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.ObjectArrayTaskFunc))]
    public void ObjectArrayTaskFuncTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.ObjectArrayTaskProperty))]
    public void ObjectArrayTaskPropertyTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    public static IEnumerable<ITheoryDataRow> TheoryDataRowFunc()
    {
        return
        [
            new TheoryDataRow(
            [
                "foo",
                5,
                new Data("bar")
            ]),
            new TheoryDataRow(
            [
                "foobar",
                6,
                new Data("baz")
            ])
        ];
    }

    public static async Task<IEnumerable<ITheoryDataRow>> TheoryDataRowFuncTask()
    {
        return await Task.FromResult<IEnumerable<ITheoryDataRow>>(
        [
            new TheoryDataRow(
            [
                "foo",
                5,
                new Data("bar")
            ]),
            new TheoryDataRow(
            [
                "foobar",
                6,
                new Data("baz")
            ])
        ]);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.TheoryDataRowFuncTask))]
    public void TheoryDataRowFuncTaskTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.TheoryDataRowFunc))]
    public void TheoryDataRowFuncTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.TheoryDataRowPropertyTask))]
    public void TheoryDataRowPropertyTaskTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }

    [Theory]
    [MemberData(nameof(UsingMemberData.TheoryDataRowProperty))]
    public void TheoryDataRowPropertyTest(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data.Name);
    }
}
