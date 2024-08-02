

public class ThreadArguments {
    public string FileToRead { get; set; }
    public string FileToWrite { get; set; }
}

public static class Program {
   static object obj = new object();

    static void WorkWithFile(object args) {
        var thrArgs = (ThreadArguments)args;

        FileStream file = new FileStream(thrArgs.FileToRead, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(file);
        string temp = reader.ReadToEnd();
        reader.Close();
        file.Close();

        lock (obj) {
            FileStream file3 = new FileStream(thrArgs.FileToWrite, FileMode.OpenOrCreate, FileAccess.Write);
            file3.Position = file3.Length;
            StreamWriter writer = new StreamWriter(file3);
            writer.Write(temp);
            writer.Close();
            file3.Close();
        }
    }


    public static void Main(string[] args) {
        string finalFileName = "TextFinal.txt";

        var file1 = new Thread(WorkWithFile);
        file1.Start(new ThreadArguments{
            FileToWrite = finalFileName,
            FileToRead = "TextFile1.txt"
        });
        var file2 = new Thread(WorkWithFile);
        file2.Start(new ThreadArguments {
            FileToWrite = finalFileName,
            FileToRead = "TextFile2.txt"
        });
        
    }

}