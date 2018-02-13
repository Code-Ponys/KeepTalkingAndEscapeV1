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
        private static void ValidateDatabaseData(ItemDatabase database) {
            foreach(var testitem in database.ItemDatabaseList) {
                var combinePartnerValid = false;
                var combineItemValid = false;
                var count = 0;
                foreach(var checkitem in database.ItemDatabaseList) {
                    if(string.Equals(testitem.ItemId, checkitem.ItemId,
                                     StringComparison.CurrentCultureIgnoreCase)) {
                        count++;
                    }

                    if(testitem.CombineWithItem.ToLower() != "none") {
                        if(string.Equals(checkitem.CombineWithItem, testitem.ItemId,
                                         StringComparison.CurrentCultureIgnoreCase)) {
                            if(string.Equals(testitem.CombineWithItem, checkitem.ItemId,
                                             StringComparison.CurrentCultureIgnoreCase)) {
                                combinePartnerValid = true;
                            }
                        }

                        if(string.Equals(testitem.NextItem, checkitem.ItemId, StringComparison.CurrentCultureIgnoreCase)) {
                            combineItemValid = true;
                        }
                    }
                }

                if(combinePartnerValid == false) {
                    throw new Exception("The Item to combine with is not existent.Exception Occured with Item ID: " + testitem.ItemId);
                }

                if(combineItemValid == false) {
                    throw new Exception("The combined item id does not exist. Exception Occured with Item ID: " + testitem.ItemId);
                }

                if(count != 1) {
                    throw new Exception("The itemId " + testitem.ItemId + " is not unique. Please Check your itemDatabase.yml");
                }
            }
        }
