using nHash.App;
using nHash.Application.Shared.Conversions;

var source = """
<?xml version="1.0" encoding="ISO-8859-1"?>  
        <note>  
    <to>Tove</to>  
    <from>Jani</from>  
    <heading>Reminder</heading>  
    <body>Don't forget me this weekend!</body>  
    </note> 
""";
var target= Conversion.ToYaml(source, ConversionType.XML);
Console.WriteLine(target);

var services = new ServiceCollection();
Startup.RegisterServices(services);
var provider = services.BuildServiceProvider();

return await Startup.StartAsync(args, provider);