# Purpose of Tizen.Net

Internet privileges for the Tizen Studio (With a Javascript approach) do not work. Therefore we have to change our development approach from JavaScript to .NET for our front-end. In this way, we don't have to use tizen studio. Instead we can use Visual Studio for Tizen.NET project.

# Setting up a Development Environment

In order to create Tizen.NET application, set up a Visual Studio. You can follow this [link](https://docs.tizen.org/application/vstools/install/).


1. **Create an Tizen .NET Application.:** Follow this [link](https://docs.tizen.org/application/dotnet/get-started/first-app/#create-a-project) in order to create Tizen .NET Application.

2. **Add Internet Priviliges:** Add this line inside `http://tizen.org/privilege/internet` the Tizen-Manifest.xml file before <ui-application> tag.

3. **Create Developer Certificate:** Create a Certificate using this [link](https://docs.tizen.org/application/dotnet/get-started/certificates/creating-certificates//). Without this certificate we can not run our App on the Galaxy Active2. I will also include my certificate `author` in the project directory. You can also include the already existing certificate using the provided link.

4. **Connect Your Wearable:** Follow this [link](https://developer.samsung.com/sdp/blog/en-us/2021/04/12/remote-device-manager-an-easy-way-to-launch-your-application-with-tizen-studio) in order to successfully connect the remote device to your development machine. On the Visual Studio 2022 press Ctrl+F5 to run it without debugging.

# Documentation of the Code:

Proper documentation has been done in their respective files.



