# Number to words converter

A tiny project that converts a currency from numbers into words.

For instance:

| Input  | Output                                  |
|--------|-----------------------------------------|
| 0      | zero dollars                            |
| 1      | one dollar                              |
| 25,1   | twenty-five dollars and ten cents       |
| 0,01   | zero dollars and one cent               |
| 45 100 | forty-five thousand one hundred dollars |

## Project structure

* `Protos`: Contains the Protocol Buffers definition used for implementing the gRPC service
* `Server`: Contains a CLI application that implements the gRPC server and holds the logic for the conversions
* `Server.Test`: Contains the tests cases used for verifying the conversions in the server-side
* `WpfClient`: Contains a basic Windows Presentation Foundation (WPF) application that implements the gRPC client
* `CliClient`: Contains a basic CLI application that implements the gRPC client

## Before starting

* The projects were developed in .NET 5.0 (a.k.a. `net5.0`), and they were not tested with other versions
* The `Server` and `CliClient` can be executed in Linux. However, `WpfClient` must be executed in Windows
* Currently, the only fully implemented conversion is to dollars but others will be introduced in the future (see further improvements below)
* Have fun! 💸

## Execute

For a quick execution, you can run the projects as follows:

### Server

#### Default

````shell
cd .\Server\
dotnet run
````

The default binding address is `https://0.0.0.0:9001`. If you want to customize it, then you can run:

````shell
cd .\Server\
dotnet run --urls "http://127.0.0.1:9000;https://127.0.0.1:9001"
````

### WpfClient

````shell
cd .\WpfClient\
dotnet run
````

### CliClient

````shell
cd .\CliClient\
dotnet run --server https://localhost:9001
````

`--server` specifies the server endpoint.

## Further improvements

- [ ] Add missing unit test for `CliClient` and `WpfClient`
- [ ] Enable CI for automatically crosscompiling `Server` and `CliClient` artifacts
- [ ] Dockerize `Server`
- [ ] Implement _Extension methods_ in `Server` for handling the conversion
- [ ] Extend conversion to full words (for instance, _tenths_, _hundredths_ is missing) 
- [ ] Extend conversion to other currencies (e.g., Euro)