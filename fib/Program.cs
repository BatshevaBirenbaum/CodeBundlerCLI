//fib bundle --output D:folder\bundleFile.txt

using System.CommandLine;

//פונקציה שעוברת על תתי תיקיות
//(ומחזירה רשימה של ניתובים לקבצים ( רק אלה שנמצאים בתוך תיקייה  אחת לפחות
static List<string> OperateOnSourceFiles(string source)
{
    List<string> folders = new List<string>();

    void RecursiveOperation(string currentSource)
    {
        //עובר כל כל התתי תיקיות חוץ מתיקיוה ששמה bin
        List<string> subfolders = Directory.GetDirectories(currentSource).Where(d => !d.Contains("bin")).ToList();
        for (int i = 0; i < subfolders.Count; i++) { 
            
        }
        foreach (var subfolder in subfolders)
        {
            folders.Add(subfolder);
            RecursiveOperation(subfolder);
        }
    }
    RecursiveOperation(source);
    return folders;
}

var bundleCommand = new Command("bundle", "Bundle code files to a single file");
var create_rsp = new Command("create-rsp", "shorter command");
var bundleOption = new Option<FileInfo>("--output", "File path and name");
var languageOption = new Option<string>("--language", "list of language");
var noteOption = new Option<Boolean>("--note", "yes or no");
var sortOption = new Option<Boolean>("--sort", "sort by extension");
var removetOption = new Option<Boolean>("--remove", "remove empty lines");
var authorOption = new Option<string>("--author", "Registering the name of the creator of the file");

languageOption.IsRequired = true;

// סוגי ביומות שיכול לקבל
string[] arrTypes = { "txt", "json", "css", "html", "java","js"};
//רשימת הניתובים של הקבצים שאותם רוצים להעביר
List<string> types = new List<string>();
string allText = "";
// מחרוזת שבא יכנסו כל הטקסטים שאני רוצה למלא בקובץ חדש
bundleOption.AddAlias("-o");
languageOption.AddAlias("-l");
noteOption.AddAlias("-n");
sortOption.AddAlias("-s");
removetOption.AddAlias("-r");
authorOption.AddAlias("-a");


bundleCommand.AddOption(bundleOption);
bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(removetOption);
bundleCommand.AddOption(authorOption);

bundleCommand.SetHandler((output, language, note, sort, remove, author) =>
{
    // מערך של סטרינג שמחלק את סוגי השפות שהמשתמש רוצה להעביר 
    string[] str = language.Split(',');
    try
    {
        // הרשימה תכיל את הרשימה שחזרה מהפונקצייה שטיפלה בתתי תיקיות
        List<string> subFolders = OperateOnSourceFiles(Directory.GetCurrentDirectory());

        for (int j = 0; j < str.Length; j++)
        {
            //מטפל בקבצים החיצוניים שלא בתוך תיקיות
            foreach (string file1 in Directory.EnumerateFiles(Directory.GetCurrentDirectory()))
            {
                // מוסיף את הניתוב לרשימה במקרה שהסיומת שהמשתמש רוצה אפשרית
                // אם המשתמש הכניס אול הוא מכניס כל קובץ שמצא בלי הגבלות
                if ((arrTypes.Contains(str[j]) && file1.EndsWith(str[j])) || language == "all")
                {
                    types.Add(file1);
                }
            }
            //עובר על הניתובים של התתי תיקיות
            for (int l = 0; l < subFolders.Count; l++)
            {
                foreach (string file in Directory.EnumerateFiles(subFolders[l]))
                {

                    if ((arrTypes.Contains(str[j]) && file.EndsWith(str[j])) || language == "all")
                    {
                        types.Add(file);
                    }
                }
            }
            //בדיקה
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine("before==" + types[i]);
            }

            Console.WriteLine("----------------------------------------");
            if (sort)
                // ממיין לפי סיומת
                types = types.OrderBy(fn => Path.GetExtension(fn)).ToList();
            else
                // ממיין לפ הא' ב'
                types = types.OrderBy(fn => Path.GetFileName(fn)).ToList();

            // עובר על כל הניתובים וממלא את הקבצים
            if (!string.IsNullOrEmpty(author)) 
            allText +=   author+ "\n";


            foreach (string type in types)
            {
                if (remove)
                {
                    // מוחק שורות ריקות
                    var lines = System.IO.File.ReadAllLines(type).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToArray();
                    System.IO.File.WriteAllLines(type, lines);
                }

                string ss = System.IO.File.ReadAllText(type);

                // במקרה שהמשתמש כתב נוט, מוסיף לפני הטקסט את הניתוב שלו 
                if (note)
                    allText += "//" + type + "\n";

                // מכניס את התוכן למחרוזת 
                allText += ss + "\n";
            }

           // הדפסת הרשימה
            for (int i = 0; i < types.Count; i++)
            {

                Console.WriteLine("after==" + types[i]);
            }
            //דוחף את המחרוזת של התוכן לקובץ
            System.IO.File.WriteAllText(output.FullName, allText);
        }
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("Error: File path is invalid");
    }
}, bundleOption, languageOption, noteOption, sortOption, removetOption, authorOption);

var rootCommand = new RootCommand("Root command for file bundler cli");


create_rsp.SetHandler(() =>
{
    string commandStr = "";
    try
    {
        Console.WriteLine("Enter a file name and type");
        commandStr+= "-o "+Console.ReadLine();


        Console.WriteLine("Enter the types of languages ​​you want to copy with a comma between them");
        string lan = Console.ReadLine();
       
            if (lan != "")
            {
                    commandStr += " " + "-l " + lan;   
              }
            else
              {
            Console.WriteLine("Error: languageOption is requierd");
              }
    

        Console.WriteLine("Do you want to get routing before the whole file? (y/n) ");
        
        if (Console.ReadLine() == "y")
            commandStr += " "+"-n ";

        Console.WriteLine("Do you want to sort by file type? (y/n)");
        if (Console.ReadLine() == "y")
            commandStr += " "+"-s ";

        Console.WriteLine("Do you want to delete empty rows? (y/n)");
        if (Console.ReadLine() == "y")
            commandStr += " "+"-r ";
        Console.WriteLine("If you want to put your name at the top of the file - write it down.");
        string au= Console.ReadLine();
        if (au != "")
            commandStr += " "+"-a " + au;

        File.WriteAllText("RspFile.rsp", commandStr);
    }
    catch
    {
        Console.WriteLine("Error: The command was not found");
    }
});
rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(create_rsp);

rootCommand.InvokeAsync(args);




