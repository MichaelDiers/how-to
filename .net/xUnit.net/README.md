### [xUnit.net](https://xUnit.net/)

Howto of [xUnit.net](https://xUnit.net/) including matrix test data, shared context and test case ordering.

- Create a custom test data class: **ClassData**
- Create a matrix of test data: **MatrixTheoryData**
- Specify test data as a class member: **MemberData**
- Use parallel and sequential execution of tests
- Define a shared test context:
  - within a class: **Construtor**, **Dispose**
  - within a collection of test classes: **ICollectionFixture**
- Skip tests at compile- and at run-time: **Skip**, **SkipWhen**, **SkipType**, **SkipUnless*
- Specify test case ordering: **ITestCaseOrderer**
- Run tests only is explictly selected: **Explicit**
- Async initialization of tests: **IAsyncLifetime**