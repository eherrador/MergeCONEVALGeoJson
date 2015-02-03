using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using GeoJSON.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AgebZonasMetropolitanas
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string geoJsonText;
			string datosConeval;
			int agebs = 0;

			string originalsGeoJsonPath = @"/Users/eherrador/Desktop/GFK/GeoJSON/Modificados y Filtrados V3/";
			string newGeoJsonPath = @"/Users/eherrador/Desktop/GFK/GeoJSON/Modificados y Filtrados V3/Con Datos CONEVAL/";
			string datosConevalPath = @"/Users/eherrador/Desktop/GFK/Base de datos Pobreza - CONEVAL/";

			string datosConevalFileName = @"DatosFiltradosCONEVAL.txt";

			//string geoJsonFileName = @"AGSUrbAgeb.json";
			//string geoJsonFileName = @"BCUrbAgeb.json";
			//string geoJsonFileName = @"DFUrbAgeb.json";
			//string geoJsonFileName = @"MEXUrbAgeb.json";
			//string geoJsonFileName = @"GTOUrbAgeb.json";
			//string geoJsonFileName = @"JLUrbAgeb.json";
			//string geoJsonFileName = @"MORUrbAgeb.json";
			//string geoJsonFileName = @"NLUrbAgeb.json";
			//string geoJsonFileName = @"QROUrbAgeb.json";
			//string geoJsonFileName = @"SINUrbAgeb.json";
			//string geoJsonFileName = @"SLPUrbAgeb.json";
			string geoJsonFileName = @"SONUrbAgeb.json";


			System.Collections.Generic.List<GeoJSON.Net.Feature.Feature> featuresList = new List<GeoJSON.Net.Feature.Feature> ();

			using (StreamReader streamGeoJSON = new StreamReader (originalsGeoJsonPath + geoJsonFileName)) {
				geoJsonText = streamGeoJSON.ReadToEnd ();
				Console.WriteLine("GeoJSON Original leído...");
				Console.WriteLine(geoJsonText);
			}

			var features = JsonConvert.DeserializeObject<GeoJSON.Net.Feature.FeatureCollection>(geoJsonText);

			Console.WriteLine ("GeoJson Deserializado...");
			Console.WriteLine ("Se inicia la lectura de los datos de CONEVAL... ");

			using (StreamReader streamDatosConeval = new StreamReader (datosConevalPath + datosConevalFileName)) {
				while ((datosConeval = streamDatosConeval.ReadLine ()) != null) {
					string[] datos = datosConeval.Split(',');
					Console.WriteLine ("Buscando la clave ageb: " + datos[0]);
					foreach (GeoJSON.Net.Feature.Feature f in features.Features) {
						if (f.Properties.ContainsValue (datos[0])) {
							Console.WriteLine ("Se ha encontrado y se van a agregar las nuevas propiedades...");
							f.Properties.Add ("Viviendas particulares habitadas", datos [2].ToString ());
							f.Properties.Add ("Población de 15 años y más con educación básica incompleta", datos[3].ToString());
							f.Properties.Add ("Población de 15 a 24 años que no asiste a la escuela", datos[4].ToString());
							f.Properties.Add ("Población sin derechohabiencia a servicios de salud", datos[5].ToString());
							f.Properties.Add ("Personas que viven en hacinamiento", datos[6].ToString());
							f.Properties.Add ("Viviendas que no disponen de excusado o sanitario", datos[7].ToString());
							f.Properties.Add ("Viviendas que no disponen de lavadora", datos[8].ToString());
							f.Properties.Add ("Viviendas que no disponen de refrigerador", datos[9].ToString());
							f.Properties.Add ("Viviendas que no disponen de teléfono fijo", datos[10].ToString());
							f.Properties.Add ("Población de 15 años o más analfabeta", datos[11].ToString());
							f.Properties.Add ("Población de 6 a 14 años que no asiste a la escuela", datos[12].ToString());
							f.Properties.Add ("Viviendas con piso de tierra", datos[13].ToString());
							f.Properties.Add ("Viviendas que no disponen de agua entubada de la red pública", datos[14].ToString());
							f.Properties.Add ("Viviendas que no disponen de drenaje", datos[15].ToString());
							f.Properties.Add ("Viviendas que no disponen de energía eléctrica", datos[16].ToString());
							f.Properties.Add ("Grado de rezago social", datos[17].ToString());
						}
					}
				}
			}

			//GeoJSON.Net.Feature.FeatureCollection fc = new GeoJSON.Net.Feature.FeatureCollection (featuresList);
			string featuresSerialized = JsonConvert.SerializeObject (features);
			Console.WriteLine ("Se ha serializado el objeto lista de agebs...");

			using (StreamWriter outfile = new StreamWriter(newGeoJsonPath + geoJsonFileName))
			{
				outfile.Write (featuresSerialized);
			}
			Console.WriteLine("Se ha generado un nuevo archivo GeoJSON...");
		}
	}
}
