using Minio;

namespace PetTracerAPI.Services
{
    public static class MinioService
    {
        public static async Task<Stream> RetrieveFileFromMinio(string objectName)
        {
            var minioEndpoint = Environment.GetEnvironmentVariable("MinioServer");
            var minioAccessKey = Environment.GetEnvironmentVariable("MinioAccessKey");
            var minioSecretKey = Environment.GetEnvironmentVariable("MinioSecretKey");
            var minioSecureMode = true;

            MinioClient minio = new MinioClient()
                .WithEndpoint(minioEndpoint)
                .WithCredentials(minioAccessKey, minioSecretKey)
                .WithSSL(minioSecureMode)
                .Build();

            var bucketName = Environment.GetEnvironmentVariable("BucketName");
            Stream outputStream = new MemoryStream();
            StatObjectArgs statObjectArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            await minio.StatObjectAsync(statObjectArgs);

            GetObjectArgs getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) =>
                {
                    stream.CopyTo(outputStream);
                });


            await minio.GetObjectAsync(getObjectArgs);

            return outputStream;
        }
    }
}