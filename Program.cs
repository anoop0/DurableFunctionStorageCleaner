using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DurableFunctionStorageCleaner;

public class Program
{

    static void Main(string[] args)
    {

        string? StorageConnectionString = null;
        string? StorageEntityPrefix = null;

        try
        {
            Console.WriteLine("*** Start cleaning durable function storage Containers, queues and tables");
            if (args != null && args.Length > 0)
            {
                StorageConnectionString = args[0];
                StorageEntityPrefix = args.Length > 1 ? args[1] : null;
            }
            if (string.IsNullOrEmpty(StorageConnectionString))
            {
                Console.WriteLine(" *** Enter  StorageConnectionString: ");
                StorageConnectionString = Console.ReadLine();
            }

            if (string.IsNullOrEmpty(StorageEntityPrefix))
            {
                Console.WriteLine(" *** Enter  StorageEntityPrefix: <Leave blank for default value: testhubname> ");
                StorageEntityPrefix = Console.ReadLine();
                StorageEntityPrefix = string.IsNullOrEmpty(StorageEntityPrefix) ? "testhubname" : StorageEntityPrefix;
            }

            try
            {
                Console.WriteLine("Deleting Durable function queues");
                var queueServiceClient = new QueueServiceClient(StorageConnectionString);
                var queues = queueServiceClient.GetQueues();
                queues.ToList().ForEach(x =>
                {
                    if (x.Name.Contains(StorageEntityPrefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        queueServiceClient.DeleteQueue(x.Name);
                        Console.WriteLine($"** Queue deteted:  {x.Name}");
                    }

                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                Console.WriteLine("Deleting Durable function Blob containers");
                var blobServiceClient = new BlobServiceClient(StorageConnectionString);
                var containerList = blobServiceClient.GetBlobContainers();
                containerList.ToList().ForEach(x =>
                {
                    if (x.Name.Contains(StorageEntityPrefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        blobServiceClient.DeleteBlobContainer(x.Name);
                        Console.WriteLine($"** Blob container deteted:  {x.Name}");
                    }

                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                Console.WriteLine("Deleting Durable function Tables");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                TableContinuationToken continuationToken = null;
                var tableClient = storageAccount.CreateCloudTableClient();
                var allTables = tableClient.ListTablesSegmentedAsync(continuationToken).Result;
                allTables.ToList().ForEach(async x =>
                {
                    if (x.Name.Contains(StorageEntityPrefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        await x.DeleteAsync();
                        Console.WriteLine($"** Tables deteted:  {x.Name}");
                    }


                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("Completed cleaning up  Durable function storage");
        Console.ReadLine();
    }
}
