using RabbitDirectorySender.Console.src;

var senderRabbit = new Sender();
Console.WriteLine("Digite um diretorio");
var directory = Console.ReadLine() ?? "";
Console.WriteLine("Lendo...");
FileSystemWatcher watcher = new FileSystemWatcher(directory);
watcher.EnableRaisingEvents = true;

watcher.Created += (sender, e) =>
{
    Console.WriteLine($"Novo arquivo visto {e.Name}");
    senderRabbit.SenderToRabbit($@"{directory}\{e.Name}");

};

Console.ReadLine();
