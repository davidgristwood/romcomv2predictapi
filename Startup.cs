using System;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using romcom;


[assembly: FunctionsStartup(typeof(Startup))]
namespace romcom
{
    public class Startup : FunctionsStartup
    {
        private  string _environment;
        private  string _modelPath;

        public Startup()
        {
            _environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
            Console.WriteLine($"_environment {_environment}");
            Console.WriteLine($"CurrentDirectory {Environment.CurrentDirectory}");

            if (_environment == "Development")
            {
                _modelPath = Path.Combine(@"..\..\..\MLModels", "onnxmlmodel.zip");
            }
            else
            {
                string deploymentPath = @"D:\home\site\wwwroot\";
                _modelPath = Path.Combine(deploymentPath, "MLModels", "onnxmlmodel.zip");
            }
            Console.WriteLine($"_modelPath {_modelPath}");
        }
        

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // default is github project - "https://github.com/davidgristwood/romcomv2predictapi/raw/master/MLModels/onnxmlmodel.zip"
            string uri = Environment.GetEnvironmentVariable("MODELURI");
            Console.WriteLine($"model uri {uri}");
            builder.Services.AddPredictionEnginePool<OnnxInput, OnnxOutput>()
                    .FromUri(modelName: "RomComModel", uri);
                    
        }
    }
}