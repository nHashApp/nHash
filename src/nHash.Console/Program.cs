﻿
var services = new ServiceCollection();
Startup.RegisterServices(services);
var provider = services.BuildServiceProvider();

return await Startup.StartAsync(args, provider);