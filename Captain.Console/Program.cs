using Docker.DotNet;
using Docker.DotNet.Models;
using System.Text;
using System.Threading;

DockerClient dockerClient = new DockerClientConfiguration(new Uri("http://192.168.1.55:2375")).CreateClient();
//var stream = await dockerClient.Containers.AttachContainerAsync("3aeef8bd9a1ee41fd14f4ae2e6bc0287e0176239e825175c3ffde85d7c6a5c22", false, new Docker.DotNet.Models.ContainerAttachParameters());
//await stream.CopyOutputToAsync(Console.OpenStandardInput(),
//                Console.OpenStandardOutput(),
//                Console.OpenStandardError(), CancellationToken.None);




string lastLine = null;
var stream = await dockerClient.Containers.AttachContainerAsync("4118f9aa3dbe", true, new ContainerAttachParameters() { Stderr = true,  Stdin = true, Stdout = true, Stream = true });
var write = Task.Run(async () => {
    while (true)
    {
        lastLine = Console.ReadLine();

       
    }
});
var read = Task.Run(async () => {
    while (true)
    {
        var buffer = new byte[1024];
        bool finishedReading = false;
        while (finishedReading == false)
        {
            var result = await stream.ReadOutputAsync(buffer, 0, buffer.Length, default).ConfigureAwait(false);
            var output = Encoding.UTF8.GetString(buffer, 0, result.Count);
            if(output.TrimEnd() != lastLine)
            {
                Console.Write(output);
            }
           
            Array.Clear(buffer, 0, buffer.Length);
            if (result.EOF)
            {
                finishedReading = true;
            }
        }
    }
});
await write.ConfigureAwait(false);
await read.ConfigureAwait(false);