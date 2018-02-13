using System;
using System.IO;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Execution;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TrustfallGames.KeepTalkingAndEscape.DataController {
        public static ItemDatabase LoadDataBase() {
            string content;

            //Load yml
            using(var sr = new StreamReader(DataPath.ItemDatabase)) {
                content = sr.ReadToEnd();
            }

            var input = new StringReader(content);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new PascalCaseNamingConvention())
                .Build();
            var database = deserializer.Deserialize<ItemDatabase>(input);
            ValidateDatabaseData(database);
            return database;
        }
