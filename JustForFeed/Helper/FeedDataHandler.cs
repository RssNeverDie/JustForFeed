using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using JustForFeed.ViewModel;

namespace JustForFeed.Helper
{
    public static class FeedDataHandler
    {


        /// <summary>
        /// Saves the favorites feed (the first feed of the feeds list) to local storage. 
        /// </summary>
        public static void SaveFavoritesAsync(this FeedViewModel favorites)
        {
            //var file = await ApplicationData.Current.LocalFolder
            //    .CreateFileAsync("favorites.dat", CreationCollisionOption.ReplaceExisting);
            //var file = File.Create("");
            //byte[] array = Serialize(favorites);
            //file.WriteAsync(array,0,array.Length,null);

            //await FileIO.WriteBytesAsync(file, array);

            using (Stream fs = new FileStream(Path.Combine(RunTime.DataPath, "feeds.xml"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(FeedViewModel));
                dcs.WriteObject(fs, favorites);

                //XmlSerializer ser = new XmlSerializer(typeof(FeedViewModel));
                //ser.Serialize(fs, favorites);
            }
        }

        /// <summary>
        /// Saves the feed data (not including the Favorites feed) to local storage. 
        /// </summary>
        public static void SaveAsync(this IEnumerable<FeedViewModel> feeds)
        {
            //var file = await ApplicationData.Current.LocalFolder
            //    .CreateFileAsync("feeds.dat", CreationCollisionOption.ReplaceExisting);
            //byte[] array = Serializer.Serialize(feeds.Select(feed => new[] { feed.Name, feed.Link.ToString() }).ToArray());
            //await FileIO.WriteBytesAsync(file, array);
        }


        /// <summary>
        /// Serializes the specified object as a byte array.
        /// </summary>
        public static byte[] Serialize<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(T));
            xs.Serialize(stream, obj);
            //DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            //dcs.WriteObject(stream, obj);
            return stream.ToArray();
        }

        /// <summary>
        /// Deserializes the specified byte array as an instance of type T. 
        /// </summary>
        public static T Deserialize<T>(byte[] buffer)
        {
            MemoryStream stream = new MemoryStream(buffer);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(stream);
            //DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            //return (T)dcs.ReadObject(stream);
        }
    }
}
