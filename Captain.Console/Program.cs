using Docker.DotNet;
using Docker.DotNet.Models;
using k8s;
using System.Text;
using System.Threading;

//DockerClient dockerClient = new DockerClientConfiguration(new Uri("http://192.168.1.55:2375")).CreateClient();
//var stream = await dockerClient.Containers.AttachContainerAsync("3aeef8bd9a1ee41fd14f4ae2e6bc0287e0176239e825175c3ffde85d7c6a5c22", false, new Docker.DotNet.Models.ContainerAttachParameters());
//await stream.CopyOutputToAsync(Console.OpenStandardInput(),
//                Console.OpenStandardOutput(),
//                Console.OpenStandardError(), CancellationToken.None);




//string lastLine = null;
//var stream = await dockerClient.Containers.AttachContainerAsync("4118f9aa3dbe", true, new ContainerAttachParameters() { Stderr = true,  Stdin = true, Stdout = true, Stream = true });
//var write = Task.Run(async () => {
//    while (true)
//    {
//        lastLine = Console.ReadLine();


//    }
//});
//var read = Task.Run(async () => {
//    while (true)
//    {
//        var buffer = new byte[1024];
//        bool finishedReading = false;
//        while (finishedReading == false)
//        {
//            var result = await stream.ReadOutputAsync(buffer, 0, buffer.Length, default).ConfigureAwait(false);
//            var output = Encoding.UTF8.GetString(buffer, 0, result.Count);
//            if(output.TrimEnd() != lastLine)
//            {
//                Console.Write(output);
//            }

//            Array.Clear(buffer, 0, buffer.Length);
//            if (result.EOF)
//            {
//                finishedReading = true;
//            }
//        }
//    }
//});
//await write.ConfigureAwait(false);
//await read.ConfigureAwait(false);

//var config =  new KubernetesClientConfiguration() { Host = "http://192.168.1.55:8001", AccessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImZKZzM0bjU5dDZWMlhJek1BX1dFa3hFWkZ0d3EzWVpWbGxyT2tGeTZoOWsifQ.eyJhdWQiOlsiaHR0cHM6Ly9rdWJlcm5ldGVzLmRlZmF1bHQuc3ZjLmNsdXN0ZXIubG9jYWwiXSwiZXhwIjoxNzE3MTc1MjY3LCJpYXQiOjE2ODQ3NzUyNjcsImlzcyI6Imh0dHBzOi8va3ViZXJuZXRlcy5kZWZhdWx0LnN2Yy5jbHVzdGVyLmxvY2FsIiwia3ViZXJuZXRlcy5pbyI6eyJuYW1lc3BhY2UiOiJrdWJlcm5ldGVzLWRhc2hib2FyZCIsInNlcnZpY2VhY2NvdW50Ijp7Im5hbWUiOiJhZG1pbi11c2VyIiwidWlkIjoiYzg2YmE3YWEtMjA5OC00MGI1LTgyNWEtOWVmNjRlMTYwNDJmIn19LCJuYmYiOjE2ODQ3NzUyNjcsInN1YiI6InN5c3RlbTpzZXJ2aWNlYWNjb3VudDprdWJlcm5ldGVzLWRhc2hib2FyZDphZG1pbi11c2VyIn0.QvPu5QPGRYXHqfl-TULtLiDuKkuS_Fmm__fMY58FSxyFU70vlF-C_GrdCpNgt-vejegjG2hIzi-LkTQIYtBOvcbuvj8mwOlw0tWJUK1Qvl_OZIZcYqKy3J_PC2fARbuqAdiwkEy1Yjmc888NhhrneHUJEkBBEd5rBCsJD8iJSRRNhjKA6LUD6x46odQgiD5gW5YpurYO5vhmeKLPyfSIlAu_-v-kg6jziIenP7m3J5cR64NahiQrIqVa1V5EiGzmn_rZmoEygvcvYo56FP8CjqLclfZXdRZKPGRBz8X_x99Ei_y7DHdOiN1mJiBLgeisFytZop9HFf6qa4BFbccbEQ" };
var config = new KubernetesClientConfiguration() { Host = "http://192.168.1.55:8001" };

var client = new Kubernetes(config);
var namespaces = client.CoreV1.ListNamespace();
foreach (var ns in namespaces.Items)
{
    Console.WriteLine(ns.Metadata.Name);
    var list = client.CoreV1.ListNamespacedPod(ns.Metadata.Name);
    foreach (var item in list.Items)
    {
        Console.WriteLine("     " + item.Metadata.Name);
    }
}
Console.ReadLine();