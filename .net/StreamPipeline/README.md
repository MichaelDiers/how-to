# [Custom Streams](StreamPipeline)

Combine multiple [streams](https://learn.microsoft.com/en-gb/dotnet/api/system.io.stream) to a single stream pipeline: Compress data using [Brotli](https://developer.mozilla.org/en-US/docs/Glossary/Brotli_compression) followed by [AES](https://csrc.nist.gov/glossary/term/advanced_encryption_standard) encryption.

- create a custom stream
- add a [Brotli](https://developer.mozilla.org/en-US/docs/Glossary/Brotli_compression) stream to compress/decompress the stream data
- add an [AES](https://csrc.nist.gov/glossary/term/advanced_encryption_standard) stream to encrypt/decrypt the stream data