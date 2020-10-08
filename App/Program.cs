//using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {

        static async Task Main()
        {
            Console.WriteLine("Hello Sensor!");

            await Send(SensorType.temperature, 67.8, null);
            await Send(SensorType.pressure, 10001.2, null);
            await Send(SensorType.accelerometer, 0, new List<double> { 0.789, 0.234, 0.456 }); 

            Console.WriteLine("Hello Sensor! End");
            
        }
        static async Task Send(SensorType sensorType, Double value, List<Double> values)
        {
            try
            {
                System.Net.Http.HttpClient httpClient = new HttpClient();
                Guid guid = Guid.NewGuid();
                long TimeStamp = DateTime.Now.Ticks;
                Sensor _Sensor = new Sensor();
                //_Sensor.No = 137;
                _Sensor.Id = guid.ToString();
                _Sensor.SensorType = sensorType;
                _Sensor.TimeStamp = TimeStamp;
                _Sensor.Value = value;
                _Sensor.Values = values;

                Console.WriteLine(_Sensor.SensorType);

                var dataAsString = JsonConvert.SerializeObject(_Sensor);
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res2 =  await httpClient.PostAsync("https://blazorsensorsppsvc.azurewebsites.net/Sensor", content);

                Console.WriteLine(res2.StatusCode.ToString());
                var sr = await res2.Content.ReadAsStringAsync();
                Console.WriteLine(sr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }


    public class Sensor
    {
        public const char DecimalPointSubChar = 'X'; //Use in Decimal Parameter Strings
        public static int Count { get; set; } = 137;
        public int No { get; set; }
        public string Id { get; set; }
        public double? Value { get; set; }

        //public int TemperatureF => 32 + (int)(Value / 0.5556);

        public bool State { get; set; }
        public List<double>? Values { get; set; }
        public SensorType SensorType { get; set; }
        public long TimeStamp { get; set; }

        public Sensor()
        {
            No=Count++;
        }

    }

    public enum SensorType { temperature, pressure, humidity, luminosity, accelerometer, environment, sswitch }
}
